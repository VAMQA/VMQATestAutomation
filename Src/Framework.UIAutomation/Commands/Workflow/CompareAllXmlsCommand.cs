
namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Workflow
{
    using System;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;

    [CanParse(@"^\s*CompareAllXmls\s*")]
    public class CompareAllXmlsCommand : BaseCommand
    {

        private readonly ExecutionParameters executionParameters;

        public CompareAllXmlsCommand(ExecutionParameters executionParameters)
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
            throw new NotImplementedException();
        }
        #endregion

        # region Only signature of the Validation method
        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {

        }
        #endregion

    }
}
