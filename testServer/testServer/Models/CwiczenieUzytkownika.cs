using System;
using System.Collections.Generic;

namespace testServer.Models;

public partial class CwiczenieUzytkownika
{
    public int Id { get; set; }

    public string Nazwa { get; set; } = null!;

    public string? Opis { get; set; }

    public int UzytkownikId { get; set; }

    public virtual ICollection<CwiczenieBlok> CwiczenieBloks { get; set; } = new List<CwiczenieBlok>();

    public virtual Uzytkownik Uzytkownik { get; set; } = null!;
}
