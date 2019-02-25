namespace VM.Platform.TestAutomationFramework.Core
{
    using System.Collections.Generic;

    public class TestCase
    {
        public TestCase(int id)
        {
            this.Id = id;
        }

        public int Id { get; private set; }
        public string Title { get; set; }
        public IEnumerable<TestStep> TestSteps { get; set; }
    } 
}