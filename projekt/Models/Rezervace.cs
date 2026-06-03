using System;

namespace projekt.Models;



public class Rezervace
{
    public int Id { get; set; }

    public string JmenoUzivatele { get; set; } = "";

    public string NazevZdroje { get; set; } = "";

    public string TypZdroje { get; set; } = "";

    public DateTime Datum { get; set; }

    public TimeSpan CasOd { get; set; }

    public TimeSpan CasDo { get; set; }

    public string Poznamka { get; set; } = "";
}