namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    public interface IFileWriter
    {
        string CreateFile(byte[] buffer, string extension);
    }
}