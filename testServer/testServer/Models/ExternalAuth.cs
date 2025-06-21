using System;
using System.Collections.Generic;

namespace testServer.Models;

public partial class ExternalAuth
{
    public int Id { get; set; }

    public int UzytkownikId { get; set; }

    public string Dostawca { get; set; } = null!;

    public string IdOdDostawcy { get; set; } = null!;

    public virtual Uzytkownik Uzytkownik { get; set; } = null!;
}
