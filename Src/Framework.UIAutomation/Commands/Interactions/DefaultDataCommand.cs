namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Extensions;

    [CanParse(@"^DefaultData\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class DefaultDataCommand : InteractionCommand
    {
        public DefaultDataCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters, uiAdapter)
        { }

        # region execute method
        public override void Execute(TestRunContext context)
        {
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                Validation validation;
                try
                {
                    var controlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];

                    validation = this.ValidateDefaultData(controlDefinition);

                    var testFinding = new TestFinding
                    {
                        FlowIdentifier = context.FlowIdentifier,
                        DataIdentifier = context.Iteration,
                        Action = string.Format("Validate default data for" +
                                               " {0} as {1}", this.logicalFieldName, this.logicalFieldValue),
                        ActualResult = validation.Actual,
                        ExpectedResult = validation.Expected,
                        Value = validation.Succeeds
                                ? "Expectations matched."
                                : string.Format("Unexpected value: {0}", validation.Actual),
                        TestResult = validation.Succeeds
                                ? TestResult.Pass
                                : TestResult.Fail
                    };
                    context.TestFindings.Add(testFinding);
                }
                catch (Exception ex)
                {
                    throw new WorkflowFailedException("failed to run the default data command", ex);
                }              

                if (!validation.Succeeds)
                {
                    this.uiAdapter.TakeScreenshot();
                    throw new ExpectationFailedException();
                }
            }
        }

        private Validation ValidateDefaultData(ControlDefinition controlDefinition)
        {
            return TryValidatingNonEmptyTextBox(controlDefinition)
                   ?? TryValidatingDropDownItemCount(controlDefinition)
                   ?? TryValidatingExpectedValue(controlDefinition);
        }

        private Validation TryValidatingNonEmptyTextBox(ControlDefinition controlDefinition)
        {
            //if (!controlDefinition.TagName.Equals("Input", StringComparison.OrdinalIgnoreCase)) return null;
            if (!this.logicalFieldValue.Equals("ODVV", StringComparison.OrdinalIgnoreCase)) return null;

            return new Validation
            {
                Actual = this.uiAdapter.GetElementValue(controlDefinition),
                SuccessMeasure = validation => validation.Actual.ToString().IsNotNullOrEmpty()
            };
        }

        private Validation TryValidatingDropDownItemCount(ControlDefinition controlDefinition)
        {
            if (!controlDefinition.TagName.Equals("Select", StringComparison.OrdinalIgnoreCase)) return null;
            if (!this.logicalFieldValue.Equals("ODVV", StringComparison.OrdinalIgnoreCase)) return null;

            return new Validation
            {
                Actual = this.uiAdapter.GetNumberOfOptions(controlDefinition),
                Expected = 2,
                SuccessMeasure = validation => ((int)validation.Actual) >= ((int)validation.Expected)
            };
        }

        private Validation TryValidatingExpectedValue(ControlDefinition controlDefinition)
        {
            Validation validation = null;
            var valueToCheck = this.logicalFieldValue.Equals("{BLANK}", StringComparison.OrdinalIgnoreCase)
                ? string.Empty
                : this.logicalFieldValue;

            if (this.uiAdapter.IsSelectableInput(controlDefinition))
            {
                validation = new Validation
                {
                    Actual = this.uiAdapter.IsSelected(controlDefinition.OrUnderlyingControl()),
                    Expected = valueToCheck.Equals("TRUE", StringComparison.OrdinalIgnoreCase)
                };
            }
            else
            {
                validation = new Validation
                {
                    Actual = this.uiAdapter.GetElementValue(controlDefinition),
                    Expected = valueToCheck
                };
            }

            return validation;
        }

        # endregion
    }
}
