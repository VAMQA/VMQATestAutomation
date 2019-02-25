namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Workflow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Extensions;
    using VM.Platform.TestAutomationFramework.UIAutomation.Properties;
    using System.Data.SqlClient;
    using System.Text.RegularExpressions;

   // [CanParse(@"^\s*LoadTestDataItem\s*\{\s*(?<CurrentPage>.*)\s*\}\s*$")]
    [CanParse(@"^LoadTestDataItem\s*\{\s*(?<CurrentPage>.+?)\s*(?:-\s*(?<FlowIdentifier>.+?))?\s*(?:\[(?<DiIndicator>.+?)\])?\s*\}\s*$")]
    public class LoadTestDataItemCommand : BaseCommand
    {
        private readonly ExecutionParameters executionParameters;
        private readonly string currentPage;
        private readonly TestStepParser testStepParser;
        private readonly TestCaseConfiguration testCaseConfiguration;

        public LoadTestDataItemCommand(ExecutionParameters executionParameters, TestStepParser testStepParser, TestCaseConfiguration testCaseConfiguration)
            : base(executionParameters)
        {
            this.executionParameters = executionParameters;
            this.testStepParser = testStepParser;
            this.testCaseConfiguration = testCaseConfiguration;
            this.currentPage = this.executionParameters["CurrentPage"];
        }

        public override ExecutionParameters ExecutionParameters
        {
            get { return this.executionParameters; }
        }

        #region Validation methods
        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {
            var isValid = this.HasValidPageName(parameters);
            if (!isValid)
            {
                throw new ArgumentException(Resources.InvalidPageName, "parameters");
            }
        }

        private bool HasValidPageName(IReadOnlyDictionary<string, string> parameters)
        {
            return parameters.ContainsKey("CurrentPage") &&
                   parameters["CurrentPage"].IsAlphaNumeric(x => x.Length > 0);
        }

        #endregion

        #region Execution methods
        public override void Execute(TestRunContext context)
        {
            context.iSeqNumberIndicator = 0;
            context.getSequenceNumber = addSeqNumToList(context);
            var testDirectivesToRun = from td in context.TestData[this.currentPage.Contains('-') ? this.currentPage.Split('-')[0] : this.currentPage]
                                      where (!(context.DiIndicator =(context.DiStart==0?false:context.DiIndicator))
                                      || (td.DataIdentifier == context.Iteration))
                                            &&
                                            (!context.FlowIdentifier.HasValue || (td.FlowIdentifier == context.FlowIdentifier))
                                            &&
                                            td.ShouldExecute
                                      select td;

            var commandStrings = new List<string>();
            foreach (var testDataDirective in testDirectivesToRun)
            {
                
                //LN commented TFS/Excel Exe
                //commandStrings.AddRange(
                //    testDataDirective.Interactions.Select(
                //        testDataInteraction =>
                //            string.Format("{0} {{{1} || {2}}}",
                //                testDataDirective.Indicator,
                //                testDataInteraction.LogicalFieldName,
                //                GetOrSubstitute(testDataInteraction.LogicalFieldName, testDataInteraction.Value)
                //                    .ToSingleLine())));

                //DB Execution
                commandStrings.Add(
                                   string.Format("{0} {{{1} || {2}}}",
                                    testDataDirective.Indicator,
                                    testDataDirective.Interactions[0].Value,
                    //GetOrSubstitute(testDataDirective, context))
                                    GetOrSubstitute(testDataDirective.Interactions[0].Value, testDataDirective.Interactions[1].Value))
                                    );
            }

            var interactionCommands = this.testStepParser.GetCommands(commandStrings);

            foreach (var interactionCommand in interactionCommands)
            {
                interactionCommand.Execute(context);
            }
        }

        private string GetOrSubstitute(string logicalFieldName, string value)
        {
            return this.testCaseConfiguration.SubstituteValues.ContainsKey(logicalFieldName)
                ? this.testCaseConfiguration.SubstituteValues[logicalFieldName]
                : value;
        }
        public List<string> addSeqNumToList(TestRunContext TestRunContext)
        {

            string querySeqNumTD;
            List<string> objSeq = new System.Collections.Generic.List<string>();
            objSeq.Clear();
            using (SqlConnection sourceConnection = new SqlConnection(testCaseConfiguration.DataBaseConnectionString))
            {
                sourceConnection.Open();
                SqlDataReader myReader = null;
                string pattern = @"^Execute?\s*\d*$";
                var isExecute = testCaseConfiguration.IgnorableColumnPatterns.ToList().FirstOrDefault();

                if (isExecute == pattern)
                    querySeqNumTD = "SELECT distinct T.SeqNumber FROM [TestData] T inner join MasterOR M on t.MasterORID = m.MasterORID inner join PageNames pn on pn.PageID = t.PageID where T.MasterORID = M.MasterORID and TestCaseId = " + TestRunContext.TestCaseId + "  and T.FlowIdentifier=" + TestRunContext.FlowIdentifier + " and Pagename = '" + TestRunContext.CurrentPage + "' order by SeqNumber ASC";
                else
                    querySeqNumTD = "SELECT distinct T.SeqNumber FROM [TestData] T inner join MasterOR M on t.MasterORID = m.MasterORID inner join PageNames pn on pn.PageID = t.PageID where T.MasterORID = M.MasterORID and TestCaseId = " + TestRunContext.TestCaseId + "  and T.[Execute]='Yes' and T.FlowIdentifier=" + TestRunContext.FlowIdentifier + " and Pagename = '" + TestRunContext.CurrentPage + "' order by SeqNumber ASC";

                //querySeqNumTD = "SELECT T.SeqNumber FROM [TestData] T, MasterOR M where T.MasterORID = M.MasterORID and TestCaseId = " + TestRunContext.TestCaseId + " and T.[Execute]='Yes' and T.PageID = (select PageID from PageNames where Pagename = '" + TestRunContext.CurrentPage + "' and ProjectID=" + TestRunContext.ProjectID + ") and T.FlowIdentifier=" + TestRunContext.FlowIdentifier + " order by SeqNumber ASC";
                    SqlCommand myCommand = new SqlCommand(querySeqNumTD, sourceConnection);
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        objSeq.Add(myReader["SeqNumber"].ToString());
                    }
                    myReader.Close();
                return objSeq;
            }

        }
        #endregion
    }
}
