namespace Orc.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class KeyValueStringParser
    {
        public const char KeyValuePairsDelimiter = ',';
        public const char KeyValueDelimiter = '=';
        private static readonly Regex KeyValueGroupRegex;

        static KeyValueStringParser()
        {
            //NOTE: regex is not compiled because this operation is very rare
            KeyValueGroupRegex = new Regex($@"(?<key>\w+)\s*{KeyValueDelimiter}\s*(?<value>.+)(?:\s|$)");
        }

        public static Dictionary<string, string> Parse(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return new Dictionary<string, string>();
            }

            var keyValuePairs = source.Split(KeyValuePairsDelimiter);

            var keyValueDictionary = new Dictionary<string, string>();

            foreach (var keyValuePair in keyValuePairs)
            {
                var match = KeyValueGroupRegex.Match(keyValuePair.Trim());
                if (!match.Success)
                {
                    continue;
                }

                var key = match.Groups["key"].Value;
                var value = match.Groups["value"].Value;

                keyValueDictionary[key] = value;
            }

            return keyValueDictionary;
        }

        public static string FormatToKeyValueString(IEnumerable<KeyValuePair<string, string>> keyPairs)
        {
            ArgumentNullException.ThrowIfNull(keyPairs);

            return string.Join($"{KeyValuePairsDelimiter} ", keyPairs.Select(x => FormatKeyValue(x.Key, x.Value)));
        }

        public static string? GetValue(string source, string key)
        {
            var keyValuePairs = Parse(source);
            return keyValuePairs.ContainsKey(key) ? keyValuePairs[key] : null;
        }

        public static string SetValue(string source, string key, string value)
        {
            var oldValue = GetValue(source, key);
            return oldValue is not null ? source.Replace(oldValue, value) : $"{source}{KeyValuePairsDelimiter} {FormatKeyValue(key, value)}";
        }

        private static string FormatKeyValue(string key, string value)
        {
            return $"{key}{KeyValueDelimiter}{value}";
        }
    }
}
