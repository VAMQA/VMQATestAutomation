namespace VM.Platform.TestAutomationFramework.Extensions
{
    using System;
    using TechTalk.SpecFlow;

    public static class ScenarioContextExtensions
    {
        public static T GetValueOr<T>(this ScenarioContext scenarioContext, Func<T> func)
        {
            T item;
            if (scenarioContext.TryGetValue(out item)) return item;

            item = func();
            scenarioContext.Set(item);

            return scenarioContext.Get<T>();
        }

        public static T GetValueOr<T>(this ScenarioContext scenarioContext, Func<T> func, string key)
        {
            T item;
            if (scenarioContext.TryGetValue(key, out item)) return item;

            item = func();
            scenarioContext.Set(item, key);

            return scenarioContext.Get<T>(key);
        }

        public static T GetValueOrNew<T>(this ScenarioContext scenarioContext)
            where T : new()
        {
            return GetValueOr(scenarioContext, () => new T());
        }

        public static T GetValueOrNew<T>(this ScenarioContext scenarioContext, string key)
            where T : new()
        {
            return GetValueOr(scenarioContext, () => new T(), key);
        }

        public static T GetValueOrDefault<T>(this ScenarioContext scenarioContext)
        {
            return GetValueOr(scenarioContext, () => default(T));
        }

        public static T GetValueOrDefault<T>(this ScenarioContext scenarioContext, string key)
        {
            return GetValueOr(scenarioContext, () => default(T), key);
        }
    }
}