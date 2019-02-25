namespace VM.Platform.TestAutomationFramework.Extensions
{
    using System.IO;

    public static class FileSystemInfoExtensions
    {
        public static void DeleteIfExists(this FileSystemInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }

        public static void DeleteIfEmpty(this FileInfo fileInfo)
        {
            if (fileInfo.Exists && fileInfo.Length == 0)
            {
                fileInfo.Delete();
            }
        }
    }
}