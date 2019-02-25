namespace VM.Platform.TestAutomationFramework.Core.Decorators
{
    using System;
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    public abstract class CommandDecorator : ITestCommand
    {
        protected readonly ITestCommand Command;

        protected CommandDecorator(ITestCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command", "Decorator has no underlying operation");
            }
            this.Command = command;
        }


        public virtual void Execute(TestRunContext context)
        {
            this.Command.Execute(context);
        }

        public ExecutionParameters ExecutionParameters
        {
            get { return this.Command.ExecutionParameters; }
        }
    }
}
