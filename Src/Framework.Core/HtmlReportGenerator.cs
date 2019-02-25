namespace VM.Platform.TestAutomationFramework.Core
{
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Xsl;

    public class HtmlReportGenerator
    {
        private readonly TestCaseConfiguration testCaseConfiguration;

        public HtmlReportGenerator(TestCaseConfiguration testCaseConfiguration)
        {
            this.testCaseConfiguration = testCaseConfiguration;
        }

        public void Run(TestRunContext testRunContext, TextWriter textWriter,TestRun run)
        {
            var reportTemplate = Properties.Resources.TestRunResultsReportTemplate;
            var styleSheet = XDocument.Parse(reportTemplate);
            var xslt = new XslCompiledTransform();
            xslt.Load(styleSheet.CreateReader(),
                new XsltSettings(false, true),
                new XmlUrlResolver());

            var data = new XDocument(
                new XElement("testRun",
                    new XElement("info",
                        new XElement("testCaseId", testRunContext.TestCase.Id),
                        new XElement("teamProjectName", testCaseConfiguration.ProjectName),
                        new XElement("testPlan", testCaseConfiguration.TestPlan),
                        new XElement("testSuite", testRunContext.TestSuite),
                        new XElement("testResult", testRunContext.TestResult.ToString()),
                        new XElement("Time", run.DateCreated)),
                    new XElement("testFindings")));

            foreach (var testFinding in testRunContext.TestFindings)
            {
                data.Element("testRun").Element("testFindings").Add(
                    new XElement("testFinding",
                            new XElement("flowIdentifier", this.GetValueOrEmptyAsString(testFinding.FlowIdentifier)),
                            new XElement("dataIdentifier", this.GetValueOrEmptyAsString(testFinding.DataIdentifier)),
                            new XElement("action", testFinding.Action),
                            new XElement("expectedResult", testFinding.ExpectedResult),
                            new XElement("actualResult", testFinding.ActualResult),
                            new XElement("testResult", testFinding.TestResult),
                            new XElement("value", testFinding.Value),
                            new XElement("Time", run.DateCreated)));
            }

            xslt.Transform(data.CreateReader(), new XsltArgumentList(), textWriter);
            textWriter.Close();
        }

        private string GetValueOrEmptyAsString(int? nullableValue)
        {
            return nullableValue.HasValue
                ? nullableValue.Value.ToString()
                : string.Empty;
        }
    }
}
