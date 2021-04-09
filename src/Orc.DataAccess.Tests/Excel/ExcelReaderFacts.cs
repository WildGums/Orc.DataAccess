// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyValueStringParser.cs" company="WildGums">
//   Copyright (c) 2008 - 2019 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.DataAccess.Tests
{
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Excel;
    using NUnit.Framework;

    [TestFixture]
    public class ExcelReaderFacts
    {
        [TestCase, Apartment(ApartmentState.STA)]
        public void CorrectlyReadExcelFile()
        {
            var dataDirectory = GetDataDirectory();
            var testDataFilePath = Path.Combine(dataDirectory, "testData.xlsx");

            using (var reader = new ExcelReader($"FilePath={testDataFilePath}"))
            {
                var fieldHeaders = reader.FieldHeaders;
                while (reader.Read())
                {
                    foreach (var fieldHeader in fieldHeaders)
                    {
                        var currentValue = reader[fieldHeader];
                    }
                }

                Assert.AreEqual(reader.TotalRecordCount, 8);
            }
        }

        private string GetDataDirectory([CallerFilePath] string sourceFilePath = "")
        {
            var directory = Path.GetDirectoryName(sourceFilePath);
            directory = Path.GetDirectoryName(directory);

            return Path.Combine(directory, @"Data");
        }
    }
}
