namespace VM.Platform.TestAutomationFramework.Core.Logging
{
    public abstract class Logger
    {
        public abstract void Debug(object message);
        public abstract void Debug(string format, object arg);
        public abstract void Debug(string format, object arg, params object[] args);

        public abstract void Info(object message);
        public abstract void Info(string format, object arg);
        public abstract void Info(string format, object arg, params object[] args);
        
        public abstract void Warn(object message);
        public abstract void Warn(string format, object arg);
        public abstract void Warn(string format, object arg, params object[] args);
        
        public abstract void Error(object message);
        public abstract void Error(string format, object arg);
        public abstract void Error(string format, object arg, params object[] args);
        
        public abstract void Conditions(object message);
        public abstract void Conditions(string format, object arg);
        public abstract void Conditions(string format, object arg, params object[] args);

        protected string conditionsLogFileName;
        public virtual string ConditionsLogFileName 
        { 
            get
            {
                return this.conditionsLogFileName;
            }
            set
            {
                this.conditionsLogFileName = value;
            }
            
        }

        protected string executionLogFileName;
        public virtual string ExecutionLogFileName 
        {
            get
            {
                return this.executionLogFileName;
            }
            set
            {
                this.executionLogFileName = value;
            }
        }
    }
}