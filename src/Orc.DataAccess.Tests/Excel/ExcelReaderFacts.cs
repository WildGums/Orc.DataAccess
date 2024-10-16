﻿namespace Orc.DataAccess.Tests;

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

        using var reader = new ExcelReader($"FilePath={testDataFilePath}");
        var fieldHeaders = reader.FieldHeaders;

        while (reader.Read())
        {
            foreach (var fieldHeader in fieldHeaders)
            {
                var currentValue = reader[fieldHeader];
            }
        }

        Assert.That(reader.TotalRecordCount, Is.EqualTo(8));
    }

    private static string GetDataDirectory([CallerFilePath] string sourceFilePath = "")
    {
        var directory = Path.GetDirectoryName(sourceFilePath);
        directory = Path.GetDirectoryName(directory);

        return Path.Combine(directory, @"Data");
    }
}
