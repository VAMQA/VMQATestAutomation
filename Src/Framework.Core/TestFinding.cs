namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    public class TestFinding
    {
        public int? FlowIdentifier { get; set; }
        public int? DataIdentifier { get; set; }
        public string Action { get; set; }
        public object ExpectedResult { get; set; }
        public object ActualResult { get; set; }
        public TestResult TestResult { get; set; }
        public string Value { get; set; }
        public DateTime Time { get; set; }
    }
}
