using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics.TraceListeners;

namespace System.Diagnostics.LoggerService
{
	/// <summary>
	/// Displays a user interface which allows the user to select a Trace Listener.
	/// </summary>
	public partial class AddListenerDialog : Form
	{
		/// <summary>
		/// Stores the list of current trace listeners
		/// </summary>
		private List<TraceListenerInfo> CurrentListeners;
		/// <summary>
		/// Stores the Trace Listerner Info object which is populated and returned to the calling form.
		/// </summary>
		private TraceListenerInfo ListenerInfo;
		/// <summary>
		/// Stores the Initialization Parameters Grid Source
		/// </summary>
		private DataSet GridSource;
		/// <summary>
		/// Stores information about available assemblies and trace listener classes.
		/// </summary>
		private AssemblyInfoDictionary Assemblies;

		private static class Constants
		{
			/// <summary>
			/// Name of the DataSet used for the Grid Source.
			/// </summary>
			public const string GridDataSetName = "GridSource";
			/// <summary>
			/// Name of the DataTable used for the Grid Source.
			/// </summary>
			public const string GridDataTableName = "Parameters";
			/// <summary>
			/// The "Name" Grid Column name.
			/// </summary>
			public const string GridColumnName = "Name";
			/// <summary>
			/// The "Value" Grid Column name.
			/// </summary>
			public const string GridColumnValue = "Value";
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="currentListeners">A list containing current, active Trace Listener Info objects.</param>
		public AddListenerDialog(List<TraceListenerInfo> currentListeners)
		{
			CurrentListeners = currentListeners;
			ListenerInfo = new TraceListenerInfo();

			InitializeComponent();

			TxtName.DataBindings.Add("Text", ListenerInfo, "Name");

			InitGridSource();
			Assemblies = AssemblyInfoDictionary.Deserialize();
			PopulateAssemblyComboBox();
		}

		/// <summary>
		/// Initalizes the Grid Source DataSet
		/// </summary>
		[SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
			Justification = "Method creates and initializes an IDisposable object, object is disposed in another method.")]
		private void InitGridSource()
		{
			GridSource = new DataSet(Constants.GridDataSetName);
			GridSource.Locale = CultureInfo.CurrentUICulture;

			DataTable parameters = new DataTable(Constants.GridDataTableName);
			parameters.Locale = CultureInfo.CurrentUICulture;
			GridSource.Tables.Add(parameters);

			GridSource.Tables[Constants.GridDataTableName].Columns.Add(Constants.GridColumnName);
			GridSource.Tables[Constants.GridDataTableName].Columns.Add(Constants.GridColumnValue);

			GrdParameters.DataSource = GridSource;
			GrdParameters.DataMember = Constants.GridDataTableName;
		}

		/// <summary>
		/// Disposes of IDisposable resources used by the form.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosed(EventArgs e)
		{
			GridSource.Dispose();
			base.OnClosed(e);
		}

		/// <summary>
		/// Populates the Assembly combo box.
		/// </summary>
		private void PopulateAssemblyComboBox()
		{
			CboAssembly.Items.Clear();

			foreach (string key in Assemblies.Keys)
			{
				CboAssembly.Items.Add(key);
			}
		}

		/// <summary>
		/// Populates the Trace Listener Class combo box depending upon the Assembly chosen.
		/// </summary>
		private void PopulateClassComboBox()
		{
			CboClass.Items.Clear();

			if (Assemblies.ContainsKey(CboAssembly.Text))
			{
				AssemblyInfo info = Assemblies[CboAssembly.Text];

				foreach (string key in info.Classes.Keys)
				{
					CboClass.Items.Add(key);
				}
			}
		}

		/// <summary>
		/// Populates the Initalization Parameters grid depending upon the Trace Listener class choosen.
		/// </summary>
		private void PopulateGrid()
		{
			if (Assemblies.ContainsKey(CboAssembly.Text))
			{
				AssemblyInfo assembly = Assemblies[CboAssembly.Text];

				if (assembly.Classes.ContainsKey(CboClass.Text))
				{
					ClassInfo classInfo = assembly.Classes[CboClass.Text];
					DataTable table = GridSource.Tables[Constants.GridDataTableName];
					table.Rows.Clear();

					foreach (string parameter in classInfo.Parameters)
					{
						DataRow row = table.NewRow();
						row[Constants.GridColumnName] = parameter;
						table.Rows.Add(row);
					}
				}
			}
		}

		/// <summary>
		/// Displays the Add Trace Listener Dialog.
		/// </summary>
		/// <param name="owner">The calling form.</param>
		/// <param name="currentListeners">A list containing current, active Trace Listener Info objects.</param>
		/// <returns>A Trace Listener Information object containing the user selected trace listener information.</returns>
		public static TraceListenerInfo GetTraceListenerInfo(IWin32Window owner, List<TraceListenerInfo> currentListeners)
		{
			using (AddListenerDialog form = new AddListenerDialog(currentListeners))
			{
				form.ShowDialog(owner);
				return form.ListenerInfo;
			}
		}

		/// <summary>
		/// Ok Button - Returns a Trace Listener Information object containing the user selected information.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnOk_Click(object sender, System.EventArgs e)
		{
			if (TxtName.Text.Length == 0)
			{
				ShowErrorMessage();
			}
			else
			{
				bool unique = true;

				if (CurrentListeners != null)
				{
					foreach (TraceListenerInfo info in CurrentListeners)
					{
						if (info.Name == TxtName.Text)
						{
							unique = false;
							ShowErrorMessage();
							break;
						}
					}
				}

				if (unique)
				{
					DialogResult = DialogResult.OK;

					if (Assemblies.ContainsKey(CboAssembly.Text))
					{
						AssemblyInfo assembly = Assemblies[CboAssembly.Text];

						if (assembly.Classes.ContainsKey(CboClass.Text))
						{
							ClassInfo classInfo = assembly.Classes[CboClass.Text];
							ListenerInfo.Listener = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", classInfo.Name, assembly.Name);
						}
					}
					else
					{
						ListenerInfo.Listener = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", CboClass.Text, CboAssembly.Text); ;
					}

					foreach (DataRow row in GridSource.Tables[Constants.GridDataTableName].Rows)
					{
						ListenerInfo.Parameters.Add((string)row[Constants.GridColumnName], row[Constants.GridColumnValue].ToString());
					}

					Close();
				}
			}
		}

		/// <summary>
		/// Displays an Error Message to the user when the user fails to specify a Unique Name for the trace listener.
		/// </summary>
		private static void ShowErrorMessage()
		{
			MessageBox.Show("You must provide a unique name for the trace listener.", "Unique Name Required", MessageBoxButtons.OK,
				MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, 0);
		}

		/// <summary>
		/// Cancel Button - Returns null.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BtnCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			GrdParameters.DataSource = null;
			ListenerInfo = null;
			Close();
		}

		/// <summary>
		/// Updates the Trace Listener Class combo box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CboAssembly_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PopulateClassComboBox();
		}

		/// <summary>
		/// Updates the Initialization Parameters grid.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CboClass_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PopulateGrid();
		}
	}
}