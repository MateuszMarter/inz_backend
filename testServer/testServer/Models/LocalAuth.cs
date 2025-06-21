using System;
using System.Collections.Generic;

namespace testServer.Models;

public partial class LocalAuth
{
    public int UzytkownikId { get; set; }

    public string? PasswordHash { get; set; }

    public string PasswordSalt { get; set; } = null!;

    public virtual Uzytkownik Uzytkownik { get; set; } = null!;
}
