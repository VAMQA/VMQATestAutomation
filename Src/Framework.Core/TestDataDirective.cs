namespace VM.Platform.TestAutomationFramework.Core
{
    using System.Collections.Generic;

    public class TestDataDirective
    {
        public string TestCaseNumber { get; set; }
        public int FlowIdentifier { get; set; }
        public int DataIdentifier { get; set; }
        public string Indicator { get; set; }
        public bool ShouldExecute { get; set; }

        public List<TestDataInteraction> Interactions { get; set; }

        public TestDataDirective()
        {
            ShouldExecute = true;
            this.Interactions = new List<TestDataInteraction>();
        }
    }
}
