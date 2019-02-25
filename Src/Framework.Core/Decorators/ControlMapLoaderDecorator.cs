namespace VM.Platform.TestAutomationFramework.Core.Decorators
{
    using System;
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    public abstract class ControlMapLoaderDecorator : IControlMapLoader
    {
        protected readonly IControlMapLoader ControlMapLoader;

        protected ControlMapLoaderDecorator(IControlMapLoader controlMapLoader)
        {
            if (controlMapLoader == null)
            {
                throw new ArgumentNullException("controlMapLoader", "Decorator has no underlying object");
            }
            this.ControlMapLoader = controlMapLoader;
        }

        public virtual ControlMap GetControlMapFromTestCase(int testCaseId, string mapName = null)
        {
            return this.ControlMapLoader.GetControlMapFromTestCase(testCaseId, mapName: mapName);
        }

        public virtual ControlMap GetControlMapFromSharedStep(string sharedStepId, string mapName = null)
        {
            return this.ControlMapLoader.GetControlMapFromSharedStep(sharedStepId, mapName);
        }
    }
}
