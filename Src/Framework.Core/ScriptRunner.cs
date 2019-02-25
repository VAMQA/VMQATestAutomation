using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Platform.TestAutomationFramework.Core
{
    using System.CodeDom.Compiler;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using Microsoft.CSharp;

    public class ScriptRunner
    {
        public object EvaluateAsCSharp(string code)
        {
            CodeDomProvider codeDomProvider = new CSharpCodeProvider();
            var compilerParameters = new CompilerParameters(
                new[]
                {
                    "system.dll",
                    "system.xml.dll",
                    "system.data.dll",
                    "system.windows.forms.dll",
                    "system.drawing.dll"
                })
            {
                CompilerOptions = "/t:library", GenerateInMemory = true
            };

            var sb = new StringBuilder();
            sb.AppendLine("namespace VM.Platform.TestAutomationFramework.Core.Transformers");
            sb.AppendLine("{ ");
            sb.AppendLine("    using System;");
            sb.AppendLine("    using System.Xml;");
            sb.AppendLine("    using System.Data;");
            sb.AppendLine("    using System.Data.SqlClient;");
            sb.AppendLine("    using System.Windows.Forms;");
            sb.AppendLine("    using System.Drawing;");

            sb.AppendLine("    public class CSharpCodeEvaluator");
            sb.AppendLine("    {");
            sb.AppendLine("        public object GetValue()");
            sb.AppendLine("        {");
            sb.AppendLine("            return " + code + "; ");
            sb.AppendLine("        } ");
            sb.AppendLine("    } ");
            sb.AppendLine("}");

            var compilerResults = codeDomProvider.CompileAssemblyFromSource(compilerParameters, sb.ToString());
            if (compilerResults.Errors.Count > 0)
            {
                var errorMessage = compilerResults.Errors
                    .Cast<CompilerError>()
                    .Select(err => err.ErrorText)
                    .Aggregate((a, i) => a + "\t\n" + i);
                throw new WorkflowFailedException(string.Format("Could not evaluate C# code. Reason:\n{0}", errorMessage));
            }

            var assembly = compilerResults.CompiledAssembly;
            var evaluator = assembly.CreateInstance("VM.Platform.TestAutomationFramework.Core.Transformers." +
                                              "CSharpCodeEvaluator");
            var type = evaluator.GetType();
            var getValueMethod = type.GetMethod("GetValue");
            var result = getValueMethod.Invoke(evaluator, null);

            return result;
        }
    }
}
