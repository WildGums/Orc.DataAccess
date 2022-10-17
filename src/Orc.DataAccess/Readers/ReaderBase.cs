namespace Orc.DataAccess
{
    using System.Globalization;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Data;

    public abstract class ReaderBase : IReader
    {
#pragma warning disable IDE1006 // Naming Styles
        protected readonly string Source;
#pragma warning restore IDE1006 // Naming Styles

        protected ReaderBase(string source, int offset = 0, int fetchCount = 0)
        {
            Argument.IsNotNullOrWhitespace(() => source);

            Source = source;
            ValidationContext = new ValidationContext();
            Offset = offset;
            FetchCount = fetchCount;
        }

        protected void AddValidationError(string message)
        {
            ValidationContext.AddValidationError(message, $"DataSource: '{Source}'");
        }

        public IValidationContext ValidationContext { get; }
        public abstract string[] FieldHeaders { get; }
        public abstract object this[int index] { get; }
        public abstract object this[string name] { get; }
        public abstract int TotalRecordCount { get; }
        public CultureInfo Culture { get; set; }
        public int Offset { get; set; }
        public int FetchCount { get; set; }

        #region IReader Members
        public abstract bool Read();

        public virtual async Task<bool> NextResultAsync()
        {
            return false;
        }

        public abstract void Dispose();
        #endregion
    }
}
