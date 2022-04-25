using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace System.Diagnostics.LoggerService
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
			Justification = "If an exception bubbles up to this point just allow the application to exit.")]
		static void Main()
		{
			try
			{
				Application.Run(new MainForm());
			}
			catch { }
		}
	}
}
