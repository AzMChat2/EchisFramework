using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>
	/// A DataGridView Editing Control which uses a mask to distinguish between proper and improper input
	/// </summary>
	public class DataGridViewMaskedTextBoxEditingControl : MaskedTextBox, IDataGridViewEditingControl
	{
		/// <summary>
		/// Gets or sets a value indicating whether the cell contents need to be repositioned whenever the value changes.
		/// </summary>
		public bool RepositionEditingControlOnValueChange { get { return false; } }

		/// <summary>
		/// Gets or sets a value indicating whether the value of the editing control differs from the value of the hosting cell.
		/// </summary>
		public bool EditingControlValueChanged { get; set; }

		/// <summary>
		/// Gets or sets the index of the hosting cell's parent row.
		/// </summary>
		public int EditingControlRowIndex { get; set; }

		/// <summary>
		/// Gets or sets the System.Windows.Forms.DataGridView that contains the cell.
		/// </summary>
		public DataGridView EditingControlDataGridView { get; set; }

		/// <summary>
		/// Gets the cursor used when the mouse pointer is over the System.Windows.Forms.DataGridView.EditingPanel but not over the editing control.
		/// </summary>
		public Cursor EditingPanelCursor { get { return Cursors.IBeam; } }

		/// <summary>
		/// Gets or sets the formatted value of the cell being modified by the editor.
		/// </summary>
		public object EditingControlFormattedValue { get { return Text; } set { Text = Convert.ToString(value, CultureInfo.CurrentCulture); } }

		/// <summary>
		/// Raises the System.Windows.Forms.Control.TextChanged event.
		/// </summary>
		/// <param name="e">An System.EventArgs that contains event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			EditingControlValueChanged = true;
			if (EditingControlDataGridView != null) EditingControlDataGridView.NotifyCurrentCellDirty(true);
		}

		/// <summary>
		/// Retrieves the formatted value of the cell.
		/// </summary>
		/// <param name="context">A bitwise combination of System.Windows.Forms.DataGridViewDataErrorContexts values that specifies the context in which the data is needed.</param>
		/// <returns>An System.Object that represents the formatted version of the cell contents.</returns>
		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return Text;
		}

		/// <summary>
		/// Determines whether the specified key is a regular input key that the editing control should process or a special key that the System.Windows.Forms.DataGridView should process.
		/// </summary>
		/// <param name="keyData">A System.Windows.Forms.Keys that represents the key that was pressed.</param>
		/// <param name="dataGridViewWantsInputKey">true when the System.Windows.Forms.DataGridView wants to process the System.Windows.Forms.Keys in keyData; otherwise, false.</param>
		/// <returns> true if the specified key is a regular input key that should be handled by the editing control; otherwise, false.</returns>
		public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
		{
			switch (keyData & Keys.KeyCode)
			{
				case Keys.Right:
					return (!(SelectionLength == 0 && SelectionStart == Text.Length));
				case Keys.Left:
					return (!(SelectionLength == 0 && SelectionStart == 0));
				case Keys.Home:
				case Keys.End:
					return (SelectionLength != Text.Length);
				case Keys.Prior:
				case Keys.Next:
					return EditingControlValueChanged;
				case Keys.Delete:
					return (SelectionLength > 0 || SelectionStart < Text.Length);
			}

			return !dataGridViewWantsInputKey;
		}

		/// <summary>
		/// Prepares the currently selected cell for editing.
		/// </summary>
		/// <param name="selectAll">True to select all of the cell's content; otherwise, false.</param>
		public void PrepareEditingControlForEdit(bool selectAll)
		{
			if (selectAll)
				SelectAll();
			else
				SelectionStart = Text.Length;
		}

		/// <summary>
		///  Changes the control's user interface (UI) to be consistent with the specified cell style.
		/// </summary>
		/// <param name="dataGridViewCellStyle"></param>
		public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
			if (dataGridViewCellStyle == null) throw new ArgumentNullException("dataGridViewCellStyle");

			Font = dataGridViewCellStyle.Font;
			ForeColor = dataGridViewCellStyle.ForeColor;
			BackColor = dataGridViewCellStyle.BackColor;
			TextAlign = TranslateAlignment(dataGridViewCellStyle.Alignment);
		}

		/// <summary>
		/// Translates a DataGridViewContentAlignment value to a HorizonatalAlignment value.
		/// </summary>
		private static HorizontalAlignment TranslateAlignment(DataGridViewContentAlignment align)
		{
			switch (align)
			{
				case DataGridViewContentAlignment.TopCenter:
				case DataGridViewContentAlignment.MiddleCenter:
				case DataGridViewContentAlignment.BottomCenter:
					return HorizontalAlignment.Center;

				case DataGridViewContentAlignment.TopRight:
				case DataGridViewContentAlignment.MiddleRight:
				case DataGridViewContentAlignment.BottomRight:
					return HorizontalAlignment.Right;
			}

			return HorizontalAlignment.Left;
		}
	}
}
