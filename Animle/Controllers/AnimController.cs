using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Animle.interfaces;
using Animle.services.Token;
using Animle.Interfaces;
using Animle.Services;

namespace Animle.Controllers
{
    [Route("anime")]
    [ApiController]
    public class AnimController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IRequestCacheManager _cacheManager;
        private readonly IAnimeService _animeService;

        public AnimController(TokenService tokenService, IRequestCacheManager cacheManager, IAnimeService animeService)
        {
            _tokenService = tokenService;
            _cacheManager = cacheManager;
            _animeService = animeService;
        }

        [HttpGet]
        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [EnableRateLimiting("fixed")]
        [Route("daily")]
        public async Task<IActionResult> Daily()
        {
            if (HttpContext.Items["user"] is User user)
            {
                var result = await _animeService.GetDailyChallengeAsync(user);
                return result != null ? Ok(result) : BadRequest(new { Response = "You have already played this game." });
            }
            return Unauthorized();
        }

        [HttpPost]
        [EnableRateLimiting("fixed")]
        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [Route("contest/{type}")]
        public async Task<IActionResult> DailyResult(string type, DailyGameResult gameResult)
        {
            if (HttpContext.Items["user"] is User user)
            {
                var result = await _animeService.SubmitGameResultAsync(type, user, gameResult);
                return result.IsSuccess ? Ok(result) : BadRequest(result.Response);
            }
            return NotFound(new { Response = "User not found" });
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        [Route("filter")]
        public async Task<IActionResult> SearchAnime([FromQuery] string q)
        {
            var filteredList = await _animeService.FilterAnimeAsync(q);
            return Ok(filteredList);
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        [Route("Random")]
        public IActionResult Random()
        {
            var randomAnimes = _animeService.GetRandomAnimes();
            return Ok(randomAnimes);
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [Route("emoji-quiz")]
        public IActionResult EmojiQuiz()
        {
            if (HttpContext.Items["user"] is User user)
            {
                var quiz = _animeService.GetEmojiQuiz(user);
                return quiz != null ? Ok(quiz) : BadRequest(new { Response = "You have already played this game." });
            }
            return Unauthorized();
        }
    }
}
