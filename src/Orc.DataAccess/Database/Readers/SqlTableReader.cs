﻿namespace Orc.DataAccess.Database;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Catel.Logging;
using DataAccess;

public class SqlTableReader : ReaderBase
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly DatabaseSource _databaseSource;

    private DbSourceGatewayBase? _gateway;
    private DbDataReader? _reader;

    private string[] _fieldHeaders = Array.Empty<string>();
    private int _totalRecordCount;
    private bool _isFieldHeadersInitialized;
    private bool _isInitialized;
    private bool _isTotalRecordCountInitialized;

    public SqlTableReader(string source, int offset = 0, int fetchCount = 0, DataSourceParameters? parameters = null)
        : this(new DatabaseSource(source), offset, fetchCount, parameters)
    {
    }

    public SqlTableReader(DatabaseSource source, int offset = 0, int fetchCount = 0, DataSourceParameters? parameters = null)
        : base(source.ToString(), offset, fetchCount)
    {
        ArgumentNullException.ThrowIfNull(source);

        _databaseSource = source;
        _totalRecordCount = 0;
        QueryParameters = parameters;
    }

    public override string[] FieldHeaders
    {
        get
        {
            if (_isFieldHeadersInitialized)
            {
                return _fieldHeaders;
            }

            TryInitialize();
            return _fieldHeaders;
        }
    }

    public override object this[int index] => GetValue(index);
    public override object this[string name] => GetValue(name);

    public override int TotalRecordCount
    {
        get
        {
            if (_isTotalRecordCountInitialized)
            {
                return _totalRecordCount;
            }

            if (!_isInitialized)
            {
                Initialize();
            }

            _totalRecordCount = QueryParameters is null || _gateway is null ? 0 : (int)_gateway.GetCount(QueryParameters);

            _isTotalRecordCountInitialized = true;
            return _totalRecordCount;
        }
    }

    public int ReadCount { get; private set; }
    public int ResultIndex { get; private set; }
    public DataSourceParameters? QueryParameters { get; set; }
    public bool HasRows => _reader?.HasRows ?? false;

    public async Task<bool> ReadAsync()
    {
        try
        {
            TryInitialize();
            if (_reader is null)
            {
                return false;
            }

            var readResult = await _reader.ReadAsync();
            if (readResult)
            {
                ReadCount++;
            }

            return readResult;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to read source '{Source}'");
            AddValidationError($"Failed to read data: '{ex.Message}'");
            return false;
        }
    }

    public override bool Read()
    {
        try
        {
            TryInitialize();

            if (_reader is null)
            {
                return false;
            }

            var readResult = _reader.Read();
            if (readResult)
            {
                ReadCount++;
            }

            return readResult;
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to read source '{Source}'");
            AddValidationError($"Failed to read data: '{ex.Message}'");
            return false;
        }
    }

    public override void Dispose()
    {
        if (_reader is not null)
        {
            _reader.Close();
            _reader.Dispose();
        }

        if (_gateway is null)
        {
            return;
        }

        _gateway.Close();
        _gateway.Dispose();
    }

    public object GetValue(int index)
    {
        if (_reader is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>($"Cannot get value from source. '{nameof(_reader)}' was null");
        }
        return _reader[index];
    }

    public object GetValue(string name)
    {
        if (_reader is null)
        {
            throw Log.ErrorAndCreateException<InvalidOperationException>($"Cannot get value from source. '{nameof(_reader)}' was null");
        }

        return _reader[name];
    }

    public override async Task<bool> NextResultAsync()
    {
        if (_reader is null)
        {
            return false;
        }

        var result = await _reader.NextResultAsync();
        if (result)
        {
            ResultIndex++;
            ReadCount = 0;
            _isFieldHeadersInitialized = false;
        }

        return result;
    }

    private void TryInitialize()
    {
        if (!_isInitialized)
        {
            Initialize();
        }

        if (_reader is null)
        {
            InitializeReader();
        }

        if (!_isFieldHeadersInitialized)
        {
            InitializeFieldHeaders();
            _isFieldHeadersInitialized = true;
        }
    }

    private void Initialize()
    {
        if (_isInitialized)
        {
            return;
        }

        try
        {
            _gateway = _databaseSource.CreateGateway();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to initialize reader for data source '{Source}'");

            AddValidationError($"Filed to initialize reader: '{ex.Message}'");
        }
        finally
        {
            _isInitialized = true;
        }
    }

    private void InitializeReader()
    {
        try
        {
            var userParameters = QueryParameters?.Parameters.ToDictionary(x => x.Name.ToUpperInvariant()) ?? new Dictionary<string, DataSourceParameter>();
            var queryParameters = _gateway?.GetQueryParameters() ?? new();
            queryParameters.Parameters.ForEach(x => x.Name = x.Name.ToUpperInvariant());
    
            foreach (var parameter in queryParameters.Parameters)
            {
                if (userParameters.TryGetValue(parameter.Name, out var userParameter))
                {
                    parameter.Value = userParameter.Value;
                }
            }

            _reader = _gateway?.GetRecords(queryParameters, Offset, FetchCount);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to initialize SelectAllReader for data source '{Source}'");
            _reader?.Dispose();
            _reader = null;

            AddValidationError($"Filed to initialize reader: '{ex.Message}'");
        }
    }

    private void InitializeFieldHeaders()
    {
        if (_reader is null)
        {
            return;
        }

        _fieldHeaders = _reader.GetHeaders();

#if DEBUG
        Log.Debug($"'{_fieldHeaders.Length}' headers of table were read");
#endif
    }
}
