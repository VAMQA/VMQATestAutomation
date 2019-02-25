using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.WebServices.TestAutomationFramework
{
    public static class Constants
    {
        public const string xlsx = ".xlsx";
        public const string xls = ".xls";
        public const string csv = ".csv";
        public const string xml = ".xml";
        public const string xlsxProvider = "XlsxProvider";
        public const string xlsProvider = "XlsProvider";

        public const string environment = "Environment";
        public const string requestHeader = "RequestHeader";
        public const string executeFlag = "Execute";
        public const string tcAction = "Action";
        public const string testSuite = "TestSuite";
        public const string policy = "Policy";
        public const string billing = "Billing";
        public const string Yes = "Y";

        public const string logFilesPath = @"\Reports";
        public const string logArchieveFilesPath = @"\Reports\LogsArchieve";
        public const string htmlArchieveFilesPath = @"\Reports\HtmlReportArchieve";
        public const string uRI = "URI";
        public const string responseXML = "ResponseXML";
        public const string configFile = "\\Config.xml";
        public const string xmlTemplateGenPolNbrPath = @"\XMLTemplates\GeneratePolicyNumber.xml";
        public const string generatePolicy = "Generate Policy";

        public const string cStep = "Steps=";
        public const string cURI = "URI=";
        public const string cAction = "Action=";
        public const string cEnv = "Environment=";
        public const string cUserStory = "UserStory=";


        public const string cTestCaseID = "TestCaseID=";

        public const string sContentType = "application/json";
        public const string sDefaultContentType = "application/xml";

        //Excel FilePaths
        public const string TESTCASEEXCELFILE = @"\TestCases\NonTimeTravelTestCases\PolicyBilling-TestCases.xlsx";
        public const string TESTPolicynumberEXCELFILE = @"\TestCases\NonTimeTravelTestCases\GetPolicynumbers.xlsx";
        public const string TIMETRAVELTESTCASEEXCELFILE = @"\TestCases\TimeTravelTestCases\TimeTravel-TestCases.xlsx";
        public const string TCSTESTDATAFILES = @"\TestCasesXMLData\";

        //Non-Time Travel Scrum level Excel Book
        public const string TESTCOREADC_ASEEXCELFILE = @"\TestCases\NonTimeTravelTestCases\PolicyBilling-CoreADC-TestCases.xlsx";
        public const string TESTCOREVM_ASEEXCELFILE = @"\TestCases\NonTimeTravelTestCases\PolicyBilling-CoreVM-TestCases.xlsx";
        public const string TESTPAYMENTADC_ASEEXCELFILE = @"\TestCases\NonTimeTravelTestCases\PolicyBilling-PaymentADC-TestCases.xlsx";
        public const string TESTDISBURSEMENTADC_ASEEXCELFILE = @"\TestCases\NonTimeTravelTestCases\PolicyBilling-DisbursementADC-TestCases.xlsx";
        public const string TESTPAYMENTVM_ASEEXCELFILE = @"\TestCases\NonTimeTravelTestCases\PolicyBilling-PaymentVM-TestCases.xlsx";

        //Time Travel Scrum level Excel Book
        public const string TIMETRAVELCOREADC_ASEEXCELFILE = @"\TestCases\TimeTravelTestCases\TimeTravel-CoreADC-TestCases.xlsx";
        public const string TIMETRAVELCOREVM_ASEEXCELFILE = @"\TestCases\TimeTravelTestCases\TimeTravel-CoreVM-TestCases.xlsx";
        public const string TIMETRAVELPAYMENTADC_ASEEXCELFILE = @"\TestCases\TimeTravelTestCases\TimeTravel-PaymentADC-TestCases.xlsx";
        public const string TIMETRAVELPAYMENTVM_ASEEXCELFILE = @"\TestCases\TimeTravelTestCases\TimeTravel-PaymentVM-TestCases.xlsx";
        public const string TIMETRAVELDISBURSEMENT_ASEEXCELFILE = @"\TestCases\TimeTravelTestCases\TimeTravel-Disbursements-TestCases.xlsx";
        public const string SMOKETEST_ASEEXCELFILE = @"\TestCases\NonTimeTravelTestCases\PolicyBilling-Smoke-TestCases.xlsx";

        public const string SMOKETEST_ASEUMBRELLAEXCELFILE = @"\TestCases\NonTimeTravelTestCases\PolicyBilling-SmokeUmbrella-TestCases.xlsx";

        //Sheet Names
        public const string TESTSUITE = "TestSuite";
        public const string POLICIES = "Policies";
        public const string INPUTXPATH = "InputXpath";
        //public const string INPUTXPATH = "Policy.Rq.xml";
        public const string OUTPUTXPATH = "OutputXpath";
        public const string ENVIRONMENT = "Environment";
        public const string XPATHREPOSITORY = "XPathRepository";
        public const string CUSTOMHEADER = "CustomHeader";

        //Logger Names FileNames

        public const string LOGFILEHEADER = "**************************************************************************";
        public const string LOGSTARTTESTCASEHEADER = "***********************************StartTestCaseExecution**************************************";
        public const string LOGENDTESTCASEHEADER = "***********************************EndTestCaseExecution**************************************";
        public const string LOGSTARTTESTSTEPHEADER = "***********************************StartStepDetails**************************************";
        public const string LOGENDTESTSTEPHEADER = "***********************************EndStepDetails**************************************";
        public const string ENTERINGMETHOD = "Entering Method ";
        public const string EXITINGMETHOD = "Exiting Method ";

        //others
        public const string TESTCASEID = "TestCaseID=";
        public const string VALIDATE = "Validate";
        public const string NOVALIDATE = "NoValidate";

        //XML Template Paths
        public const string TIMETRAVELXMLTEMPLATEPATH = @"\TestCases\TimeTravelTestCases\TestDataXMLs\";
        public const string XMLTEMPLATEPATH = @"\TestCases\NonTimeTravelTestCases\XMLTemplates\";
        public const string TESTDATAXMLS = @"\TestCases\NonTimeTravelTestCases\TestDataXMLs\";

        //ExcelSheet FiledNames
        public const string TESTCASE = "Test Case";
        public const string XPATH = "Xpath";
        public const string VALUE = "Value";
        public const string XML = "XML";
        public const string KEY = "Key";
        public const string VERIFY = "Key";
        public const string STEPS = "Steps";
        public const string EXECUTE = "Execute";
        public const string URI = "Uri";
        public const string ACTION = "Action";
        public const string USERSTORY = "User Story";
        public const string RQXPATH = "RqXPath";
        public const string RSXPATH = "RsXPath";


        //TestCase Status
        public const string PASS = "Pass";
        public const string FAIL = "Fail";
        public const string NOTEXECUTED = "Not Executed";

        public enum Scrum
        {
            Default,
            PaymentsADC,
            DisbursementsADC,
            Disbursements,
            PaymentsVM,
            CoreADC,
            CoreVM,
            Smoke
        }
    }
}
