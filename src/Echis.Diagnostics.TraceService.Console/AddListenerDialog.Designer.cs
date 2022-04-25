namespace System.Diagnostics.LoggerService
{
	partial class AddListenerDialog
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
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			CboAssembly = new System.Windows.Forms.ComboBox();
			LblAssembly = new System.Windows.Forms.Label();
			LblClass = new System.Windows.Forms.Label();
			GrdParameters = new System.Windows.Forms.DataGrid();
			CboClass = new System.Windows.Forms.ComboBox();
			BtnOk = new System.Windows.Forms.Button();
			BtnCancel = new System.Windows.Forms.Button();
			LblName = new System.Windows.Forms.Label();
			TxtName = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(GrdParameters)).BeginInit();
			SuspendLayout();
			// 
			// CboAssembly
			// 
			CboAssembly.Location = new System.Drawing.Point(136, 24);
			CboAssembly.Name = "CboAssembly";
			CboAssembly.Size = new System.Drawing.Size(312, 21);
			CboAssembly.TabIndex = 2;
			CboAssembly.SelectedIndexChanged += new System.EventHandler(CboAssembly_SelectedIndexChanged);
			// 
			// LblAssembly
			// 
			LblAssembly.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			LblAssembly.Location = new System.Drawing.Point(16, 24);
			LblAssembly.Name = "LblAssembly";
			LblAssembly.Size = new System.Drawing.Size(112, 21);
			LblAssembly.TabIndex = 1;
			LblAssembly.Text = "Assembly Name:";
			LblAssembly.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LblClass
			// 
			LblClass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			LblClass.Location = new System.Drawing.Point(32, 56);
			LblClass.Name = "LblClass";
			LblClass.Size = new System.Drawing.Size(96, 21);
			LblClass.TabIndex = 3;
			LblClass.Text = "Class Name:";
			LblClass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// GrdParameters
			// 
			GrdParameters.DataMember = "";
			GrdParameters.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			GrdParameters.Location = new System.Drawing.Point(16, 128);
			GrdParameters.Name = "GrdParameters";
			GrdParameters.Size = new System.Drawing.Size(432, 248);
			GrdParameters.TabIndex = 7;
			// 
			// CboClass
			// 
			CboClass.Location = new System.Drawing.Point(136, 56);
			CboClass.Name = "CboClass";
			CboClass.Size = new System.Drawing.Size(312, 21);
			CboClass.TabIndex = 4;
			CboClass.SelectedIndexChanged += new System.EventHandler(CboClass_SelectedIndexChanged);
			// 
			// BtnOk
			// 
			BtnOk.Location = new System.Drawing.Point(152, 392);
			BtnOk.Name = "BtnOk";
			BtnOk.Size = new System.Drawing.Size(75, 32);
			BtnOk.TabIndex = 8;
			BtnOk.Text = "&Ok";
			BtnOk.Click += new System.EventHandler(BtnOk_Click);
			// 
			// BtnCancel
			// 
			BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			BtnCancel.Location = new System.Drawing.Point(240, 392);
			BtnCancel.Name = "BtnCancel";
			BtnCancel.Size = new System.Drawing.Size(75, 32);
			BtnCancel.TabIndex = 9;
			BtnCancel.Text = "&Cancel";
			BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
			// 
			// LblName
			// 
			LblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			LblName.Location = new System.Drawing.Point(24, 88);
			LblName.Name = "LblName";
			LblName.Size = new System.Drawing.Size(104, 21);
			LblName.TabIndex = 5;
			LblName.Text = "Listener Name:";
			LblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// TxtName
			// 
			TxtName.Location = new System.Drawing.Point(136, 88);
			TxtName.Name = "TxtName";
			TxtName.Size = new System.Drawing.Size(312, 20);
			TxtName.TabIndex = 6;
			TxtName.Text = "";
			// 
			// AddListenerDialog
			// 
			AcceptButton = BtnOk;
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			CancelButton = BtnCancel;
			ClientSize = new System.Drawing.Size(466, 442);
			ControlBox = false;
			Controls.Add(TxtName);
			Controls.Add(LblName);
			Controls.Add(BtnCancel);
			Controls.Add(BtnOk);
			Controls.Add(CboClass);
			Controls.Add(GrdParameters);
			Controls.Add(LblClass);
			Controls.Add(LblAssembly);
			Controls.Add(CboAssembly);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			Name = "AddListenerDialog";
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			((System.ComponentModel.ISupportInitialize)(GrdParameters)).EndInit();
			ResumeLayout(false); 
		}

    private System.Windows.Forms.ComboBox CboAssembly;
    private System.Windows.Forms.ComboBox CboClass;
    private System.Windows.Forms.Label LblAssembly;
    private System.Windows.Forms.Label LblClass;
    private System.Windows.Forms.Label LblName;
    private System.Windows.Forms.Button BtnOk;
    private System.Windows.Forms.Button BtnCancel;
    private System.Windows.Forms.TextBox TxtName;
    private System.Windows.Forms.DataGrid GrdParameters;
		#endregion
	}
}