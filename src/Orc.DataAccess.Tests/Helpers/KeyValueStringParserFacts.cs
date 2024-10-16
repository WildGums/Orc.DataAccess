namespace Orc.DataAccess.Tests;

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class KeyValueStringParserFacts
{
    [TestCase($@"ConnectionString=Data Source=myServer\sqlexpress;Initial Catalog=TestProject;Integrated Security=True;    {KeyValueStringParser.KeyValuePairsDelimiter}Table=RelationOperations{KeyValueStringParser.KeyValuePairsDelimiter}  Dialect=SQLSERVER_2008   ",
        new[] {"ConnectionString", "Table", "Dialect"},
        new[] {@"Data Source=myServer\sqlexpress;Initial Catalog=TestProject;Integrated Security=True;", "RelationOperations", "SQLSERVER_2008"})]
    [TestCase(@$"FilePath=ElsinoreData.xlsx{KeyValueStringParser.KeyValuePairsDelimiter}  Worksheet=Operation{KeyValueStringParser.KeyValuePairsDelimiter} CellRange=A212 ",
        new[] {"FilePath", "Worksheet", "CellRange"},
        new[] {@"ElsinoreData.xlsx", "Operation", "A212"})]
    public void CorrectlyParseSourceIntoKeyValueDictionary(string source, string[] expectedKeys, string[] expectedValues)
    {
        var results = KeyValueStringParser.Parse(source);
        var expectedKeyValues = expectedKeys.Zip(expectedValues, (key, value) => new KeyValuePair<string, string>(key, value)).OrderBy(x => x.Key);

        var isEquals = results.OrderBy(x => x.Key).SequenceEqual(expectedKeyValues);
        Assert.That(isEquals, Is.True);
    }

    [TestCase(new[] {"ConnectionString", "Table", "Dialect"}, new[] {@"Data Source=myServer\sqlexpress;Initial Catalog=TestProject;Integrated Security=True;", "RelationOperations", "SQLSERVER_2008"},
        $@"ConnectionString=Data Source=myServer\sqlexpress;Initial Catalog=TestProject;Integrated Security=True;{KeyValueStringParser.KeyValuePairsDelimiter} Table=RelationOperations{KeyValueStringParser.KeyValuePairsDelimiter} Dialect=SQLSERVER_2008")]
    [TestCase(new[] {"FilePath", "Worksheet", "CellRange"}, new[] {@"ElsinoreData.xlsx", "Operation", "A212"},
        $@"FilePath=ElsinoreData.xlsx{KeyValueStringParser.KeyValuePairsDelimiter} Worksheet=Operation{KeyValueStringParser.KeyValuePairsDelimiter} CellRange=A212")]
    public void CorrectlyFormatToKeyValueString(string[] keys, string[] values, string expectedResult)
    {
        var sourceKeyValues = keys.Zip(values, (key, value) => new KeyValuePair<string, string>(key, value));

        var result = KeyValueStringParser.FormatToKeyValueString(sourceKeyValues);

        Assert.That(expectedResult, Is.EqualTo(result));
    }

    [TestCase($@"ConnectionString=Data Source=myServer\sqlexpress;Initial Catalog=TestProject;Integrated Security=True;{KeyValueStringParser.KeyValuePairsDelimiter} Table=RelationOperations{KeyValueStringParser.KeyValuePairsDelimiter} Dialect=SQLSERVER_2008", "Catalog",
        "SomeValue", $@"ConnectionString=Data Source=myServer\sqlexpress;Initial Catalog=TestProject;Integrated Security=True;{KeyValueStringParser.KeyValuePairsDelimiter} Table=RelationOperations{KeyValueStringParser.KeyValuePairsDelimiter} Dialect=SQLSERVER_2008{KeyValueStringParser.KeyValuePairsDelimiter} Catalog=SomeValue")]
    [TestCase($@"ConnectionString=Data Source=myServer\sqlexpress;Initial Catalog=TestProject;Integrated Security=True;{KeyValueStringParser.KeyValuePairsDelimiter} Table=RelationOperations{KeyValueStringParser.KeyValuePairsDelimiter} Dialect=SQLSERVER_2008", "ConnectionString",
        "Other connection string", $@"ConnectionString=Other connection string{KeyValueStringParser.KeyValuePairsDelimiter} Table=RelationOperations{KeyValueStringParser.KeyValuePairsDelimiter} Dialect=SQLSERVER_2008")]
    [TestCase($@"FilePath =ElsinoreData.xlsx{KeyValueStringParser.KeyValuePairsDelimiter} Worksheet=Operation{KeyValueStringParser.KeyValuePairsDelimiter} CellRange=A212", "Worksheet", "Other worksheet", $@"FilePath =ElsinoreData.xlsx{KeyValueStringParser.KeyValuePairsDelimiter} Worksheet=Other worksheet{KeyValueStringParser.KeyValuePairsDelimiter} CellRange=A212")]
    public void CorrectlySetValueByKeyToKeyValueString(string source, string key, string value, string expectedSource)
    {
        var result = KeyValueStringParser.SetValue(source, key, value);

        Assert.That(expectedSource, Is.EqualTo(result));
    }
}
