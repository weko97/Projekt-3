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

    private RezervaceServis()   // konstruktor (zmena na private)
    {
        rezervace = appDataSpravce.NactiRezervace();
    }

    public List<Rezervace> ZiskejVsechnyRezervace() // vraci seznam rezervaci 
    {
        return rezervace;
    }

    public string PridejRezervaci(Rezervace novaRezervace) // pridava nove rezervace
    {
        string chyba = ZkontrolujRezervaci(novaRezervace); // validace vstupu do rezervaci

        if (chyba != "")
        {
            return chyba;
        }

        if (MaKolizi(novaRezervace))
        {
            return "Tento termín je už obsazený.";
        }

        novaRezervace.Id = ZiskejNoveId(); 
        rezervace.Add(novaRezervace); // prida rezervaci do seznamu 

        appDataSpravce.UlozRezervace(rezervace);

        return ""; // zda vrati prazdny string, all good
    }

    public void SmazRezervaci(int id) // smaze vybranou rezervaci, podle ID
    {
        Rezervace? nalezenaRezervace = rezervace.FirstOrDefault(r => r.Id == id); //projde seznam

        if (nalezenaRezervace != null) // kontorla zda ID neextistuje 
        {
            rezervace.Remove(nalezenaRezervace);
            appDataSpravce.UlozRezervace(rezervace);// zavola ukladani do txt at je vse stejne 
        }
    }

    public string ZkontrolujRezervaci(Rezervace novaRezervace) // zakladni validace vstupu
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

    public bool MaKolizi(Rezervace novaRezervace) // kontroluje, zda se nova rezervace casove neprekryva s nejakou, co uz v systemu je
    {
        return rezervace.Any(staraRezervace =>
            staraRezervace.Id != novaRezervace.Id && // kontrola aby jsme nebrali jako kolizi stejnou rezervaci 
            staraRezervace.NazevZdroje == novaRezervace.NazevZdroje && // kontrola zda se jedna o jinou mistnost/predmet nez v vybrane rezervaci
            staraRezervace.Datum.Date == novaRezervace.Datum.Date && // kontroluje zda jsou v jiny den i kdyz se čas překrývá 
            novaRezervace.CasOd < staraRezervace.CasDo &&
            novaRezervace.CasDo > staraRezervace.CasOd // hlavni kontorla, zda se casy prekrivaji 
        );
    }

    public List<Rezervace> FiltrujPodleData(DateTime datum) // vraci rezervaci podle dne
    {
        return rezervace
            .Where(r => r.Datum.Date == datum.Date)
            .ToList();
    }

    public List<Rezervace> FiltrujPodleZdroje(string nazevZdroje) // vraci rezervaci podle typu rezervace (napr. ucebna, vybavení, sportoviště atd)
    {
        return rezervace
            .Where(r => r.NazevZdroje == nazevZdroje)
            .ToList();
    }

    public List<Rezervace> FiltrujPodleUzivatele(string jmenoUzivatele) // vraci rezervaci podle uzivatele ktery ji udelal
    {
        return rezervace
            .Where(r => r.JmenoUzivatele == jmenoUzivatele)
            .ToList();
    }

    public void ExportujRezervace(string vlastniCesta = null) // predelava aktualni seznam do AppData
    {
        appDataSpravce.ExportujRezervace(rezervace, vlastniCesta);
    }

    public string ZiskejCestuKeSlozceAppData() // vraci cestu ke slozce AppData
    {
        return appDataSpravce.ZiskejCestuKeSlozce();
    }

    private int ZiskejNoveId() // vytvari nove ID pro kazdou rezervaci 
    {
        if (rezervace.Count == 0)
        {
            return 1;
        }

        return rezervace.Max(r => r.Id) + 1; 
    }
}