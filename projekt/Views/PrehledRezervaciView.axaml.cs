using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO; 
using projekt.Models;
using projekt.Servis;

namespace projekt.Views
{
    public partial class PrehledRezervaciView : UserControl
    {
        // ziska sdilenou instanci (tzv. Singleton), aby cela aplikace pracovala se stejnymi daty
        private RezervaceServis _servis => RezervaceServis.Instance;

        public PrehledRezervaciView()
        {
            InitializeComponent();
            NactiRezervaceDoSeznamu();
        }

        private void NactiRezervaceDoSeznamu()
        {
            // vytahne z pameti vsechny rezervace a preda je do grafickeho seznamu (ListBoxu)
            RezervaceListBox.ItemsSource = _servis.ZiskejVsechnyRezervace();
        }

        private void SmazatTlacitko_Click(object sender, RoutedEventArgs e)
        {
            // zjisti, jestli ma uzivatel v seznamu vybranou nejakou polozku. Pokud ano, smaze ji
            if (RezervaceListBox.SelectedItem is Rezervace vybrana)
            {
                _servis.SmazRezervaci(vybrana.Id);
                NactiRezervaceDoSeznamu();
            }
        }

        private void ExportTlacitko_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // nacte cestu ulozenou v nastaveni
                string cesta = NastaveniAplikace.CestaKSouboru;
                string slozkaProExport = cesta;
                
                // pokud je v ceste tecka (uzivatel zadal rovnou nejaky soubor), orizne cestu jen na danou slozku
                if (!string.IsNullOrWhiteSpace(cesta))
                {
                    if (cesta.Contains("."))
                    {
                        slozkaProExport = Path.GetDirectoryName(cesta) ?? cesta;
                    }
                }

                // spusti samotny export do teto ziskane slozky
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

            // pokud bylo vybrano konkretni datum, promaze ze seznamu vsechny ostatni dny
            if (FiltrKalendar.SelectedDate.HasValue)
            {
                vyfiltrovane = vyfiltrovane.Where(r => r.Datum.Date == FiltrKalendar.SelectedDate.Value.Date).ToList();
            }

            var vybranaPolozka = FiltrZdrojComboBox.SelectedItem as ComboBoxItem;
            string vybraneMisto = vybranaPolozka?.Content?.ToString() ?? "";
            
            // pokud bylo vybrano konkretni misto, necha v seznamu jen ho
            if (vybraneMisto != "Zobrazit vše" && !string.IsNullOrEmpty(vybraneMisto))
            {
                vyfiltrovane = vyfiltrovane.Where(r => r.NazevZdroje == vybraneMisto).ToList();
            }

            // vrati do obrazovky proskrtany seznam
            RezervaceListBox.ItemsSource = vyfiltrovane;
        }

        private void ZrusitFiltry_Click(object sender, RoutedEventArgs e)
        {
            // Vvynuluje oba filtry a necha nacist puvodni kompletni seznam
            FiltrKalendar.SelectedDate = null;
            FiltrZdrojComboBox.SelectedIndex = 0;
            NactiRezervaceDoSeznamu();
        }
    }
}