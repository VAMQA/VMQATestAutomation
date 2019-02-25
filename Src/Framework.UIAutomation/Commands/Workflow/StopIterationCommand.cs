
namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Workflow
{
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    [CanParse(@"^\s*StopIteration\s*$")]
    public class StopIterationCommand : BaseCommand
    {
        private readonly IUiAdapter uiAdapter;
        private readonly ExecutionParameters executionParameters;

        public override ExecutionParameters ExecutionParameters
        {
            get { return this.executionParameters; }
        }
        public StopIterationCommand(IUiAdapter uiAdapter,ExecutionParameters executionParameters) : base(executionParameters)
        {
            this.executionParameters = executionParameters;
            this.uiAdapter = uiAdapter;
        }
        #region Command Execution methods
        public override void Execute(TestRunContext context)
        {
            context.StepPositionId = -1;    // Set StepPositionId is -1 as in the current behavior.
            if (!context.IsRandomExecution)
            {
                context.MoveToNextIteration();
                var disStop = context.DiStop;
                if (context.Iteration <= disStop)
                {
                    // To dicuss context.IterationStartCommand = context.CurrentCommand;
                    context.NextCommand = context.StartIterationCommand;
                }
                else
                {
                    context.Iteration = null; // No longer in iteration
                }
            }
            else
            {
                var disStop = context.DiStop;
                if (context.Iteration < disStop)
                {
                    // To dicuss context.IterationStartCommand = context.CurrentCommand;
                    context.NextCommand = context.StartIterationCommand;
                }
                else
                {
                    context.Iteration = null; // No longer in iteration
                }
            }
            
            if (context.IsCleanUpWebDriverEndOfIteration)
            {
                this.uiAdapter.CloseCurrentBrowser();
            }

        }
        #endregion

        #region Only signature of the Validation method
        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {

        }
        #endregion

    }
}
