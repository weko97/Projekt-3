using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using projekt.Views; 

namespace projekt
{
    // tohle je hlavni okno cele aplikace, do ktereho se nacitaji jednotlive podstranky
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // hned pri spusteni programu se do hlavni casti okna nacte domovska stranka
            MainContent.Content = new HomeView();
        }

        // nasledujici metody funguji jako jednoduche navigacni menu
        // po kliknuti na prislusne tlacitko zmeni obsah (Content) v hlavnim okne na jinou obrazovku
        
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