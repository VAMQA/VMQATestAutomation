using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.WebServices.TestAutomationFramework.Common
{
    public class TestSuite:FWLogger
    {
        public Dictionary<string,List<TestCase>> TestCases { get; set; }     

        public TestSuite(DataTable dtTestSuite)
        {
            
            DataTable dtDistinctTestSuite = dtTestSuite.DefaultView.ToTable(true, Constants.TESTCASE);
            TestCases = new Dictionary<string, List<TestCase>>();
            foreach (DataRow testcase in dtDistinctTestSuite.Rows)
            {
                if (testcase[Constants.TESTCASE] != null)
                {

                    var CurrentTestCase = testcase[Constants.TESTCASE].ToString().Trim();
                    if (CurrentTestCase != "")
                    {
                        string strFilter = string.Format("[{0}]='{1}'", Constants.TESTCASE, CurrentTestCase);
                        var tcSteps = dtTestSuite.Select(strFilter);
                        List<TestCase> lstTestCase = new List<TestCase>();
                        foreach (var tcStep in tcSteps)
                        {
                            lstTestCase.Add(new TestCase(tcStep));
                        }
                        TestCases.Add(CurrentTestCase, lstTestCase);
                    }
                }
            }
        }
    }
}
