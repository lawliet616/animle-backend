using Microsoft.AspNetCore.Mvc;
using Animle;
using Animle.interfaces;
using Microsoft.AspNetCore.RateLimiting;
using Animle.services.Token;
using Microsoft.EntityFrameworkCore;
using Animle.Controllers;
using Azure;
using System.Security.Claims;

namespace StoryTeller.Controllers
{
    [EnableRateLimiting("fixed")]
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;

        private readonly TokenService _tokenService;
        private readonly AnimleDbContext _animleDbContext;


        public UserController(ILogger<UserController> logger, TokenService tokenService, AnimleDbContext animleDbContext)
        {
            _tokenService = tokenService;
            _logger = logger;
            _animleDbContext = animleDbContext;
        }

        [HttpPost]
        public IActionResult CreateUser(User user)
        {

            SimpleResponse simpleResponse = new SimpleResponse();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                PasswordManager passwordManager = new PasswordManager();
                user.Password = passwordManager.HashPassword(user.Password);
                user.Rating = 1000;
                _animleDbContext.Users.Add(user);
                _animleDbContext.SaveChanges();
                simpleResponse.Response = "User Created";
                return Ok(simpleResponse);

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("Name") || ex.Message.Contains("Email"))
                {
                    simpleResponse.Response = "Username or Password is taken";
                    return Conflict(simpleResponse);
                }

                return StatusCode(500, "Internal server error");
            }



        }


        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginInfos loginInfos)
        {
            SimpleResponse response = new SimpleResponse();
            var user = _animleDbContext.Users.FirstOrDefault(u => u.Name == loginInfos.Name);
            if (user != null)
            {
                PasswordManager passwordManager = new PasswordManager();


                bool isAuthenticated = passwordManager.VerifyPassword(loginInfos.Password, user.Password);

                if (isAuthenticated)
                {
                    TokenResponse tokenResponse = new();
                    tokenResponse.Token = _tokenService.CreateToken(user);

                    return Ok(tokenResponse);
                }
                else
                {
                    response.Response = "Username or Password is incorrect!";
                    return Unauthorized(response);
                }
            }
            else
            {
                response.Response = "User not found!";
                return NotFound(response);
            }

        }
        [Route("is-signed-in")]
        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [HttpGet]
        public IActionResult IsSignedIn()
        {

            if (HttpContext.Items["user"] is User user)
            {

        
                return Ok(new {Response = user.Name});

            }
            else
            {
                return Unauthorized();
            }

              

        }

        [Route("leaderboard/{type}")]
        [HttpGet]
        public IActionResult leaderBoard(string type)
        {

           var result = _animleDbContext.GameContests.Where((g)=> g.Type == type ).Include(x => x.User).Select(x => new 
           {
               Id = x.Id,
               UserId = x.User.Id,
               Name = x.User.Name,
               Points = x.Points

           }).OrderByDescending(x => x.Id).Take(10).ToList();
           return Ok(result);

        }

    }
}


