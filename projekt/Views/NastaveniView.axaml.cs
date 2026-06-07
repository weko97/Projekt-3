using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using projekt.Models; 

namespace projekt.Views
{
    public partial class NastaveniView : UserControl
    {
        public NastaveniView()
        {
            InitializeComponent();

            // nacte aktualni nastaveni z pameti a zobrazi ho v grafickych prvcich (textboxy, zaskrtavatka)
            CestaTextBox.Text = NastaveniAplikace.CestaKSouboru;
            BarvyCheckBox.IsChecked = NastaveniAplikace.BarevneOznaceni;
            KolizeCheckBox.IsChecked = NastaveniAplikace.UpozorneniKolize;
            DarkModeCheckBox.IsChecked = NastaveniAplikace.DarkMode;
        }

        private void UlozitTlacitko_Click(object sender, RoutedEventArgs e)
        {
            // prevezme naklikane hodnoty a ulozi je do pameti aplikace
            // dva otazniky (??) rikaji: pokud je to nevyplnene (null), ber to automaticky jako false
            NastaveniAplikace.CestaKSouboru = CestaTextBox.Text;
            NastaveniAplikace.BarevneOznaceni = BarvyCheckBox.IsChecked ?? false;
            NastaveniAplikace.UpozorneniKolize = KolizeCheckBox.IsChecked ?? false;
            NastaveniAplikace.DarkMode = DarkModeCheckBox.IsChecked ?? false;

            // zavola metodu, ktera tyto zmeny fyzicky zapise do souboru na disk
            NastaveniAplikace.UlozNastaveni();

            // okamzite prepne cely vzhled aplikace na tmavy nebo svetly rezim podle toho, co uzivatel zaskrtl
            if (Application.Current != null)
            {
                Application.Current.RequestedThemeVariant = NastaveniAplikace.DarkMode 
                    ? ThemeVariant.Dark 
                    : ThemeVariant.Light;
            }
        }

        private void ZrusitTlacitko_Click(object sender, RoutedEventArgs e)
        {
            // ignoruje to, co uzivatel naklikal, a vrati formular zpatky do naposledy ulozeneho stavu
            CestaTextBox.Text = NastaveniAplikace.CestaKSouboru;
            BarvyCheckBox.IsChecked = NastaveniAplikace.BarevneOznaceni;
            KolizeCheckBox.IsChecked = NastaveniAplikace.UpozorneniKolize;
            DarkModeCheckBox.IsChecked = NastaveniAplikace.DarkMode;
        }
    }
}