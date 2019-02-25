
namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Workflow
{
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;

    [CanParse(@"^\s*Abort\s*$")]
    public class AbortCommand : BaseCommand
    {
        private readonly ExecutionParameters executionParameters;
        public AbortCommand( ExecutionParameters executionParameters)
            : base(executionParameters)
        {
            this.executionParameters = executionParameters;
        }

        public override ExecutionParameters ExecutionParameters
        {
            get { return this.executionParameters; }
        }

        #region Only signature with Not Implemented  Execution Methods

        public override void Execute(TestRunContext context)
        {
            // Defferred implementation
        }

        #endregion

        # region Only signature of the Validation method

        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {

        }

        #endregion

    }
}
