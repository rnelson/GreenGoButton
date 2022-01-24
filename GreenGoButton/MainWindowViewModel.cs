using System.Collections.ObjectModel;
using System.ComponentModel;
using ICSharpCode.AvalonEdit.Highlighting;

namespace GreenGoButton;

public class MainWindowViewModel : INotifyPropertyChanged
{
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;
    
    private IHighlightingDefinition? highlightingDefinition;
    public IHighlightingDefinition? HighlightingDefinition
    {
        get => highlightingDefinition;
        set
        {
            if (highlightingDefinition != value)
            {
                highlightingDefinition = value;
                OnPropertyChanged(nameof(HighlightingDefinition));
            }
        }
    }

    public ReadOnlyCollection<IHighlightingDefinition>? HighlightingDefinitions
    {
        get
        {
            var manager = HighlightingManager.Instance;
            if (manager != null)
                return manager.HighlightingDefinitions;

            return null;
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}