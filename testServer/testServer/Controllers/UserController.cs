using Microsoft.AspNetCore.Mvc;
using testServer.DTO;
using testServer.Services;

namespace testServer.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITreningService _treningService;

    public UserController(IUserService userService, ITreningService treningService)
    {
        _userService = userService;
        _treningService = treningService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound("User not found");
        }
        
        return Ok(user);
    }

    [HttpGet("GenTraining")]
    public async Task<IActionResult> GetUserGenTraining(int aproxTime, int days)
    {
        
        if (days < 1 || days > 7)
        {
            return BadRequest("Days must be between 1 and 7");
        }

        if (aproxTime < 30 || aproxTime > 120)
        {
            return BadRequest("Aprox time must be between 30 and 120");
        }

        return Ok(await _treningService.GetTreningAsync(aproxTime, days));

    }

    [HttpPost("AddPlan/{userId}")]
    public async Task<IActionResult> AddPlan([FromBody] List<TreningDTO> trenings, int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);

        if (user == null)
        {
            return NotFound("User not found");
        }
        
        var message = await _userService.AddNewPlan(trenings, userId);

        if (message.ToLower() == "ok")
        {
            return Ok("Plan successfully added");
        }
        
        return BadRequest(message);
    }
}