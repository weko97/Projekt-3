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
        bool uzivatelExistuje = uzivatele.Any(u => u.Jmeno == novyUzivatel.Jmeno);
        if (uzivatelExistuje)
        {
            return "Tento uživatel už existuje.";
        }
        novyUzivatel.Id = ZiskejNoveId();
        uzivatele.Add(novyUzivatel);
        appDataSpravce.UlozUzivatele(uzivatele);
        return "";
    }
    public void SmazUzivatele(int id)
    {
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
        return uzivatele.Max(u => u.Id) + 1;
    }
}