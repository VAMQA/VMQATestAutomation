namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    public interface ITestCommandRunner
    {
        void Execute(TestRunContext testRunContext);
    }
}