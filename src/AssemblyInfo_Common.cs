using System;
using System.Reflection;
using System.Resources;

[assembly: AssemblyCompany("Echis Software")]
[assembly: AssemblyProduct("Echis Application Framework")]
[assembly: AssemblyCopyright("Copyright © Echis Software 2008-2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: NeutralResourcesLanguage("en-US")]


#if DEBUG
  [assembly: AssemblyConfiguration("Debug")]
#else
  [assembly: AssemblyConfiguration("Release")]
#endif

#if NON_CLS
[assembly: CLSCompliant(false)]
#else
[assembly: CLSCompliant(true)]
#endif