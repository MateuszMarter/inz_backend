using Microsoft.AspNetCore.Mvc;
using testServer.Services;

namespace testServer.Controllers;

[Route("[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ITestService _testService;

    public TestController(ITestService testService)
    {
        _testService = testService;
    }

    [HttpGet("connection")]
    public async Task<IActionResult> GetConnection()
    {
        var canConnect = await _testService.TestConnection();

        if (canConnect)
        {
            return Ok();
        }
        
        return BadRequest("a"); 
    }
}