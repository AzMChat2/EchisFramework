using System.Globalization;

namespace System.Windows.Forms
{
	/// <summary>
	/// Displays editable, masked text information in a <c ref="System.Windows.Forms.DataGridView">DataGridView</c> control
	/// </summary>
	public class DataGridViewMaskedTextBoxCell : DataGridViewTextBoxCell
	{
		/// <summary>
		/// Gets the type of the cell's hosted editing control.
		/// </summary>
		public override Type EditType { get { return typeof(DataGridViewMaskedTextBoxEditingControl); } }

		/// <summary>
		/// Intializes and attaches the hosted editing control.
		/// </summary>
		/// <param name="rowIndex">The index of the row being edited</param>
		/// <param name="initialFormattedValue">the intitial value to be displayed in the control</param>
		/// <param name="dataGridViewCellStyle">A cell style that is used to determine the appearance of the hosted control.</param>
		public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
		{
			base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

			DataGridViewMaskedTextBoxEditingControl editingControl = DataGridView.EditingControl as DataGridViewMaskedTextBoxEditingControl;
			DataGridViewMaskedTextBoxColumn column = OwningColumn as DataGridViewMaskedTextBoxColumn;

			if ((editingControl != null) && (column != null))
			{
				editingControl.Mask = column.Mask;
				editingControl.PromptChar = column.PromptChar;
				editingControl.ValidatingType = column.ValidatingType;
				editingControl.Text = Convert.ToString(Value, CultureInfo.CurrentCulture);
			}
		}
	}
}
