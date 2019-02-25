namespace VM.Platform.TestAutomationFramework.Core
{
    public class ReportFailureToHtml
    {
        public TestRunContext Run(TestRunContext context, string Action, string value, TestResult Status)
        {
            //Report the Error to HTML REPORT
            var testFinding = new TestFinding
            {
                FlowIdentifier = context.FlowIdentifier,
                DataIdentifier = context.Iteration,
                Action = Action,
                Value = value,
                TestResult = TestResult.Fail
            };
            context.TestFindings.Add(testFinding);
            return context;
        }
    }
}
