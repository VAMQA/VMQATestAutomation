namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.Collections.Generic;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using System.Text;

    public class TestRunContext //: Dictionary<string, object>
    {
        public TestRunContext()
        {
            this.TestData = new Dictionary<string, IEnumerable<TestDataDirective>>();    
            this.TestFindings = new List<TestFinding>();
            this.ControlMap = new ControlMap(new Dictionary<string, Dictionary<string, ControlDefinition>>()); 
            this.Conditions = new List<string>();
            this.CapturedValues = new Dictionary<string, string>();
            this.DetailsTab = new TestExecutionDeatils();
        }

        public TestCase TestCase { get; set; }

        public int TestCaseId
        {
            get { return TestCase.Id; }
        }

        public TestExecutionDeatils DetailsTab { get; set; }
        public ControlMap ControlMap { get; set; }
        public Uri Url { get; set; }
        public int StopIteration { get; set; }
        public int TestRunID { get; set; }
        public string CurrentPage { get; set; }
        public bool DiIndicator { get; set; }
        public bool IsRandomExecution { get; set; }
        public string RequestedEnvironment { get; set; }
        public int Retry { get; set; }
        public int ProjectID { get; set; }  
        public string RetentionId { get; set; }
        public bool IsCleanUpWebDriverEndOfIteration { get; set; }

        /// <summary>
        /// Gets or sets the iteration. Used to identify the data to be entered in a certain page for a given iteration or condition.
        /// Also known as condition or data-identifier (in the excel data sheets).
        /// </summary>
        /// <value>
        /// The iteration.
        /// </value>
        public int? Iteration { get; set; }
        public int? ReqIdentifier { get; set; }
        public bool ExecutionIsStartingFromTheMiddle { get; set; }
        public int DiLoopVariable { get; set; } // This is the current iteration
        public int DiStart { get; set; }    // First iteration to run
        public int DiStop { get; set; }     // Last iteration to run
        public int TestStepIndex { get; set; } //changed
        public LinkedListNode<ITestCommand> StartIterationCommand { get; set; }// Changed name from IterationStartRowNum
        public int StepPositionId { get; set; } // TODO : Figure out what this means, and should we improve the implementation
        public Dictionary<string, IEnumerable<TestDataDirective>> TestData { get; set; }
        public LinkedListNode<ITestCommand> CurrentCommand { get; set; }
        public LinkedListNode<ITestCommand> NextCommand { get; set; }
        public TestResult TestResult { get; set; }
        public StringBuilder sb = new StringBuilder();
        /// <summary>
        /// Gets or sets which workflow will be used by a LoadTestDataItem step
        /// </summary>
        /// <value>
        /// The flow identifier.
        /// </value>
        public int? FlowIdentifier { get; set; }

        public string TestSuite { get; set; }
        public List<TestFinding> TestFindings { get; set; }
        public List<string> Conditions { get; private set; }
        public Dictionary<string, string> CapturedValues { get; set; }

        public bool HasAnotherIterationToRun { get { return DiStop > Iteration; } }

        public void MoveToNextIteration()
        {
            this.Iteration += 1;
        }
        public List<string> getSequenceNumber { get; set; }

        public int iSeqNumberIndicator { get; set; }
        public bool debugMode { get; set; }

        public string debugFromSeqNumber { get; set; }
        public string previousPage { get; set; }

        public string debugReturnValue { get; set; }

        public int debugRetryCount { get; set; }
    }
}
