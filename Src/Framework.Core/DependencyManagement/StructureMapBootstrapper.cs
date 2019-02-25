namespace VM.Platform.TestAutomationFramework.Core.DependencyManagement
{
    using System;
    using System.Security;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using StructureMap;
    using StructureMap.Graph;
    using System.Collections;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal class StructureMapBootstrapper
    {
        public static Container GetContainer(string repositoryLocation, string projectName, string userName, SecureString password, string targetBrowser, string controlMapSource, string mapName, string testPlan)
        {
            var container = new Container(_ =>
            {
                _.AddRegistry(new RepositoryCredentialsRegistry(repositoryLocation, projectName, testPlan, userName, password));
                _.AddRegistry(new TestConfigurationRegistry(targetBrowser, projectName, testPlan));
                _.AddRegistry(new ControlMapRegistry(controlMapSource, mapName));
                //LN Added
                _.AddRegistry(new TestAutomationWorkflowRegistry(projectName));

                _.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssembliesFromApplicationBaseDirectory(asm => asm.FullName.Contains("VM.Platform.TestAutomationFramework"));

                    scan.With(new SubClassConvention<ITestCommand, BaseCommand>());
                    scan.AddAllTypesOf<IPostCommandScript>();
                    scan.LookForRegistries();
                });
            });

            container.Configure(cfg => cfg.For<IContainer>().Use(container).Singleton());
            //Added to remove TFS from loading for local execution            

            if (projectName == null)
            {


                if (container.Model.InstancesOf<ITestRunPublisher>().Where(v => v.ReturnedType.Name.StartsWith("TfsTestRunPublisher")).FirstOrDefault() != null)
                    container.Model.InstancesOf<ITestRunPublisher>().Where(v => v.ReturnedType.Name.StartsWith("TfsTestRunPublisher")).FirstOrDefault().EjectAndRemove();

                if (container.Model.InstancesOf<IDataFileReader>().Where(v => v.ReturnedType.Name.StartsWith("ExcelMapReader")).FirstOrDefault() != null)
                    container.Model.InstancesOf<IDataFileReader>().Where(v => v.ReturnedType.Name.StartsWith("ExcelMapReader")).FirstOrDefault().EjectAndRemove();

            }
            else
            {
                if (container.Model.InstancesOf<ITestRunPublisher>().Where(v => v.ReturnedType.Name.StartsWith("LocalTestRunPublisher")).FirstOrDefault() != null)
                    container.Model.InstancesOf<ITestRunPublisher>().Where(v => v.ReturnedType.Name.StartsWith("LocalTestRunPublisher")).FirstOrDefault().EjectAndRemove();

                if (container.Model.InstancesOf<IDataFileReader>().Where(v => v.ReturnedType.Name.StartsWith("LocalExcelMapReader")).FirstOrDefault() != null)
                    container.Model.InstancesOf<IDataFileReader>().Where(v => v.ReturnedType.Name.StartsWith("LocalExcelMapReader")).FirstOrDefault().EjectAndRemove();


            }

            //Console.WriteLine(container.WhatDoIHave());
            return container;
        }
    }
}