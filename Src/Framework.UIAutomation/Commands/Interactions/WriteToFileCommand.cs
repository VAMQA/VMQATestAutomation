namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    [CanParse(@"^WriteToFile\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class WriteToFileCommand : InteractionCommand
    {        
        private readonly ScriptRunner scriptRunner;

        public WriteToFileCommand(ExecutionParameters parameters, IUiAdapter uiAdapter, ScriptRunner scriptRunner)
            : base(parameters, uiAdapter)
        {
            this.scriptRunner = scriptRunner;
        }

        public override void Execute(TestRunContext context)
        {
            try
                {
                    this.WriteAndSaveTextFile(context);

                }
                catch (Exception ex)
                {
                    this.uiAdapter.TakeScreenshot();
                    throw new WorkflowFailedException(
                        string.Format("Could not save file with Content '{0}'.", this.logicalFieldValue),
                        ex);
                }            
        }
        private void WriteAndSaveTextFile(TestRunContext context)
        {
            SetLogicalFieldValue(context, this.logicalFieldValue);
            var overallConent = this.logicalFieldValue.Split(':');
            string[] content = overallConent[0].Split('/');
            var filename = overallConent[1];
            var fileLoc = overallConent[2];

            if (File.Exists(@".\\temp.txt"))
            {
                File.Delete(@".\\temp.txt");                
                File.WriteAllLines(@".\\temp.txt", content);
                File.Copy(@".\\temp.txt", fileLoc + filename+".idx");
                File.Copy(@".\\claim.jpg", fileLoc + filename+".jpg");
            }
            else
            {
                File.WriteAllLines(@".\\temp.txt", content);
                File.Copy(@".\\temp.txt", fileLoc + filename+".idx");
                File.Copy(@".\\claim.jpg", fileLoc + filename + ".jpg");
            }
        }

        private void SetLogicalFieldValue(TestRunContext context, string value)
        {   
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
