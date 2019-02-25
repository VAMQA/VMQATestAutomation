namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    using System.Collections.Generic;

    public interface ITestDataLoader
    {
        Dictionary<string, IEnumerable<TestDataDirective>> GetTestData(int testCaseId);
    }
}