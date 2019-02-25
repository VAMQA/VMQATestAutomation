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
    using System.Xml.XPath;
    using System.Data.OleDb;
    using VM.Platform.TestAutomationFramework.Core.Configuration;
    using System.Data;

    [CanParse(@"^SaveToDB\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class SaveToDBCommand : InteractionCommand
    {

        private readonly ScriptRunner scriptRunner;
        private string connString;
        public SaveToDBCommand(ExecutionParameters parameters, IUiAdapter uiAdapter, ScriptRunner scriptRunner, TestCaseConfiguration testCaseConfiguration)
            : base(parameters, uiAdapter)
        {
            this.scriptRunner = scriptRunner;
            connString = testCaseConfiguration.DataBaseConnectionString;
        }

        public override void Execute(TestRunContext context)
        {

            try
            {
                string value = this.logicalFieldValue;
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
                    this.SaveToDatabase(context);
                }
            }

            catch (Exception ex)
            {
                this.uiAdapter.TakeScreenshot();
                throw new WorkflowFailedException(
                    string.Format("Could not save data with Content '{0}'.", this.logicalFieldValue),
                    ex);
            }
        }
        private void SaveToDatabase(TestRunContext context)
        {
            try
            {
                string sqlQuery = string.Empty;
                sqlQuery = "update TestData set ActionORData = '" + this.logicalFieldValue + "' where TestCaseID = " + context.TestCaseId + " and Indicator = (select ActionKeyword_ID from ActionKeyword where ActionName='TestData') and MasterORID = (select DISTINCT MasterORID from MasterOR where Label = '" + this.logicalFieldName + "' and ProjectID = (select DISTINCT ProjectID from TestData where TestCaseId=" + context.TestCaseId + " ))";
                System.Data.DataSet result = SqlHelper.ExecuteDataset(connString, System.Data.CommandType.Text, sqlQuery);

            }
            catch (Exception ex)
            {
                string.Format("Could not update, '{0}' in DB.", this.logicalFieldName);
            }

        }
    }
}
