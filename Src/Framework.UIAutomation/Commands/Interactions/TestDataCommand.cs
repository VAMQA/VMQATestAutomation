namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using Framework.SikuliAutomation;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    [CanParse(@"^TestData\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class TestDataCommand : InteractionCommand
    {
        private ControlDefinition ctrlDefinition;
        private readonly ScriptRunner scriptRunner;


        public TestDataCommand(ExecutionParameters parameters, IUiAdapter uiAdapter, ScriptRunner scriptRunner)
            : base(parameters, uiAdapter)
        {
            this.scriptRunner = scriptRunner;
        }

        public override void Execute(TestRunContext context)
        {
            if (ShouldIgnoreThisField()) return;
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
              
                try
                {
                    ShouldRemoveSpeicalKeyWordFromLogicalFieldValue();
                    this.ctrlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];
                    this.TransformLogicalFieldValue(context);
                    this.SetTestDataInteraction();

                }
                catch (Exception ex)
                {
                    ReportFailureToHtml ReportError = new ReportFailureToHtml();
                    context = ReportError.Run(context, string.Format("TestData Command for " +
                                           " {0}", this.logicalFieldName), string.Format("Could not set TestData for field '{0}'.", this.logicalFieldName), TestResult.Fail);
                    this.uiAdapter.TakeScreenshot();
                    throw new WorkflowFailedException(
                        string.Format("Could not set test data for field '{0}'.", this.logicalFieldName),
                        ex);
                }
           }
        }

        private bool ShouldIgnoreThisField()
        {
            const string ignoreFieldPattern = @"^({BLANK}|nofill|09/09/9009)$";

            return Regex.IsMatch(this.logicalFieldValue.Trim(), ignoreFieldPattern, RegexOptions.IgnoreCase);
        }

        private void ShouldRemoveSpeicalKeyWordFromLogicalFieldValue()
        {
            const string removePattern = "SPECIAL=";
            this.logicalFieldValue = Regex.Replace(this.logicalFieldValue.Trim(), removePattern, "");
        }

        private void TransformLogicalFieldValue(TestRunContext context)
        {
            // TODO: Implement various transformations, such as random values, email IDs, etc.
            TryTransformAsEmail();
            TryTransformAsRandomize();
            SetLogicalFieldValue(context, this.logicalFieldValue);
            TryTransformAsCSharpScript();
            
        }

        private void TryTransformAsRandomize()
        {
            #region old Code
            var regex = new Regex(@"^RANDOMIZE\((?<alphas>\d+),(?<numerics>\d+)\)$");

            var match = regex.Match(this.logicalFieldValue);
            if (match.Success)
            {
                var random = new Random();
                var randomTextBuilder = new StringBuilder();
                var alphaCount = int.Parse(match.Groups["alphas"].Value);
                var numericCount = int.Parse(match.Groups["numerics"].Value);
                const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                for (var i = 0; i < alphaCount; i++)
                {
                    randomTextBuilder.Append(letters[random.Next(letters.Length)]);
                }
                for (var i = 0; i < numericCount; i++)
                {
                    randomTextBuilder.Append(random.Next(2, 10));
                }

                this.logicalFieldValue = randomTextBuilder.ToString();
            }
            #endregion old Code

            #region New Code
            ////var regex = new Regex(@"^RANDOMIZE\((?<alphas>\d+),(?<numerics>\d+)\)$");
            //var regex = new Regex(@"^RANDOMIZE\((?<alphas>\d+),(?<numerics>\d+)\)$");

            //var match = regex.Match(this.logicalFieldValue);
            //var match2 = regex.Matches(this.logicalFieldValue);
            //if (logicalFieldValue.Contains("RANDOMIZE"))
            ////if (match.Success)
            //{
            //    var random = new Random();
            //    var randomTextBuilder = new StringBuilder();
            //   // randomTextBuilder.Length = 7;
            //    //var alphaCount = int.Parse(match.Groups["alphas"].Value);
            //    //var numericCount = int.Parse(match.Groups["numerics"].Value);
            //    const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //    for (var i = 0; i < 7; i++)
            //    {
            //        randomTextBuilder.Append(letters[random.Next(letters.Length)]);
            //    }
            //    //for (var i = 0; i < numericCount; i++)
            //    //{
            //    //    randomTextBuilder.Append(random.Next(2, 10));
            //    //}
            //    //randomTextBuilder.Append(letters[random.Next(letters.Length)]);
            //    this.logicalFieldValue = randomTextBuilder.ToString();
            //}
            #endregion New Code
        }

        private void TryTransformAsCSharpScript()
        {
            var regex = new Regex(@"^=C#\((?<code>.+?)\)$");
            var match = regex.Match(this.logicalFieldValue);
            if (match.Success)
            {
                var code = match.Groups["code"].Value;
                this.logicalFieldValue = this.scriptRunner.EvaluateAsCSharp(code).ToString();
            }
        }

        private void TryTransformAsEmail()
        {
            if (this.logicalFieldValue.Equals("EmailID", StringComparison.OrdinalIgnoreCase))
            {
                this.logicalFieldValue = string.Format("johnsmith{0}@VM.com", new Random().Next());
            }
        }

        private void SetTestDataInteraction()
        {
          
            switch (GetInteractionType().ToLower())
            {
                case "textarea":
                case "input":
                    var textBoxElementCommand = new TextBoxElementCommand(this.uiAdapter, this.ctrlDefinition, this.logicalFieldValue);
                    if (!string.IsNullOrEmpty(ctrlDefinition.ImagePath))
                    {
                        CallImageForTextInput();
                    }
                    else
                    {
                        textBoxElementCommand.Execute();
                    }
                    
                    break;

                case "select":
                    var dropDownElementCommand = new DropDownElementCommand(this.uiAdapter, this.ctrlDefinition, false, this.logicalFieldValue);
                    dropDownElementCommand.Execute();
                    break;

                case "label":
                case "radiobutton":
                case "checkbox":
                    var checkBoxOrRadioButtonCommand = new CheckBoxOrRadioButton(this.uiAdapter, this.ctrlDefinition, this.logicalFieldValue);
                    checkBoxOrRadioButtonCommand.Execute();
                    break;
                case "a":
                    var hyperlinkElementCommand = new HyperLinkElementCommand(this.uiAdapter, this.ctrlDefinition);
                    hyperlinkElementCommand.Execute();
                    break;
                default:
                    throw new WorkflowFailedException("TestData indicator not supported for this element type");
            }
        }

        private void CallImageForTextInput()
        {
            try
            {
                SikuliTest sikulitest = new SikuliTest();
                sikulitest.CallImageForTextInput(ctrlDefinition.ImagePath, logicalFieldValue);
            }
            catch (Exception ex1)
            {
                throw;
            }
        }

        private string GetInteractionType()
        {
            string setInteractionType;

            if (this.ctrlDefinition.TagName.Equals("input", StringComparison.OrdinalIgnoreCase) 
                && this.ctrlDefinition.Type.Equals("radio", StringComparison.OrdinalIgnoreCase))
            {
                setInteractionType= "radiobutton";
            }
            else if (this.ctrlDefinition.TagName.ToLower() == "input" && this.ctrlDefinition.Type.ToLower() == "check")
            {
                setInteractionType = "checkbox";
            }
            else
            {
                setInteractionType = this.ctrlDefinition.TagName;
            }

            return setInteractionType;
        }


        private void SetLogicalFieldValue(TestRunContext context, string value)
        {
            //if (value.StartsWith("{") && value.EndsWith("}"))
            //{
            //    this.logicalFieldValue = context.CapturedValues[value];
            //}
            var cFormat = Regex.Matches(value, @"{[A-Za-z]|^[A-Za-z][0-9]\-}");

            if (cFormat.Count > 0)
            {
                string[] values = value.Split(' ');

                for (int i = 0; i < values.Length; i++)
                {
                    var mFormat = Regex.Matches(values[i], @"\{.*?\}");
                    foreach (var item in mFormat)
                    {
                        if (context.CapturedValues.ContainsKey(item.ToString().ToUpper()))
                        {
                            //values[i] = values[i].ToUpper().Replace(item.ToString().ToUpper(), context.CapturedValues[item.ToString().ToUpper()]);
                            values[i] = values[i].Replace(item.ToString(), context.CapturedValues[item.ToString().ToUpper()]);

                        }

                        else
                        {
                            string[] val = this.logicalFieldValue.Split(' ');
                            values[i] = val[i];
                        }
                    }
                }
                this.logicalFieldValue = string.Join(" ", values);
            }
        }
    }
}

