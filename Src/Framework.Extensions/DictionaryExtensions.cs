namespace VM.Platform.TestAutomationFramework.Extensions
{
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        public static bool TryGet<T>(this IDictionary<string, object> dictionary, string key, out T value)
            where T : class
        {
            object objectValue;
            var objectFound = dictionary.TryGetValue(key, out objectValue);

            value = objectValue as T;

            return objectFound;
        }
    }
}