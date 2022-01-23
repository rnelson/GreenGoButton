using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GgbCompiler
{
    public class DummyCompiler
    {
        private string directory;
        private string source;

        public DummyCompiler(string code, string directory)
        {
            this.directory = directory;
            source = code;
        }

        public void Compile(string outputFilename)
        {
            var runtimeDirectory = Path.GetDirectoryName(typeof(object).GetTypeInfo().Assembly.Location);
            
            var trees = new[] { CSharpSyntaxTree.ParseText(source) };
            var references = new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                //MetadataReference.CreateFromFile(typeof(SyntaxTree).Assembly.Location),
                //MetadataReference.CreateFromFile(typeof(CSharpSyntaxTree).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(runtimeDirectory, @"System.Runtime.dll")),
            };
            var outputKind = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            var pdb = outputFilename.ToLower().EndsWith(".dll")
                ? $"{outputFilename[0..^4]}.pdb" : $"{outputFilename}.pdb";

            var il = CSharpCompilation.Create(outputFilename, trees, references, outputKind);
            var result = il.Emit(Path.Combine(directory, outputFilename), Path.Combine(directory, pdb));
        }
    }
}