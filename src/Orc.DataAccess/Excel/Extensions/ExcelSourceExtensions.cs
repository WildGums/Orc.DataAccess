namespace Orc.DataAccess.Excel;

using System;
using System.Collections.Generic;
using Catel.IoC;

public static class ExcelSourceExtensions
{
    public static IReadOnlyList<string> GetWorksheetsList(this ExcelSource excelSource)
    {
        ArgumentNullException.ThrowIfNull(excelSource);

        var source = excelSource.ToString();
        if (string.IsNullOrEmpty(source))
        {
            return new List<string>();
        }

        using var reader = new ExcelReader(source);
        return reader.GetWorksheetsList();
    }
}
