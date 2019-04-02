using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
using System.Linq;
using System.Data.OleDb;
using ClosedXML.Excel;
using RandomNameGenerator;
using System.Globalization;


namespace VM.WebServices.TestAutomationFramework.Common
{
    public class Library : FWLogger
    {
        public static Dictionary<string, string> Keywords = new Dictionary<string, string>();

        public static DataTable ReadExcel(string excelFileName, string sheetName)
        {
            FileInfo xlFileInfo = new FileInfo(excelFileName);
            if (!xlFileInfo.Exists) { throw new Exception("Error, File Info doesn't exists!"); }
            string xlFileExtn = xlFileInfo.Extension;
            string strProvider = xlFileExtn == Constants.xlsx ? Constants.xlsxProvider : Constants.xlsProvider;
            XmlNode nodeProvider = XmlManipulator.ReadSingleNodeByXPath(GetPath() + Constants.configFile, strProvider, null);
            OleDbConnection connection = new OleDbConnection(nodeProvider.InnerText + excelFileName + ";");
            DataTable dtTable = new DataTable();
            OleDbDataAdapter command = new OleDbDataAdapter("select * from [" + sheetName + "$]", connection);
            command.TableMappings.Add("Table", sheetName);
            dtTable.TableName = sheetName;
            command.Fill(dtTable);
            return dtTable;
        }

        public static DataSet ReadExcel(string excelFileName)
        {
            try
            {
                FileInfo xlFileInfo = new FileInfo(excelFileName);

                if (!xlFileInfo.Exists)
                {
                    Logger.Info("Error, File Info doesn't exists!. File name : " + excelFileName);
                    throw new Exception("Error, File Info doesn't exists!. File name : " + excelFileName);
                }

                string xlFileExtn = xlFileInfo.Extension;

                if (xlFileExtn != Constants.xlsx)
                {
                    Logger.Info("Error, File type not supported!. Given Excel File name : " + excelFileName);
                    throw new Exception("Error, File type not supported!. Given Excel File name : " + excelFileName);
                }

                var workBook = new XLWorkbook(excelFileName);

                DataSet ds = new DataSet();

                DataTable dtTable = new DataTable();

                bool firstRow = true;

                foreach (IXLWorksheet worksheet in workBook.Worksheets)
                {

                    dtTable = new DataTable();
                    foreach (IXLRow xlrow in worksheet.Rows())
                    {
                        if (firstRow)
                        {
                            foreach (IXLCell cell in xlrow.Cells())
                            {
                                dtTable.Columns.Add(cell.Value.ToString());
                            }
                            firstRow = false;
                        }
                        else
                        {
                            dtTable.Rows.Add();
                            int i = 0;
                            foreach (IXLCell cell in xlrow.Cells())
                            {
                                dtTable.Rows[dtTable.Rows.Count - 1][i] = cell.Value.ToString();
                                i++;
                            }
                        }
                    }
                    dtTable.TableName = worksheet.Name;
                    if (!ds.Tables.Contains(dtTable.TableName))
                        ds.Tables.Add(dtTable);
                    firstRow = true;

                }

                return ds;
            }
            catch (Exception e)
            {
                Logger.Info("ReadExcel : " + (e.InnerException == null ? e.StackTrace.ToString() : e.InnerException.ToString()));
                throw e;
            }
        }

        public static string GetPath()
        {
            string path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            return path;
        }

        public static string GetSolutionPath()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())))); 
        }


        public static string EvaluateXPath(XDocument XMLDoc, string Xpath)
        {
            string ActualVal = "";
            try
            {
                //XDocument XDoc = XDocument.Parse(XMLDoc.OuterXML);
                //XDocument XDoc1 = XDocument.Parse(XDoc.OuterXml);
                //var Res = XDoc.XPathEvaluate(Xpath);
                //IEnumerable Res1 = Res as IEnumerable;

                //if (Res1 != null)
                //{

                    //    if (Res1.OfType<XAttribute>().Count() > 0)
                    //    {
                    //        ActualVal = Res1.Cast<XAttribute>().FirstOrDefault().Value;
                    //    }
                    //    else if (Res1.OfType<XElement>().Count() > 0)
                    //    {
                    //        ActualVal = Res1.Cast<XElement>().FirstOrDefault().Value;
                    //    }
                    //    else
                    //    {
                    //        ActualVal = Res.ToString();
                    //    }

                    //}

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(XMLDoc.ToString());
                   // XmlNodeList n = doc.GetElementsByTagName(Xpath.Replace("//", "").ToString());
                    XmlNodeList n = doc.SelectNodes(Xpath.ToString());
    
                if (n.Count > 1)
                    {
                        for (int i = n.Count; i < n.Count; i++)
                        {
                            ActualVal = n[i].InnerText.Trim();
                        }

                    }
                    else
                    {
                        ActualVal = n[0].InnerText.Trim();
                    }
                

                //else
                //{

                //    ActualVal = Res.ToString();
                //}

            }
            catch (Exception e)
            {
                Logger.Info("EvaluateXPath : " + (e.InnerException == null ? e.StackTrace.ToString() : e.InnerException.ToString()));
                throw e;
            }
            return ActualVal;
        }

        public static string BuildJSonRequestFromExcel(DataTable DT, string TestCaseName)
        {
            try
            {
                DataTable FilteredTable = DT.DefaultView.ToTable(false, new string[] { "Parameter", TestCaseName });
                string ReturnXML = "";
                foreach (DataRow R in FilteredTable.Rows)
                {
                    string TCN = R[TestCaseName] == null ? "" : R[TestCaseName].ToString().Trim();
                    string XMLStr = R["Parameter"] == null ? "" : R["Parameter"].ToString().Trim();

                    if (TCN.ToUpper().Contains("~"))
                    {
                        foreach (string K in Keywords.Keys)
                        {
                            TCN = TCN.Replace(K, Keywords[K]);
                        }
                    }
                    if (TCN != "" && TCN != "NA")
                    {
                        if (ReturnXML.Trim().Length != 0) { ReturnXML += ","; }
                        ReturnXML += "\"" + XMLStr + "\": " + "\"" + TCN + "\"";
                    }
                    else
                    {
                        //ReturnXML += R["RequestXML"] + "\n";
                    }
                }


                if (FilteredTable.Rows.Count == 0)
                {
                    Logger.Info("Error, JSon Test case mapping not located in Test Data Sheet!. Test case name : " + TestCaseName);
                    throw new Exception("Error, JSon Test case mapping not located in Test Data Sheet!. Test case name : " + TestCaseName);
                    return "";
                }
                else { return "{" + ReturnXML + "}"; }

            }
            catch (Exception e)
            {
                Logger.Info("BuildJSonRequestFromExcel : " + (e.InnerException == null ? e.StackTrace.ToString() : e.InnerException.ToString()));
                throw e;
            }
        }

        public static void GenerateHTMLReport1(TestSuite TS)
        {
            string Head = @"<HTML><head><style> button{width: 30px;
	        height: 30px;
	        border-radius: 30px;
	        font-size: 20px;
	        color: darkgreen;
	        text-shadow: 0 1px 0 #666;
	        text-align: center;
	        text-decoration: none;
	        background: skyblue;
	        opacity: .95;
	        margin-right: 0;
	        float: left;
	        }</style> </head>";


            string Body1 = @"<TABLE BORDER=0 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>
                                 <TR>
                                      <TD COLSPAN=6 BGCOLOR=#66699 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>VM Web Services Automation Result</B></FONT></TD>
                                 </TR>";
            string Body2_1 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#66699 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Total Test Cases :</B>{0}</FONT></TD>
                                 </TR>", TS.TestCases.Keys.Count);
            int PassCount = 0;
            int FailCount = 0;
            int NotExecuted = 0;
            foreach (List<TestCase> TCL in TS.TestCases.Values)
            {
                if (TCL.Where(v => v.Result == Constants.FAIL).Count() > 0)
                    FailCount++;
                else if (TCL.Where(v => v.Result == Constants.PASS).Count() == TCL.Count)
                    PassCount++;
                else if (TCL.Where(v => v.Result == Constants.PASS).Count() > 0)
                    FailCount++;
                else
                    NotExecuted++;

            }

            string Body2_2 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#66699 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Failed :</B>{0}</FONT></TD>
                                 </TR>", FailCount);
            string Body2_3 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#66699 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Passed :</B>{0}</FONT></TD>
                                 </TR>", PassCount);
            string Body2_4 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#66699 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Not Executed :</B>{0}</FONT></TD>
                                 </TR>", NotExecuted);


            string Body3 = @"  <TR COLS=2>
                                  <TD BGCOLOR=orange WIDTH=15% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>TestCase</B></FONT></TD>
                                  <TD BGCOLOR=orange WIDTH=85% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>Details</B></FONT></TD>
                              </TR>";
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
            string Report = Head + Script + Body1 + Body2_1 + Body2_2 + Body2_3 + Body2_4 + Body3;

            foreach (string tcid in TS.TestCases.Keys)
            {
                List<TestCase> testSteps = TS.TestCases[tcid];
                if (testSteps.All(step => step.Execute == Constants.Yes))
                {

                    Report += " <TR COLS=2>";
                    Report += String.Format("<TD WIDTH=15% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>{0}</B></FONT></TD>", tcid);
                    Report += "<TD WIDTH=85% ALIGN=CENTER style='border:solid black 0.5px'>";
                    Report += "<div>";
                    Report += "<button onclick=toggle(event) style='width:4%'>+</button>";
                    Report += "<div style='display:none;' style='width:96%;float:left;'>";
                    string StepString = @"<TABLE BORDER=0 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>
                                                        <TR COLS=9>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>StepNumber</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>StartTime</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>EndTime</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Execute</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Action</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Environment</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>URI</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>XMLFile</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Result</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=55% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Validation</B></FONT></TD>
                                                        </TR>";
                    Report += StepString;
                    foreach (TestCase testcase in TS.TestCases[tcid])
                    {
                        Report += " <TR COLS=9>";
                        Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.Steps);
                        Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.StartTime.ToString());
                        Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.EndTime.ToString());
                        Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.Execute);
                        Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.Action);
                        Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.Environment);
                        Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.URI);
                        Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.XMLFile);
                        if (testcase.Result.ToLower() == "pass")
                            Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=GREEN SIZE=1><B>{0}</B></FONT></TD>", testcase.Result);
                        else if (testcase.Result.ToLower() == "fail")
                            Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=RED SIZE=1><B>{0}</B></FONT></TD>", testcase.Result);
                        else
                            Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.Result);
                        Report += String.Format("<TD  WIDTH=55% ALIGN=CENTER><FONT FACE=VERDANA COLOR=BLACK SIZE=1>");

                        #region Validation Results
                        if (testcase.Results != null && testcase.Results.Count > 0)
                        {

                            Report += "<div>";
                            Report += "<button onclick=toggle(event) style='width:8%'>+</button>";
                            Report += "<div style='display:none;' style='width:92%;float:left;'>";
                            string ValidationString = @"<TABLE BORDER=0  CELLPADDING=3 CELLSPACING=1 WIDTH=100%>
                                                                <TR COLS=4>
                                                                <TD  BGCOLOR=LIGHTGREY WIDTH=50% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Xpath</B></FONT></TD>
                                                                <TD  BGCOLOR=LIGHTGREY WIDTH=20% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Actual</B></FONT></TD>
                                                                <TD  BGCOLOR=LIGHTGREY WIDTH=20% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Expected</B></FONT></TD>
                                                                <TD  BGCOLOR=LIGHTGREY WIDTH=10% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Result</B></FONT></TD>
                                                                </TR>";

                            Report += ValidationString;
                            foreach (ValidationResult Y in testcase.Results)
                            {
                                Report += " <TR COLS=4>";
                                Report += String.Format("<TD  WIDTH=50% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", Y.XPath);
                                Report += String.Format("<TD  WIDTH=20% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", Y.ActualValue);
                                Report += String.Format("<TD  WIDTH=20% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", Y.ExpectedValue);
                                Report += String.Format("<TD  WIDTH=10% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", Y.Result);
                                Report += "</TR>";


                            }
                            Report += "</TABLE>";
                            Report += "</div>";
                            Report += "</div>";

                        }
                        else
                        {
                            Report += "No Results to display";
                        }
                        Report += "</FONT></TD>";
                        Report += "</TR>";
                        #endregion
                    }
                    Report += "</TABLE>";
                    Report += "</TD>";
                    Report += "</div>";
                    Report += "</div>";
                    Report += "</TR>";
                }
            }

            Report += "</TABLE>";
            Report += "</BODY>";
            Report += "</HTML>";

            //string strFilename = "";
            //strFilename = "\\HTMLReport" + DateTime.UtcNow.ToString(@"yyyy-MM-dd") + ".html";

            //if (File.Exists(Library.GetPath() + Constants.logFilesPath + strFilename))
            //{
            //    File.AppendAllText(Library.GetPath() + Constants.logFilesPath + strFilename, Report);
            //}
            //else
            //{
            //    File.WriteAllText(Library.GetPath() + Constants.logFilesPath + strFilename, Report);
            //}

            DateTime CD = DateTime.UtcNow;
            string CurrentDate = string.Format("{0}-{1}-{2}_{3}-{4}-{5}-{6}", CD.Year, CD.Month, CD.Day, CD.Hour, CD.Minute, CD.Second, CD.Millisecond);
            File.WriteAllText(Directory.GetCurrentDirectory() + Constants.logFilesPath + "\\HTMLReport" + CurrentDate + ".html", Report);

        }

        public static void GenerateHTMLReport(TestSuite TS, string TC)
        {
            string Head = @"<HTML><head><style> button{width: 30px;
	        height: 30px;
	        border-radius: 30px;
	        font-size: 20px;
	        color: darkgreen;
	        text-shadow: 0 1px 0 #666;
	        text-align: center;
	        text-decoration: none;
	        background: skyblue;
	        opacity: .95;
	        margin-right: 0;
	        float: left;
	        }</style> </head>";


            string Body1 = @"<TABLE BORDER=0 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>
                                 <TR>
                                      <TD COLSPAN=6 BGCOLOR=#66699 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>VM Web Services Automation Result</B></FONT></TD>
                                 </TR>";
            string Body2_1 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#66699 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Total Test Cases :</B>{0}</FONT></TD>
                                 </TR>", TS.TestCases.Keys.Count);
            int PassCount = 0;
            int FailCount = 0;
            int NotExecuted = 0;

            var tc = TS.TestCases
             .Where(dict => dict.Key.Contains(TC))
             .Select(dict => dict.Value)
             .ToList();

            foreach (List<TestCase> TCL in tc)
            {
                if (TCL.Where(v => v.Result == Constants.FAIL).Count() > 0)
                    FailCount++;
                else if (TCL.Where(v => v.Result == Constants.PASS).Count() == TCL.Count)
                    PassCount++;
                else if (TCL.Where(v => v.Result == Constants.PASS).Count() > 0)
                    FailCount++;
                else
                    NotExecuted++;

            }

            string Body2_2 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#66699 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Failed :</B>{0}</FONT></TD>
                                 </TR>", FailCount);
            string Body2_3 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#66699 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Passed :</B>{0}</FONT></TD>
                                 </TR>", PassCount);
            string Body2_4 = string.Format(@"  <TR>
                                      <TD COLSPAN=6 BGCOLOR=#66699 style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=WHITE SIZE=2><B>Not Executed :</B>{0}</FONT></TD>
                                 </TR>", NotExecuted);


            string Body3 = @"  <TR COLS=2>
                                  <TD BGCOLOR=orange WIDTH=15% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>TestCase</B></FONT></TD>
                                  <TD BGCOLOR=orange WIDTH=85% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>Details</B></FONT></TD>
                              </TR>";
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
            string Report = Head + Script + Body1 + Body2_1 + Body2_2 + Body2_3 + Body2_4 + Body3;

            List<TestCase> testSteps = TS.TestCases[TC];

            if (testSteps.All(step => step.Execute == Constants.Yes))
            {

                Report += " <TR COLS=2>";
                Report += String.Format("<TD WIDTH=15% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=2><B>{0}</B></FONT></TD>", TC);
                Report += "<TD WIDTH=85% ALIGN=CENTER style='border:solid black 0.5px'>";
                Report += "<div>";
                Report += "<button onclick=toggle(event) style='width:4%'>+</button>";
                Report += "<div style='display:none;' style='width:96%;float:left;'>";
                string StepString = @"<TABLE BORDER=0 CELLPADDING=3 CELLSPACING=1 WIDTH=100%>
                                                        <TR COLS=9>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>StepNumber</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>StartTime</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>EndTime</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Execute</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Action</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Environment</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>URI</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>XMLFile</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Result</B></FONT></TD>
                                                        <TD  BGCOLOR=LIGHTBLUE WIDTH=55% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Validation</B></FONT></TD>
                                                        </TR>";
                Report += StepString;
                foreach (TestCase testcase in TS.TestCases[TC])
                {
                    Report += " <TR COLS=9>";
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.Steps);
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.StartTime.ToString());
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.EndTime.ToString());
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.Execute);
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.Action);
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.Environment);
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.URI);
                    Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.XMLFile);
                    if (testcase.Result.ToLower() == "pass")
                        Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=GREEN SIZE=1><B>{0}</B></FONT></TD>", testcase.Result);
                    else if (testcase.Result.ToLower() == "fail")
                        Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=RED SIZE=1><B>{0}</B></FONT></TD>", testcase.Result);
                    else
                        Report += String.Format("<TD  WIDTH=5% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", testcase.Result);
                    Report += String.Format("<TD  WIDTH=55% ALIGN=CENTER><FONT FACE=VERDANA COLOR=BLACK SIZE=1>");

                    #region Validation Results
                    if (testcase.Results != null && testcase.Results.Count > 0)
                    {

                        Report += "<div>";
                        Report += "<button onclick=toggle(event) style='width:8%'>+</button>";
                        Report += "<div style='display:none;' style='width:92%;float:left;'>";
                        string ValidationString = @"<TABLE BORDER=0  CELLPADDING=3 CELLSPACING=1 WIDTH=100%>
                                                                <TR COLS=4>
                                                                <TD  BGCOLOR=LIGHTGREY WIDTH=50% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Xpath</B></FONT></TD>
                                                                <TD  BGCOLOR=LIGHTGREY WIDTH=20% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Actual</B></FONT></TD>
                                                                <TD  BGCOLOR=LIGHTGREY WIDTH=20% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Expected</B></FONT></TD>
                                                                <TD  BGCOLOR=LIGHTGREY WIDTH=10% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>Result</B></FONT></TD>
                                                                </TR>";

                        Report += ValidationString;
                        foreach (ValidationResult Y in testcase.Results)
                        {
                            Report += " <TR COLS=4>";
                            Report += String.Format("<TD  WIDTH=50% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", Y.XPath);
                            Report += String.Format("<TD  WIDTH=20% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", Y.ActualValue);
                            Report += String.Format("<TD  WIDTH=20% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", Y.ExpectedValue);
                            Report += String.Format("<TD  WIDTH=10% ALIGN=CENTER style='border:solid black 0.5px'><FONT FACE=VERDANA COLOR=BLACK SIZE=1><B>{0}</B></FONT></TD>", Y.Result);
                            Report += "</TR>";


                        }
                        Report += "</TABLE>";
                        Report += "</div>";
                        Report += "</div>";

                    }
                    else
                    {
                        Report += "No Results to display";
                    }
                    Report += "</FONT></TD>";
                    Report += "</TR>";
                    #endregion
                }
                Report += "</TABLE>";
                Report += "</TD>";
                Report += "</div>";
                Report += "</div>";
                Report += "</TR>";
            }

            Report += "</TABLE>";
            Report += "</BODY>";
            Report += "</HTML>";

            DateTime CD = DateTime.UtcNow;
            string CurrentDate = string.Format("{0}{1}{2}_{3}-{4}-{5}-{6}", CD.Year, CD.Month, CD.Day, CD.Hour, CD.Minute, CD.Second, CD.Millisecond);
            // File.WriteAllText(Library.GetPath() + Constants.logFilesPath + "\\" + TC + "_HTMLReport" + ".html", Report);
           // File.WriteAllText(Directory.GetCurrentDirectory() + Constants.logFilesPath + "\\" + TC + "_HTMLReport" + ".html", Report);
            File.WriteAllText(Directory.GetCurrentDirectory() + Constants.logFilesPath + "\\HTMLReport" + CurrentDate + ".html", Report);

        }

        public static XDocument BuildXMLFromXPath(string XMLFilePath, string TestCaseID, string XMLFileName, DataTable InputXPathTable, DataTable XPathRepository,string policynumber ="")
        {
            XDocument Local = XDocument.Load(XMLFilePath);

            try
            {

                if (File.Exists(XMLFilePath)) { Local = XDocument.Load(XMLFilePath); }
                else
                {
                    Logger.Info("Error, File Info doesn't exists!. File name : " + XMLFilePath);
                    throw new Exception("Error, File Info doesn't exists!. File name : " + XMLFilePath);
                }

                DataTable FilteredTable = InputXPathTable.DefaultView.ToTable(false, new string[] { "RqXPath", "RsXPath", TestCaseID });

                foreach (DataRow CurrentRow in FilteredTable.Rows)
                {
                    if (CurrentRow[0].ToString() != "NA" && CurrentRow[2].ToString() != "NA" && CurrentRow[0].ToString() != "")
                    {

                        string xPathKeywordFilter = string.Format("([{0}]='{1}')", Constants.KEY, CurrentRow[0].ToString());
                        DataRow[] xPathKeywordRows = XPathRepository.Select(xPathKeywordFilter);
                        string XPath = "";
                        if (xPathKeywordRows.Count() == 1)
                        {
                            foreach (DataRow xpathKeywordRow in xPathKeywordRows)
                            {
                                XPath = xpathKeywordRow[Constants.XPATH].ToString();
                            }
                        }
                        else
                        {
                            Logger.Info("The Validate XPath Repository is having duplicate keys for XPaths. Please rectify them.");
                        }

                        string Value = CurrentRow[2].ToString();
                        if (CurrentRow[0].ToString() == "PPAPIFirstName" || CurrentRow[0].ToString() == "PPAPILastname" || CurrentRow[0].ToString() == "CyclePPAPILastname" || CurrentRow[0].ToString() == "CyclePPAPIFirstName")
                        {
                            Value = NameGenerator.GenerateFirstName(0);
                        }
                        if (Value.StartsWith("~"))
                        {
                            if (Value.Contains("~POLICYNUMBER") | Value.Contains("~ISSUEDPOLICYNUMBER")) { Value = policynumber; } 
                            else
                            Value = Keywords[Value];
                        }
                        var Res = Local.XPathEvaluate(XPath);
                        IEnumerable Res1 = Res as IEnumerable;

                        if (Res1 != null)
                        {

                            if (Res1.OfType<XAttribute>().Count() > 0)
                            {
                                foreach (XAttribute y in Res1)
                                    y.Value = Value;
                            }
                            else if (Res1.OfType<XElement>().Count() > 0)
                            {
                                foreach (XElement y in Res1)
                                    y.Value = Value;
                            }
                            else
                            {
                                Console.WriteLine("Xpath Doesn't Evaluate to any element or attribute---" + XPath);
                            }


                        }
                        else
                        {
                            Console.WriteLine("Xpath Doesn't Evaluate to any element or attribute---" + XPath);
                        }



                    }
                }
            }
            catch (Exception e)
            {
                Logger.Info("BuildXMLFromXPath : " + (e.InnerException == null ? e.StackTrace.ToString() : e.InnerException.ToString()));
                throw e;// new Exception(e.InnerException == null ? e.StackTrace.ToString() : e.InnerException.ToString());
            }
            return Local;
        }

        public static XDocument BuildXMLFromXPathPCN(string XMLFilePath, string TestCaseID, string XMLFileName, DataTable InputXPathTable, DataTable XPathRepository)
        {
            XDocument Local = XDocument.Load(XMLFilePath);
            string strFilter = string.Format("[{0}]='{1}' AND [{2}]= '{3}'", Constants.TESTCASE, TestCaseID, Constants.XML, XMLFileName);
            var FilteredXpathTable = InputXPathTable.Select(strFilter);



            foreach (DataRow CurrentRow in FilteredXpathTable)
            {
                string xPathKeywordFilter = string.Format("([{0}]='{1}')", Constants.KEY, CurrentRow[Constants.XPATH]);
                DataRow[] xPathKeywordRows = XPathRepository.Select(xPathKeywordFilter);
                string XPath = "";
                if (xPathKeywordRows.Count() == 1)
                {
                    foreach (DataRow xpathKeywordRow in xPathKeywordRows)
                    {
                        XPath = xpathKeywordRow[Constants.XPATH].ToString();
                    }
                }
                else
                {
                    Logger.Info("The Validate XPath Repository is having duplicate keys for XPaths. Please rectify them.");
                }

                string Value = CurrentRow[Constants.VALUE].ToString();
                if (Value.StartsWith("~"))
                {
                    Value = Keywords[Value];
                }
                var Res = Local.XPathEvaluate(XPath);
                IEnumerable Res1 = Res as IEnumerable;

                if (Res1 != null)
                {

                    if (Res1.OfType<XAttribute>().Count() > 0)
                    {
                        foreach (XAttribute y in Res1)
                            y.Value = Value;
                    }
                    else if (Res1.OfType<XElement>().Count() > 0)
                    {
                        foreach (XElement y in Res1)
                            y.Value = Value;
                    }
                    else
                    {
                        Console.WriteLine("Xpath Doesn't Evaluate to any element or attribute---" + XPath);
                    }


                }
                else
                {
                    Console.WriteLine("Xpath Doesn't Evaluate to any element or attribute---" + XPath);
                }


            }
            return Local;


        }

        public static XDocument BuildXMLFromXPathPCNTemplate(XDocument XMLFile, string TestCaseID, string XMLFileName, DataTable TemplateXPathTable, DataTable XPathRepository)
        {
            XDocument Local = XMLFile;
            if (!TemplateXPathTable.Columns.Contains(TestCaseID))
                return Local;
            var FilteredXpathTable = new DataView(TemplateXPathTable).ToTable(false, new string[] { "RqXpath", TestCaseID });
            string strFilter = string.Format("[{0}]!='' AND [{1}]!= ''", "RqXpath", TestCaseID);

            var ActualFilteredXpathTable = FilteredXpathTable.Select(strFilter);


            foreach (DataRow CurrentRow in ActualFilteredXpathTable)
            {
                string xPathKeywordFilter = string.Format("([{0}]='{1}')", Constants.KEY, CurrentRow[Constants.XPATH]);
                DataRow[] xPathKeywordRows = XPathRepository.Select(xPathKeywordFilter);
                string XPath = "";
                if (xPathKeywordRows.Count() == 1)
                {
                    foreach (DataRow xpathKeywordRow in xPathKeywordRows)
                    {
                        XPath = xpathKeywordRow[Constants.XPATH].ToString();
                    }
                }
                else
                {
                    Logger.Info("The Validate XPath Repository is having duplicate keys for XPaths. Please rectify them.");
                }

                string Value = CurrentRow[Constants.VALUE].ToString();
                if (Value.StartsWith("~"))
                {
                    Value = Keywords[Value];
                }
                var Res = Local.XPathEvaluate(XPath);
                IEnumerable Res1 = Res as IEnumerable;

                if (Res1 != null)
                {

                    if (Res1.OfType<XAttribute>().Count() > 0)
                    {
                        foreach (XAttribute y in Res1)
                            y.Value = Value;
                    }
                    else if (Res1.OfType<XElement>().Count() > 0)
                    {
                        foreach (XElement y in Res1)
                            y.Value = Value;
                    }
                    else
                    {
                        Console.WriteLine("Xpath Doesn't Evaluate to any element or attribute---" + XPath);
                    }


                }
                else
                {
                    Console.WriteLine("Xpath Doesn't Evaluate to any element or attribute---" + XPath);
                }


            }
            return Local;


        }

       
        public static void ReplaceXMLFromXPath(TestCase TestCaseDetails, string TestCaseID, DataTable OutputXPathTable, DataTable XPathRepository)
        {
            try
            {
                List<string> errors = new List<string>();
                string Xpath = string.Empty;
                XDocument XDoc = TestCaseDetails.ResponseXML;
               // XDocument XDoc = TestCaseDetails.ResponseXML;
                //XmlDocument XDoc = new XmlDocument();
                //var xmldoc = XDoc.ToXmlDocument(XDoc1);
               
                string Filter = string.Format("([{0}]='{1}') AND ([{2}]='{3}')", Constants.TESTCASE, TestCaseID, Constants.XML, TestCaseDetails.XMLFile);
                DataRow[] FilteredXPathTable = OutputXPathTable.Select(Filter);
                foreach (DataRow arrDtRow in FilteredXPathTable)
                {
                    string xPathKeywordFilter = string.Format("([{0}]='{1}')", Constants.KEY, arrDtRow[Constants.XPATH]);
                    DataRow[] xPathKeywordRows = XPathRepository.Select(xPathKeywordFilter);

                    if (xPathKeywordRows.Count() == 1)
                    {
                         foreach (DataRow xpathKeywordRow in xPathKeywordRows)
                        {
                            Xpath = xpathKeywordRow[Constants.XPATH].ToString();
                        }
                    }
                    else
                    {
                        Logger.Info("The Validate XPath Repository is having duplicate keys for XPaths. Please rectify them.");
                        if (arrDtRow[Constants.XPATH].ToString() == "NA")
                        {
                            string expValue = arrDtRow[Constants.VALUE].ToString();
                            string[] strVarValue = expValue.Split('-');
                            Keywords.Add(strVarValue[0], strVarValue[1]);
                            Logger.Info("TestCaseId: " + TestCaseID + "XPath Label for Expected Value: " + expValue + "Actual Value of the XPath: " + strVarValue[1]);
                        }
                    }
                    if (arrDtRow[Constants.XPATH].ToString() != "NA")
                    {
                        string expValue = arrDtRow[Constants.VALUE].ToString();
                        string actValue = EvaluateXPath(XDoc, Xpath);
                        if (expValue.StartsWith("~") && actValue != "")
                        {
                            if(actValue.Contains("Data added successfully and InvalidAccountId is: "))
                            {
                                Regex.Replace(actValue, "Data added successfully and InvalidAccountId is: ", "");
                                //actValue.Replace("Data added successfully and InvalidAccountId is: ", "");
                            }
                            Keywords.Add(expValue, actValue);
                            Logger.Info("TestCaseId: " + TestCaseID + "XPath Label for Expected Value: " + expValue + "Actual Value of the XPath: " + actValue);
                        }
                        else if (expValue.StartsWith("Minus") && actValue != "")
                        {
                            Keywords.Add(expValue, actValue);
                            Logger.Info("TestCaseId: " + TestCaseID + "XPath Label for Expected Value: " + expValue + "Actual Value of the XPath: " + actValue);
                        }
                        else if (expValue.StartsWith("~") && actValue.Trim() == "")
                        {
                            Logger.Info("Given Xpath" + Xpath + "is not found int the XML Document");
                            throw new Exception("Given Xpath" + Xpath + "is not found int the XML Document");
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Logger.Info("ReplaceXMLFromXPath : " + (e.InnerException == null ? e.StackTrace.ToString() : e.InnerException.ToString()));
                throw e;

            }

        }

        //public static XmlDocument ToXmlDocument(this XDocument xDocument)
        //{
        //    var xmlDocument = new XmlDocument();
        //    using (var xmlReader = xDocument.CreateReader())
        //    {
        //        xmlDocument.Load(xmlReader);
        //    }
        //    return xmlDocument;
        //}
        public static void ValidatePolicyXMLFromXPath(TestCase TestCaseDetails, string TestCaseID, DataTable OutputXPathTable, DataTable XPathRepository)
        {
            try
            {
                XDocument XDoc = TestCaseDetails.ResponseXML;
                string Filter = string.Format("([{0}]='{1}') AND ([{2}]='{3}')", Constants.TESTCASE, TestCaseID, Constants.XML, TestCaseDetails.XMLFile);
                DataRow[] FilteredXpathTable = OutputXPathTable.Select(Filter);
                List<ValidationResult> lstVR = new List<ValidationResult>();
                foreach (DataRow arrDataRow in FilteredXpathTable)
                {
                    string xPathKeywordFilter = string.Format("([{0}]='{1}')", Constants.KEY, arrDataRow[Constants.XPATH]);
                    DataRow[] xPathKeywordRows = XPathRepository.Select(xPathKeywordFilter);

                    ValidationResult validateResult = new ValidationResult();
                    string Xpath = string.Empty;
                    if (xPathKeywordRows.Count() == 1)
                    {
                        foreach (DataRow xpathKeywordRow in xPathKeywordRows)
                        {
                            Xpath = xpathKeywordRow[Constants.XPATH].ToString();
                        }
                    }
                    else
                    {
                        Logger.Info("Given XPath Key : " + arrDataRow[Constants.XPATH] + " is having duplicate entries in XPath Repository!. Please rectify them.");
                        throw new Exception("Given XPath Key : " + arrDataRow[Constants.XPATH] + " is having duplicate entries in XPath Repository!. Please rectify them.");
                    }

                    if (arrDataRow[2].ToString().ToUpper() == Constants.Yes)
                    {
                        string ExpectedValue = arrDataRow[Constants.VALUE].ToString();
                        if (ExpectedValue.Contains("Equals"))
                        {
                            ExpectedValue = Library.Keywords[ExpectedValue.Split('-')[1].ToString()];
                        }
                        else if (ExpectedValue.StartsWith("~"))
                        {
                            ExpectedValue = Library.Keywords[ExpectedValue.ToString()];
                        }
                        else if (ExpectedValue.Contains("Minus"))
                        {
                            ExpectedValue = ComputeFormula(lstVR, ExpectedValue);
                        }
                        string ActualVal = EvaluateXPath(XDoc, Xpath);
                        validateResult.ActualValue = ActualVal;
                        validateResult.ExpectedValue = ExpectedValue;
                        validateResult.XPath = Xpath;

                        if (!(ActualVal == ExpectedValue))
                        {
                            validateResult.Result = Constants.FAIL;
                        }
                        else
                        {
                            validateResult.Result = Constants.PASS;
                        }
                    }
                    else
                    {
                        validateResult.Result = Constants.NOTEXECUTED;
                    }

                    lstVR.Add(validateResult);

                }
                TestCaseDetails.Results = lstVR;
            }
            catch (Exception e)
            {
                Logger.Info("ValidatePolicyXMLFromXPath : " + (e.InnerException == null ? e.StackTrace.ToString() : e.InnerException.ToString()));
                throw e;
            }
            finally
            {

            }
        }


        public static void ValidateXMLFromXPath(TestCase TestCaseDetails, string TestCaseID, DataTable OutputXPathTable, DataTable XPathRepository)
        {
            try
            {
                XDocument XDoc = TestCaseDetails.ResponseXML;
                string Filter = string.Format("([{0}]='{1}') AND ([{2}]='{3}')", Constants.TESTCASE, TestCaseID, Constants.XML, TestCaseDetails.XMLFile);
                DataRow[] FilteredXpathTable = OutputXPathTable.Select(Filter);
                List<ValidationResult> lstVR = new List<ValidationResult>();
                foreach (DataRow arrDataRow in FilteredXpathTable)
                {
                    string xPathKeywordFilter = string.Format("([{0}]='{1}')", Constants.KEY, arrDataRow[Constants.XPATH]);
                    DataRow[] xPathKeywordRows = XPathRepository.Select(xPathKeywordFilter);

                    ValidationResult validateResult = new ValidationResult();
                    string Xpath = string.Empty;
                    if (xPathKeywordRows.Count() == 1)
                    {
                        foreach (DataRow xpathKeywordRow in xPathKeywordRows)
                        {
                            Xpath = xpathKeywordRow[Constants.XPATH].ToString();
                        }
                    }
                    else
                    {
                        Logger.Info("The Validate XPath Repository is having duplicate keys for XPaths. Please rectify them.");
                    }

                    if (arrDataRow[2].ToString().ToUpper() == Constants.Yes)
                    {
                        string ExpectedValue = arrDataRow[Constants.VALUE].ToString().Trim();

                        if (ExpectedValue.ToUpper().StartsWith("COMPUTE"))
                        {
                            ExpectedValue = Compute(ExpectedValue);
                            if (!(ExpectedValue == "TRUE"))
                                validateResult.Result = Constants.FAIL;
                            else
                                validateResult.Result = Constants.PASS;
                        }
                        else
                        {
                            if (ExpectedValue.ToUpper().StartsWith("EQUALS"))
                            {
                                ExpectedValue = Library.Keywords[ExpectedValue.Split('-')[1].ToString()];
                            }
                            else if (ExpectedValue.StartsWith("~"))
                            {
                                ExpectedValue = Library.Keywords[ExpectedValue.ToString()];
                            }
                            else if (ExpectedValue.ToUpper().StartsWith("MINUS"))
                            {
                                ExpectedValue = ComputeFormula(lstVR, ExpectedValue);
                            }
                            string ActualVal = EvaluateXPath(XDoc, Xpath);
                            validateResult.ActualValue = ActualVal;
                            validateResult.ExpectedValue = ExpectedValue;
                            validateResult.XPath = Xpath;

                            if (System.Text.RegularExpressions.Regex.IsMatch(ActualVal, "^[0-9]\\d{0,12}(\\.\\d{1,4})?%?$"))
                            {
                                double actualResult = Convert.ToDouble(ActualVal).CompareTo(Convert.ToDouble(ExpectedValue));
                                if (actualResult.ToString() == "0")
                                    validateResult.Result = Constants.PASS;
                                else
                                    validateResult.Result = Constants.FAIL;
                            }
                            else
                            {
                                if (!(ActualVal == ExpectedValue))
                                    validateResult.Result = Constants.FAIL;
                                else
                                    validateResult.Result = Constants.PASS;
                            }
                        }
                    }
                    else
                    {
                        string expValue = arrDataRow[Constants.VALUE].ToString().Trim();
                        string ExpectedValue = string.Empty;

                        if (expValue.ToUpper().StartsWith("COMPUTE"))
                        {
                            ExpectedValue = Compute(expValue);
                        }
                        //   validateResult.Result = Constants.NOTEXECUTED;
                    }

                    lstVR.Add(validateResult);

                }
                TestCaseDetails.Results = lstVR;
            }
            catch (Exception e)
            {
                Logger.Info("ValidateXMLFromXPath : " + (e.InnerException == null ? e.StackTrace.ToString() : e.InnerException.ToString()));
                throw e;
            }
        }


        public static string Compute(string ExpectResult)
        {
            string sComputedValue = string.Empty;
            string operandWholeValue = ExpectResult.Split('-')[1];
            string operandOneValue = operandWholeValue.Split(',')[0].ToString();
            string operandTwoValue = operandWholeValue.Split(',')[1].ToString();
            string operand = operandWholeValue.Split(',')[2].ToString();

            if (operandOneValue.StartsWith("~"))
            {
                operandOneValue = Library.Keywords[operandOneValue.ToString()];
            }

            if (operandTwoValue.StartsWith("~"))
            {
                operandTwoValue = Library.Keywords[operandTwoValue.ToString()];
            }

            switch (operand.ToUpper())
            {
                case "ADD":
                    double d = Convert.ToDouble(operandOneValue) + Convert.ToDouble(operandTwoValue);
                    sComputedValue = d.ToString();
                    break;
                case "MULTIPLY":
                    double d1 = Convert.ToDouble(operandOneValue) * Convert.ToDouble(operandTwoValue);
                    sComputedValue = d1.ToString();
                    break;
                case "EQUAL":
                    if (operandOneValue == operandTwoValue)
                        sComputedValue = "TRUE";
                    else
                        sComputedValue = "FALSE";
                    break;
            }

            if (ExpectResult.Split('-')[2].StartsWith("~"))
            {
                Keywords.Add(ExpectResult.Split('-')[2], sComputedValue);
            }
            return sComputedValue;
        }

        public static string ComputeFormula(List<ValidationResult> lstVR, string ExpectResult)
        {
            string operandOneValue = lstVR[0].ActualValue;
            string[] operandTwoValue = lstVR[1].ExpectedValue.Split('-');
            double d = Convert.ToDouble(operandOneValue) - Convert.ToDouble(operandTwoValue[1]);
            ExpectResult = d.ToString();
            return ExpectResult;
        }

        public static string ReplaceURIwithGivenKeyword(TestCase TestCaseDetails, string strURI)
        {
            try
            {
                string[] currStepActions = TestCaseDetails.Action.Split('-');
                //if (currStepActions[0].Contains("Set"))
                //{
                //    string strKey = currStepActions[3].ToString().Trim();
                //    string strValue = Library.Keywords[strKey];
                //    strURI = strURI.Replace(currStepActions[2], strValue);
                //    Keywords.Add(currStepActions[1].ToString(), strURI);
                //}

                for (int i = 0; i <= currStepActions.Length - 1; i++)
                {
                    switch (currStepActions[i].Trim())
                    {
                        case "~POLICYNUMBER":
                            string strPolicyKey = currStepActions[i].ToString().Trim();
                            //string strPolicyValue = Library.Keywords[strPolicyKey];
                            //strURI = strURI.Replace(currStepActions[i - 1], strPolicyValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;

                        case "~ShellAccountId":
                            string strshellPolicyKey = currStepActions[i].ToString().Trim();
                            string strshellPolicyValue = Library.Keywords[strshellPolicyKey];
                            strURI = strURI.Replace(currStepActions[i - 1], strshellPolicyValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;
                        case "~ISSUEDPOLICYNUMBER":
                            //string strIssuedPolicyKey = currStepActions[i].ToString().Trim();
                            //string strIssuedPolicyValue = Library.Keywords[strIssuedPolicyKey];
                            //strURI = strURI.Replace(currStepActions[i - 1], strIssuedPolicyValue);
                            //if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            //{
                            //    Keywords[currStepActions[1].ToString()] = strURI;
                            //}
                            //else
                            //{
                            //    Keywords.Add(currStepActions[1].ToString(), strURI);
                            //}
                            break;

                        case "~PartyId":
                            string strPartyIDKey = currStepActions[i].ToString().Trim();
                            string strPartyIDValue = Library.Keywords[strPartyIDKey];
                            strURI = strURI.Replace(currStepActions[i - 1], strPartyIDValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;
                        case "~ETag":
                            string strEtagKey = currStepActions[i].ToString().Trim();
                            string strEtagValue = Library.Keywords[strEtagKey];
                            strURI = strURI.Replace(currStepActions[i - 1], strEtagValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;
                        case "~PaymentId":
                            string strPaymentIDKey = currStepActions[i].ToString().Trim();
                            string strPaymentIDValue = Library.Keywords[strPaymentIDKey];
                            strURI = strURI.Replace(currStepActions[i - 1], strPaymentIDValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;
                        case "~ExtractionId":
                            string strExtractionIDKey = currStepActions[i].ToString().Trim();
                            string strExtractionIDValue = Library.Keywords[strExtractionIDKey];
                            strURI = strURI.Replace(currStepActions[i - 1], strExtractionIDValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;

                        case "~{accountid}":
                            string straccIdKey = currStepActions[i].ToString().Trim();
                            string stracdIdValue = Library.Keywords[straccIdKey];
                            straccIdKey = straccIdKey.Replace("~", "");
                            strURI = strURI.Replace(straccIdKey, stracdIdValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;

                        case "~{startdate}":
                            string strStartDtKey = currStepActions[i].ToString().Trim();
                            string strStartDtValue = Library.Keywords[strStartDtKey];
                            strStartDtKey = strStartDtKey.Replace("~", "");
                            strURI = strURI.Replace(strStartDtKey, strStartDtValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;

                        case "~{EffDate}":
                            string strEffDtKey = currStepActions[i].ToString().Trim();
                            string strEffDtValue = Library.Keywords[strEffDtKey];
                            straccIdKey = strEffDtKey.Replace("~", "");
                            strURI = strURI.Replace(strEffDtKey, strEffDtValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;
                        case "~EntryDate":
                            string strEntryDtKey = currStepActions[i].ToString().Trim();
                            string strEntryDtValue = Library.Keywords[strEntryDtKey];
                            straccIdKey = strEntryDtKey.Replace("~", "");
                            strURI = strURI.Replace(strEntryDtKey, strEntryDtValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;

                        case "~{Lob}":
                            string strLOBKey = currStepActions[i].ToString().Trim();
                            string strLOBValue = Library.Keywords[strLOBKey];
                            strLOBKey = strLOBKey.Replace("~", "");
                            strURI = strURI.Replace(strLOBKey, strLOBValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;


                        case "~PaymentMethodId":
                            string strKey = currStepActions[i].ToString().Trim();
                            string strValue = Library.Keywords[strKey];
                            strURI = strURI.Replace(currStepActions[i - 1], strValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;
                        case "~Date":
                            string strdtKey = currStepActions[i].ToString().Trim();
                            string strdtValue = Library.Keywords[strdtKey];
                            strdtValue = DateTime.Now.ToString("yyyy-MM-dd");
                            strURI = strURI.Replace(currStepActions[i - 1], strdtValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;
                        case "~BatchId":
                            string strbatchKey = currStepActions[i].ToString().Trim();
                            string strbatchValue = Library.Keywords[strbatchKey];
                            strURI = strURI.Replace(currStepActions[i - 1], strbatchValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;

                        case "~UserId":
                            string struseridKey = currStepActions[i].ToString().Trim();
                            string struseridValue = Library.Keywords[struseridKey];
                            strURI = strURI.Replace(currStepActions[i - 1], struseridValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;
                        case "~BatchCode":
                            string strbatchidKey = currStepActions[i].ToString().Trim();
                            string strbatchidValue = Library.Keywords[strbatchidKey];
                            strURI = strURI.Replace(currStepActions[i - 1], strbatchidValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;
                        case "~TokenizedAccountNumber":
                            string strtokenKey = currStepActions[i].ToString().Trim();
                            string strtokenValue = Library.Keywords[strtokenKey];
                            strURI = strURI.Replace(currStepActions[i - 1], strtokenValue);
                            if (Keywords.ContainsKey(currStepActions[1].ToString()))
                            {
                                Keywords[currStepActions[1].ToString()] = strURI;
                            }
                            else
                            {
                                Keywords.Add(currStepActions[1].ToString(), strURI);
                            }
                            break;
                    }

                }

                return strURI;
            }
            catch (Exception e)
            {
                Logger.Info("ReplaceURIwithGivenKeyword : " + (e.InnerException == null ? e.StackTrace.ToString() : e.InnerException.ToString()));
                throw e;
            }
        }

        public static string GetValuefromKeywordDictonary(TestCase TestCaseDetails)
        {
            try
            {
                string strValue = string.Empty;
                string[] currStepActions = TestCaseDetails.Action.Split('-');
                if (currStepActions[0].ToString() == "GET")
                {
                    strValue = Keywords[currStepActions[1]];
                }
                else strValue = Keywords[currStepActions[0]];
                return strValue;
            }
            catch (Exception e)
            {
                Logger.Info("GetValuefromKeywordDictonary : " + (e.InnerException == null ? e.StackTrace.ToString() : e.InnerException.ToString()));
                throw e;
            }
        }

        public static void BuildDictonaryObjectFromXML(TestCase TestCaseDetails, string TestCaseID, DataTable OutputXPathTable, DataTable XPathRepository)
        {
            try
            {
                XDocument XDoc = TestCaseDetails.ResponseXML;
                //string Filter = string.Format("([{0}]='{1}') AND ([{2}]='{3}')", Constants.TESTCASE, TestCaseID, Constants.XML, TestCaseDetails.XMLFile);
                string Filter = string.Format("([{0}]='{1}') AND ([{2}]='{3}')", Constants.TESTCASE, TestCaseID, Constants.XML, "NA");
                DataRow[] FilteredXpathTable = OutputXPathTable.Select(Filter);
                List<ValidationResult> lstVR = new List<ValidationResult>();
                foreach (DataRow arrDataRow in FilteredXpathTable)
                {
                    string xPathKeywordFilter = string.Format("([{0}]='{1}')", Constants.KEY, arrDataRow[Constants.XPATH]);
                    DataRow[] xPathKeywordRows = XPathRepository.Select(xPathKeywordFilter);

                    ValidationResult validateResult = new ValidationResult();
                    string Xpath = string.Empty;
                    if (xPathKeywordRows.Count() == 1)
                    {
                        foreach (DataRow xpathKeywordRow in xPathKeywordRows)

                        {
                            Xpath = xpathKeywordRow[Constants.XPATH].ToString();
                        }
                    }
                    else
                    {
                        Logger.Info("The Validate XPath Repository is having duplicate keys for XPaths. Please rectify them.");
                    }

                    string expValue = arrDataRow[Constants.VALUE].ToString();
                    string actValue = EvaluateXPath(XDoc, Xpath);
                    if (expValue.StartsWith("~") && actValue != "")
                    {
                        Keywords.Add(expValue, actValue);
                        Logger.Info("TestCaseId: " + TestCaseID + "XPath Label for Expected Value: " + expValue + "Actual Value of the XPath: " + actValue);
                    }
                    else
                    {
                        Logger.Info("Given Xpath" + Xpath + "is not found int the XML Document");
                    }

                }
            }
            catch (Exception e)
            {
                Logger.Info("BuildDictonaryObjectFromXML : " + e.InnerException.ToString());
            }
            finally
            {

            }
        }
    }
}
