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
            var tb = (TextBox)sender;
            string text = tb.Text.Replace(".", "");
            if (text.Length > 4) text = text.Substring(0, 2) + "." + text.Substring(2, 2) + "." + text.Substring(4);
            else if (text.Length > 2) text = text.Substring(0, 2) + "." + text.Substring(2);
            if (tb.Text != text) { tb.Text = text; tb.CaretIndex = text.Length; }
        }

        private void Cas_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            string text = tb.Text.Replace(":", "");
            if (text.Length > 2) text = text.Substring(0, 2) + ":" + text.Substring(2);
            if (tb.Text != text) { tb.Text = text; tb.CaretIndex = text.Length; }
        }

        private void VytvoritTlacitko_Click(object sender, RoutedEventArgs e)
        {
            // Reset chybové hlášky
            ChybovaHlaska.Foreground = Brushes.Red;
            ChybovaHlaska.Text = "";

            if (!DateTime.TryParse(DatumTextBox.Text, out DateTime datum))
            {
                ChybovaHlaska.Text = "Neplatné datum! (DD.MM.RRRR)";
                return;
            }

            if (!TimeSpan.TryParse(CasOdTextBox.Text, out TimeSpan casOd) || 
                !TimeSpan.TryParse(CasDoTextBox.Text, out TimeSpan casDo))
            {
                ChybovaHlaska.Text = "Neplatný čas! (HH:mm)";
                return;
            }

            Rezervace novaRezervace = new Rezervace {
                JmenoUzivatele = JmenoTextBox.Text,
                NazevZdroje = (ZdrojComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "",
                Datum = datum,
                CasOd = casOd,
                CasDo = casDo
            };

            // Ověření přes servis (Singleton)
            string chyba = RezervaceServis.Instance.PridejRezervaci(novaRezervace);
            
            if (!string.IsNullOrEmpty(chyba))
            {
                ChybovaHlaska.Text = chyba;
            }
            else
            {
                ChybovaHlaska.Foreground = Brushes.Green;
                ChybovaHlaska.Text = "Rezervace úspěšně vytvořena!";
                
                // Vymazání polí
                JmenoTextBox.Text = "";
                DatumTextBox.Text = "";
                CasOdTextBox.Text = "";
                CasDoTextBox.Text = "";
            }
        }
    }
}