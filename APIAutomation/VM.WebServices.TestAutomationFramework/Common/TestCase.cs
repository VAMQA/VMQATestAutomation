using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace VM.WebServices.TestAutomationFramework.Common
{
    public class TestCase
    {
        public string TCNbr;
        public string Steps;
        public string XMLFile;
        public string Execute;
        public string URI;
        public string Environment;
        public string RequestHeader;
        public string Action;
        public string UserStory;
        public XDocument RequestXML;
        public XDocument ResponseXML;
        public string strRequest;
        public string XPathKey;
        public string Data;
        public List<ValidationResult> Results;
        public DateTime StartTime;
        public DateTime EndTime;
        public string Result = Constants.NOTEXECUTED;


        public TestCase(DataRow TestCaseRow)
        {
            //TCNbr = TestCaseRow["Test Case"].ToString().Trim();
            //Steps = TestCaseRow["Steps"].ToString().Trim();
            //Execute = TestCaseRow["Execute"].ToString().Trim();
            //URI = TestCaseRow["URI"].ToString().Trim();
            //Environment = TestCaseRow["Environment"].ToString().Trim();
            //Action = TestCaseRow["Action"].ToString().Trim();
            //UserStory = TestCaseRow["User Story"].ToString().Trim();
            //StartTime = EndTime = DateTime.UtcNow;
            //XMLFile = TestCaseRow["XML"].ToString().Trim();
            TCNbr = TestCaseRow[Constants.TESTCASE].ToString().Trim();
            Steps = TestCaseRow[Constants.STEPS].ToString().Trim();
            Execute = TestCaseRow[Constants.EXECUTE].ToString().Trim();
            URI = TestCaseRow[Constants.URI].ToString().Trim();
            Environment = TestCaseRow[Constants.ENVIRONMENT].ToString().Trim();
            RequestHeader = TestCaseRow[Constants.requestHeader].ToString().Trim();
            Action = TestCaseRow[Constants.ACTION].ToString().Trim();
            UserStory = TestCaseRow[Constants.USERSTORY].ToString().Trim();
            StartTime = EndTime = DateTime.UtcNow;
            XMLFile = TestCaseRow[Constants.XML].ToString().Trim();
        }


    }

    public class ValidationResult
    {
        public string XPath;
        public string ActualValue;
        public string ExpectedValue;
        public string Result;
    }
}
