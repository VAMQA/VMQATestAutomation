namespace VM.Platform.TestAutomationFramework.Core
{
    public class WorkItem
    {
        public WorkItem(int workItemId, string title, string type)
        {
            this.Id = workItemId;
            this.Title = title;
            this.Type = type;
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Type { get; private set; }
    }
}