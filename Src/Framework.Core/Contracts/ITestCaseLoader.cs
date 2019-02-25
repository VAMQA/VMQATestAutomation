namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    using System.Collections.Generic;

    public interface ITestCaseLoader
    {
        TestCase GetTestCase(int testCaseId);
        IEnumerable<int> GetTestCaseIds(string testSuitePath);
    }
}