namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    public interface ITestCaseRunner
    {
        TestRunContext ExecuteSingleTestCase(int testCaseId, string targetEnvironment,string repository, int Retry);
        //void ExecuteSingleTestCase(int testCaseId,int projectId, string targetEnvironment);
    }
}