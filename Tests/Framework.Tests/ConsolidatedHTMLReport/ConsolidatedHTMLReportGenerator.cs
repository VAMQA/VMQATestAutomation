using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Tests.Auto;
using VM.Platform.TestAutomationFramework.Core;

namespace Framework.Tests.ConsolidatedHTMLReport
{
    public class ConsolidatedHTMLReportGenerator
    {
        public static void GenerateHTMLReport(List<Auto.AutoCustomerTests.ConsolidatedTestResults> Results, TimeSpan OveallExeTime)
        {
            string Head = @"<HTML><head><style> button{width: 30px;
	        height: 30px;
	        border-radius: 30px;
	        font-size: 20px;
	        color: darkgreen;
	        text-shadow: 0 1px 0 #666;
	        text-align: center;
	        text-decoration: none;
	        background: blue;
	        opacity: .95;
	        margin-right: 0;
	        float: left;
	        }</style> </head>";

            string Script = @"<BODY><script type='text/javascript'>
                       function toggle(e) {
                                var ele = e.target.parentNode.childNodes;
                                if (ele[1].style.display == 'block') {
                                    ele[1].style.display = 'none';
                                    ele[0].innerHTML = '+';
                            }
                            else {
                                ele[1].style.display = 'block';
                                ele[0].innerHTML = '-';
                              

                            }
                            }</script>";

            string Body1 = @"<TABLE BORDER=0 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>
                                 <TR>
                                     <TD COLSPAN=6 BGCOLOR=#191970 style='border:solid black 0.5px' align='center'><FONT FACE=VERDANA COLOR=WHITE SIZE=4><B>Test Automation Results</B></FONT></TD>                               
                                 </TR>";
            string Body2_1 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#191970 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=3><B>Total Test Cases :</B>{0}</FONT></TD>
                                </TR>", Results.Count);
            int PassCount = 0;
            int FailCount = 0;
            int NotExecuted = 0;
            var DateExecuted = DateTime.Now;
            int ResCount = Results.Count;
            //int TestCount = Results[0].TestFindings.Count;
            TimeSpan TotalExecutonTime = OveallExeTime;
            string[] Split = TotalExecutonTime.ToString().Split('.');
            string ExecutionTime = Split[0];
            string[] SplitTime = ExecutionTime.Split(':');
            string ExecutionSeconds = SplitTime[2];
            string ExecutionMinutes = SplitTime[1];


            foreach (Auto.AutoCustomerTests.ConsolidatedTestResults tcResult in Results)
            {
                //if (tcResult.TestFindings.Count > 0)
                //{
                //    if (tcResult.TestFindings.Where(v => v.TestResult == TestResult.Fail).Count() > 0)
                //        FailCount++;
                //    else if (tcResult.TestFindings.Where(v => v.TestResult == TestResult.Pass).Count() == tcResult.TestFindings.Count)
                //        PassCount++;
                //    else if (tcResult.TestFindings.Where(v => v.TestResult == TestResult.Pass).Count() > 0)
                //        FailCount++;
                //    else
                //        NotExecuted++;
                //}
                //else
                //{
                    if (tcResult.testResult == "Fail")
                        FailCount++;
                    else if (tcResult.testResult == "Pass")
                        PassCount++;
                    else
                        NotExecuted++;
               // }

            }

            string Body2_2 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#191970 ><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Failed :</B>{0}</FONT></TD>
                                 </TR>", FailCount);
            string Body2_3 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#191970 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Passed :</B>{0}</FONT></TD>
                                 </TR>", PassCount);
            string Body2_4 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#191970 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Not Executed :</B>{0}</FONT></TD>
                                 </TR>", NotExecuted);

            string Body2_5 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#191970 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Date Executed :</B>{0}</FONT></TD>
                                 </TR>", DateExecuted);
            string Body2_6 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#191970 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Total Execution Time :</B>{0}</FONT></TD>"
                                 , ExecutionMinutes + "." + ExecutionSeconds + "Minutes </TR>");

            string Body3 = @"  <TR COLS=2>
                                  <TD BGCOLOR=#8B4513 WIDTH=10% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>TestCase</B></FONT></TD>
                                  <TD BGCOLOR=#8B4513 WIDTH=90% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Details</B></FONT></TD>
                              </TR>";

            string Report = Head + Script + Body1 + Body2_1 + Body2_2 + Body2_3 + Body2_4 + Body2_5 + Body3;

            foreach (Auto.AutoCustomerTests.ConsolidatedTestResults tcResult in Results)
            {
                string testLogPath = Directory.GetCurrentDirectory() + "\\" + "Execution.log";
                //var testLogPath = Directory.GetCurrentDirectory() + "\\" + "Execution.log";
                //string OverallTestResult = tcResult.TestFindings.Count != 0 ? (tcResult.TestFindings.Where(v => v.TestResult == TestResult.Fail).Count() > 0 ? "Fail" : "Pass") : (tcResult.testResult == "Pass" ? "Pass" : "Fail");
                string OverallTestResult = tcResult.testResult;
                Report += " <TR COLS=2>";
                Report += String.Format("<TD WIDTH=15% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=5></FONT>");
                //Report += "<a href ='" + tcResult.testCaseId + "'>" + "TestCaseID </a>";
                Report += "<a href ='" + testLogPath + "'><B>" + tcResult.testCaseId + "</B></a>";                
                Report += "</TD>";
                Report += "<TD WIDTH=85% ALIGN=CENTER style='border:solid black 0.5px'>";
                Report += "<div>";



                if (tcResult.clmPolicyNbr!=null)
                {
                    string StepString = @"<TABLE BORDER=0 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>
                                                        <TR COLS=9>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>ClaimNumber</B></FONT></TD>
                                                        <TD  BGCOLOR=#00BFFF WIDTH=15% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Result</B></FONT></TD>
                                                        <TD  BGCOLOR=#00BFFF WIDTH=55% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Validation</B></FONT></TD>
                                                        </TR>";
                    Report += StepString;

                }
                else
                {
                    string StepString = @"<TABLE BORDER=0 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>
                                                        <TR COLS=9>

                                                        <TD  BGCOLOR=#00BFFF WIDTH=15% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Result</B></FONT></TD>
                                                        <TD  BGCOLOR=#00BFFF WIDTH=55% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Validation</B></FONT></TD>
                                                        </TR>";

                    Report += StepString;
                }
                Report += " <TR COLS=3>";
                if (tcResult.clmPolicyNbr != null)
                {
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>{0}</B></FONT></TD>", tcResult.clmPolicyNbr);
                }
                if (OverallTestResult == "Pass")
                {
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=GREEN SIZE=3><B>{0}</B></FONT></TD>", OverallTestResult);
                }
                else Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=RED SIZE=3><B>{0}</B></FONT></TD>", OverallTestResult);


                Report += String.Format("<TD  WIDTH=55% ALIGN=CENTER><FONT FACE=VERDANA COLOR=BLACK SIZE=2>");
                Report += "<div>";

                Report += "<button onclick=toggle(event) style='Background-color:lightgrey; width:8%' >+</button>";
                Report += "<div style='display:none;' style='width:92%;float:left;'>";

                string ValidationString = @"<TABLE BORDER=0  CELLPADDING=3 CELLSPACING=1 WIDTH=100%>
                                                                    <TR COLS=4>
                                                                    <TD  BGCOLOR=LIGHTGREY WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>FlowIdentifier</B></FONT></TD>
                                                                    
                                                                    <TD  BGCOLOR=LIGHTGREY WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>DataIdentifier</B></FONT></TD>
                                                                    <TD  BGCOLOR=LIGHTGREY WIDTH=40% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>       Action         </B></FONT></TD>

                                                                    <TD  BGCOLOR=LIGHTGREY WIDTH=15% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>ExpectedResult</B></FONT></TD>

                                                                    <TD  BGCOLOR=LIGHTGREY WIDTH=15% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>ActualResult</B></FONT></TD>

                                                                    <TD  BGCOLOR=LIGHTGREY WIDTH=20% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>  Time  </B></FONT></TD>

                                                                    <TD  BGCOLOR=LIGHTGREY WIDTH=10% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>TestResult</B></FONT></TD>
                                                                    
                                                                    </TR>";


                Report += ValidationString;




                foreach (var testFindings in tcResult.TestFindings)
                {
                    Report += " <TR COLS=4>";
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>{0}</B></FONT></TD>", testFindings.FlowIdentifier);
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>{0}</B></FONT></TD>", testFindings.DataIdentifier);
                    Report += String.Format("<TD  WIDTH=40% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>{0}</B></FONT></TD>", testFindings.Action);
                    Report += String.Format("<TD  WIDTH=15% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>{0}</B></FONT></TD>", testFindings.ExpectedResult);
                    Report += String.Format("<TD  WIDTH=15% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>{0}</B></FONT></TD>", testFindings.ActualResult);
                    Report += String.Format("<TD  WIDTH=20% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>{0}</B></FONT></TD>", testFindings.Time);

                    //if (testFindings.ScreenshotPath != "" && testFindings.ScreenshotPath != null)
                    // Report += "<a href ='" + testFindings.Time + "'>" + "Time </a>";
                    //Report += "</TD>";
                    if (testFindings.TestResult.ToString() == "Pass")
                    {
                        Report += String.Format("<TD  WIDTH=10% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=GREEN SIZE=2><B>{0}</B></FONT></TD>", testFindings.TestResult);
                    }
                    else Report += String.Format("<TD  WIDTH=10% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=RED SIZE=2><B>{0}</B></FONT></TD>", testFindings.TestResult);

                    //Report += String.Format("<TD  WIDTH=10% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testFindings.Value);
                    Report += "</TR>";
                }

                Report += "</TABLE>";
                Report += "</div>";
                Report += "</div>";
                Report += "</TD>";
                Report += "</TR>";
                Report += "</TABLE>";
                Report += "</div>";
                Report += "</TD>";
                Report += "</TR>";
            }
            Report += "</TR>";
            Report += "</TABLE>";
            Report += "</BODY>";
            Report += "</HTML>";

            DateTime CD = DateTime.UtcNow;
            string CurrentDate = string.Format("{0}-{1}-{2}_{3}-{4}-{5}-{6}", CD.Year, CD.Month, CD.Day, CD.Hour, CD.Minute, CD.Second, CD.Millisecond);
            File.WriteAllText(@".\\"+System.Environment.UserName+"_ConsolidatedReport" + CurrentDate + ".html", Report);
            
        }
    }

    public class ListConsolidatedResults
    {
        public static List<Auto.AutoCustomerTests.ConsolidatedTestResults> ConsolidatedTestCaseResults = new List<Auto.AutoCustomerTests.ConsolidatedTestResults>();
    }
}