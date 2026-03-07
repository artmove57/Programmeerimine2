using System.Windows;
using KooliProjekt.WpfClient.ViewModels;

namespace KooliProjekt.WpfClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// MVVM pattern - kogu loogika on ViewModelis
/// MainWindow seostab ViewModeli külge OnError action'i
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Seo ViewModeli OnError action MessageBox'iga
        if (DataContext is MainWindowViewModel viewModel)
        {
            viewModel.OnError = ShowErrorMessage;
        }
    }

    /// <summary>
    /// Näita veateadet kasutajale
    /// See meetod seostatakse ViewModeli OnError action'iga
    /// </summary>
    private void ShowErrorMessage(string errorMessage)
    {
        MessageBox.Show(
            errorMessage,
            "Viga",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}
