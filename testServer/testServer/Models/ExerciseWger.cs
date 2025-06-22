using System.Text.Json.Serialization;

namespace testServer.Models;

public class ExerciseWger
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("muscles")]
    public List<int> Muscles { get; set; }

    [JsonPropertyName("secondary_muscles")]
    public List<int> SecondaryMuscles { get; set; }
    
    [JsonPropertyName("language")]
    public int Language { get; set; }
    
    [JsonPropertyName("category")]
    public int Category { get; set; }
}