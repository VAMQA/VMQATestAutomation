namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using System.Collections.Generic;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Extensions;
    using VM.Platform.TestAutomationFramework.UIAutomation.Properties;

    [CanParse(@"^StartCondition\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public  class StartConditionCommand: InteractionCommand
    {
        public StartConditionCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters, uiAdapter)
        { }

        public override void Execute(TestRunContext context)
        {
            context.Conditions.Add(this.logicalFieldValue);
            // TODO: Revisit to add html report support
        }
    }
}
