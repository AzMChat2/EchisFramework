using System.Diagnostics;
using System.Windows.Forms;

namespace System.Diagnostics.LoggerService
{
	/// <summary>
	/// Displays a user interface which allows the user to select a Trace Level.
	/// </summary>
	public partial class TraceLevelDialog : Form
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public TraceLevelDialog()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Displays the Select Trace Level Dialog and returns the user's selection.
		/// </summary>
		/// <param name="owner">The calling form.</param>
		/// <param name="defaultLevel">The default Trace Level to be displayed.  This is also the trace level returned if the user selects cancel.</param>
		/// <returns>The user selected Trace Level.</returns>
		public static TraceLevel GetTraceLevel(IWin32Window owner, TraceLevel defaultLevel)
		{
			TraceLevel retVal = defaultLevel;
			using (TraceLevelDialog form = new TraceLevelDialog())
			{
				form.TraceLevel = defaultLevel;

				if (form.ShowDialog(owner) == DialogResult.OK)
				{
					retVal = form.TraceLevel;
				}

				return retVal;
			}
		}

		/// <summary>
		/// Gets or Sets the selected Trace Level.
		/// </summary>
		private TraceLevel TraceLevel
		{
			get
			{
				TraceLevel retVal = TraceLevel.Off;

				if (RbtnError.Checked)
				{
					retVal = TraceLevel.Error;
				}
				else if (RbtnWarning.Checked)
				{
					retVal = TraceLevel.Warning;
				}
				else if (RbtnInformation.Checked)
				{
					retVal = TraceLevel.Info;
				}
				else if (RbtnVerbose.Checked)
				{
					retVal = TraceLevel.Verbose;
				}

				return retVal;
			}
			set
			{
				switch (value)
				{
					case TraceLevel.Error:
						RbtnError.Checked = true;
						break;
					case TraceLevel.Warning:
						RbtnWarning.Checked = true;
						break;
					case TraceLevel.Info:
						RbtnInformation.Checked = true;
						break;
					case TraceLevel.Verbose:
						RbtnVerbose.Checked = true;
						break;
					default:
						RbtnOff.Checked = true;
						break;
				}
			}
		}

		/// <summary>
		/// Ok button - Returns the selected Trace Level.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnOk_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// Cancel button - Returns the defaulted Trace Level.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

	}
}