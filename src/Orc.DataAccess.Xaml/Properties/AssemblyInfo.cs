using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Markup;

// All other assembly info is defined in SolutionAssemblyInfo.cs

[assembly: AssemblyTitle("Orc.DataAccess.Xaml")]
[assembly: AssemblyProduct("Orc.DataAccess.Xaml")]
[assembly: AssemblyDescription("Orc.DataAccess.Xaml library")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: XmlnsPrefix("http://schemas.wildgums.com/orc/dataaccess", "orcdataaccess")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/dataaccess", "Orc.DataAccess")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/dataaccess", "Orc.DataAccess.Behaviors")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/dataaccess", "Orc.DataAccess.Controls")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/dataaccess", "Orc.DataAccess.Converters")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/dataaccess", "Orc.DataAccess.Fonts")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/dataaccess", "Orc.DataAccess.Markup")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/dataaccess", "Orc.DataAccess.Views")]
//[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/dataaccess", "Orc.DataAccess.Windows")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page, 
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page, 
                                              // app, or any theme specific resource dictionaries)
    )]
