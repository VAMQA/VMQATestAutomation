namespace VM.Platform.TestAutomationFramework.Core
{
    using System.Collections.Generic;

    public class TestStep
    {
        public string Action { get; set; }
        public string ExpectedResult { get; set; }

        public IEnumerable<TestAttachment> Attachments { get; set; }
    }

    public class SharedTestStep : TestStep
    {
        public string Id { get; set; }
    }
}