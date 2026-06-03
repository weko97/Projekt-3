using System.Collections.Generic;
using System.Linq;
using projekt.Models;

namespace projekt.Services;

public class RezervaceServis
{
    private List<Rezervace> rezervace = new List<Rezervace>();

    public List<Rezervace> ZiskejVsechnyRezervace()
    {
        return rezervace;
    }

    public string PridejRezervaci(Rezervace novaRezervace)
    {
        string chyba = ZkontrolujRezervaci(novaRezervace);

        if (chyba != "")
        {
            return chyba;
        }

        if (MaKolizi(novaRezervace))
        {
            return "Tento termín je už obsazený.";
        }

        novaRezervace.Id = rezervace.Count + 1;
        rezervace.Add(novaRezervace);

        return "";
    }

    public string ZkontrolujRezervaci(Rezervace novaRezervace)
    {
        if (novaRezervace.JmenoUzivatele == "")
        {
            return "Jméno uživatele nesmí být prázdné.";
        }

        if (novaRezervace.NazevZdroje == "")
        {
            return "Musíš zadat učebnu, sportoviště nebo vybavení.";
        }

        if (novaRezervace.CasOd >= novaRezervace.CasDo)
        {
            return "Čas začátku musí být dřív než čas konce.";
        }

        return "";
    }

    public bool MaKolizi(Rezervace novaRezervace)
    {
        return rezervace.Any(staraRezervace =>
            staraRezervace.NazevZdroje == novaRezervace.NazevZdroje &&
            staraRezervace.Datum.Date == novaRezervace.Datum.Date &&
            novaRezervace.CasOd < staraRezervace.CasDo &&
            novaRezervace.CasDo > staraRezervace.CasOd
        );
    }
}