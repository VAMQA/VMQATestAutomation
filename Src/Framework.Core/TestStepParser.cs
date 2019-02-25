namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Extensions;
    using StructureMap;

    public class TestStepParser
    {
        private readonly IContainer container;

        public TestStepParser(IContainer container)
        {
            this.container = container;
        }

        public IEnumerable<ITestCommand> GetCommands(IEnumerable<string> testSteps)
        {
            if (testSteps == null) throw new ArgumentNullException("testSteps");
            var steps = testSteps as string[] ?? testSteps.ToArray();

            var commandTypes = this.GetCommandTypes().ToArray();
            
            foreach (var testStep in steps)
            {
                if (testStep == null || testStep.IsNullOrEmpty())
                {
                    continue;
                }

                ITestCommand command = null;
                if (commandTypes.Any(t => TryParse(t, testStep, out command)))
                {
                    yield return command;

                }
                else
                {
                    throw new ArgumentOutOfRangeException("testSteps", testStep, "unknown test step");
                }
            }

            //return commands;
        }

        private IEnumerable<Type> GetCommandTypes()
        {
            var commandTypes = this.container.Model.InstancesOf<ITestCommand>().Select(ct => ct.ReturnedType);
            return commandTypes;
        }

        private bool TryParse(Type commandType, string testStep, out ITestCommand command)
        {
            command = null;
            var recognizedPatterns = GetPattern(commandType);
            var executionParameters = new ExecutionParameters();
            foreach (var recognizedPattern in recognizedPatterns)
            {
                var regex = new Regex(recognizedPattern, RegexOptions.IgnoreCase);
                var match = regex.Match(testStep);
                if (match.Success)
                {
                    for (int i = 1; i < match.Groups.Count; i++)
                    {
                        var parameter = regex.GroupNameFromNumber(i);
                        var value = match.Groups[i].Value;

                        executionParameters.Add(parameter, value);
                    }
                    command = this.container.With(executionParameters).GetInstance<ITestCommand>(commandType.Name);
                    break; // no more pattern recognition required
                }
            }

            return command != null;
        }

        private static IEnumerable<string> GetPattern(Type commandType)
        {
            var attributes = (CanParseAttribute[])Attribute.GetCustomAttributes(commandType, typeof (CanParseAttribute));

            var patterns = attributes.Select(a => a.CommandPattern);
            return patterns;
        }
    }
}
