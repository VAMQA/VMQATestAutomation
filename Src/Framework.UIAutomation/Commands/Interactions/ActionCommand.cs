
namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using System.Text.RegularExpressions;
    using Framework.SikuliAutomation;

    //using Sikuli4Net.sikuli_REST;
    //using Sikuli4Net.sikuli_UTIL;

    [CanParse(@"^Action\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class ActionCommand : InteractionCommand
    {
        private readonly TestCaseConfiguration testCaseConfiguration;
        private ControlDefinition controlDefinition;

        public ActionCommand(ExecutionParameters parameters, IUiAdapter uiAdapter, TestCaseConfiguration testCaseConfiguration)
            : base(parameters, uiAdapter)
        {
            this.testCaseConfiguration = testCaseConfiguration;
        }

        public override void Execute(TestRunContext context)
        {
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                try
                {
                    this.controlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];
                    if (!string.IsNullOrEmpty(controlDefinition.ImagePath))
                    {
                        CallImageinAction();                        
                    }
                    else
                    {
                        
                        this.uiAdapter.WaitForElementToBeClickable(this.controlDefinition);
                        this.Action(this.logicalFieldValue);
                    }

                }
                catch (Exception ex)
                {
                    if (!string.IsNullOrEmpty(controlDefinition.ImagePath))
                    {
                        CallImageinAction();
                    }
                    else
                    {
                        this.uiAdapter.TakeScreenshot();
                        //Report the Error to HTML REPORT
                        ReportFailureToHtml ReportError = new ReportFailureToHtml();
                        context = ReportError.Run(context, string.Format("Error for {0}", this.logicalFieldName), string.Format("Waited, but the {0} control never appeared.", this.logicalFieldName), TestResult.Fail);

                        throw new WorkflowFailedException(ex.Message, ex);
                    }
                }
            }
        }

        private void CallImageinAction()
        {
            try
            {
                SikuliTest sikulitest = new SikuliTest();
                sikulitest.CallImage(controlDefinition.ImagePath);
            }
            catch (Exception ex1)
            {
                throw;
            }
        }

        private void Action(string logicFieldValue)
        {
            const string actionPattern =
                @"^(?<tableCell>(?<row>\d+)-(?<col>\d+)\:)?(?<action>Click|\w*)-?(?<actionType>\w*)(?:\((?<value>[\w ]*)\))?$";
            var regex = new Regex(actionPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(logicFieldValue);
            if (!match.Success) throw new WorkflowFailedException("Unsupported action:" + logicFieldValue);

            if (match.Groups["action"].Value.Equals("Click", StringComparison.OrdinalIgnoreCase))
            {
                // Handle CLICK actions
                if (match.Groups["tableCell"].Value != string.Empty)
                {
                    string operation = "";
                    // Handle Clicks in a table cell
                    var row = int.Parse(match.Groups["row"].Value);
                    var column = int.Parse(match.Groups["col"].Value);
                    if (match.Groups.Count == 7) // 7 - including operation either delete / edit
                        operation = match.Groups[5].Value;
                    this.uiAdapter.ClickTableCell(controlDefinition, row, column, operation);

                }
                else if (match.Groups["actionType"].Value.Equals(string.Empty, StringComparison.OrdinalIgnoreCase))
                {
                    //Handle plain old CLICK action
                    try
                    {
                        this.uiAdapter.ClickElement(controlDefinition);
                    }
                    catch (Exception)
                    {
                        this.uiAdapter.ClickUsingJavascript(controlDefinition);
                    }
                }
                else if (match.Groups["actionType"].Value.Equals("Wait", StringComparison.OrdinalIgnoreCase))
                {
                    // Handle Click-Wait([milliseconds])
                    var duration = string.IsNullOrWhiteSpace(match.Groups["value"].Value)
                        ? TimeSpan.FromSeconds(2)
                        : TimeSpan.FromMilliseconds(double.Parse(match.Groups["value"].Value));

                    var clickWaitOption = this.testCaseConfiguration.ClickWaitOption;
                    if ((clickWaitOption & ClickWaitOption.WaitBefore) == ClickWaitOption.WaitBefore)
                    {
                        this.uiAdapter.WaitExplicitly(duration);
                    }
                    this.uiAdapter.WaitForElementToBeClickable(controlDefinition);
                    this.uiAdapter.ClickElement(this.controlDefinition);
                    if ((clickWaitOption & ClickWaitOption.WaitAfter) == ClickWaitOption.WaitAfter)
                    {
                        this.uiAdapter.WaitExplicitly(duration);
                        //this.uiAdapter.WaitExplicitly(TimeSpan.FromSeconds(10));
                    }
                }
                else if (match.Groups["actionType"].Value.Equals("This", StringComparison.OrdinalIgnoreCase))
                {
                    // TODO: Handle ClickThis
                    throw new NotImplementedException("CLICKTHIS");
                }
                else if (match.Groups["actionType"].Value.Equals("At", StringComparison.OrdinalIgnoreCase))
                {
                    // Handle Click-At(Center|Top|Bottom|Left|Right)
                    var clickPoint =
                        (ClickPoint)Enum.Parse(typeof(ClickPoint), match.Groups["value"].Value, ignoreCase: true);
                    this.uiAdapter.ClickElementAt(this.controlDefinition, clickPoint);
                }
                else if (match.Groups["actionType"].Value.Equals("IfExists", StringComparison.OrdinalIgnoreCase))
                {
                    if (this.uiAdapter.IsElementPresent(this.controlDefinition))
                        this.uiAdapter.ClickElement(this.controlDefinition);

                }
            }
            else if (match.Groups["action"].Value.Equals("DoubleClick", StringComparison.OrdinalIgnoreCase))
            {
                // Handle the BACK action
                this.uiAdapter.DoubleClickElement(controlDefinition);
            }
            else if (match.Groups["action"].Value.Equals("Back", StringComparison.OrdinalIgnoreCase))
            {
                // Handle the BACK action
                this.uiAdapter.BackNavigation();
            }
            else if (match.Groups["action"].Value.Equals("Hover", StringComparison.OrdinalIgnoreCase))
            {
                // Handle the Hover action      
                var value = match.Groups["value"].Value;
                this.uiAdapter.HoverElement(this.controlDefinition, value);
            }
            else if (match.Groups["action"].Value.Equals("Keys", StringComparison.OrdinalIgnoreCase))
            {
                // Handle Keyboard Inpus      
                var value = match.Groups["value"].Value;
                this.uiAdapter.SendKeyboadKeys(this.controlDefinition, value);
            }
            //else if (match.Groups["action"].Value.Equals("SwitchTo", StringComparison.OrdinalIgnoreCase))
            //{
            //    var value = match.Groups["value"].Value;
            //    this.uiAdapter.SwitchToWindow(value);
            //}
            //else if (match.Groups["action"].Value.Equals("SwitchBack", StringComparison.OrdinalIgnoreCase))
            //{
            //    this.uiAdapter.SwitchToFirstWindow();
            //}

        }
    }
}
