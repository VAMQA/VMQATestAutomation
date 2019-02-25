
namespace VM.Platform.TestAutomationFramework.Extensions

{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string validateString)
        {
            return string.IsNullOrEmpty(validateString);
        }

        public static bool IsNullOrWhiteSpace(this string validateString)
        {
            return string.IsNullOrWhiteSpace(validateString);
        }

        public static bool IsAlpha(this string validateString)
        {
            return validateString != null && Regex.IsMatch(validateString, "^[A-Za-z]+");
        }

        public static bool IsNumeric(this string validateString, Func<int, bool> predicate = null )
        {
            return validateString != null && Regex.IsMatch(validateString, "[0-9]+") &&
                   (predicate == null || predicate(int.Parse(validateString)));
        }

        public static bool IsAlphaNumeric(this string validateString,Func<string, bool> predicate = null )
        {
            return validateString != null && Regex.IsMatch(validateString, "^[A-Za-z]|^[A-Za-z][0-9]+$") &&
                  (predicate == null || predicate((validateString)));
        }

        public static bool IsNotNullOrEmptyOrWhiteSpace(this string validateString)
        {
            return !(string.IsNullOrEmpty(validateString) || string.IsNullOrWhiteSpace(validateString));
        }

        public static bool IsNotNullOrEmpty(this string validateString)
        {
            return !(string.IsNullOrEmpty(validateString));
        }

        public static string Indented(this string message)
        {
            return "\t" + message;
        }

        public static string ToSingleLine(this string multiLineString)
        {
            return Regex.Replace(multiLineString, "\r\n", string.Empty);
        }

        public static double? TryGetDoubleValue(this string s)
        {
            var number = Regex.Match(s, @"(?<number>\d+(?:\.\d+)?)", RegexOptions.IgnoreCase).Groups["number"].Value;
            return number.IsNotNullOrEmptyOrWhiteSpace()
                ? double.Parse(number)
                : (double?) null;
        }

        public static string ConcatWithSingleQuotes(this string s)
        {
            return s.Contains("'")
                ? "concat('" + s.Split('\'').Aggregate((a, i) => a + "',\"'\", '" + i) + "')"
                : "'" + s + "'";
        }
    }
}
