using System;
using System.Collections.Generic;
using System.Linq;
using projekt.AppData;
using projekt.Models;

namespace projekt.Servis;

public class RezervaceServis 
{
    private static RezervaceServis _instance = new RezervaceServis();
    public static RezervaceServis Instance => _instance;

    private List<Rezervace> rezervace = new List<Rezervace>();
    private AppDataSpravce appDataSpravce = new AppDataSpravce();

    private RezervaceServis()   
    {
        rezervace = appDataSpravce.NactiRezervace();
    }

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

        novaRezervace.Id = ZiskejNoveId(); 
        rezervace.Add(novaRezervace);

        appDataSpravce.UlozRezervace(rezervace);

        return "";
    }

    public void SmazRezervaci(int id) 
    {
        Rezervace? nalezenaRezervace = rezervace.FirstOrDefault(r => r.Id == id);

        if (nalezenaRezervace != null)
        {
            rezervace.Remove(nalezenaRezervace);
            appDataSpravce.UlozRezervace(rezervace);
        }
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

        if (novaRezervace.Datum == default)
        {
            return "Musíš zadat datum rezervace.";
        }

        if (novaRezervace.CasOd >= novaRezervace.CasDo)
        {
            return "Čas začátku musí být dřív než čas konce.";
        }

        DateTime zacatekRezervace = novaRezervace.Datum.Date + novaRezervace.CasOd;
        
        if (zacatekRezervace < DateTime.Now)
        {
            return "Nelze vytvořit rezervaci v minulosti.";
        }

        return "";
    }

    public bool MaKolizi(Rezervace novaRezervace)
    {
        return rezervace.Any(staraRezervace =>
            staraRezervace.Id != novaRezervace.Id &&
            staraRezervace.NazevZdroje == novaRezervace.NazevZdroje &&
            staraRezervace.Datum.Date == novaRezervace.Datum.Date &&
            novaRezervace.CasOd < staraRezervace.CasDo &&
            novaRezervace.CasDo > staraRezervace.CasOd
        );
    }

    public List<Rezervace> FiltrujPodleData(DateTime datum)
    {
        return rezervace
            .Where(r => r.Datum.Date == datum.Date)
            .ToList();
    }

    public List<Rezervace> FiltrujPodleZdroje(string nazevZdroje)
    {
        return rezervace
            .Where(r => r.NazevZdroje == nazevZdroje)
            .ToList();
    }

    public List<Rezervace> FiltrujPodleUzivatele(string jmenoUzivatele)
    {
        return rezervace
            .Where(r => r.JmenoUzivatele == jmenoUzivatele)
            .ToList();
    }

    public void ExportujRezervace(string vlastniCesta = null)
    {
        appDataSpravce.ExportujRezervace(rezervace, vlastniCesta);
    }

    public string ZiskejCestuKeSlozceAppData()
    {
        return appDataSpravce.ZiskejCestuKeSlozce();
    }

    private int ZiskejNoveId()
    {
        if (rezervace.Count == 0)
        {
            return 1;
        }

        return rezervace.Max(r => r.Id) + 1;
    }
}