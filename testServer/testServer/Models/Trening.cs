using System;
using System.Collections.Generic;

namespace testServer.Models;

public partial class Trening
{
    public int Id { get; set; }

    public int PlanId { get; set; }

    public virtual ICollection<CwiczenieBlok> CwiczenieBloks { get; set; } = new List<CwiczenieBlok>();

    public virtual Plan Plan { get; set; } = null!;
}
