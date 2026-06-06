using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using projekt.Models;
// pouzivam z importu toto Environment ; DateTime ; TimeSpan ; prace s listem ; CultureInfo.InvariantCulture (pro datum); Path ; Directory a File (pro praci se soubory)

namespace projekt.AppData;

public class AppDataSpravce
{
    private readonly string slozkaAplikace;
    private readonly string souborRezervaci;
    //proměne, jako cesty

    public AppDataSpravce() //konstrukotr
    {
        //dalsi cesty
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        slozkaAplikace = Path.Combine(appData, "RezervacniSystem");
        souborRezervaci = Path.Combine(slozkaAplikace, "rezervace.txt");

        Directory.CreateDirectory(slozkaAplikace);
        // vytvari tu slozku zda jeste neni
    }

    public void UlozRezervace(List<Rezervace> rezervace) // uklada seznam rezervaci do txt souboru
    {
        List<string> radky = new List<string>();
        

        foreach (Rezervace jednaRezervace in rezervace) //projede kazdou rezervaci 
        {
            string radek =
                jednaRezervace.Id + "|" +
                OsetriText(jednaRezervace.JmenoUzivatele) + "|" +
                OsetriText(jednaRezervace.NazevZdroje) + "|" +
                OsetriText(jednaRezervace.TypZdroje) + "|" +
                jednaRezervace.Datum.ToString("yyyy-MM-dd") + "|" +
                jednaRezervace.CasOd.ToString(@"hh\:mm") + "|" +
                jednaRezervace.CasDo.ToString(@"hh\:mm") + "|" +
                OsetriText(jednaRezervace.Poznamka);
                                                               //prevadime na txt !
            radky.Add(radek);
        }

        File.WriteAllLines(souborRezervaci, radky);
    }

    public List<Rezervace> NactiRezervace()  // vezme txt a a vytvori z nej seznam objektu Rezervace
    {
        List<Rezervace> rezervace = new List<Rezervace>();

        if (!File.Exists(souborRezervaci)) // existuje txt? 
        {
            return rezervace;
        }

        string[] radky = File.ReadAllLines(souborRezervaci);

        foreach (string radek in radky)
        {
            string[] casti = radek.Split('|');

            if (casti.Length < 8)
            {
                continue;
            }      // kontroluje jestli objekt ma vsechny atributy 

            Rezervace jednaRezervace = new Rezervace
            {
                Id = int.Parse(casti[0]),
                JmenoUzivatele = casti[1],
                NazevZdroje = casti[2],
                TypZdroje = casti[3],
                Datum = DateTime.ParseExact(casti[4], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                CasOd = TimeSpan.Parse(casti[5]),
                CasDo = TimeSpan.Parse(casti[6]),
                Poznamka = casti[7]
                                                  // prevadime txt zas na objekty 
            };

            rezervace.Add(jednaRezervace); // dame rezervaci do seznamu 
        }

        return rezervace; 
    }

    public void ExportujRezervace(List<Rezervace> rezervace)    // vytvarime dalsi txt, ktery bude hezci nez ten s kterym pracuje program 
    {
        string souborExportu = Path.Combine(slozkaAplikace, "export_rezervaci.txt"); 

        List<string> radky = new List<string>(); 

        radky.Add("EXPORT REZERVACI");
        radky.Add("================");
        radky.Add("");                    // visual things

        foreach (Rezervace jednaRezervace in rezervace) // prochazi rezervace 
        {
            radky.Add("ID: " + jednaRezervace.Id);
            radky.Add("Uživatel: " + jednaRezervace.JmenoUzivatele);
            radky.Add("Zdroj: " + jednaRezervace.NazevZdroje);
            radky.Add("Typ: " + jednaRezervace.TypZdroje);
            radky.Add("Datum: " + jednaRezervace.Datum.ToString("dd.MM.yyyy"));
            radky.Add("Čas: " + jednaRezervace.CasOd.ToString(@"hh\:mm") + " - " + jednaRezervace.CasDo.ToString(@"hh\:mm"));
            radky.Add("Poznámka: " + jednaRezervace.Poznamka);
            radky.Add("----------------");
        }

        File.WriteAllLines(souborExportu, radky); // zapise export do hezciho txt 
    }

    public string ZiskejCestuKeSlozce()
    {
        return slozkaAplikace;
    }                                  // vraci cesty

    private string OsetriText(string text)
    {
        
        // (ochrana) v txt nemuzem mit tento znak | bo to je oddělovač 
        return text.Replace("|", " ");
    }
}