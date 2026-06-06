using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO; // Přidáno pro práci s cestami (Path)
using projekt.Models;
using projekt.Servis;

namespace projekt.Views
{
    public partial class PrehledRezervaciView : UserControl
    {
        // Reference na sdílenou instanci
        private RezervaceServis _servis => RezervaceServis.Instance;

        public PrehledRezervaciView()
        {
            InitializeComponent();
            NactiRezervaceDoSeznamu();
        }

        private void NactiRezervaceDoSeznamu()
        {
            RezervaceListBox.ItemsSource = _servis.ZiskejVsechnyRezervace();
        }

        private void SmazatTlacitko_Click(object sender, RoutedEventArgs e)
        {
            if (RezervaceListBox.SelectedItem is Rezervace vybrana)
            {
                _servis.SmazRezervaci(vybrana.Id);
                NactiRezervaceDoSeznamu();
            }
        }

        // TATO METODA JE NYNÍ PROPOJENÁ S NASTAVENÍM
        private void ExportTlacitko_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Získání cesty ze statické třídy NastaveniAplikace
                string cesta = NastaveniAplikace.CestaKSouboru;

                // 2. Ošetření, aby se exportovalo do složky
                // Pokud uživatel zadal např. "C:\Slozka\soubor.txt", potřebujeme jen "C:\Slozka\"
                string slozkaProExport = cesta;
                
                if (!string.IsNullOrWhiteSpace(cesta))
                {
                    // Pokud cesta obsahuje tečku (předpokládáme příponu souboru), vezmeme z ní adresář
                    if (cesta.Contains("."))
                    {
                        slozkaProExport = Path.GetDirectoryName(cesta) ?? cesta;
                    }
                }

                // 3. Zavolání servisu (pokud je slozkaProExport prázdná, servis použije výchozí AppData)
                _servis.ExportujRezervace(slozkaProExport);

                Debug.WriteLine("Export úspěšně vytvořen do: " + (string.IsNullOrWhiteSpace(slozkaProExport) ? "Výchozí AppData" : slozkaProExport));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Chyba při exportu: " + ex.Message);
            }
        }

        private void PouzitFiltry_Click(object sender, RoutedEventArgs e)
        {
            var vyfiltrovane = _servis.ZiskejVsechnyRezervace().ToList();

            if (FiltrKalendar.SelectedDate.HasValue)
            {
                vyfiltrovane = vyfiltrovane.Where(r => r.Datum.Date == FiltrKalendar.SelectedDate.Value.Date).ToList();
            }

            var vybranaPolozka = FiltrZdrojComboBox.SelectedItem as ComboBoxItem;
            string vybraneMisto = vybranaPolozka?.Content?.ToString() ?? "";
            
            if (vybraneMisto != "Zobrazit vše" && !string.IsNullOrEmpty(vybraneMisto))
            {
                vyfiltrovane = vyfiltrovane.Where(r => r.NazevZdroje == vybraneMisto).ToList();
            }

            RezervaceListBox.ItemsSource = vyfiltrovane;
        }

        private void ZrusitFiltry_Click(object sender, RoutedEventArgs e)
        {
            FiltrKalendar.SelectedDate = null;
            FiltrZdrojComboBox.SelectedIndex = 0;
            NactiRezervaceDoSeznamu();
        }
    }
}