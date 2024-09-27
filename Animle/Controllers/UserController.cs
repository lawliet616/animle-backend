using Animle.interfaces;
using Animle.services.Token;
using Animle;
using Microsoft.AspNetCore.Mvc;
using Animle.Models;

[Route("user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;
    private readonly TokenService _tokenService;


    public UserController(IUserService userService, ILogger<UserController> logger, TokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var createdUser = await _userService.CreateUserAsync(user);
            return Ok(new SimpleResponse { Response = "User Created" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating user.");
            // Handle exception
            return StatusCode(500, new SimpleResponse { Response = "An error occurred while saving data" });
        }
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginInfos loginInfos)
    {
        var user = await _userService.AuthenticateUserAsync(loginInfos);
        if (user != null)
        {
            var tokenResponse = new TokenResponse { Token = _tokenService.CreateToken(user) };
            return Ok(tokenResponse);
        }

        return Unauthorized(new SimpleResponse { Response = "Username or Password is incorrect!" });
    }


    [Route("is-signed-in")]
    [ServiceFilter(typeof(CustomAuthorizationFilter))]
    [HttpGet]
    public IActionResult IsSignedIn()
    {

        if (HttpContext.Items["user"] is User user)
        {

            return Ok(new { Response = user.Name });

        }
        else
        {
            return Unauthorized();
        }



    }


    [Route("leaderboard/{type}")]
    [HttpGet]
    public async Task<IActionResult> LeaderBoard(string type)
    {
        List<GameContest> games = await _userService.GetDailyLeaderBoard(type);   
        return Ok(games);
    }

}
