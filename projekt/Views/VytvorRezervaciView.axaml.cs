using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using projekt.Models;
using projekt.Servis;

namespace projekt.Views
{
    public partial class VytvoritRezervaciView : UserControl
    {
        public VytvoritRezervaciView() => InitializeComponent();

        private void Datum_TextChanged(object sender, TextChangedEventArgs e)
        {
            // automaticky doplnuje tecky pri psani data, aby to uzivatel nemusel mackat rucne
            var tb = (TextBox)sender;
            string text = tb.Text.Replace(".", "");
            if (text.Length > 4) text = text.Substring(0, 2) + "." + text.Substring(2, 2) + "." + text.Substring(4);
            else if (text.Length > 2) text = text.Substring(0, 2) + "." + text.Substring(2);
            if (tb.Text != text) { tb.Text = text; tb.CaretIndex = text.Length; }
        }

        private void Cas_TextChanged(object sender, TextChangedEventArgs e)
        {
            // automaticky doplnuje dvojtecku pri psani casu
            var tb = (TextBox)sender;
            string text = tb.Text.Replace(":", "");
            if (text.Length > 2) text = text.Substring(0, 2) + ":" + text.Substring(2);
            if (tb.Text != text) { tb.Text = text; tb.CaretIndex = text.Length; }
        }

        private void VytvoritTlacitko_Click(object sender, RoutedEventArgs e)
        {
            ChybovaHlaska.Foreground = Brushes.Red;
            ChybovaHlaska.Text = "";

            // zkusi prevest zadany text na realne datum. Pokud to nejde, napise chybu a nepusti kod dal (return)
            if (!DateTime.TryParse(DatumTextBox.Text, out DateTime datum))
            {
                ChybovaHlaska.Text = "Neplatné datum! (DD.MM.RRRR)";
                return;
            }

            // to same s casem. Zkontroluje, jestli davaji oba casy smysl
            if (!TimeSpan.TryParse(CasOdTextBox.Text, out TimeSpan casOd) || 
                !TimeSpan.TryParse(CasDoTextBox.Text, out TimeSpan casDo))
            {
                ChybovaHlaska.Text = "Neplatný čas! (HH:mm)";
                return;
            }

            // pripravi si novy objekt rezervace z toho, co uzivatel naklikal a napsal
            Rezervace novaRezervace = new Rezervace {
                JmenoUzivatele = JmenoTextBox.Text,
                NazevZdroje = (ZdrojComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "",
                Datum = datum,
                CasOd = casOd,
                CasDo = casDo
            };

            // pokusi se ulozit rezervaci. Pokud narazi na problem (napr. kolize s jinou rezervaci), vrati chybovou hlasku
            string chyba = RezervaceServis.Instance.PridejRezervaci(novaRezervace);
            
            if (!string.IsNullOrEmpty(chyba))
            {
                // pokud servis vratil chybu, vypise ji na obrazovku
                ChybovaHlaska.Text = chyba;
            }
            else
            {
                // vse proslo v poradku, napise to zelene a vyprazdni policka pro dalsi zadavani
                ChybovaHlaska.Foreground = Brushes.Green;
                ChybovaHlaska.Text = "Rezervace úspěšně vytvořena!";
                
                JmenoTextBox.Text = "";
                DatumTextBox.Text = "";
                CasOdTextBox.Text = "";
                CasDoTextBox.Text = "";
            }
        }
    }
}