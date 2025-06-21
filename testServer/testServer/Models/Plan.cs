using System;
using System.Collections.Generic;

namespace testServer.Models;

public partial class Plan
{
    public int Id { get; set; }

    public string Nazwa { get; set; } = null!;

    public int UzytkownikId { get; set; }

    public virtual ICollection<Trening> Trenings { get; set; } = new List<Trening>();

    public virtual Uzytkownik Uzytkownik { get; set; } = null!;
}
