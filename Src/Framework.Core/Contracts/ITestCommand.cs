namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    public interface ITestCommand
    {
        void Execute(TestRunContext context);
        ExecutionParameters ExecutionParameters { get; }
    }
}