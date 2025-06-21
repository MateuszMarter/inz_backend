using testServer.Context;

namespace testServer.Services;

public interface ITestService
{
    public Task<bool> TestConnection();
}

public class TestService : ITestService
{
    private readonly MainDbContext _db;
    
    public TestService(MainDbContext db)
    {
        _db = db;
    }

    public async Task<bool> TestConnection()
    {
        var canConnect = await _db.Database.CanConnectAsync();
        
        return canConnect;
    }
}