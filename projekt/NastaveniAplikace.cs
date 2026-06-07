using System;
using System.IO;

namespace projekt.Models;

// staticka trida (v programu existuje prave jednou) pro uchovani globalniho nastaveni
public static class NastaveniAplikace
{
    public static string CestaKSouboru { get; set; } = @"C:\RezervacniSystem\";
    public static bool BarevneOznaceni { get; set; } = true;
    public static bool UpozorneniKolize { get; set; } = true;
    public static bool DarkMode { get; set; } = false;

    // cesta, kam se nastaveni fyzicky uklada (do skryte slozky AppData primo ve Windows)
    private static string CestaConfigu = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RezervacniSystem", "config.txt");

    public static void UlozNastaveni()
    {
        try 
        {
            // zajisti, ze slozka pro ulozeni opravdu existuje (pokud ne, vytvori ji)
            Directory.CreateDirectory(Path.GetDirectoryName(CestaConfigu)); 
            
            // spoji vsechny hodnoty do jednoho radku textu, oddeli je svislitkem a zapise do souboru
            string obsah = $"{CestaKSouboru}|{BarevneOznaceni}|{UpozorneniKolize}|{DarkMode}";
            File.WriteAllText(CestaConfigu, obsah);
        }
        catch { /* osetreni chyby pri zapisu */ }
    }

    public static void NactiNastaveni()
    {
        // nejdriv zkontroluje, jestli soubor s nastavenim vubec existuje
        if (File.Exists(CestaConfigu))
        {
            // nacte text ze souboru a rozseka ho podle svislitka na jednotlive hodnoty
            string[] casti = File.ReadAllText(CestaConfigu).Split('|');
            
            if (casti.Length == 4)
            {
                CestaKSouboru = casti[0];
                
                // prevede textovou podobu "True/False" zpatky na pouzitelnou logickou hodnotu (bool)
                BarevneOznaceni = bool.Parse(casti[1]);
                UpozorneniKolize = bool.Parse(casti[2]);
                DarkMode = bool.Parse(casti[3]);
            }
        }
    }
}