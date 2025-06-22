using testServer.Models;

namespace testServer.DTO;

public class CwiczenieBlokDTO
{
    public List<ExerciseWger> Cwiczenia { get; set; } = new();
    public List<SetDTO> Sets { get; set; } = new()!;
}