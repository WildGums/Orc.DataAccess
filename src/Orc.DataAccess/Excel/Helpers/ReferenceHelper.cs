namespace Orc.DataAccess.Excel
{
    using System;
    using System.Text.RegularExpressions;

    public static class ReferenceHelper
    {
        public static int[] ReferenceToColumnAndRow(string reference)
        {
            var regex = new Regex("([a-zA-Z]*)([0-9]*)", RegexOptions.None, TimeSpan.FromSeconds(1));
            var upper = regex.Match(reference).Groups[1].Value.ToUpper();
            var s = regex.Match(reference).Groups[2].Value;

            var num1 = 0;
            var num2 = 1;

            unchecked
            {
                for (var index = upper.Length - 1; index >= 0; --index)
                {
                    var num3 = upper[index] - 65 + 1;
                    num1 += num2 * num3;
                    num2 *= 26;
                }
            }

            return new[] {int.Parse(s), num1};
        }
    }
}
