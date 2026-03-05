using System.Windows;

namespace KooliProjekt.WpfClient;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// MVVM pattern - kogu loogika on ViewModelis
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // DataContext seatakse XAML failis
    }
}
