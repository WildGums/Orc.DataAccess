namespace Orc.DataAccess.Excel
{
    public class ExcelSource : DataSourceBase
    {
        private string _topLeftCell = "A1";

        #region Constructors
        public ExcelSource()
            : this(string.Empty)
        {
        }

        public ExcelSource(string location)
            : base(location)
        {
        }
        #endregion

        #region Properties
        public string FilePath { get; set; }
        public string Worksheet { get; set; }

        public string TopLeftCell
        {
            get => _topLeftCell;
            set
            {
                if (Equals(_topLeftCell, value))
                {
                    return;
                }

                _topLeftCell = value;
                RaisePropertyChanged(nameof(TopLeftCell));
            }
        }
        #endregion
    }
}
