namespace VM.Platform.TestAutomationFramework.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static string CommaDelimited(this IEnumerable<string> items)
        {
            return items.Aggregate((a, i) => a + ", " + i);
        }
    }
}