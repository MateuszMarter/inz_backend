using Microsoft.EntityFrameworkCore;
using testServer.Context;
using testServer.DTO;
using testServer.Models;

namespace testServer.Services;

public interface IUserService
{
    public Task<Uzytkownik?> GetUserByIdAsync(int id);
    public Task<string> AddNewPlan(List<TreningDTO> trenings, int userId);
}

public class UserService : IUserService
{
    private readonly MainDbContext _db;

    public UserService(MainDbContext db)
    {
        _db = db;
    }

    public async Task<Uzytkownik?> GetUserByIdAsync(int id)
    {
        var user = await _db.Uzytkowniks.FirstOrDefaultAsync(u => u.Id == id);
        
        return user;
    } 

    public async Task<string> AddNewPlan(List<TreningDTO> trenings, int userId)
    {
        var user = await _db.Uzytkowniks.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return "User not found";

        var plan = new Plan
        {
            Nazwa = $"Plan_{DateTime.Now:yyyyMMdd_HHmmss}",
            UzytkownikId = userId,
            Trenings = new List<Trening>()
        };

        foreach (var treningDto in trenings)
        {
            var trening = new Trening
            {
                CwiczenieBloks = new List<CwiczenieBlok>()
            };

            foreach (var blokDto in treningDto.Cwiczenia)
            {
                foreach (var setDto in blokDto.Sets)
                {
                    var set = new Set
                    {
                        IloscPowtorzen = setDto.IloscPowtorzen,
                        Ciezar = setDto.Ciezar
                    };
                    _db.Sets.Add(set);
                    await _db.SaveChangesAsync();

                    foreach (var exercise in blokDto.Cwiczenia)
                    {
                        var blok = new CwiczenieBlok
                        {
                            CwiczenieWger = exercise.Id, 
                            SetId = set.Id
                        };

                        trening.CwiczenieBloks.Add(blok);
                    }
                }
            }

            plan.Trenings.Add(trening);
        }

        _db.Plans.Add(plan);
        await _db.SaveChangesAsync();

        return "ok";
    }

}