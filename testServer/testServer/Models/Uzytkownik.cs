using System;
using System.Collections.Generic;

namespace testServer.Models;

public partial class Uzytkownik
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<CwiczenieUzytkownika> CwiczenieUzytkownikas { get; set; } = new List<CwiczenieUzytkownika>();

    public virtual ICollection<ExternalAuth> ExternalAuths { get; set; } = new List<ExternalAuth>();

    public virtual LocalAuth? LocalAuth { get; set; }

    public virtual ICollection<Plan> Plans { get; set; } = new List<Plan>();
}
