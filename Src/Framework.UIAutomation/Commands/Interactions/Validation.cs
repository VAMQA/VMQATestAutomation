namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;

    internal class Validation
    {
        public object Expected { get; set; }
        public object Actual { get; set; }
        public Func<Validation, bool> SuccessMeasure { get; set; }

        public Validation()
        {
            SuccessMeasure =
                validation =>
                    validation.Actual.ToString().Equals(validation.Expected.ToString(), 
                        StringComparison.OrdinalIgnoreCase);
        }

        public bool Succeeds { get { return SuccessMeasure(this); } }
    }
}