using System;
using System.IO;

namespace projekt.Models;

public static class NastaveniAplikace
{
    public static string CestaKSouboru { get; set; } = @"C:\RezervacniSystem\";
    public static bool BarevneOznaceni { get; set; } = true;
    public static bool UpozorneniKolize { get; set; } = true;
    public static bool DarkMode { get; set; } = false;

    private static string CestaConfigu = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RezervacniSystem", "config.txt");

    public static void UlozNastaveni()
    {
        try 
        {
            // Zajistí, že složka existuje, jinak ji vytvoří
            Directory.CreateDirectory(Path.GetDirectoryName(CestaConfigu)); 
            string obsah = $"{CestaKSouboru}|{BarevneOznaceni}|{UpozorneniKolize}|{DarkMode}";
            File.WriteAllText(CestaConfigu, obsah);
        }
        catch { /* Ošetření chyby při zápisu */ }
    }

    public static void NactiNastaveni()
    {
        if (File.Exists(CestaConfigu))
        {
            string[] casti = File.ReadAllText(CestaConfigu).Split('|');
            if (casti.Length == 4)
            {
                CestaKSouboru = casti[0];
                BarevneOznaceni = bool.Parse(casti[1]);
                UpozorneniKolize = bool.Parse(casti[2]);
                DarkMode = bool.Parse(casti[3]);
            }
        }
    }
}