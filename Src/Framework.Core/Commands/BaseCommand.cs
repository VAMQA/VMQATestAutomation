namespace VM.Platform.TestAutomationFramework.Core.Commands
{
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    public abstract class BaseCommand : ITestCommand
    {
        public abstract ExecutionParameters ExecutionParameters { get; }

        public abstract void Execute(TestRunContext context);
        protected abstract void ValidateExecutionParameters(ExecutionParameters parameters);

        protected BaseCommand(ExecutionParameters executionParameters)
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            // The abstract method is only running with parameters, never with instance members
            this.ValidateExecutionParameters(executionParameters);
        }
     
    }
}
