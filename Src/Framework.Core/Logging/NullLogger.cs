namespace VM.Platform.TestAutomationFramework.Core.Logging
{
    public class NullLogger : Logger
    {
        public override void Debug(object message) { }

        public override void Debug(string format, object arg) { }

        public override void Debug(string format, object arg, params object[] args) { }

        public override void Info(object message) { }

        public override void Info(string format, object arg) { }

        public override void Info(string format, object arg, params object[] args) { }

        public override void Warn(object message) { }

        public override void Warn(string format, object arg) { }

        public override void Warn(string format, object arg, params object[] args) { }

        public override void Error(object message) { }

        public override void Error(string format, object arg) { }

        public override void Error(string format, object arg, params object[] args) { }

        public override void Conditions(object message) { }

        public override void Conditions(string format, object arg) { }

        public override void Conditions(string format, object arg, params object[] args) { }
    }
}