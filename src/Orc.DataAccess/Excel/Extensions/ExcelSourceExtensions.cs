// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelSourceExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.DataAccess.Excel
{
    using System.Collections.Generic;
    using Catel;
    using Catel.Data;
    using Catel.IoC;

    public static class ExcelSourceExtensions
    {
        public static List<string> GetWorkseetsList(this ExcelSource excelSource)
        {
            Argument.IsNotNull(() => excelSource);

            var source = excelSource.ToString();
            if (string.IsNullOrEmpty(source))
            {
                return new List<string>();
            }

            var typeFactory = excelSource.GetTypeFactory();
            using (var reader = new ExcelReader(source))
            {
                return reader.GetWorkseetsList();
            }
        }
    }
}
