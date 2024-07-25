using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using AVI = Custom.Database.AssemblyVersionInfo;

// In SDK-style projects such as this one, several assembly attributes that were historically
// defined in this file are now automatically added during build and populated with
// values defined in project properties. For details of which attributes are included
// and how to customise this process see: https://aka.ms/assembly-info-properties

[assembly: AssemblyCompany(AVI.COMPANY)]
[assembly: AssemblyProduct(AVI.PRODUCT)]
[assembly: AssemblyCopyright(AVI.COPYRIGHT)]
[assembly: AssemblyVersion(AVI.VERSION)]
[assembly: AssemblyFileVersion(AVI.FILE_VERSION)]
[assembly: NeutralResourcesLanguage(AVI.NEUTRAL_RESOURCES_LANGUAGE)]
[assembly: ComVisible(false)]
