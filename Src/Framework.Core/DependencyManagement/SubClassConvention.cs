namespace VM.Platform.TestAutomationFramework.Core.DependencyManagement
{
    using System;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;
    using StructureMap.TypeRules;

    class SubClassConvention<TPluginType, TSubPluginType> : IRegistrationConvention
        where TSubPluginType : TPluginType
    {
        public void Process(Type type, Registry registry)
        {
            var isWrongType = !type.IsConcrete() || !type.CanBeCreated() || !typeof(TSubPluginType).IsAssignableFrom(type);
            if (isWrongType) return;

            registry.For(typeof(TPluginType)).Add(type).Named(type.Name);
        }
    }
}