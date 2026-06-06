using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using projekt.Views; // Ujisti se, že tento namespace odpovídá tvému projektu!

namespace projekt
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Při startu aplikace zobrazí HomeView
            MainContent.Content = new HomeView();
        }

        private void OnHome_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HomeView();
        }

        private void OnVytvorRezervaci_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new VytvoritRezervaciView();
        }

        private void OnPrehledRezervaci_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PrehledRezervaciView();
        }

        private void OnNastaveni_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new NastaveniView();
        }
    }
}