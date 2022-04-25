using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace System.Windows.Forms
{
	/// <summary>
	/// Represents a column in a System.Windows.Forms.DataGridView control.
	/// </summary>
	public class DataGridViewMaskedTextBoxColumn : DataGridViewColumn
	{
		/// <summary>
		///  Gets or sets the input mask to use at run time.
		/// </summary>
		public string Mask { get; set; }

		/// <summary>
		///  Gets or sets the character used to represent the absence of user input
		/// </summary>
		public char PromptChar { get; set; }

		/// <summary>
		/// Gets or sets the data type used to verify the data input by the user.
		/// </summary>
		public Type ValidatingType { get; set; }

		/// <summary>
		/// Constructor. Creates a new instance of the DataGridVeiwMaskedTextBoxColumn
		/// </summary>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Disposable object is disposed by base class")]
		public DataGridViewMaskedTextBoxColumn() : base(new DataGridViewMaskedTextBoxCell())
		{
			Mask = string.Empty;
			PromptChar = '_';
			ValidatingType = typeof(string);
		}

		/// <summary>
		/// Gets or sets the template used to create new cells.
		/// </summary>
		[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DataGridViewMaskedTextBoxCell", Justification = "Literal is the name of the expected type")]
		public override DataGridViewCell CellTemplate
		{
			get { return base.CellTemplate; }
			set
			{
				if ((value != null) && !(value is DataGridViewMaskedTextBoxCell)) throw new InvalidCastException("Cell type is not based upon the DataGridViewMaskedTextBoxCell.");
				base.CellTemplate = value;
			}
		}
	}
}
