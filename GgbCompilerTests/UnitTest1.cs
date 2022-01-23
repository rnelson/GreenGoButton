using GgbCompiler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GgbCompilerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            const string code = $@"
using System;

public class MyTestClass
{{
    public void Print(string message)
    {{
        Console.WriteLine(message);
    }}
}}";

            var dir = @"D:\Projects\GreenGoButton";
            var file = @"out.dll";

            var compiler = new DummyCompiler(code, dir);
            compiler.Compile(file);
        }
    }
}