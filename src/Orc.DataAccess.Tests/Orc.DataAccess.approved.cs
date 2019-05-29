[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.6", FrameworkDisplayName=".NET Framework 4.6")]
public class static ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.DataAccess
{
    public class CsvReader : Orc.DataAccess.ReaderBase
    {
        public CsvReader(string source, Orc.Csv.ICsvReaderService csvReaderService, Orc.FileSystem.IFileService fileService) { }
        public override string[] FieldHeaders { get; }
        public override object this[int index] { get; }
        public override object this[string name] { get; }
        public override int TotalRecordCount { get; }
        public override void Dispose() { }
        public override bool Read() { }
    }
    public abstract class DataSourceBase : Catel.Data.ModelBase
    {
        protected readonly System.Collections.Generic.Dictionary<string, string> Properties;
        public static readonly Catel.Data.PropertyData ValidationContextProperty;
        protected DataSourceBase() { }
        protected DataSourceBase(string location) { }
        public Catel.Data.IValidationContext ValidationContext { get; }
        public System.Collections.Generic.IReadOnlyDictionary<string, string> AsDictionary() { }
        public virtual string GetLocation() { }
        protected override void OnPropertyChanged(Catel.Data.AdvancedPropertyChangedEventArgs args) { }
        public void SetProperty(string propertyName, string propertyValueStr) { }
        public override string ToString() { }
        protected virtual bool TryConvertFromString(string propertyName, string propertyValueStr, out object propertyValue) { }
        public virtual void Validate() { }
    }
    public class static DataSourceBaseExtensions
    {
        public static bool IsValid(this Orc.DataAccess.DataSourceBase dataSource) { }
    }
    public class DataSourceParameter
    {
        public DataSourceParameter() { }
        public string Name { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
    }
    public class DataSourceParameters : Catel.Data.SavableModelBase<Orc.DataAccess.DataSourceParameters>
    {
        public static readonly Catel.Data.PropertyData ParametersProperty;
        public DataSourceParameters() { }
        public System.Collections.Generic.List<Orc.DataAccess.DataSourceParameter> Parameters { get; set; }
    }
    public class static DataSourceParametersExtensions
    {
        public static bool IsEmpty(this Orc.DataAccess.DataSourceParameters databaseQueryParameters) { }
        public static bool IsSameAs(this Orc.DataAccess.DataSourceParameters databaseQueryParameters, Orc.DataAccess.DataSourceParameters other) { }
        public static string ToArgsNamesString(this Orc.DataAccess.DataSourceParameters queryParameters, string argsPrefix = "") { }
        public static string ToArgsValueString(this Orc.DataAccess.DataSourceParameters queryParameters) { }
    }
    public class static DbCommandExtensions
    {
        public static System.Data.Common.DbCommand AddParameter(this System.Data.Common.DbCommand dbCommand, Orc.DataAccess.DataSourceParameter parameter) { }
        public static System.Data.Common.DbCommand AddParameter(this System.Data.Common.DbCommand dbCommand, string name, object value) { }
        public static System.Data.Common.DbCommand AddParameters(this System.Data.Common.DbCommand dbCommand, Orc.DataAccess.DataSourceParameters parameters) { }
        public static long GetRecordsCount(this System.Data.Common.DbCommand command) { }
    }
    public class static DbDataReaderExtensions
    {
        public static string[] GetHeaders(this System.Data.Common.DbDataReader reader) { }
        public static System.Collections.Generic.List<Orc.DataAccess.RecordTable> ReadAll(this System.Data.Common.DbDataReader reader) { }
    }
    public class static ICollectionExtensions
    {
        public static TTarget FindTypeOrCreateNew<T, TTarget>(this System.Collections.Generic.ICollection<T> collection, System.Func<TTarget> func)
            where TTarget : T { }
    }
    public interface IReader : System.IDisposable
    {
        System.Globalization.CultureInfo Culture { get; set; }
        int FetchCount { get; set; }
        string[] FieldHeaders { get; }
        object this[int index] { get; }
        object this[string name] { get; }
        int Offset { get; set; }
        int TotalRecordCount { get; }
        Catel.Data.IValidationContext ValidationContext { get; }
        System.Threading.Tasks.Task<bool> NextResultAsync();
        bool Read();
    }
    public class static IReaderExtensions
    {
        public static System.Collections.Generic.List<Orc.DataAccess.RecordTable> ReadAll(this Orc.DataAccess.IReader reader) { }
    }
    public class static IValidationContextExtensions
    {
        public static void AddValidationError(this Catel.Data.IValidationContext validationContext, string message, string tag = null) { }
    }
    public class static KeyValueStringParser
    {
        public const char KeyValueDelimiter = '=';
        public const char KeyValuePairsDelimiter = ',';
        public static string FormatToKeyValueString(System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> keyPairs) { }
        public static string GetValue(string source, string key) { }
        public static System.Collections.Generic.Dictionary<string, string> Parse(string source) { }
        public static string SetValue(string source, string key, string value) { }
    }
    public abstract class ReaderBase : Orc.DataAccess.IReader, System.IDisposable
    {
        protected readonly string Source;
        protected ReaderBase(string source, int offset = 0, int fetchCount = 0) { }
        public System.Globalization.CultureInfo Culture { get; set; }
        public int FetchCount { get; set; }
        public abstract string[] FieldHeaders { get; }
        public abstract object this[int index] { get; }
        public abstract object this[string name] { get; }
        public int Offset { get; set; }
        public abstract int TotalRecordCount { get; }
        public Catel.Data.IValidationContext ValidationContext { get; }
        protected void AddValidationError(string message) { }
        public abstract void Dispose();
        public virtual System.Threading.Tasks.Task<bool> NextResultAsync() { }
        public abstract bool Read();
    }
    public class Record : System.Collections.Generic.Dictionary<string, object>
    {
        public Record() { }
    }
    public class RecordTable : System.Collections.Generic.List<Orc.DataAccess.Record>
    {
        public RecordTable() { }
        public string[] Headers { get; set; }
    }
    public class static RecordTableExtensions
    {
        public static bool HasHeaders(this Orc.DataAccess.RecordTable table) { }
    }
    public class static StringExtensions
    {
        public const string InitVector = "tu89geji340t89u2";
        public static string Decrypt(this string cipherText) { }
        public static string Encrypt(this string plainText) { }
    }
    public class static TypeExtensions
    {
        public static System.Collections.Generic.IList<System.Type> GetAllAssignableFrom(this System.Type type) { }
    }
}
namespace Orc.DataAccess.Database
{
    [System.AttributeUsageAttribute(System.AttributeTargets.Class | System.AttributeTargets.Struct | System.AttributeTargets.All)]
    public class ConnectToProviderAttribute : System.Attribute
    {
        public ConnectToProviderAttribute(string providerInvariantName) { }
        public string ProviderInvariantName { get; }
    }
    public class DatabaseSource : Orc.DataAccess.DataSourceBase
    {
        public static readonly Catel.Data.PropertyData ConnectionStringProperty;
        public static readonly Catel.Data.PropertyData ProviderNameProperty;
        public static readonly Catel.Data.PropertyData TableProperty;
        public static readonly Catel.Data.PropertyData TableTypeProperty;
        public DatabaseSource() { }
        public DatabaseSource(string location) { }
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        public string ConnectionString { get; set; }
        [System.ComponentModel.DataAnnotations.RequiredAttribute()]
        public string ProviderName { get; set; }
        public string Table { get; set; }
        public Orc.DataAccess.Database.TableType TableType { get; set; }
        protected override bool TryConvertFromString(string propertyName, string propertyValueStr, out object propertyValue) { }
    }
    public class static DatabaseSourceExtensions
    {
        public static System.Data.Common.DbConnection CreateConnection(this Orc.DataAccess.Database.DatabaseSource databaseSource) { }
        public static Orc.DataAccess.Database.DbSourceGatewayBase CreateGateway(this Orc.DataAccess.Database.DatabaseSource databaseSource) { }
        public static System.Collections.Generic.IList<Orc.DataAccess.Database.DbObject> GetObjectsOfType(this Orc.DataAccess.Database.DatabaseSource databaseSource, Orc.DataAccess.Database.TableType tableType) { }
        public static Orc.DataAccess.Database.DbProvider GetProvider(this Orc.DataAccess.Database.DatabaseSource databaseSource) { }
    }
    public class DbConnectionString : Catel.Data.ModelBase
    {
        public static readonly Catel.Data.PropertyData PropertiesProperty;
        public DbConnectionString(System.Data.Common.DbConnectionStringBuilder connectionStringBuilder, Orc.DataAccess.Database.DbProviderInfo dbProvider) { }
        public System.Data.Common.DbConnectionStringBuilder ConnectionStringBuilder { get; }
        public Orc.DataAccess.Database.DbProviderInfo DbProvider { get; }
        public System.Collections.Generic.IReadOnlyDictionary<string, Orc.DataAccess.Database.DbConnectionStringProperty> Properties { get; }
        public virtual string ToDisplayString() { }
        public override string ToString() { }
    }
    public class static DbConnectionStringExtensions
    {
        public static Orc.DataAccess.Database.DbConnectionStringProperty TryGetProperty(this Orc.DataAccess.Database.DbConnectionString connectionString, string propertyName) { }
    }
    public class DbConnectionStringProperty : Catel.Data.ObservableObject
    {
        public DbConnectionStringProperty(bool isSensitive, System.Data.Common.DbConnectionStringBuilder dbConnectionStringBuilder, System.ComponentModel.PropertyDescriptor propertyDescriptor) { }
        public bool IsSensitive { get; }
        public string Name { get; }
        public object Value { get; set; }
    }
    public class DbObject
    {
        public DbObject(Orc.DataAccess.Database.TableType type) { }
        public string Name { get; set; }
        public Orc.DataAccess.Database.TableType Type { get; }
    }
    public class DbProvider
    {
        public DbProvider(Orc.DataAccess.Database.DbProviderInfo info) { }
        public DbProvider(string providerInvariantName) { }
        public virtual System.Type ConnectionType { get; }
        protected System.Data.Common.DbProviderFactory DbProviderFactory { get; }
        public string Dialect { get; }
        public virtual Orc.DataAccess.Database.DbProviderInfo Info { get; }
        public string ProviderInvariantName { get; }
        public virtual System.Data.Common.DbConnection CreateConnection() { }
        public virtual Orc.DataAccess.Database.DbConnectionString CreateConnectionString(string connectionString = null) { }
        protected virtual Orc.DataAccess.Database.DbProviderInfo GetInfo() { }
        public static Orc.DataAccess.Database.DbProvider GetRegisteredProvider(string invariantName) { }
        public static System.Collections.Generic.IReadOnlyDictionary<string, Orc.DataAccess.Database.DbProvider> GetRegisteredProviders() { }
        public static void RegisterCustomProvider(Orc.DataAccess.Database.DbProvider provider) { }
        public static void RegisterProvider(Orc.DataAccess.Database.DbProviderInfo providerInfo) { }
        public static void UnregisterProvider(Orc.DataAccess.Database.DbProviderInfo providerInfo) { }
    }
    public class static DbProviderExtensions
    {
        public static void ConnectType<TBaseType>(this Orc.DataAccess.Database.DbProvider provider, System.Type type) { }
        public static T CreateConnectedInstance<T>(this Orc.DataAccess.Database.DbProvider dbProvider, params object[] parameters) { }
        public static System.Data.Common.DbConnection CreateConnection(this Orc.DataAccess.Database.DbProvider dbProvider, Orc.DataAccess.Database.DatabaseSource databaseSource) { }
        public static System.Data.Common.DbConnection CreateConnection(this Orc.DataAccess.Database.DbProvider dbProvider, string connectionString) { }
        public static Orc.DataAccess.Database.DbSourceGatewayBase CreateDbSourceGateway(this Orc.DataAccess.Database.DbProvider dbProvider, Orc.DataAccess.Database.DatabaseSource databaseSource) { }
        public static System.Collections.Generic.IList<System.Type> GetConnectedTypes<T>(this Orc.DataAccess.Database.DbProvider provider) { }
        public static T GetOrCreateConnectedInstance<T>(this Orc.DataAccess.Database.DbProvider dbProvider) { }
    }
    public class DbProviderFactoryRepository
    {
        public DbProviderFactoryRepository() { }
        public void Add(Orc.DataAccess.Database.DbProviderInfo providerInfo) { }
        public void Remove(Orc.DataAccess.Database.DbProviderInfo providerInfo) { }
    }
    public class DbProviderInfo
    {
        public DbProviderInfo() { }
        public string AssemblyQualifiedName { get; set; }
        public string Description { get; set; }
        public string InvariantName { get; set; }
        public string Name { get; set; }
        protected bool Equals(Orc.DataAccess.Database.DbProviderInfo other) { }
        public override bool Equals(object obj) { }
        public override int GetHashCode() { }
    }
    public abstract class DbSourceGatewayBase : System.IDisposable
    {
        public DbSourceGatewayBase(Orc.DataAccess.Database.DatabaseSource source) { }
        public virtual System.Data.Common.DbConnection Connection { get; }
        public virtual Orc.DataAccess.Database.DbProvider Provider { get; }
        public Orc.DataAccess.Database.DatabaseSource Source { get; }
        public void Close() { }
        public void Dispose() { }
        public abstract long GetCount(Orc.DataAccess.DataSourceParameters queryParameters = null);
        public abstract System.Collections.Generic.IList<Orc.DataAccess.Database.DbObject> GetObjects();
        protected System.Data.Common.DbConnection GetOpenedConnection() { }
        public abstract Orc.DataAccess.DataSourceParameters GetQueryParameters();
        public abstract System.Data.Common.DbDataReader GetRecords(Orc.DataAccess.DataSourceParameters queryParameters = null, int offset = 0, int fetchCount = -1);
    }
    [Orc.DataAccess.Database.ConnectToProviderAttribute("FirebirdSql.Data.FirebirdClient")]
    public class FirebirdSourceGateway : Orc.DataAccess.Database.SqlDbSourceGatewayBase
    {
        public FirebirdSourceGateway(Orc.DataAccess.Database.DatabaseSource source) { }
        protected override System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<Orc.DataAccess.DataSourceParameters>> DataSourceParametersFactory { get; }
        protected override System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<System.Data.Common.DbConnection, System.Data.Common.DbCommand>> GetObjectListCommandsFactory { get; }
        protected override System.Data.Common.DbCommand CreateGetTableRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled) { }
        protected override System.Data.Common.DbCommand CreateTableCountCommand(System.Data.Common.DbConnection connection) { }
    }
    [Orc.DataAccess.Database.ConnectToProviderAttribute("System.Data.SqlClient")]
    public class MsSqlDbSourceGateway : Orc.DataAccess.Database.SqlDbSourceGatewayBase
    {
        public MsSqlDbSourceGateway(Orc.DataAccess.Database.DatabaseSource source) { }
        protected override System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<Orc.DataAccess.DataSourceParameters>> DataSourceParametersFactory { get; }
        protected override System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<System.Data.Common.DbConnection, System.Data.Common.DbCommand>> GetObjectListCommandsFactory { get; }
        protected override System.Data.Common.DbCommand CreateGetTableRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled) { }
        protected override System.Data.Common.DbCommand CreateTableCountCommand(System.Data.Common.DbConnection connection) { }
    }
    [Orc.DataAccess.Database.ConnectToProviderAttribute("MySql.Data.MySqlClient")]
    public class MySqlSourceGateway : Orc.DataAccess.Database.SqlDbSourceGatewayBase
    {
        public MySqlSourceGateway(Orc.DataAccess.Database.DatabaseSource source) { }
        protected override System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<Orc.DataAccess.DataSourceParameters>> DataSourceParametersFactory { get; }
        protected override System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<System.Data.Common.DbConnection, System.Data.Common.DbCommand>> GetObjectListCommandsFactory { get; }
        protected override System.Data.Common.DbCommand CreateGetFunctionRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount) { }
        protected override System.Data.Common.DbCommand CreateGetTableRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled) { }
        protected override System.Data.Common.DbCommand CreateTableCountCommand(System.Data.Common.DbConnection connection) { }
    }
    [Orc.DataAccess.Database.ConnectToProviderAttribute("Oracle.ManagedDataAccess.Client")]
    public class OracleSourceGateway : Orc.DataAccess.Database.SqlDbSourceGatewayBase
    {
        public OracleSourceGateway(Orc.DataAccess.Database.DatabaseSource source) { }
        protected override System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<Orc.DataAccess.DataSourceParameters>> DataSourceParametersFactory { get; }
        protected override System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<System.Data.Common.DbConnection, System.Data.Common.DbCommand>> GetObjectListCommandsFactory { get; }
        protected override System.Data.Common.DbCommand CreateGetFunctionRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount) { }
        protected override System.Data.Common.DbCommand CreateGetStoredProcedureRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount) { }
        protected override System.Data.Common.DbCommand CreateGetTableRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled) { }
        protected override System.Data.Common.DbCommand CreateTableCountCommand(System.Data.Common.DbConnection connection) { }
        public override Orc.DataAccess.DataSourceParameters GetQueryParameters() { }
    }
    [Orc.DataAccess.Database.ConnectToProviderAttribute("Npgsql")]
    public class PostgreSqlDbSourceGateway : Orc.DataAccess.Database.SqlDbSourceGatewayBase
    {
        public PostgreSqlDbSourceGateway(Orc.DataAccess.Database.DatabaseSource source) { }
        protected override System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<System.Data.Common.DbConnection, System.Data.Common.DbCommand>> GetObjectListCommandsFactory { get; }
        protected override System.Data.Common.DbCommand CreateGetStoredProcedureRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount) { }
        protected override System.Data.Common.DbCommand CreateGetTableRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled) { }
        protected override System.Data.Common.DbCommand CreateTableCountCommand(System.Data.Common.DbConnection connection) { }
        public override Orc.DataAccess.DataSourceParameters GetQueryParameters() { }
    }
    public abstract class SqlDbSourceGatewayBase : Orc.DataAccess.Database.DbSourceGatewayBase
    {
        protected SqlDbSourceGatewayBase(Orc.DataAccess.Database.DatabaseSource source) { }
        protected virtual System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<Orc.DataAccess.DataSourceParameters>> DataSourceParametersFactory { get; }
        protected virtual System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<System.Data.Common.DbConnection, System.Data.Common.DbCommand>> GetObjectListCommandsFactory { get; }
        protected virtual System.Data.Common.DbCommand CreateGetFunctionRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount) { }
        protected virtual System.Data.Common.DbCommand CreateGetStoredProcedureRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount) { }
        protected abstract System.Data.Common.DbCommand CreateGetTableRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled);
        protected virtual System.Data.Common.DbCommand CreateGetViewRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled) { }
        protected abstract System.Data.Common.DbCommand CreateTableCountCommand(System.Data.Common.DbConnection connection);
        protected virtual System.Data.Common.DbCommand CreateViewCountCommand(System.Data.Common.DbConnection connection) { }
        protected Orc.DataAccess.DataSourceParameters GetArgs(string query) { }
        public override long GetCount(Orc.DataAccess.DataSourceParameters queryParameters = null) { }
        public override System.Collections.Generic.IList<Orc.DataAccess.Database.DbObject> GetObjects() { }
        public override Orc.DataAccess.DataSourceParameters GetQueryParameters() { }
        public override System.Data.Common.DbDataReader GetRecords(Orc.DataAccess.DataSourceParameters queryParameters = null, int offset = 0, int fetchCount = -1) { }
        protected virtual System.Collections.Generic.IList<Orc.DataAccess.Database.DbObject> ReadAllDbObjects(System.Data.Common.DbCommand command) { }
    }
    [Orc.DataAccess.Database.ConnectToProviderAttribute("System.Data.SQLite")]
    public class SqLiteSourceGateway : Orc.DataAccess.Database.SqlDbSourceGatewayBase
    {
        public SqLiteSourceGateway(Orc.DataAccess.Database.DatabaseSource source) { }
        protected override System.Collections.Generic.Dictionary<Orc.DataAccess.Database.TableType, System.Func<System.Data.Common.DbConnection, System.Data.Common.DbCommand>> GetObjectListCommandsFactory { get; }
        protected override System.Data.Common.DbCommand CreateGetTableRecordsCommand(System.Data.Common.DbConnection connection, Orc.DataAccess.DataSourceParameters parameters, int offset, int fetchCount, bool isPagingEnabled) { }
        protected override System.Data.Common.DbCommand CreateTableCountCommand(System.Data.Common.DbConnection connection) { }
        public override Orc.DataAccess.DataSourceParameters GetQueryParameters() { }
    }
    public class SqlTableReader : Orc.DataAccess.ReaderBase
    {
        public SqlTableReader(string source, int offset = 0, int fetchCount = 0, Orc.DataAccess.DataSourceParameters parameters = null) { }
        public SqlTableReader(Orc.DataAccess.Database.DatabaseSource source, int offset = 0, int fetchCount = 0, Orc.DataAccess.DataSourceParameters parameters = null) { }
        public override string[] FieldHeaders { get; }
        public bool HasRows { get; }
        public override object this[int index] { get; }
        public override object this[string name] { get; }
        public Orc.DataAccess.DataSourceParameters QueryParameters { get; set; }
        public int ReadCount { get; }
        public int ResultIndex { get; }
        public override int TotalRecordCount { get; }
        public override void Dispose() { }
        public object GetValue(int index) { }
        public object GetValue(string name) { }
        public override System.Threading.Tasks.Task<bool> NextResultAsync() { }
        public override bool Read() { }
        public System.Threading.Tasks.Task<bool> ReadAsync() { }
    }
    public class static StringExtensions
    {
        public static string DecryptConnectionString(this string connectionString, string providerName) { }
        public static string EncryptConnectionString(this string connectionString, string providerName) { }
        public static string GetConnectionStringProperty(this string connectionString, string providerName, string propertyName) { }
    }
    public enum TableType
    {
        Table = 0,
        View = 1,
        StoredProcedure = 2,
        Function = 3,
        Sql = 4,
    }
}