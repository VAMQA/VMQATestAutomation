namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    using System.Collections.Generic;

    public interface IDataFileReader
    {
        Dictionary<string, Dictionary<string, T>> GetMapOfMaps<T>(string source)
            where T : class, new();

        Dictionary<string, IEnumerable<TestDataDirective>> GetTestDataDirectives(string source);
    }
}