using System.Collections.Generic;
using System.Linq;
using projekt.AppData;
using projekt.Models;

namespace projekt.Servis;

public class ZdrojServis
{
    private List<Zdroj> zdroje = new List<Zdroj>();
    private AppDataSpravce appDataSpravce = new AppDataSpravce();
    public ZdrojServis()
    {
        zdroje = appDataSpravce.NactiZdroje();
    }
    public List<Zdroj> ZiskejVsechnyZdroje()
    {
        return zdroje;
    }
    public string PridejZdroj(Zdroj novyZdroj)
    {
        if (novyZdroj.Nazev == "")
        {
            return "Název zdroje nesmí být prázdný.";
        }
        if (novyZdroj.Typ == "")
        {
            return "Typ zdroje nesmí být prázdný.";
        }
        bool zdrojExistuje = zdroje.Any(z => z.Nazev == novyZdroj.Nazev);
        if (zdrojExistuje)
        {
            return "Tento zdroj už existuje.";
        }
        novyZdroj.Id = ZiskejNoveId();
        zdroje.Add(novyZdroj);
        appDataSpravce.UlozZdroje(zdroje);
        return "";
    }
    public void SmazZdroj(int id)
    {
        Zdroj? nalezenyZdroj = zdroje.FirstOrDefault(z => z.Id == id);
        if (nalezenyZdroj != null)
        {
            zdroje.Remove(nalezenyZdroj);
            appDataSpravce.UlozZdroje(zdroje);
        }
    }
    public List<Zdroj> FiltrujPodleTypu(string typ)
    {
        return zdroje
            .Where(z => z.Typ == typ)
            .ToList();
    }
    private int ZiskejNoveId()
    {
        if (zdroje.Count == 0)
        {
            return 1;
        }
        return zdroje.Max(z => z.Id) + 1;
    }
}