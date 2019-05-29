[assembly: System.CLSCompliantAttribute(true)]
[assembly: System.Reflection.AssemblyDefaultAliasAttribute("System.Xaml.dll")]
[assembly: System.Reflection.AssemblyDelaySignAttribute(true)]
[assembly: System.Reflection.AssemblyKeyFileAttribute("f:\\dd\\tools\\devdiv\\EcmaPublicKey.snk")]
[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Resources.SatelliteContractVersionAttribute("4.0.0.0")]
[assembly: System.Runtime.CompilerServices.DependencyAttribute("mscorlib,", System.Runtime.CompilerServices.LoadHint.Always)]
[assembly: System.Runtime.CompilerServices.DependencyAttribute("System,", System.Runtime.CompilerServices.LoadHint.Always)]
[assembly: System.Runtime.CompilerServices.DependencyAttribute("System.Xml,", System.Runtime.CompilerServices.LoadHint.Sometimes)]
[assembly: System.Runtime.InteropServices.ComVisibleAttribute(false)]
[assembly: System.Runtime.InteropServices.DefaultDllImportSearchPathsAttribute(System.Runtime.InteropServices.DllImportSearchPath.System32 | System.Runtime.InteropServices.DllImportSearchPath.AssemblyDirectory | System.Runtime.InteropServices.DllImportSearchPath.LegacyBehavior)]
[assembly: System.Security.AllowPartiallyTrustedCallersAttribute()]
[assembly: System.Security.SecurityRulesAttribute(System.Security.SecurityRuleSet.Level2, SkipVerificationInFullTrust=true)]
[assembly: System.Windows.Markup.XmlnsDefinitionAttribute("http://schemas.microsoft.com/winfx/2006/xaml", "System.Windows.Markup")]
namespace System.Windows.Markup
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=true, Inherited=true)]
    [System.ObsoleteAttribute("This is not used by the XAML parser. Please look at XamlSetMarkupExtensionAttribu" +
        "te.")]
    public class AcceptedMarkupExtensionExpressionTypeAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Method | System.AttributeTargets.Property | System.AttributeTargets.All, Inherited=true)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class AmbientAttribute : System.Attribute { }
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("PresentationFramework, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856a" +
        "d364e35")]
    [System.Windows.Markup.ContentPropertyAttribute("Items")]
    [System.Windows.Markup.MarkupExtensionReturnTypeAttribute(typeof(System.Array))]
    public class ArrayExtension : System.Windows.Markup.MarkupExtension { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Property | System.AttributeTargets.All, AllowMultiple=false, Inherited=false)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class ConstructorArgumentAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class ContentPropertyAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=true, Inherited=true)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class ContentWrapperAttribute : System.Attribute { }
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public class DateTimeValueSerializer : System.Windows.Markup.ValueSerializer { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Method | System.AttributeTargets.Property | System.AttributeTargets.All, AllowMultiple=true)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class DependsOnAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class DictionaryKeyPropertyAttribute : System.Attribute { }
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public interface IComponentConnector { }
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public interface INameScope { }
    public interface INameScopeDictionary : System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<string, object>>, System.Collections.Generic.IDictionary<string, object>, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object>>, System.Collections.IEnumerable, System.Windows.Markup.INameScope { }
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("PresentationFramework, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856a" +
        "d364e35")]
    public interface IProvideValueTarget { }
    public interface IQueryAmbient { }
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("PresentationCore, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e" +
        "35")]
    public interface IUriContext { }
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public interface IValueSerializerContext : System.ComponentModel.ITypeDescriptorContext, System.IServiceProvider { }
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public interface IXamlTypeResolver { }
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public abstract class MarkupExtension { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Property | System.AttributeTargets.All, AllowMultiple=true, Inherited=false)]
    public sealed class MarkupExtensionBracketCharactersAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    public sealed class MarkupExtensionReturnTypeAttribute : System.Attribute { }
    public abstract class MemberDefinition { }
    public class NameReferenceConverter : System.ComponentModel.TypeConverter { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class NameScopePropertyAttribute : System.Attribute { }
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("PresentationFramework, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856a" +
        "d364e35")]
    [System.Windows.Markup.MarkupExtensionReturnTypeAttribute(typeof(object))]
    public class NullExtension : System.Windows.Markup.MarkupExtension { }
    public class PropertyDefinition : System.Windows.Markup.MemberDefinition { }
    [System.Windows.Markup.ContentPropertyAttribute("Name")]
    public class Reference : System.Windows.Markup.MarkupExtension { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.All)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class RootNamespaceAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class RuntimeNamePropertyAttribute : System.Attribute { }
    [System.ComponentModel.TypeConverterAttribute(typeof(System.Windows.Markup.StaticExtensionConverter))]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("PresentationFramework, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856a" +
        "d364e35")]
    [System.Windows.Markup.MarkupExtensionReturnTypeAttribute(typeof(object))]
    public class StaticExtension : System.Windows.Markup.MarkupExtension { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class TrimSurroundingWhitespaceAttribute : System.Attribute { }
    [System.ComponentModel.TypeConverterAttribute(typeof(System.Windows.Markup.TypeExtensionConverter))]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("PresentationFramework, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856a" +
        "d364e35")]
    [System.Windows.Markup.MarkupExtensionReturnTypeAttribute(typeof(System.Type))]
    public class TypeExtension : System.Windows.Markup.MarkupExtension { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=false)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class UidPropertyAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    public sealed class UsableDuringInitializationAttribute : System.Attribute { }
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public abstract class ValueSerializer { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class WhitespaceSignificantCollectionAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Property | System.AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    public sealed class XamlDeferLoadAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    public sealed class XamlSetMarkupExtensionAttribute : System.Attribute { }
    public class XamlSetMarkupExtensionEventArgs : System.Windows.Markup.XamlSetValueEventArgs { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=false, Inherited=true)]
    public sealed class XamlSetTypeConverterAttribute : System.Attribute { }
    public class XamlSetTypeConverterEventArgs : System.Windows.Markup.XamlSetValueEventArgs { }
    public class XamlSetValueEventArgs : System.EventArgs { }
    [System.Windows.Markup.ContentPropertyAttribute("Text")]
    public sealed class XData { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.All, AllowMultiple=false)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class XmlLangPropertyAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.All, AllowMultiple=true)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class XmlnsCompatibleWithAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.All, AllowMultiple=true)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class XmlnsDefinitionAttribute : System.Attribute { }
    [System.AttributeUsageAttribute(System.AttributeTargets.Assembly | System.AttributeTargets.All, AllowMultiple=true)]
    [System.Runtime.CompilerServices.TypeForwardedFromAttribute("WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
    public sealed class XmlnsPrefixAttribute : System.Attribute { }
}
namespace System.Xaml
{
    public class AmbientPropertyValue { }
    public class AttachableMemberIdentifier : System.IEquatable<System.Xaml.AttachableMemberIdentifier> { }
    public class static AttachablePropertyServices { }
    public interface IAmbientProvider { }
    public interface IAttachedPropertyStore { }
    public interface IDestinationTypeProvider { }
    public interface INamespacePrefixLookup { }
    public interface IRootObjectProvider { }
    public interface IXamlIndexingReader { }
    public interface IXamlLineInfo { }
    public interface IXamlLineInfoConsumer { }
    public interface IXamlNameProvider { }
    public interface IXamlNameResolver { }
    public interface IXamlNamespaceResolver { }
    public interface IXamlObjectWriterFactory { }
    public interface IXamlSchemaContextProvider { }
    [System.Diagnostics.DebuggerDisplayAttribute("Prefix={Prefix} Namespace={Namespace}")]
    public class NamespaceDeclaration { }
    public class XamlBackgroundReader : System.Xaml.XamlReader, System.Xaml.IXamlLineInfo { }
    public abstract class XamlDeferringLoader { }
    public class XamlDirective : System.Xaml.XamlMember { }
    public class XamlDuplicateMemberException : System.Xaml.XamlException { }
    public class XamlException : System.Exception { }
    public class XamlInternalException : System.Xaml.XamlException { }
    public class static XamlLanguage
    {
        public const string Xaml2006Namespace = "http://schemas.microsoft.com/winfx/2006/xaml";
        public const string Xml1998Namespace = "http://www.w3.org/XML/1998/namespace";
    }
    public class XamlMember : System.IEquatable<System.Xaml.XamlMember> { }
    public class XamlNodeList { }
    public class XamlNodeQueue { }
    public enum XamlNodeType : byte
    {
        None = 0,
        StartObject = 1,
        GetObject = 2,
        EndObject = 3,
        StartMember = 4,
        EndMember = 5,
        Value = 6,
        NamespaceDeclaration = 7,
    }
    public class XamlObjectEventArgs : System.EventArgs { }
    public class XamlObjectReader : System.Xaml.XamlReader { }
    public class XamlObjectReaderException : System.Xaml.XamlException { }
    public class XamlObjectReaderSettings : System.Xaml.XamlReaderSettings { }
    public class XamlObjectWriter : System.Xaml.XamlWriter, System.Xaml.IXamlLineInfoConsumer { }
    public class XamlObjectWriterException : System.Xaml.XamlException { }
    public class XamlObjectWriterSettings : System.Xaml.XamlWriterSettings { }
    public class XamlParseException : System.Xaml.XamlException { }
    public abstract class XamlReader : System.IDisposable { }
    public class XamlReaderSettings { }
    public class XamlSchemaContext { }
    public class XamlSchemaContextSettings { }
    public class XamlSchemaException : System.Xaml.XamlException { }
    public class static XamlServices { }
    public class XamlType : System.IEquatable<System.Xaml.XamlType> { }
    public abstract class XamlWriter : System.IDisposable { }
    public class XamlWriterSettings { }
    public class XamlXmlReader : System.Xaml.XamlReader, System.Xaml.IXamlLineInfo { }
    public class XamlXmlReaderSettings : System.Xaml.XamlReaderSettings { }
    public class XamlXmlWriter : System.Xaml.XamlWriter { }
    public class XamlXmlWriterException : System.Xaml.XamlException { }
    public class XamlXmlWriterSettings : System.Xaml.XamlWriterSettings { }
}
namespace System.Xaml.Permissions
{
    public class XamlAccessLevel { }
    public sealed class XamlLoadPermission : System.Security.CodeAccessPermission, System.Security.Permissions.IUnrestrictedPermission { }
}
namespace System.Xaml.Schema
{
    [System.FlagsAttribute()]
    public enum AllowedMemberLocations
    {
        None = 0,
        Attribute = 1,
        MemberElement = 2,
        Any = 3,
    }
    public enum ShouldSerializeResult
    {
        Default = 0,
        True = 1,
        False = 2,
    }
    public enum XamlCollectionKind : byte
    {
        None = 0,
        Collection = 1,
        Dictionary = 2,
        Array = 3,
    }
    public class XamlMemberInvoker { }
    public class XamlTypeInvoker { }
    [System.Diagnostics.DebuggerDisplayAttribute("{{{Namespace}}}{Name}{TypeArgStringForDebugger}")]
    public class XamlTypeName { }
    public class XamlTypeTypeConverter : System.ComponentModel.TypeConverter { }
    public class XamlValueConverter<TConverterBase> : System.IEquatable<System.Xaml.Schema.XamlValueConverter<TConverterBase>>
        where TConverterBase :  class { }
}