namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    public interface IControlMapLoader
    {
        ControlMap GetControlMapFromTestCase(int testCaseId, string mapName = null);
        ControlMap GetControlMapFromSharedStep(string sharedStepId, string mapName = null);
    }
}