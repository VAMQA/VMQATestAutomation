namespace VM.Platform.TestAutomationFramework.Core.Decorators
{
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    public class ControlMapCacher : ControlMapLoaderDecorator
    {
        private static ControlMap cachedControlMap;

        static ControlMapCacher()
        {
            cachedControlMap = null;
        }

        public ControlMapCacher(IControlMapLoader controlMapLoader, string precachedControlMapSource, string mapName)
            : base(controlMapLoader)
        {
            if (precachedControlMapSource != null)
            {
                cachedControlMap = base.GetControlMapFromSharedStep(precachedControlMapSource, mapName);
            }
        }

        public override ControlMap GetControlMapFromTestCase(int testCaseId, string mapName = null)
        {
            //return cachedControlMap ?? (cachedControlMap = base.GetControlMapFromTestCase(testCaseId, mapName: mapName));
            cachedControlMap = null;
            cachedControlMap = base.GetControlMapFromTestCase(testCaseId, mapName: mapName);
            return cachedControlMap;
        }
    }
}