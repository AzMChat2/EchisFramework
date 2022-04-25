using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics.TraceListeners;
using System.Diagnostics.Loggers.Service;

namespace System.Diagnostics.LoggerService
{
	/// <summary>
	/// The Logger Service Console main form.
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Default Constructor.
		/// </summary>
		public MainForm()
		{
			Manager = new RegistryClient();
			InitializeComponent();
			RefreshTreeView();
		}

		private RegistryClient Manager = null;

		/// <summary>
		/// Stores the name of the selected Machine.
		/// </summary>
		private string Machine;
		/// <summary>
		/// Stores the name of the selected Process.
		/// </summary>
		private string Process;
		/// <summary>
		/// Stores the name of the selected Thread
		/// </summary>
		private string Thread;

		/// <summary>
		/// Determines which Machine, Process and Thread the user has selected.
		/// </summary>
		/// <returns></returns>
		private bool DetermineTraceMonitorInfo()
		{
			bool retVal = false;

			try
			{
				if (TraceMonitorsTree.SelectedNode.Nodes.Count == 0)
				{
					Thread = TraceMonitorsTree.SelectedNode.Text;
					Process = TraceMonitorsTree.SelectedNode.Parent.Text;
					Machine = TraceMonitorsTree.SelectedNode.Parent.Parent.Text;
					retVal = true;
				}
				else
				{
					Thread = null;
					Process = null;
					Machine = null;
					retVal = false;
				}
			}
			catch (NullReferenceException) { }

			return retVal;
		}

		/// <summary>
		/// Adds a Trace Listener to the selected process.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddTraceMenuItem_Click(object sender, System.EventArgs e)
		{
			if (DetermineTraceMonitorInfo())
			{
				try
				{
					TraceListenerInfo info = AddListenerDialog.GetTraceListenerInfo(this, GridSource);

					if (info != null)
					{
						try
						{
							Manager.AddTraceListener(Machine, Process, Thread, info);
							RefreshThreadData();
						}
						catch (CommunicationException ex)
						{
							string message = string.Format(CultureInfo.CurrentUICulture, "An exception occurred while querying clients.  The offending client has been removed from the registry, please try refreshing the tree.\r\n\r\n{0}", ex.GetExceptionMessage());
							MessageBox.Show(this, message, "Communication Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOption);
						}
					}
				}
				catch (IOException ex)
				{
					string message = string.Format(CultureInfo.CurrentUICulture, "Unable to load Trace Listener Types.\r\n\r\n{0}", ex.GetExceptionMessage());
					MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOption);
				}
			}
			else
			{
				MessageBox.Show(this, "The Add Trace Listener action is only valid on threads.", "Invalid Action", MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOption);
			}
		}

		/// <summary>
		/// Removes a Trace Listener from the selected process.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RemoveTraceMenuItem_Click(object sender, System.EventArgs e)
		{
			if (DetermineTraceMonitorInfo())
			{
				int idx = TraceListenersGrid.CurrentRowIndex;

				if ((idx >= 0) && (idx < GridSource.Count))
				{
					try
					{
						Manager.RemoveTraceListener(Machine, Process, Thread,
							((TraceListenerInfo)GridSource[idx]).Name);

						RefreshThreadData();
					}
					catch (CommunicationException ex)
					{
						string message = string.Format(CultureInfo.CurrentUICulture, "An exception occurred while querying clients.  The offending client has been removed from the registry, please try refreshing the tree.\r\n\r\n{0}", ex.GetExceptionMessage());
						MessageBox.Show(this, message, "Communication Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOption);
					}
				}
			}
			else
			{
				MessageBox.Show(this, "The Remove Trace Listener action is only valid on threads.", "Invalid Action", MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOption);
			}
		}

		/// <summary>
		/// Changes the Tracing Level for the selected thread.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChangeLevelMenuItem_Click(object sender, System.EventArgs e)
		{
			if (DetermineTraceMonitorInfo())
			{
				try
				{
					// TODO: Provide Appropriate Context Id!
					TraceLevel defaultLevel = Manager.GetTraceLevel(Machine, Process, null, Thread);
					Manager.SetTraceLevel(Machine, Process, null, Thread, TraceLevelDialog.GetTraceLevel(this, defaultLevel));
					RefreshThreadData();
				}
				catch (CommunicationException ex)
				{
					string message = string.Format(CultureInfo.CurrentUICulture, "An exception occurred while querying clients.  The offending client has been removed from the registry, please try refreshing the tree.\r\n\r\n{0}", ex.GetExceptionMessage());
					MessageBox.Show(this, message, "Communication Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOption);
				}
			}
			else
			{
				MessageBox.Show(this, "The Change Trace Level action is only valid on threads.", "Invalid Action", MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, MessageBoxOption);
			}
		}

		/// <summary>
		/// Determines which thread was selected by the user, then refreshes the trace information for that thread.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TraceMonitorsTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if (DetermineTraceMonitorInfo())
			{
				RefreshThreadData();
			}
			else
			{
				ThreadInfoLabel.Text = string.Empty;
				TraceLevelLabel.Text = string.Empty;
				GridSource = null;
				TraceListenersGrid.DataSource = null;
			}
		}

		/// <summary>
		/// Closes the window and exits the application.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExitMenuItem_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// Refreshes the Machine/Process/Thread tree.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RefreshMenuItem_Click(object sender, System.EventArgs e)
		{
			RefreshTreeView();
		}

		/// <summary>
		/// Stores the source of the Trace Listeners grid.
		/// </summary>
		private List<TraceListenerInfo> GridSource;

		/// <summary>
		/// Refreshes the information displayed on screen about the thread.
		/// </summary>
		private void RefreshThreadData()
		{
			if ((Machine == null) || (Process == null) || (Thread == null))
			{
				ThreadInfoLabel.Text = string.Empty;
				TraceLevelLabel.Text = string.Empty;
				GridSource = null;
				TraceListenersGrid.DataSource = null;
			}
			else
			{
				try
				{
					// TODO: Provide Appropriate Context Id!
					TraceLevel level = Manager.GetTraceLevel(Machine, Process, null, Thread);
					List<TraceListenerInfo> gridSource = Manager.GetTraceListeners(Machine, Process, Thread);

					ThreadInfoLabel.Text = Machine + ": " + Process + ": " + Thread;
					TraceLevelLabel.Text = "Current Trace Level: " + level.ToString();

					if ((gridSource == null) || (gridSource.Count == 0))
					{
						GridSource = null;
						TraceListenersGrid.DataSource = null;
					}
					else
					{
						GridSource = gridSource;
						TraceListenersGrid.DataSource = GridSource;
						TraceListenersGrid.FlatMode = true;
						TraceListenersGrid.PreferredColumnWidth = 300;
					}
				}
				catch (CommunicationException ex)
				{
					string message = string.Format(CultureInfo.CurrentUICulture, "An exception occurred while querying clients.  The offending client has been removed from the registry, please try refreshing the tree.\r\n\r\n{0}", ex.GetExceptionMessage());
					MessageBox.Show(this, message, "Communication Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOption);
				}
			}
		}

		/// <summary>
		/// Refreshes the Machine/Process/Thread tree.
		/// </summary>
		private void RefreshTreeView()
		{
			try
			{
				TraceMonitorsTree.Nodes.Clear();

				string[] machines = Manager.GetMachineNames();

				if ((machines == null) || (machines.Length == 0))
				{
					Machine = null;
					Process = null;
					Thread = null;
				}
				else
				{
					foreach (string machine in machines)
					{
						TreeNode machineNode = new TreeNode(machine);
						TraceMonitorsTree.Nodes.Add(machineNode);

						string[] processes = Manager.GetProcessNames(machine);

						if ((processes == null) || (processes.Length == 0))
						{
							Process = null;
							Thread = null;
						}
						else
						{
							// TODO: Need to change logic to wait until Process tree is expanded before grabbing threads... 
							foreach (string process in processes)
							{
								TreeNode processNode = new TreeNode(process);
								machineNode.Nodes.Add(processNode);

								string[] threads = Manager.GetThreadNames(machine, process);

								if ((threads == null) || (threads.Length == 0))
								{
									Thread = null;
								}
								else
								{
									foreach (string thread in threads)
									{
										TreeNode threadNode = new TreeNode(thread);
										processNode.Nodes.Add(threadNode);

										if (thread == Thread)
										{
											machineNode.ExpandAll();
											TraceMonitorsTree.SelectedNode = threadNode;
										}
									}
								}
							}
						}
					}
				}

				RefreshThreadData();
			}
			catch (EndpointNotFoundException eex)
			{
				string message = string.Format(CultureInfo.CurrentUICulture, "An exception occurred while connecting to the server.  Check that the Logger Service is running.\r\n\r\n{0}", eex.GetExceptionMessage());
				MessageBox.Show(this, message, "Communication Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOption);
				Close();
			}
			catch (CommunicationException ex)
			{
				string message = string.Format(CultureInfo.CurrentUICulture, "An exception occurred while querying clients.  The offending client has been removed from the registry, please try refreshing again.\r\n\r\n{0}", ex.GetExceptionMessage());
				MessageBox.Show(this, message, "Communication Error", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOption);
			}
		}

		/// <summary>
		/// Determines the appropriate MessageBoxOptions for this form.
		/// </summary>
		private MessageBoxOptions MessageBoxOption
		{
			get
			{
				MessageBoxOptions retVal = 0;

				if (this.RightToLeft == RightToLeft.Yes)
				{
					retVal = (MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
				}

				return retVal;
			}
		}
	}
}