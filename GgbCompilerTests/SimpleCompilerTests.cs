using System;
using System.IO;
using GgbCompiler;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GgbCompilerTests
{
    [TestClass]
    public class SimpleCompilerTests
    {
        private string outputDirectory;

        [TestInitialize]
        public void Initialize()
        {
            outputDirectory = Path.GetDirectoryName(typeof(SimpleCompilerTests).Assembly.Location) ?? Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            var random = Path.GetRandomFileName();
            while (Directory.Exists(Path.Combine(outputDirectory, random)))
                random = Path.GetRandomFileName();

            outputDirectory = Path.Combine(outputDirectory, random);
            Directory.CreateDirectory(outputDirectory);
        }
        
        [TestMethod]
        public void TestSimpleAssemblyByCode()
        {
            const string code = $@"
using System;
namespace TestSimpleAssemblyByCode;
public class MyTestClass
{{
    public void Print(string message)
    {{
        Console.WriteLine(message);
    }}
}}";

            var compiler = new Compiler();
            compiler.AddCode(code);
            
            compiler.Generate(
                OutputKind.DynamicallyLinkedLibrary,
                outputDirectory,
                "TestSimpleAssemblyByCode.dll"
            );
        }
    }
}