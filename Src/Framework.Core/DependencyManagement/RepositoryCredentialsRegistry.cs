namespace VM.Platform.TestAutomationFramework.Core.DependencyManagement
{
    using System.Security;
    using StructureMap.Configuration.DSL;

    internal class RepositoryCredentialsRegistry : Registry
    {
        public RepositoryCredentialsRegistry(string repositoryLocation, string projectName, string testPlan, string userName, SecureString password)
        {
            this.For<RepositoryConnection>().Use(new RepositoryConnection(repositoryLocation, projectName, testPlan, userName, password));
        }
    }
}