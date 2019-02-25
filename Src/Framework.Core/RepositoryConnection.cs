namespace VM.Platform.TestAutomationFramework.Core
{
    using System.Security;

    public class RepositoryConnection
    {
        private readonly string repositoryLocation;
        private readonly string projectName;
        private readonly string testPlan;
        private readonly string userName;
        private readonly SecureString password;

        public RepositoryConnection(string repositoryLocation, string projectName, string testPlan, string userName, SecureString password)
        {
            this.repositoryLocation = repositoryLocation;
            this.projectName = projectName;
            this.testPlan = testPlan;
            this.userName = userName;
            this.password = password;
        }

        public string RepositoryLocation
        {
            get { return this.repositoryLocation; }
        }

        public string UserName
        {
            get { return this.userName; }
        }

        public SecureString Password
        {
            get { return this.password; }
        }

        public string ProjectName
        {
            get { return this.projectName; }
        }

        public string TestPlan
        {
            get { return this.testPlan; }
        }
    }
}