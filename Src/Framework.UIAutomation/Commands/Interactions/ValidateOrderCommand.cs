namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Extensions;

    [CanParse(@"^ValidateOrder\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class ValidateOrderCommand : InteractionCommand
    {
        public ValidateOrderCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters, uiAdapter)
        {
        }

        #region Execution methods
        public override void Execute(TestRunContext context)
        {
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                var controlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];
                var validation = ValidateExpectedOrder(controlDefinition);

                var testFinding = new TestFinding
                {
                    FlowIdentifier = context.FlowIdentifier,
                    DataIdentifier = context.Iteration,
                    Action = string.Format("Validate item order in {0}", this.logicalFieldName),
                    ActualResult = validation.Actual,
                    ExpectedResult = validation.Expected,
                    Value = validation.Succeeds
                        ? "Expectations matched."
                        : string.Format("Unexpected value: {0}",
                            ((IEnumerable<string>) validation.Actual).CommaDelimited()),
                    TestResult = validation.Succeeds
                        ? TestResult.Pass
                        : TestResult.Fail
                };
                context.TestFindings.Add(testFinding);

                if (!validation.Succeeds)
                {
                    this.uiAdapter.TakeScreenshot();
                    throw new ExpectationFailedException(string.Format("{0} was expected to be {1}, but was {2}",
                        this.logicalFieldName,
                        ((IEnumerable<string>) validation.Expected).CommaDelimited(),
                        ((IEnumerable<string>) validation.Actual).CommaDelimited()));
                }
            }
        }

        private Validation ValidateExpectedOrder(ControlDefinition controlDefinition)
        {
            return TryValidateOrderInDropDownList(controlDefinition)
                   ?? TryValidateOrderInTable(controlDefinition);
        }

        private Validation TryValidateOrderInDropDownList(ControlDefinition controlDefinition)
        {
            if (!controlDefinition.TagName.Equals("select", StringComparison.OrdinalIgnoreCase)) return null;

            var match = Regex.Match(this.logicalFieldValue,
                @"^(?<order>(?:Ascending|Descending)order)(?:\{(?<ignoreTerm>.+?)\})?$");
            var direction = match.Groups["order"].Value;

            var ignoreTerm = match.Groups["ignoreTerm"].Value;
            var dropDownItems = this.uiAdapter.GetDropDownOptions(controlDefinition)
                .Where(i => !i.Equals(ignoreTerm, StringComparison.OrdinalIgnoreCase));

            return ValidateOrderOfItems(dropDownItems, GetItemOrder(match.Groups["order"].Value));
        }

        private Validation TryValidateOrderInTable(ControlDefinition controlDefinition)
        {
            if (!controlDefinition.TagName.Equals("table", StringComparison.OrdinalIgnoreCase)) return null;

            var match = Regex.Match(this.logicalFieldValue,
                @"^Col(?<column>\d+):(?<order>(?:Ascending|Descending)order)$");
            var columnNumber = int.Parse(match.Groups["column"].Value);
            var columnItems = this.uiAdapter.GetTableColumn(controlDefinition, columnNumber);

            return ValidateOrderOfItems(columnItems, GetItemOrder(match.Groups["order"].Value));
        }

        private static ItemOrder GetItemOrder(string direction)
        {
            return direction.Equals("AscendingOrder", StringComparison.OrdinalIgnoreCase)
                ? ItemOrder.Ascending
                : ItemOrder.Descending;
        }

        private static Validation ValidateOrderOfItems(IEnumerable<string> columnItems, ItemOrder direction)
        {
            return new Validation
            {
                Actual = columnItems,
                Expected = direction == ItemOrder.Ascending
                    ? columnItems.OrderBy(x => x)
                    : columnItems.OrderByDescending(x => x),
                SuccessMeasure = v => ((IEnumerable<string>) v.Actual).SequenceEqual(((IEnumerable<string>) v.Expected))
            };
        }

        #endregion
    }

    internal enum ItemOrder
    {
        Ascending,
        Descending
    }
}
