namespace System.Diagnostics.LoggerService
{
	partial class TraceLevelDialog
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
			RbtnOff = new System.Windows.Forms.RadioButton();
			RbtnError = new System.Windows.Forms.RadioButton();
			RbtnWarning = new System.Windows.Forms.RadioButton();
			RbtnInformation = new System.Windows.Forms.RadioButton();
			RbtnVerbose = new System.Windows.Forms.RadioButton();
			LblSelect = new System.Windows.Forms.Label();
			BtnOk = new System.Windows.Forms.Button();
			BtnCancel = new System.Windows.Forms.Button();
			SuspendLayout();
			// 
			// RbtnOff
			// 
			RbtnOff.Location = new System.Drawing.Point(40, 40);
			RbtnOff.Name = "RbtnOff";
			RbtnOff.TabIndex = 0;
			RbtnOff.Text = "Off";
			// 
			// RbtnError
			// 
			RbtnError.Location = new System.Drawing.Point(40, 64);
			RbtnError.Name = "RbtnError";
			RbtnError.TabIndex = 1;
			RbtnError.Text = "Error";
			// 
			// RbtnWarning
			// 
			RbtnWarning.Location = new System.Drawing.Point(40, 88);
			RbtnWarning.Name = "RbtnWarning";
			RbtnWarning.TabIndex = 2;
			RbtnWarning.Text = "Warning";
			// 
			// RbtnInformation
			// 
			RbtnInformation.Location = new System.Drawing.Point(40, 112);
			RbtnInformation.Name = "RbtnInformation";
			RbtnInformation.TabIndex = 3;
			RbtnInformation.Text = "Information";
			// 
			// RbtnVerbose
			// 
			RbtnVerbose.Location = new System.Drawing.Point(40, 136);
			RbtnVerbose.Name = "RbtnVerbose";
			RbtnVerbose.TabIndex = 4;
			RbtnVerbose.Text = "Verbose";
			// 
			// LblSelect
			// 
			LblSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			LblSelect.Location = new System.Drawing.Point(32, 16);
			LblSelect.Name = "LblSelect";
			LblSelect.Size = new System.Drawing.Size(128, 23);
			LblSelect.TabIndex = 5;
			LblSelect.Text = "Select Trace Level";
			// 
			// BtnOk
			// 
			BtnOk.Location = new System.Drawing.Point(8, 176);
			BtnOk.Name = "BtnOk";
			BtnOk.Size = new System.Drawing.Size(75, 32);
			BtnOk.TabIndex = 6;
			BtnOk.Text = "&Ok";
			BtnOk.Click += new System.EventHandler(BtnOk_Click);
			// 
			// BtnCancel
			// 
			BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			BtnCancel.Location = new System.Drawing.Point(96, 176);
			BtnCancel.Name = "BtnCancel";
			BtnCancel.Size = new System.Drawing.Size(75, 32);
			BtnCancel.TabIndex = 7;
			BtnCancel.Text = "&Cancel";
			BtnCancel.Click += new System.EventHandler(BtnCancel_Click);
			// 
			// TraceLevelDialog
			// 
			AcceptButton = BtnOk;
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			CancelButton = BtnCancel;
			ClientSize = new System.Drawing.Size(184, 224);
			ControlBox = false;
			Controls.Add(BtnCancel);
			Controls.Add(BtnOk);
			Controls.Add(LblSelect);
			Controls.Add(RbtnVerbose);
			Controls.Add(RbtnInformation);
			Controls.Add(RbtnWarning);
			Controls.Add(RbtnError);
			Controls.Add(RbtnOff);
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "TraceLevelDialog";
			ShowInTaskbar = false;
			StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			ResumeLayout(false);

		}

    private System.Windows.Forms.RadioButton RbtnOff;
    private System.Windows.Forms.RadioButton RbtnError;
    private System.Windows.Forms.RadioButton RbtnWarning;
    private System.Windows.Forms.RadioButton RbtnInformation;
    private System.Windows.Forms.RadioButton RbtnVerbose;
    private System.Windows.Forms.Label LblSelect;
    private System.Windows.Forms.Button BtnOk;
    private System.Windows.Forms.Button BtnCancel;
		#endregion
	}
}