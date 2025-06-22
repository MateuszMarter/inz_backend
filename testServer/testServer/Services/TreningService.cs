using System.Text.Json;
using testServer.Context;
using testServer.DTO;
using testServer.Models;

namespace testServer.Services;

public interface ITreningService
{
    public Task<List<TreningDTO>> GetTreningAsync(int aproxTime, int days);
    public Task<List<ExerciseWger>> GetTenRandomExercisesAsync();
}

public class TreningService : ITreningService
{
    private readonly MainDbContext _db;
    
    public TreningService(MainDbContext dbContext)
    {
        _db = dbContext;
    }
    
     public async Task<List<ExerciseWger>> GetTenRandomExercisesAsync()
{
    string url = "https://wger.de/api/v2/exerciseinfo/?limit=100";
    using HttpClient client = new HttpClient();
    var json = await client.GetStringAsync(url);
    var doc = JsonDocument.Parse(json);

    if (!doc.RootElement.TryGetProperty("results", out var resultsElement))
    {
        return new List<ExerciseWger> {
            new ExerciseWger { Name = "No 'results' key found" }
        };
    }

    var exercises = new List<ExerciseWger>();

    foreach (var element in resultsElement.EnumerateArray())
    {
        if (!element.TryGetProperty("translations", out var translationsArray)) continue;

        string name = null;
        string description = "";

        foreach (var trans in translationsArray.EnumerateArray())
        {
            if (trans.TryGetProperty("language", out var langProp) && langProp.GetInt32() == 2) // English
            {
                name = trans.GetProperty("name").GetString();
                description = trans.TryGetProperty("description", out var descEl)
                    ? descEl.GetString() ?? ""
                    : "";
                break;
            }
        }

        if (string.IsNullOrWhiteSpace(name))
            continue;
        
        var muscles = element.TryGetProperty("muscles", out var mEl)
            ? mEl.EnumerateArray().Select(m => m.GetProperty("id").GetInt32()).ToList()
            : new List<int>();

        var secondaryMuscles = element.TryGetProperty("muscles_secondary", out var sEl)
            ? sEl.EnumerateArray().Select(m => m.GetProperty("id").GetInt32()).ToList()
            : new List<int>();

        int id = element.TryGetProperty("id", out var idEl) ? idEl.GetInt32() : 0;
        int category = element.TryGetProperty("category", out var catEl) &&
                       catEl.TryGetProperty("id", out var catIdEl)
            ? catIdEl.GetInt32()
            : 0;

        exercises.Add(new ExerciseWger
        {
            Id = id,
            Name = name,
            Description = description,
            Muscles = muscles,
            SecondaryMuscles = secondaryMuscles,
            Category = category,
            Language = 2
        });

        if (exercises.Count >= 10)
            break;
    }

    return exercises.Count > 0
        ? exercises
        : new List<ExerciseWger> { new ExerciseWger { Name = "No valid English translations found." } };
}
     
    public async Task<List<TreningDTO>> GetTreningAsync(int aproxTime, int days)
    {
        int exercisesNum = (int)Math.Ceiling(aproxTime / 15.0);

        var exercises = await GetExercisesAsync();
        
        var trainingsJson =  await CreateTrening(exercises, exercisesNum, days);
        
        return ConvertJsonToTrainings(trainingsJson);
    }
    
    private List<TreningDTO> ConvertJsonToTrainings(List<List<ExerciseWger>> trainingsJson)
    {
        var treningiDto = new List<TreningDTO>();

        foreach (var dzienTreningowy in trainingsJson)
        {
            var treningDto = new TreningDTO
            {
                Cwiczenia = new List<CwiczenieBlokDTO>()
            };

            foreach (var cwiczenie in dzienTreningowy)
            {
                var sety = new List<SetDTO>();

                for (int i = 0; i < 4; i++)
                {
                    sety.Add(new SetDTO
                    {
                        IloscPowtorzen = 10,
                        Ciezar = null
                    });
                }

                var blokDto = new CwiczenieBlokDTO
                {
                    Cwiczenia = new List<ExerciseWger> { cwiczenie },
                    Sets = sety
                };

                treningDto.Cwiczenia.Add(blokDto);
            }

            treningiDto.Add(treningDto);
        }

        return treningiDto;
    }
    
    private async Task<List<ExerciseWger>> GetExercisesAsync()
{
    string url = "https://wger.de/api/v2/exerciseinfo/?limit=300";
    using HttpClient client = new HttpClient();
    var json = await client.GetStringAsync(url);
    var doc = JsonDocument.Parse(json);

    if (!doc.RootElement.TryGetProperty("results", out var resultsElement))
        return new List<ExerciseWger>();

    var exercises = new List<ExerciseWger>();

    foreach (var element in resultsElement.EnumerateArray())
    {
        // Get English translation
        if (!element.TryGetProperty("translations", out var translations)) continue;

        string name = null;
        string description = "";

        foreach (var translation in translations.EnumerateArray())
        {
            if (translation.TryGetProperty("language", out var langEl) && langEl.GetInt32() == 2)
            {
                name = translation.GetProperty("name").GetString();
                description = translation.TryGetProperty("description", out var descEl)
                    ? descEl.GetString() ?? ""
                    : "";
                break;
            }
        }

        if (string.IsNullOrWhiteSpace(name))
            continue;

        var muscles = element.TryGetProperty("muscles", out var mEl)
            ? mEl.EnumerateArray().Select(m => m.GetProperty("id").GetInt32()).ToList()
            : new List<int>();

        var secondaryMuscles = element.TryGetProperty("muscles_secondary", out var sEl)
            ? sEl.EnumerateArray().Select(m => m.GetProperty("id").GetInt32()).ToList()
            : new List<int>();

        int id = element.TryGetProperty("id", out var idEl) ? idEl.GetInt32() : 0;
        int category = element.TryGetProperty("category", out var catEl) &&
                       catEl.TryGetProperty("id", out var catIdEl)
            ? catIdEl.GetInt32()
            : 0;

        if (muscles.Count == 0 && secondaryMuscles.Count == 0)
            continue;

        exercises.Add(new ExerciseWger
        {
            Id = id,
            Name = name,
            Description = description,
            Muscles = muscles,
            SecondaryMuscles = secondaryMuscles,
            Category = category,
            Language = 2
        });
    }

    return exercises;
}
    private Task<List<List<ExerciseWger>>> CreateTrening(List<ExerciseWger> allExercises, int exercisesNum, int days)
    {
        var rng = new Random();
        var trainings = new List<List<ExerciseWger>>();
        
        int targetUniqueMuscles = Math.Max(1, 7 - days);

        for (int i = 0; i < days; i++)
        {
            var training = new List<ExerciseWger>();    
            var usedMuscles = new HashSet<int>();

            var shuffled = allExercises.OrderBy(x => rng.Next()).ToList();

            foreach (var ex in shuffled)
            {
                var primaryMuscle = ex.Muscles.FirstOrDefault();
                if (training.Count >= exercisesNum) break;
                
                if (usedMuscles.Count < targetUniqueMuscles && usedMuscles.Contains(primaryMuscle))
                    continue;

                training.Add(ex);
                usedMuscles.Add(primaryMuscle);
            }

            trainings.Add(training);
        }

        return Task.FromResult(trainings);
    }
}