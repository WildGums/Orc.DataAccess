namespace Orc.DataAccess.Excel
{
    using System;
    using System.Collections.Generic;
    using Catel.IoC;

    public static class ExcelSourceExtensions
    {
        public static List<string> GetWorkseetsList(this ExcelSource excelSource)
        {
            ArgumentNullException.ThrowIfNull(excelSource);

            var source = excelSource.ToString();
            if (string.IsNullOrEmpty(source))
            {
                return new List<string>();
            }

#pragma warning disable IDISP001 // Dispose created
            var typeFactory = excelSource.GetTypeFactory();
#pragma warning restore IDISP001 // Dispose created
            using (var reader = new ExcelReader(source))
            {
                return reader.GetWorksheetsList();
            }
        }
    }
}
