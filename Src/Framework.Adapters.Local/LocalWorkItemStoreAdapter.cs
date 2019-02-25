namespace VM.Platform.TestAutomationFramework.Adapters.Local
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security;
    using System.Threading;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Core.Configuration;
    using System.DirectoryServices.AccountManagement;
    using System.Security.AccessControl;
    using System.Security.Principal;
   

    public class LocalWorkItemStoreAdapter : ILocalReaderAdapter
    {
        //private readonly string testFolderPath;
        //private readonly string testDataFileName;
        //private readonly string testORFileName;
        //private readonly string testCaseFileName;
        private readonly IDateTimeProvider dateTimeProvider;
        private string datetimestamp;
        private XDocument configurationFile;
        Dictionary<string, string> res = new Dictionary<string, string>();
        private TestCaseConfiguration testCaseConfig;

        public LocalWorkItemStoreAdapter(RepositoryConnection repositoryConnection, IDateTimeProvider dateTimeProvider)
        {
            configurationFile = XDocument.Load(@"Config\Config.xml");
            this.dateTimeProvider = dateTimeProvider;           
        }
        public void SaveTestResults(int TestCaseID, IEnumerable<string> filesToAttach)
        {
            var testResultsPath = GetTestResultsPath(configurationFile) + "\\" + TestCaseID + "\\" + System.Environment.UserName + "_" + DateTime.Parse(datetimestamp).ToUniversalTime().ToString("yyyy-MM-dd_HH-mm-ss");            
            if (!Directory.Exists(testResultsPath))
                Directory.CreateDirectory(testResultsPath);
            foreach (string f in filesToAttach)
                  if (File.Exists(f))
                  {
                      File.Copy(f, testResultsPath + "\\" + f);
                      File.Delete(f);
                  }
                    
        }
        //public void SaveTestResults(int TestCaseID, IEnumerable<string> filesToAttach, int testrunId, string project)
        //{
        //    var testResultsPath = GetTestResultsPath(configurationFile) + "\\" + project + "\\" + TestCaseID + "\\" + System.Environment.UserName + "_" + DateTime.Parse(datetimestamp).ToUniversalTime().ToString("yyyy-MM-dd_HH-mm-ss");
        //    //var localpath = @".\\..\\..\\Reports\\"+ project + "\\" + TestCaseID + "\\" + System.Environment.UserName + "_" + DateTime.Parse(datetimestamp).ToUniversalTime().ToString("yyyy-MM-dd_HH-mm-ss");
        //    var localpath = @".\\Reports\\" + project + "\\" + TestCaseID + "\\" + System.Environment.UserName + "_" + DateTime.Parse(datetimestamp).ToUniversalTime().ToString("yyyy-MM-dd_HH-mm-ss");
        //    if (!Directory.Exists(testResultsPath))
        //    {
        //        Directory.CreateDirectory(localpath);
        //        Directory.CreateDirectory(testResultsPath);
        //    }

        //    foreach (string f in filesToAttach)
        //        if (File.Exists(f))
        //        {
        //            File.Copy(f, localpath + "\\" + f.Split('.')[0] + "_" + testrunId + "." + f.Split('.')[1]);
        //            File.Copy(f, testResultsPath + "\\" + f.Split('.')[0] + "_" + testrunId + "." + f.Split('.')[1]);
        //            File.Delete(f);
        //        }
        //}
        public void SaveTestResults(int TestCaseID, IEnumerable<string> filesToAttach, int testrunId, string project)
        {
            //var testResultsPath = GetTestResultsPath(configurationFile) + "\\" + project + "\\" + TestCaseID + "\\" + System.Environment.UserName + "_" + DateTime.Parse(datetimestamp).ToUniversalTime().ToString("yyyy-MM-dd_HH-mm-ss");
            var testResultsPath = GetTestResultsPath(configurationFile) + "\\" + project + "\\" + TestCaseID + "\\" + System.Environment.UserName + "_" + DateTime.Parse(datetimestamp).ToString("yyyy-MM-dd_HH-mm-ss");
            //var localpath = @".\\..\\..\\Reports\\"+ project + "\\" + TestCaseID + "\\" + System.Environment.UserName + "_" + DateTime.Parse(datetimestamp).ToUniversalTime().ToString("yyyy-MM-dd_HH-mm-ss");
            //var localpath = @".\\Reports\\" + project + "\\" + TestCaseID + "\\" + System.Environment.UserName + "_" + DateTime.Parse(datetimestamp).ToUniversalTime().ToString("yyyy-MM-dd_HH-mm-ss");
            var localpath = @".\\Reports\\" + project + "\\" + TestCaseID + "\\" + System.Environment.UserName + "_" + DateTime.Parse(datetimestamp).ToString("yyyy-MM-dd_HH-mm-ss");

            CopyTestResults(localpath, filesToAttach, testrunId);
            CopyTestResults(testResultsPath, filesToAttach, testrunId);
            foreach (string f in filesToAttach)
            {
                if (File.Exists(f))
                    File.Delete(f);
            }
        }
        private void CopyTestResults(string path, IEnumerable<string> filesToAttach, int testrunId)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);                
            }

            foreach (string f in filesToAttach)
            {
                if (File.Exists(f))
                    File.Copy(f, path + "\\" + f.Split('.')[0] + "_" + testrunId + "." + f.Split('.')[1]);                   
            }   
        }
        private string GetTestResultsPath(XDocument configurationFile)
        {
            var address = configurationFile.XPathSelectElement("/TestAutomationFramework/DBExecution/TestResultsPath");
            return address == null
                ? null
                : address.Value;
        }
        public int SaveTestCaseRunResult(int testCaseId, TestResult testCaseResult)
        {
            res.Add("TestCaseID", testCaseId.ToString());
            res.Add("RunBy", System.Environment.UserName);
            res.Add("Result", testCaseResult.ToString());
            datetimestamp = this.dateTimeProvider.Now.ToString();
            res.Add("DateOfExe", datetimestamp);            
            TimeSpan duration = DateTime.Parse(res["DateOfExe"]).Subtract(DateTime.Parse(res["StartTime"]));
            res.Add("ExeTime", duration.ToString(@"hh\:mm\:ss"));            

            var connString = configurationFile.XPathSelectElement("/TestAutomationFramework/DBExecution/DBConnectionString").Value;                     
            int runId=SqlHelper.ExecuteNonQuery(connString.ToString(), System.Data.CommandType.Text,res);
            return runId;
        }
        //public int SaveTestCaseRunResult(int testCaseId, TestResult testCaseResult)
        //{
        //    res.Add("TestCaseID", testCaseId.ToString());
        //    res.Add("RunBy", System.Environment.UserName);
        //    res.Add("Result", testCaseResult.ToString());
        //    datetimestamp = this.dateTimeProvider.Now.ToString();
        //    res.Add("DateOfExe", datetimestamp);
        //    TimeSpan duration = DateTime.Parse(res["DateOfExe"]).Subtract(DateTime.Parse(res["StartTime"]));
        //    res.Add("ExeTime", duration.ToString(@"hh\:mm\:ss"));

        //    var connString = configurationFile.XPathSelectElement("/TestAutomationFramework/DBExecution/DBConnectionString").Value;
        //    return(SqlHelper.ExecuteNonQuery(connString.ToString(), System.Data.CommandType.Text, res));
        //}
        public void SaveTestDetailsTab(TestExecutionDeatils testDetails, TestCaseConfiguration testCaseConfiguration)
        {
            res.Add("No.Of Cond Executed", (testDetails.TotalconditionsPassed.Count + testDetails.TotalconditionsFailed.Count).ToString());
            res.Add("No.Of Cond Passed", testDetails.TotalconditionsPassed.Count.ToString());
            res.Add("No.Of Cond Failed", testDetails.TotalconditionsFailed.Count.ToString());
            res.Add("Environment", testCaseConfiguration.GetUrl(testCaseConfiguration.RequestedEnvironment));
            res.Add("Browser", testCaseConfiguration.TargetBrowser.ToString());
            res.Add("StartTime", testDetails.ExecutionTimeStamp.ToString());
            res.Add("ProjectID", testCaseConfiguration.TestPlan.ToString());            
        }
    }
}