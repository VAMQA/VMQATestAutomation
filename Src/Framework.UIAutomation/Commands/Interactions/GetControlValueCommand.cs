namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using System.Text.RegularExpressions;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    [CanParse(@"^GetControlValue\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class GetControlValueCommand : InteractionCommand
    {
        private ControlDefinition ctrlDefinition;
        private TestFinding testFinding;

        public GetControlValueCommand(ExecutionParameters parameters, IUiAdapter uiAdapter) :
                base(parameters, uiAdapter)
        { }

        #region Exeucte methods
        public override void Execute(TestRunContext context)
        {
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                try
                {
                    this.ctrlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];
                    this.GetControlValue(this.ctrlDefinition, context);

                }
                catch (Exception ex)
                {
                    this.uiAdapter.TakeScreenshot();
                    throw new ExpectationFailedException(ex.Message, ex);
                }
            }

        }
        private void GetControlValue(ControlDefinition controlDefinition, TestRunContext context)
        {
            var controlValue = Regex.IsMatch(
                this.logicalFieldValue, @"(\{RetentionId|\{RetentionKey|\{Debug)", RegexOptions.IgnoreCase)
                ? IsRetentionKeyOrDebug(this.logicalFieldValue, controlDefinition)
                   : this.uiAdapter.GetElementValue(controlDefinition);
            this.SetTestFindingsIntoContext(controlValue, context);
            AddGetControlInfoToCaptured(context, this.logicalFieldValue, controlValue);
        }

        private string IsRetentionKeyOrDebug(string value, ControlDefinition controlDefinition)
        {
            return Regex.IsMatch(value, @"(\{Debug)", RegexOptions.IgnoreCase)
                ? this.uiAdapter.GetDebugInfo(controlDefinition)
                : this.uiAdapter.GetRetentionKey(controlDefinition);
        }

        private void SetTestFindingsIntoContext(string controlValue, TestRunContext context)
        {
            this.testFinding = new TestFinding
            {
                FlowIdentifier = context.FlowIdentifier,
                DataIdentifier = context.Iteration,
                Action = string.Format("Captured {0} as {1}", this.logicalFieldName, this.logicalFieldValue),
                TestResult = controlValue != null ? TestResult.Pass : TestResult.Fail,
                Value = controlValue
            };

            context.TestFindings.Add(this.testFinding);
        }

        private void AddGetControlInfoToCaptured(TestRunContext context, string key, string value)
        {
            context.CapturedValues[key.ToUpper()] = value ?? string.Empty;
        }

        # endregion
    }
}
