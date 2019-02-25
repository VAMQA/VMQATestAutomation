
namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    [CanParse(@"^StopCondition\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public  class StopConditionCommand : InteractionCommand
    {
        public StopConditionCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters, uiAdapter)
        { }

        public override void Execute(TestRunContext context)
        {
           //TODO: Revisit for HTML report implementation and condition result handling
        }
    }
}
