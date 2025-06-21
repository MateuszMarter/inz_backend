using System;
using System.Collections.Generic;

namespace testServer.Models;

public partial class CwiczenieBlok
{
    public int Id { get; set; }

    public int? CwiczenieUzytkownikaId { get; set; }

    public int? CwiczenieWger { get; set; }

    public int SetId { get; set; }

    public int TreningId { get; set; }

    public virtual CwiczenieUzytkownika? CwiczenieUzytkownika { get; set; }

    public virtual Set Set { get; set; } = null!;

    public virtual Trening Trening { get; set; } = null!;
}
