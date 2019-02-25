namespace VM.Platform.TestAutomationFramework.Core
{
    using System;

    public class TestRun
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateStarted { get; set; }
        public string OwnerName { get; set; }
        public int FailedTests { get { return TotalTests - PassedTests - IncompleteTests; } }
        public int PassedTests { get; set; }
        public int TotalTests { get; set; }
        public int IncompleteTests { get; set; }
    }
}