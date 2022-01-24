using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using GgbCompiler;
using Microsoft.CodeAnalysis;

namespace GreenGoButton
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var vm = new MainWindowViewModel();
            vm.HighlightingDefinition = vm.HighlightingDefinitions?.First(d => d.Name == "C#");
            DataContext = vm;
        }

        private void GoButton_OnClick(object sender, RoutedEventArgs e)
        {
            CodeTextBox.IsEnabled = false;
            GoButton.IsEnabled = false;
            
            new Thread(CompileAndRun).Start();
        }

        private void CompileAndRun()
        {
            var code = string.Empty;
            Dispatcher.Invoke(() =>
            {
                code = CodeTextBox.Text.Trim();
            });
            
            var compiler = new Compiler();
            compiler.AddCode(code);

            var root = @"C:\temp\ggb";
            var dir = Path.Combine(root, Path.GetRandomFileName());
            Directory.CreateDirectory(dir);

            var file = "myapp";
            var type = OutputKind.ConsoleApplication;

            compiler.Generate(type, dir, file);

            var proc = new ProcessStartInfo
            {
                Arguments = $"{file}.exe",
                WorkingDirectory = dir,
                FileName = "dotnet",
                UseShellExecute = true
            };
            Process.Start(proc);

            Dispatcher.Invoke(() =>
            {
                CodeTextBox.IsEnabled = true;
                GoButton.IsEnabled = true;
            });
        }
    }
}
