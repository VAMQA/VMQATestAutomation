namespace VM.Platform.TestAutomationFramework.Core.Logging
{
    using System.IO;
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Core.Properties;
    using VM.Platform.TestAutomationFramework.Extensions;
    using log4net;    
    using System.Threading;
    using log4net.Config;
    using log4net.Repository;
    using log4net.Repository.Hierarchy;
    using System;

    public class Log4NetLogger : Logger
    {        
        private ILog conditionsLog;

        private ILog executionLog;
        
        

        public Log4NetLogger()
        {
            string repoName = String.Format("Execution{0}_Repository", DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());
            ILoggerRepository loggerRepository = LogManager.CreateRepository(repoName);
            ThreadContext.Properties[Resources.ExecutionLogFileName] = this.executionLogFileName = repoName.Split('_')[0] + ".log";

            this.Configure(loggerRepository);            
            this.executionLog = LogManager.GetLogger(repoName, "execution-log");

            //GlobalContext.Properties[Resources.ExecutionLogFileName] = this.executionLogFileName = "Execution.log";            
            //GlobalContext.Properties[Resources.ConditionsLogFileName] = this.conditionsLogFileName = "Conditions.log";
            //this.ResetConfiguration();
        }
        //public Log4NetLogger(string executionLogFileName, string conditionsLogFileName)
        //{
        //    this.ExecutionLogFileName = executionLogFileName;
        //    this.ConditionsLogFileName = conditionsLogFileName;
        //}

        private void ResetConfiguration()
        {
            //XmlConfigurator.Configure(new FileInfo("log4net.config"));
            //this.Configure();

            this.conditionsLog = LogManager.GetLogger("conditions-log");
            this.executionLog = LogManager.GetLogger("execution-log");
        }

        #region log4net logging method implementation
        public override void Debug(object message)
        {
            this.executionLog.Debug(message);
        }

        public override void Debug(string format, object arg)
        {
            this.executionLog.DebugFormat(format, arg);
        }

        public override void Debug(string format, object arg, params object[] moreArgs)
        {
            var args = new[] { arg }.Concat(moreArgs).ToArray();

            this.executionLog.DebugFormat(format, args);
        }

        public override void Info(object message)
        {
            this.executionLog.Info(message);
        }

        public override void Info(string format, object arg)
        {
            this.executionLog.InfoFormat(format, arg);
        }

        public override void Info(string format, object arg, params object[] moreArgs)
        {
            var args = new[] { arg }.Concat(moreArgs).ToArray();
            this.executionLog.InfoFormat(format, args);
        }

        public override void Warn(object message)
        {
            this.executionLog.Warn(message);
        }

        public override void Warn(string format, object arg)
        {
            this.executionLog.WarnFormat(format, arg);
        }

        public override void Warn(string format, object arg, params object[] moreArgs)
        {
            var args = new[] { arg }.Concat(moreArgs).ToArray();
            this.executionLog.WarnFormat(format, args);
        }

        public override void Error(object message)
        {
            this.executionLog.Error(message);
        }

        public override void Error(string format, object arg)
        {
            this.executionLog.ErrorFormat(format, arg);
        }

        public override void Error(string format, object arg, params object[] moreArgs)
        {
            var args = new[] { arg }.Concat(moreArgs).ToArray();
            this.executionLog.ErrorFormat(format, args);
        }

        public override void Conditions(object message)
        {
            this.conditionsLog.Info(message);
        }

        public override void Conditions(string format, object arg)
        {
            this.conditionsLog.InfoFormat(format, arg);
        }

        public override void Conditions(string format, object arg, params object[] moreArgs)
        {
            var args = new[] { arg }.Concat(moreArgs).ToArray();
            this.conditionsLog.InfoFormat(format, args);
        }

        #endregion

        public override string ExecutionLogFileName
        {
            get
            {
                return base.ExecutionLogFileName;
            }
            set
            {
                var oldLogFileName = GlobalContext.Properties[Resources.ExecutionLogFileName] as string;


                GlobalContext.Properties[Resources.ExecutionLogFileName] = base.ExecutionLogFileName = value;

                this.ResetConfiguration();

                if (oldLogFileName != null)
                {
                    var fileInfo = new FileInfo(oldLogFileName);
                    fileInfo.DeleteIfEmpty();
                }
            }
        }

        public override string ConditionsLogFileName
        {
            get
            {
                return base.ConditionsLogFileName;
            }
            set
            {
                var oldLogFileName = GlobalContext.Properties[Resources.ConditionsLogFileName] as string;

                GlobalContext.Properties[Resources.ConditionsLogFileName] = base.ConditionsLogFileName = value;

                this.ResetConfiguration();

                if (oldLogFileName != null)
                {
                    var fileInfo = new FileInfo(oldLogFileName);
                    fileInfo.DeleteIfEmpty();
                }
            }
        }

        //private void Configure()
        //{
        //    using (var stream = this.GetType().Assembly.GetManifestResourceStream("VM.Platform.TestAutomationFramework.Core.Logging.log4net.config"))
        //    {
        //        if (stream == null)
        //        {
        //            throw new FrameworkFatalException("could not find embeded log4net configuration! Please contact your administrator.");
        //        }
        //        XmlConfigurator.Configure(stream);
        //    }
        //}

        private void Configure(ILoggerRepository repository)
        {
            using (var stream = this.GetType().Assembly.GetManifestResourceStream("VM.Platform.TestAutomationFramework.Core.Logging.log4net.config"))
            {
                if (stream == null)
                {
                    throw new FrameworkFatalException("could not find embeded log4net configuration! Please contact your administrator.");
                }
                XmlConfigurator.Configure(repository, stream);
            }
        }
    }
}