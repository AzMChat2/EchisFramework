
namespace System.Diagnostics.LoggerService
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);

			if (Manager != null)
			{
				Manager.Dispose();
			}
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.TraceMonitorsTree = new System.Windows.Forms.TreeView();
			this.MainMenu = new System.Windows.Forms.MainMenu(this.components);
			this.ActionMenuItem = new System.Windows.Forms.MenuItem();
			this.RefreshMenuItem = new System.Windows.Forms.MenuItem();
			this.SeparatorMenuItem1 = new System.Windows.Forms.MenuItem();
			this.AddTraceMenuItem = new System.Windows.Forms.MenuItem();
			this.RemoveTraceMenuItem = new System.Windows.Forms.MenuItem();
			this.ChangeLevelMenuItem = new System.Windows.Forms.MenuItem();
			this.SeparatorMenuItem2 = new System.Windows.Forms.MenuItem();
			this.ExitMenuItem = new System.Windows.Forms.MenuItem();
			this.FormSplitter = new System.Windows.Forms.Splitter();
			this.InfoPanel = new System.Windows.Forms.Panel();
			this.TraceLevelLabel = new System.Windows.Forms.Label();
			this.ThreadInfoLabel = new System.Windows.Forms.Label();
			this.TraceListenersGrid = new System.Windows.Forms.DataGrid();
			this.dataGridTableStyle1 = new System.Windows.Forms.DataGridTableStyle();
			this.dataGridTextBoxColumn1 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.dataGridTextBoxColumn2 = new System.Windows.Forms.DataGridTextBoxColumn();
			this.TreeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addTraceListenerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.changeTraceLevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.GridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.removeTraceListenerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.InfoPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TraceListenersGrid)).BeginInit();
			this.TreeContextMenu.SuspendLayout();
			this.GridContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// TraceMonitorsTree
			// 
			this.TraceMonitorsTree.ContextMenuStrip = this.TreeContextMenu;
			this.TraceMonitorsTree.Dock = System.Windows.Forms.DockStyle.Left;
			this.TraceMonitorsTree.Location = new System.Drawing.Point(0, 0);
			this.TraceMonitorsTree.Name = "TraceMonitorsTree";
			this.TraceMonitorsTree.Size = new System.Drawing.Size(360, 677);
			this.TraceMonitorsTree.TabIndex = 0;
			this.TraceMonitorsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TraceMonitorsTree_AfterSelect);
			// 
			// MainMenu
			// 
			this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ActionMenuItem});
			// 
			// ActionMenuItem
			// 
			this.ActionMenuItem.Index = 0;
			this.ActionMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.RefreshMenuItem,
            this.SeparatorMenuItem1,
            this.AddTraceMenuItem,
            this.RemoveTraceMenuItem,
            this.ChangeLevelMenuItem,
            this.SeparatorMenuItem2,
            this.ExitMenuItem});
			this.ActionMenuItem.Text = "&Action";
			// 
			// RefreshMenuItem
			// 
			this.RefreshMenuItem.Index = 0;
			this.RefreshMenuItem.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.RefreshMenuItem.Text = "Refresh Tree";
			this.RefreshMenuItem.Click += new System.EventHandler(this.RefreshMenuItem_Click);
			// 
			// SeparatorMenuItem1
			// 
			this.SeparatorMenuItem1.Index = 1;
			this.SeparatorMenuItem1.Text = "-";
			// 
			// AddTraceMenuItem
			// 
			this.AddTraceMenuItem.Index = 2;
			this.AddTraceMenuItem.Text = "Add &Trace Listener";
			this.AddTraceMenuItem.Click += new System.EventHandler(this.AddTraceMenuItem_Click);
			// 
			// RemoveTraceMenuItem
			// 
			this.RemoveTraceMenuItem.Index = 3;
			this.RemoveTraceMenuItem.Text = "&Remove Trace Listener";
			this.RemoveTraceMenuItem.Click += new System.EventHandler(this.RemoveTraceMenuItem_Click);
			// 
			// ChangeLevelMenuItem
			// 
			this.ChangeLevelMenuItem.Index = 4;
			this.ChangeLevelMenuItem.Text = "&Change Trace Level";
			this.ChangeLevelMenuItem.Click += new System.EventHandler(this.ChangeLevelMenuItem_Click);
			// 
			// SeparatorMenuItem2
			// 
			this.SeparatorMenuItem2.Index = 5;
			this.SeparatorMenuItem2.Text = "-";
			// 
			// ExitMenuItem
			// 
			this.ExitMenuItem.Index = 6;
			this.ExitMenuItem.Text = "E&xit";
			this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
			// 
			// FormSplitter
			// 
			this.FormSplitter.Location = new System.Drawing.Point(360, 0);
			this.FormSplitter.Name = "FormSplitter";
			this.FormSplitter.Size = new System.Drawing.Size(3, 677);
			this.FormSplitter.TabIndex = 2;
			this.FormSplitter.TabStop = false;
			// 
			// InfoPanel
			// 
			this.InfoPanel.Controls.Add(this.TraceLevelLabel);
			this.InfoPanel.Controls.Add(this.ThreadInfoLabel);
			this.InfoPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.InfoPanel.Location = new System.Drawing.Point(363, 0);
			this.InfoPanel.Name = "InfoPanel";
			this.InfoPanel.Size = new System.Drawing.Size(733, 56);
			this.InfoPanel.TabIndex = 3;
			// 
			// TraceLevelLabel
			// 
			this.TraceLevelLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.TraceLevelLabel.Location = new System.Drawing.Point(0, 24);
			this.TraceLevelLabel.Name = "TraceLevelLabel";
			this.TraceLevelLabel.Size = new System.Drawing.Size(733, 23);
			this.TraceLevelLabel.TabIndex = 1;
			// 
			// ThreadInfoLabel
			// 
			this.ThreadInfoLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.ThreadInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ThreadInfoLabel.Location = new System.Drawing.Point(0, 0);
			this.ThreadInfoLabel.Name = "ThreadInfoLabel";
			this.ThreadInfoLabel.Size = new System.Drawing.Size(733, 24);
			this.ThreadInfoLabel.TabIndex = 0;
			// 
			// TraceListenersGrid
			// 
			this.TraceListenersGrid.AllowNavigation = false;
			this.TraceListenersGrid.CaptionText = "Thread Trace Listeners";
			this.TraceListenersGrid.ContextMenuStrip = this.GridContextMenu;
			this.TraceListenersGrid.DataMember = "";
			this.TraceListenersGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TraceListenersGrid.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.TraceListenersGrid.Location = new System.Drawing.Point(363, 56);
			this.TraceListenersGrid.Name = "TraceListenersGrid";
			this.TraceListenersGrid.PreferredColumnWidth = 300;
			this.TraceListenersGrid.ReadOnly = true;
			this.TraceListenersGrid.Size = new System.Drawing.Size(733, 621);
			this.TraceListenersGrid.TabIndex = 4;
			this.TraceListenersGrid.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dataGridTableStyle1});
			// 
			// dataGridTableStyle1
			// 
			this.dataGridTableStyle1.DataGrid = this.TraceListenersGrid;
			this.dataGridTableStyle1.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dataGridTextBoxColumn1,
            this.dataGridTextBoxColumn2});
			this.dataGridTableStyle1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dataGridTableStyle1.PreferredColumnWidth = 300;
			// 
			// dataGridTextBoxColumn1
			// 
			this.dataGridTextBoxColumn1.Format = "";
			this.dataGridTextBoxColumn1.FormatInfo = null;
			this.dataGridTextBoxColumn1.Width = 300;
			// 
			// dataGridTextBoxColumn2
			// 
			this.dataGridTextBoxColumn2.Format = "";
			this.dataGridTextBoxColumn2.FormatInfo = null;
			this.dataGridTextBoxColumn2.Width = 400;
			// 
			// TreeContextMenu
			// 
			this.TreeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.toolStripSeparator1,
            this.addTraceListenerToolStripMenuItem,
            this.changeTraceLevelToolStripMenuItem});
			this.TreeContextMenu.Name = "TreeContextMenu";
			this.TreeContextMenu.Size = new System.Drawing.Size(181, 76);
			// 
			// refreshToolStripMenuItem
			// 
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			this.refreshToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.refreshToolStripMenuItem.Text = "Refresh Tree";
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshMenuItem_Click);
			// 
			// addTraceListenerToolStripMenuItem
			// 
			this.addTraceListenerToolStripMenuItem.Name = "addTraceListenerToolStripMenuItem";
			this.addTraceListenerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.addTraceListenerToolStripMenuItem.Text = "&Add Trace Listener";
			this.addTraceListenerToolStripMenuItem.Click += new System.EventHandler(this.AddTraceMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
			// 
			// changeTraceLevelToolStripMenuItem
			// 
			this.changeTraceLevelToolStripMenuItem.Name = "changeTraceLevelToolStripMenuItem";
			this.changeTraceLevelToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.changeTraceLevelToolStripMenuItem.Text = "&Change Trace Level";
			this.changeTraceLevelToolStripMenuItem.Click += new System.EventHandler(this.ChangeLevelMenuItem_Click);
			// 
			// GridContextMenu
			// 
			this.GridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeTraceListenerToolStripMenuItem});
			this.GridContextMenu.Name = "GridContextMenu";
			this.GridContextMenu.Size = new System.Drawing.Size(196, 26);
			// 
			// removeTraceListenerToolStripMenuItem
			// 
			this.removeTraceListenerToolStripMenuItem.Name = "removeTraceListenerToolStripMenuItem";
			this.removeTraceListenerToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.removeTraceListenerToolStripMenuItem.Text = "&Remove Trace Listener";
			this.removeTraceListenerToolStripMenuItem.Click += new System.EventHandler(this.RemoveTraceMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1096, 677);
			this.Controls.Add(this.TraceListenersGrid);
			this.Controls.Add(this.InfoPanel);
			this.Controls.Add(this.FormSplitter);
			this.Controls.Add(this.TraceMonitorsTree);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.MainMenu;
			this.Name = "MainForm";
			this.Text = "System Diagnostics Manager";
			this.InfoPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.TraceListenersGrid)).EndInit();
			this.TreeContextMenu.ResumeLayout(false);
			this.GridContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

    private System.Windows.Forms.TreeView TraceMonitorsTree;
    private System.Windows.Forms.Splitter FormSplitter;
    private System.Windows.Forms.MainMenu MainMenu;
    private System.Windows.Forms.MenuItem ActionMenuItem;
    private System.Windows.Forms.MenuItem RefreshMenuItem;
    private System.Windows.Forms.MenuItem SeparatorMenuItem1;
    private System.Windows.Forms.MenuItem AddTraceMenuItem;
    private System.Windows.Forms.MenuItem RemoveTraceMenuItem;
    private System.Windows.Forms.MenuItem ChangeLevelMenuItem;
    private System.Windows.Forms.MenuItem SeparatorMenuItem2;
    private System.Windows.Forms.MenuItem ExitMenuItem;
    private System.Windows.Forms.Panel InfoPanel;
    private System.Windows.Forms.DataGrid TraceListenersGrid;
    private System.Windows.Forms.Label ThreadInfoLabel;
    private System.Windows.Forms.Label TraceLevelLabel;
#endregion
		private System.Windows.Forms.DataGridTableStyle dataGridTableStyle1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn1;
		private System.Windows.Forms.DataGridTextBoxColumn dataGridTextBoxColumn2;
		private System.Windows.Forms.ContextMenuStrip TreeContextMenu;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addTraceListenerToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem changeTraceLevelToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip GridContextMenu;
		private System.Windows.Forms.ToolStripMenuItem removeTraceListenerToolStripMenuItem;
	}
}