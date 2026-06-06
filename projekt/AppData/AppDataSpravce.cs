using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using projekt.Models;

namespace projekt.AppData;

public class AppDataSpravce
{
    private readonly string slozkaAplikace;
    private readonly string souborRezervaci;
    private readonly string souborUzivatelu;
    private readonly string souborZdroju;

    public AppDataSpravce()
    {
        string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        slozkaAplikace = Path.Combine(appData, "RezervacniSystem");
        souborRezervaci = Path.Combine(slozkaAplikace, "rezervace.txt");
        souborUzivatelu = Path.Combine(slozkaAplikace, "uzivatele.txt");
        souborZdroju = Path.Combine(slozkaAplikace, "zdroje.txt");

        Directory.CreateDirectory(slozkaAplikace);
    }

    public void UlozRezervace(List<Rezervace> rezervace)
    {
        List<string> radky = new List<string>();

        foreach (Rezervace jednaRezervace in rezervace)
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
            radky.Add(radek);
        }

        File.WriteAllLines(souborRezervaci, radky);
    }

    public List<Rezervace> NactiRezervace()
    {
        List<Rezervace> rezervace = new List<Rezervace>();

        if (!File.Exists(souborRezervaci))
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
            }

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
            };

            rezervace.Add(jednaRezervace);
        }

        return rezervace;
    }

    public void ExportujRezervace(List<Rezervace> rezervace, string vlastniCesta = null)
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

        foreach (Rezervace jednaRezervace in rezervace)
        {
            radky.Add("ID: " + jednaRezervace.Id);
            radky.Add("Uživatel: " + jednaRezervace.JmenoUzivatele);
            radky.Add("Zdroj: " + jednaRezervace.NazevZdroje);
            radky.Add("Typ: " + jednaRezervace.TypZdroje);
            radky.Add("Datum: " + jednaRezervace.Datum.ToString("dd.MM.yyyy"));
            radky.Add("Čas: " + jednaRezervace.CasOd.ToString(@"hh\:mm") + " - " + jednaRezervace.CasDo.ToString(@"hh\:mm"));
            //radky.Add("Poznámka: " + jednaRezervace.Poznamka);
            radky.Add("----------------");
        }

        File.WriteAllLines(souborExportu, radky);
    }

    public string ZiskejCestuKeSlozce()
    {
        return slozkaAplikace;
    }

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

    public string OsetriText(string text)
    {
        return text?.Replace("|", " ") ?? "";
    }
}