using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace GgbCompiler;

public class Compiler : ICompiler
{
    protected static string RuntimeDirectory { get; set; }

    protected IList<SyntaxTree> trees;
    protected IList<PortableExecutableReference> references;

    static Compiler()
    {
        var mscorlib = typeof(object).GetTypeInfo().Assembly;
        var runtimeDirectory = Path.GetDirectoryName(mscorlib.Location);
        if (runtimeDirectory is null) throw new InvalidOperationException("unable to locate .NET runtime");

        RuntimeDirectory = runtimeDirectory;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Compiler"/> class.
    /// </summary>
    public Compiler()
    {
        trees = new List<SyntaxTree>();
        references = new List<PortableExecutableReference>();
    }

    /// <inheritdoc />
    public void AddCode(string code)
    {
        if (string.IsNullOrEmpty(code))
            throw new ArgumentException($"invalid {nameof(code)} provided", nameof(code));
        
        var tree = CSharpSyntaxTree.ParseText(code);
        trees.Add(tree);
    }

    /// <inheritdoc />
    public void AddFile(string filename)
    {
        if (!File.Exists(filename))
            throw new ArgumentException($"file <{filename}> does not exist", nameof(filename));

        var code = File.ReadAllText(filename);
        AddCode(code);
    }

    /// <inheritdoc />
    public void AddReference(string filename)
    {
        if (!File.Exists(filename))
            throw new ArgumentException($"assembly <{filename}> does not exist", nameof(filename));
        
        references.Add(MetadataReference.CreateFromFile(filename));
    }

    /// <inheritdoc />
    public void Generate(OutputKind outputKind, string destination, string baseName)
    {
        Generate(outputKind, destination, baseName, null);
    }

    /// <inheritdoc />
    public void Generate(OutputKind outputKind, string destination, string baseName, string? pdbName)
    {
        AddDefaultReferences();

        var extension = "exe";
        switch (outputKind)
        {
            case OutputKind.ConsoleApplication:
            case OutputKind.WindowsApplication:
            case OutputKind.WindowsRuntimeApplication:
                extension = "exe";
                break;
            case OutputKind.DynamicallyLinkedLibrary:
            case OutputKind.NetModule:
                extension = "dll";
                break;
            case OutputKind.WindowsRuntimeMetadata:
            default:
                throw new ArgumentOutOfRangeException(nameof(outputKind), outputKind, null);
        }

        var filename = $"{baseName}.{extension}";
        var pdb = $"{baseName}.pdb";
        var runtime = $"{baseName}.runtimeconfig.json";

        var compilation = CSharpCompilation.Create(
            baseName,
            trees,
            references,
            new CSharpCompilationOptions(outputKind)
        );
        _ = compilation.Emit(Path.Combine(destination, filename), Path.Combine(destination, pdb));
        File.WriteAllText(Path.Combine(destination, runtime), GenerateRuntimeConfig());
    }

    protected virtual void AddDefaultReferences()
    {
        var sysRuntime = Path.Combine(RuntimeDirectory, @"System.Runtime.dll");
        
        references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
        references.Add(MetadataReference.CreateFromFile(typeof(Console).Assembly.Location));
        references.Add(MetadataReference.CreateFromFile(sysRuntime));
    }

    protected virtual string GenerateRuntimeConfig()
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });

        writer.WriteStartObject();
        writer.WriteStartObject("runtimeOptions");
        writer.WriteStartObject("framework");
        writer.WriteString("name", "Microsoft.NETCore.App");
        writer.WriteString("version", RuntimeInformation.FrameworkDescription.Replace(".NET ", string.Empty));
        writer.WriteEndObject();
        writer.WriteEndObject();
        writer.WriteEndObject();
        writer.Flush();

        return Encoding.UTF8.GetString(stream.ToArray());
    }
}