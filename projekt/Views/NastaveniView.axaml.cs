using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using projekt.Models; // TOTO JE TO KLÍČOVÉ, CO TI TAM CHYBĚLO!

namespace projekt.Views
{
    public partial class NastaveniView : UserControl
    {
        public NastaveniView()
        {
            InitializeComponent();

            // Načtení dat při otevření obrazovky
            CestaTextBox.Text = NastaveniAplikace.CestaKSouboru;
            BarvyCheckBox.IsChecked = NastaveniAplikace.BarevneOznaceni;
            KolizeCheckBox.IsChecked = NastaveniAplikace.UpozorneniKolize;
            DarkModeCheckBox.IsChecked = NastaveniAplikace.DarkMode;
        }

        private void UlozitTlacitko_Click(object sender, RoutedEventArgs e)
        {
            // Uložení hodnot do paměti
            NastaveniAplikace.CestaKSouboru = CestaTextBox.Text;
            NastaveniAplikace.BarevneOznaceni = BarvyCheckBox.IsChecked ?? false;
            NastaveniAplikace.UpozorneniKolize = KolizeCheckBox.IsChecked ?? false;
            NastaveniAplikace.DarkMode = DarkModeCheckBox.IsChecked ?? false;

            // Fyzické uložení na disk
            NastaveniAplikace.UlozNastaveni();

            // Přepnutí režimu
            if (Application.Current != null)
            {
                Application.Current.RequestedThemeVariant = NastaveniAplikace.DarkMode 
                    ? ThemeVariant.Dark 
                    : ThemeVariant.Light;
            }
        }

        private void ZrusitTlacitko_Click(object sender, RoutedEventArgs e)
        {
            CestaTextBox.Text = NastaveniAplikace.CestaKSouboru;
            BarvyCheckBox.IsChecked = NastaveniAplikace.BarevneOznaceni;
            KolizeCheckBox.IsChecked = NastaveniAplikace.UpozorneniKolize;
            DarkModeCheckBox.IsChecked = NastaveniAplikace.DarkMode;
        }
    }
}