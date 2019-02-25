namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using System.Text.RegularExpressions;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    [CanParse(@"^GetDBValue\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class GetDBValueCommand : InteractionCommand
    {
        private ControlDefinition ctrlDefinition;
        private TestFinding testFinding;

        public GetDBValueCommand(ExecutionParameters parameters, IUiAdapter uiAdapter) :
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
                    this.GetDBValue(this.ctrlDefinition, context);

                }
                catch (Exception ex)
                {
                    throw new ExpectationFailedException(ex.Message, ex);
                }
            }

        }
        private void GetDBValue(ControlDefinition controlDefinition, TestRunContext context)
        {
            var controlValue = this.uiAdapter.GetValueFromDB(this.logicalFieldValue.Split('|')[1]);
            this.SetTestFindingsIntoContext(controlValue, context);
            AddGetControlInfoToCaptured(context, this.logicalFieldValue.Split('|')[0], controlValue);
        }

        private void SetTestFindingsIntoContext(string controlValue, TestRunContext context)
        {
            this.testFinding = new TestFinding
            {
                FlowIdentifier = context.FlowIdentifier,
                DataIdentifier = context.Iteration,
                Action = string.Format("Captured {0} as {1}", this.logicalFieldName, this.logicalFieldValue.Split('|')[0]),
                TestResult = controlValue != null ? TestResult.Pass : TestResult.Fail,
                Value = controlValue
            };

            context.TestFindings.Add(this.testFinding);
        }

        private void AddGetControlInfoToCaptured(TestRunContext context, string key, string value)
        {
            context.CapturedValues[key.ToUpper()] = value ?? string.Empty;
        }

        #endregion
    }
}
