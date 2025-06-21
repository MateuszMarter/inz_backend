using System;
using System.Collections.Generic;

namespace testServer.Models;

public partial class Set
{
    public int Id { get; set; }

    public int IloscPowtorzen { get; set; }

    public float? Ciezar { get; set; }

    public virtual ICollection<CwiczenieBlok> CwiczenieBloks { get; set; } = new List<CwiczenieBlok>();
}
