using testServer.Models;

namespace testServer.DTO;

public class TreningDTO
{
    public virtual ICollection<CwiczenieBlokDTO> Cwiczenia { get; set; } = new List<CwiczenieBlokDTO>();
}