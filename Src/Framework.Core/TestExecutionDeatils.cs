

namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.Collections.Generic;
  public  class TestExecutionDeatils
    {

      public TestExecutionDeatils()
      {
          this.TotalconditionsPassed = new List<int>();
          this.TotalconditionsFailed = new List<int>();
         
      }
        // Details Tab
        public int TotalConditionExecution { get; set; }
        public List<int> TotalconditionsPassed { get; set; }
        public List<int> TotalconditionsFailed { get; set; }
        public string TestExecutionStatus { get; set; }
        public string IsTestAutoExecuted { get; set; }
        public string   ExecutionTimeStamp { get; set; }
        public int CumulativeCondition { get; set; }
        public string TestCaseResult { get; set; }
        public int PrintPassedCondtion { get; set; }
        public int PrintFailedCondtion { get; set; }
       
    }
}
