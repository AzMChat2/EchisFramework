using System.Reflection;

#if DEBUG
	[assembly: AssemblyVersion("4.0.00.00000")]
	[assembly: AssemblyFileVersion("4.0.00.00000")]
#else
	/* Build # was generated using the format QYYNN where:
	 * Q = The Quarter (1=Jan-Mar, 2=Apr-Jun, 3=Jul-Sep, 4=Oct-Dec)
	 * YY = The last 2 digits of the current year (11=2011)
	 * NN = The sequential build number, reset to zero for each version.
	 */
	[assembly: AssemblyVersion("4.0.00.11250")]
	[assembly: AssemblyFileVersion("4.0.00.11250")]
#endif

[assembly: AssemblyInformationalVersion("4.0.0")]


