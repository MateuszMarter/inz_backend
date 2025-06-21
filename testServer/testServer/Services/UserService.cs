using Microsoft.EntityFrameworkCore;
using testServer.Context;
using testServer.Models;

namespace testServer.Services;

public interface IUserService
{
    public Task<Uzytkownik?> GetUserByIdAsync(int id);
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
}