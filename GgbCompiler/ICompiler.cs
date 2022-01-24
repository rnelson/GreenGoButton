using Microsoft.CodeAnalysis;

namespace GgbCompiler;

public interface ICompiler
{
    /// <summary>
    /// Adds a block of code.
    /// </summary>
    /// <param name="code">The source code to add.</param>
    /// <remarks>
    /// <paramref name="code"/> is expected to be a discrete, compilable chunk of code (like a single file from a
    /// project).
    /// </remarks>
    void AddCode(string code);
    
    /// <summary>
    /// Adds a source file.
    /// </summary>
    /// <param name="filename">The filename to the source code to add.</param>
    /// <remarks>
    /// <paramref name="filename"/> is expected to be a discrete, compilable chunk of code.
    /// </remarks>
    void AddFile(string filename);

    /// <summary>
    /// Adds a reference to an assembly.
    /// </summary>
    /// <param name="filename">The absolute path to the assembly to reference.</param>
    void AddReference(string filename);

    /// <summary>
    /// Compiles the code, creating an assembly of type <paramref name="outputKind"/> saved to
    /// <paramref name="baseName"/>.
    /// </summary>
    /// <param name="outputKind">The type of output to generate.</param>
    /// <param name="destination">The destination folder to save output file(s) to.</param>
    /// <param name="baseName">The base name for the destination assembly.</param>
    void Generate(OutputKind outputKind, string destination, string baseName);

    /// <summary>
    /// Compiles the code, creating an assembly of type <paramref name="outputKind"/> saved to
    /// <paramref name="baseName"/>.
    /// </summary>
    /// <param name="outputKind">The type of output to generate.</param>
    /// <param name="destination">The destination folder to save output file(s) to.</param>
    /// <param name="baseName">The absolute path to save the compiled assembly to.</param>
    /// <param name="baseName">The base name for the destination assembly.</param>
    void Generate(OutputKind outputKind, string destination, string baseName, string? pdbName);
}