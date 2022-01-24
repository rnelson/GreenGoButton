using System.IO;
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
        }

        private void GoButton_OnClick(object sender, RoutedEventArgs e)
        {
            var code = CodeTextBox.Text.Trim();
            var compiler = new Compiler();
            compiler.AddCode(code);

            var root = @"C:\temp\ggb";
            var dir = Path.Combine(root, Path.GetRandomFileName());
            Directory.CreateDirectory(dir);

            var file = "myapp";
            var type = OutputKind.ConsoleApplication;

            compiler.Generate(type, dir, file);
        }
    }
}
