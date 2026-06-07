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
    private readonly string souborUzivatelu;
    private readonly string souborZdroju;
    // promenne, jako cesty

    public AppDataSpravce()
    {
        //dalsi cesty
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        slozkaAplikace = Path.Combine(appData, "RezervacniSystem");
        souborRezervaci = Path.Combine(slozkaAplikace, "rezervace.txt");
        souborUzivatelu = Path.Combine(slozkaAplikace, "uzivatele.txt");
        souborZdroju = Path.Combine(slozkaAplikace, "zdroje.txt");

        Directory.CreateDirectory(slozkaAplikace);
        // vytvari tu slozku zda jeste neni
    }

    public void UlozRezervace(List<Rezervace> rezervace) // uklada seznam rezervaci do txt souboru
    {
        List<string> radky = new List<string>();

        foreach (Rezervace jednaRezervace in rezervace) // projede kazdou rezervaci 
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

    public List<Rezervace> NactiRezervace() // vezme txt a a vytvori z nej seznam objektu Rezervace
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
            } // kontroluje jestli objekt ma vsechny atributy 

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

            rezervace.Add(jednaRezervace);// dame rezervaci do seznamu 
        }

        return rezervace;
    }

    public void ExportujRezervace(List<Rezervace> rezervace, string vlastniCesta = null) // vytvarime dalsi txt, ktery bude hezci nez ten s kterym pracuje program 
    {
        string cilovaSlozka = string.IsNullOrWhiteSpace(vlastniCesta) ? slozkaAplikace : vlastniCesta;

        if (!Directory.Exists(cilovaSlozka))
        {
            Directory.CreateDirectory(cilovaSlozka);
        }

        string souborExportu = Path.Combine(cilovaSlozka, "export_rezervaci.txt");

        List<string> radky = new List<string>();

        radky.Add("EXPORT REZERVACI");
        radky.Add("================");
        radky.Add($"Vytvořeno: {DateTime.Now:dd.MM.yyyy HH:mm}");
        radky.Add("");

        foreach (Rezervace jednaRezervace in rezervace) // prochazi rezervace 
        {
            radky.Add("ID: " + jednaRezervace.Id);
            radky.Add("Uživatel: " + jednaRezervace.JmenoUzivatele);
            radky.Add("Zdroj: " + jednaRezervace.NazevZdroje);
            radky.Add("Typ: " + jednaRezervace.TypZdroje);
            radky.Add("Datum: " + jednaRezervace.Datum.ToString("dd.MM.yyyy"));
            radky.Add("Čas: " + jednaRezervace.CasOd.ToString(@"hh\:mm") + " - " + jednaRezervace.CasDo.ToString(@"hh\:mm"));
            // radky.Add("Poznámka: " + jednaRezervace.Poznamka);
            radky.Add("----------------");
        }

        File.WriteAllLines(souborExportu, radky); // zapise export do hezciho txt 
    }

    public string ZiskejCestuKeSlozce()
    {
        return slozkaAplikace;
    } // vraci cesty

    public void UlozUzivatele(List<Uzivatele> uzivatele)
    {
        List<string> radky = new List<string>();
        foreach (Uzivatele jedenUzivatel in uzivatele)
        {
            string radek =
                jedenUzivatel.Id + "|" +
                OsetriText(jedenUzivatel.Jmeno);
            radky.Add(radek);
        }
        File.WriteAllLines(souborUzivatelu, radky);
    }

    public List<Uzivatele> NactiUzivatele()
    {
        List<Uzivatele> uzivatele = new List<Uzivatele>();
        if (!File.Exists(souborUzivatelu))
        {
            return uzivatele;
        }
        string[] radky = File.ReadAllLines(souborUzivatelu);
        foreach (string radek in radky)
        {
            string[] casti = radek.Split('|');
            if (casti.Length < 2)
            {
                continue;
            }
            Uzivatele jedenUzivatel = new Uzivatele
            {
                Id = int.Parse(casti[0]),
                Jmeno = casti[1]
            };
            uzivatele.Add(jedenUzivatel);
        }
        return uzivatele;
    }

    public void UlozZdroje(List<Zdroj> zdroje)
    {
        List<string> radky = new List<string>();
        foreach (Zdroj jedenZdroj in zdroje)
        {
            string radek =
                jedenZdroj.Id + "|" +
                OsetriText(jedenZdroj.Nazev) + "|" +
                OsetriText(jedenZdroj.Typ); // TADY BYLA TA CHYBA - OPRAVENO NA jedenZdroj
            radky.Add(radek);
        }
        File.WriteAllLines(souborZdroju, radky);
    }

    public List<Zdroj> NactiZdroje()
    {
        List<Zdroj> zdroje = new List<Zdroj>();
        if (!File.Exists(souborZdroju))
        {
            return zdroje;
        }
        string[] radky = File.ReadAllLines(souborZdroju);
        foreach (string radek in radky)
        {
            string[] casti = radek.Split('|');
            if (casti.Length < 3)
            {
                continue;
            }
            Zdroj jedenZdroj = new Zdroj
            {
                Id = int.Parse(casti[0]),
                Nazev = casti[1],
                Typ = casti[2]
            };
            zdroje.Add(jedenZdroj);
        }
        return zdroje;
    }

    public string OsetriText(string text) // zmena na public
    {
        return text?.Replace("|", " ") ?? "";
    }
}