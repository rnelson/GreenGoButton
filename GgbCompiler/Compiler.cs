using System.Reflection;

namespace GgbCompiler;

public class Compiler
{
    protected static string RuntimeDirectory { get; set; }

    static Compiler()
    {
        var mscorlib = typeof(object).GetTypeInfo().Assembly;
        var runtimeDirectory = Path.GetDirectoryName(mscorlib.Location);
        if (runtimeDirectory is null) throw new InvalidOperationException("unable to locate .NET runtime");

        Compiler.RuntimeDirectory = runtimeDirectory;
    }
}