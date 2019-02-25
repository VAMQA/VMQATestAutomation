using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Drawing;
using System.Threading;
using log4net;
using log4net.Repository;
using System.IO;


namespace VM.WebServices.TestAutomationFramework.Common
{

    public class FWLogger
    {
        public static ILog Logger;
        static FWLogger()
        {
            InitializeFWLogger();
        }
        static void InitializeFWLogger()
        {
            string logFilePath = Library.GetPath() + Constants.logFilesPath;
            string archieveLogPath = Library.GetPath() + Constants.logArchieveFilesPath;
            string htmlArchieveLogPath = Library.GetPath() + Constants.htmlArchieveFilesPath;
           
            DateTime CD = DateTime.UtcNow;
            string CurrentDate = string.Format("{0}-{1}-{2}_{3}-{4}-{5}-{6}", CD.Year, CD.Month, CD.Day, CD.Hour, CD.Minute, CD.Second, CD.Millisecond);
            string logFileName = "Log_" + CurrentDate;
            string repoName = String.Format("{0}Repository", logFileName);

            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }

            List<String> logFilePaths = Directory.GetFiles(logFilePath, "*.log").ToList();
            List<String> htmlFilePaths = Directory.GetFiles(logFilePath, "*.html").ToList();
            if (!Directory.Exists(archieveLogPath))
            {
                Directory.CreateDirectory(archieveLogPath);
            }

            if (!Directory.Exists(htmlArchieveLogPath))
            {
                Directory.CreateDirectory(htmlArchieveLogPath);
            }

            foreach (var logFile in logFilePaths)
            {
                FileInfo mFile = new FileInfo(logFile);
                if (new FileInfo(logFilePath + "\\" + mFile.Name).Exists == true)
                    mFile.MoveTo(archieveLogPath + "\\" + mFile.Name);
            }
            foreach (var htmlFile in htmlFilePaths)
            {
                FileInfo mFile = new FileInfo(htmlFile);
                if (new FileInfo(logFilePath + "\\" + mFile.Name).Exists == true)
                    mFile.MoveTo(htmlArchieveLogPath + "\\" + mFile.Name);

            }

            ILoggerRepository[] allRepos = LogManager.GetAllRepositories();
            ILoggerRepository loggerRepository = allRepos.Where(x => x.Name == repoName).FirstOrDefault();
            if (loggerRepository == null)
            {
                loggerRepository = LogManager.CreateRepository(repoName);
                ThreadContext.Properties["LogName"] = logFilePath + "\\" + logFileName;
                log4net.Config.XmlConfigurator.Configure(loggerRepository);
            }
            Logger = LogManager.GetLogger(logFileName + "Repository", "Logger");
            Logger.Info("Succesfully Initialized Logger");
            Console.WriteLine("Succesfully Initialized Logger");
        }
    }
}