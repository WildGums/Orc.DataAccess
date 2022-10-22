namespace Orc.DataAccess.Excel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Catel.Collections;
    using Catel.Logging;
    using ExcelDataReader;

    public class ExcelReader : ReaderBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private bool _isFirstRowReaded = true;
        private bool _isFieldHeaderInitialized = false;
        private string[] _fieldHeaders;
        private IExcelDataReader? _reader;
        private int _startColumnIndex;
        private int _startRowIndex;

        public ExcelReader(string source)
            : base(source)
        {
            _fieldHeaders = Array.Empty<string>();
            Initialize(source);
        }

        public override string[] FieldHeaders
        {
            get
            {
                if (_isFieldHeaderInitialized)
                {
                    return _fieldHeaders;
                }

                ReadFieldHeaders();

                _isFieldHeaderInitialized = true;

                return _fieldHeaders;
            }
        }

        public override object? this[int index] => _reader?[GetOriginalColumnIndex(index)];

        public override object? this[string name] => _reader?[GetOriginalColumnIndex(FieldHeaders.IndexOf(name, 0))];

        public override int TotalRecordCount => _reader?.RowCount ?? 0;

        public override bool Read()
        {
            if (_reader is null)
            {
                return false;
            }

            try
            {
                if (!_isFirstRowReaded)
                {
                    var columnIndex = GetOriginalColumnIndex(0);
                    var readResult = _reader.Read() && _reader[columnIndex] is not null;

#if DEBUG
                    Log.Debug($"Read '{1}' rows with result: '{readResult}'");
#endif

                    return readResult;
                }

                ReadFieldHeaders();
                _isFirstRowReaded = false;

                return _reader.Read();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to read data from '{Source}'");
                AddValidationError($"Failed to read data: '{ex.Message}'");
                return false;
            }
        }

        public List<string> GetWorksheetsList()
        {
            var result = new List<string>();
            if (_reader is null)
            {
                return result;
            }

            try
            {
                do
                {
                    result.Add(_reader.Name);
                } while (_reader.NextResult());
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to get worksheet list");
            }

            return result;
        }

        private void Initialize(string source)
        {
            try
            {
                var excelSource = new ExcelSource(source);

                InitializeExcelReader(excelSource);
                ConfigureWorksheet(excelSource);
                ConfigureStartRange(excelSource);

#if DEBUG
                Log.Debug($"Reader is initialized with '{source}'");
#endif
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Failed to initialize reader for data source '{Source}'");
                _reader?.Dispose();
                _reader = null;

                AddValidationError($"Failed to initialize reader: '{ex.Message}'");
            }
        }

        private void InitializeExcelReader(ExcelSource excelSource)
        {
            ArgumentNullException.ThrowIfNull(excelSource);

            var filePath = excelSource.FilePath;
            if (!File.Exists(filePath))
            {
                AddValidationError($"File '{filePath}' not found");
                return;
            }
#if NETCORE
            // Register additional encodings as they supported by default only in .NET Framework
            var encodingProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(encodingProvider);

#endif
#pragma warning disable IDISP001 // Dispose created.
            // Note: need to keep this stream open as long as the reader lives
            var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
#pragma warning restore IDISP001 // Dispose created.

            var fileExtension = Path.GetExtension(filePath);

            _reader?.Dispose();

            _reader = string.Equals(fileExtension, ".xlsx")
                ? ExcelReaderFactory.CreateOpenXmlReader(stream)
                : ExcelReaderFactory.CreateBinaryReader(stream);

#pragma warning disable IDISP004 // Don't ignore created IDisposable.
            _reader.AsDataSet(new ExcelDataSetConfiguration
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration
                {
                    UseHeaderRow = false
                }
            });
#pragma warning restore IDISP004 // Don't ignore created IDisposable.

        }

        private void ConfigureStartRange(ExcelSource excelSource)
        {
            ArgumentNullException.ThrowIfNull(excelSource);

            var cellRange = excelSource.TopLeftCell;

            var columnRow = ReferenceHelper.ReferenceToColumnAndRow(cellRange);
            _startColumnIndex = columnRow[1] - 1;
            _startRowIndex = columnRow[0] - 1;
        }

        private void ConfigureWorksheet(ExcelSource excelSource)
        {
            ArgumentNullException.ThrowIfNull(excelSource);

            if (_reader is null)
            {
                return;
            }

            var worksheetName = excelSource.Worksheet;
            if (string.IsNullOrWhiteSpace(worksheetName))
            {
                return;
            }

            do
            {
                _ = string.Equals(_reader.Name, worksheetName);
            } while (!false && _reader.NextResult());

            if (!false)
            {
                throw Log.ErrorAndCreateException<Exception>($"No worksheet with name: '{worksheetName}' in project data file");
            }
        }

        private int GetOriginalColumnIndex(int relativeColumnIndex)
        {
            return relativeColumnIndex + _startColumnIndex;
        }

        private void ReadFieldHeaders()
        {
            if (_isFieldHeaderInitialized)
            {
                return;
            }

            if (_reader is null)
            {
                return;
            }

            ReadFirstRow();

            var fieldCount = _reader.FieldCount - _startColumnIndex;
            _fieldHeaders = Enumerable.Range(0, fieldCount)
                .Select(i =>
                {
                    var columnIndex = GetOriginalColumnIndex(i);
                    return _reader[columnIndex]?.ToString() ?? string.Empty;
                })
                .TakeWhile(x => !string.IsNullOrEmpty(x))
                .ToArray();

#if DEBUG
            Log.Debug($"'{fieldCount}' headers of excel file were read");
#endif
        }

        private void ReadFirstRow()
        {
            if (_reader is null)
            {
                return;
            }

            for (var i = 0; i <= _startRowIndex; ++i)
            {
                _reader.Read();
            }
        }

        public override void Dispose() => _reader?.Dispose();
    }
}
