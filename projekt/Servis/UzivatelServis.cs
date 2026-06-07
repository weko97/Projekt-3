using System.Collections.Generic;
using System.Linq;
using projekt.AppData;
using projekt.Models;

namespace projekt.Servis;

public class UzivatelServis
{
    private List<Uzivatele> uzivatele = new List<Uzivatele>();
    private AppDataSpravce appDataSpravce = new AppDataSpravce();
    
    public UzivatelServis()
    {
        // nacte ulozene uzivatele hned pri startu
        uzivatele = appDataSpravce.NactiUzivatele();
    }
    
    public List<Uzivatele> ZiskejVsechnyUzivatele()
    {
        return uzivatele;
    }
    
    public string PridejUzivatele(Uzivatele novyUzivatel)
    {
        if (novyUzivatel.Jmeno == "")
        {
            return "Jméno uživatele nesmí být prázdné.";
        }
        
        // zkontroluje, jestli uz nekdo se stejnym jmenem neexistuje
        bool uzivatelExistuje = uzivatele.Any(u => u.Jmeno == novyUzivatel.Jmeno);
        if (uzivatelExistuje)
        {
            return "Tento uživatel už existuje.";
        }
        
        // prideli nove ID, prida do seznamu a rovnou ulozi na disk
        novyUzivatel.Id = ZiskejNoveId();
        uzivatele.Add(novyUzivatel);
        appDataSpravce.UlozUzivatele(uzivatele);
        
        return "";
    }
    
    public void SmazUzivatele(int id)
    {
        // najde uzivatele podle ID. Kdyz existuje, smaze ho a ulozi stav
        Uzivatele? nalezenyUzivatel = uzivatele.FirstOrDefault(u => u.Id == id);
        if (nalezenyUzivatel != null)
        {
            uzivatele.Remove(nalezenyUzivatel);
            appDataSpravce.UlozUzivatele(uzivatele);
        }
    }
    
    private int ZiskejNoveId()
    {
        if (uzivatele.Count == 0)
        {
            return 1;
        }
        
        // najde to nejvyssi dosavadni ID a pricte k nemu jednicku
        return uzivatele.Max(u => u.Id) + 1;
    }
}