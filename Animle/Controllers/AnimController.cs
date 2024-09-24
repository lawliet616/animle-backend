using Microsoft.AspNetCore.Mvc;
using Animle.services;
using Animle.Models;
using Microsoft.AspNetCore.RateLimiting;
using System.Text;
using Animle.interfaces;
using NHibernate.Util;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Animle.services.Cache;
using Animle.services.Token;
using Microsoft.AspNetCore.Authorization;

namespace Animle.Controllers
{

    [Route("anime")]
    [ApiController]
    public class AnimController : ControllerBase
    {


        private readonly TokenService _tokenService;

        private readonly AnimleDbContext _animleConect;

        private readonly RequestCacheManager _cacheManager;


        public AnimController( TokenService tokenService, RequestCacheManager cacheManager, AnimleDbContext animleDbContext)
        {
            _tokenService = tokenService;
            _cacheManager = cacheManager;
            _animleConect = animleDbContext;
        }

        [HttpGet]
        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [EnableRateLimiting("fixed")]
        [Route("daily")]
        public async Task<IActionResult> Daily()
        {
            SimpleResponse simpleResponse = new();
            if (HttpContext.Items["user"] is User user)
            {
                ContestGame dailyAnimes = _cacheManager.GetCachedItem<ContestGame>("daily");
                if (dailyAnimes == null)
                {
                    Random rnd = new Random();
                    dailyAnimes = new ContestGame();
                    dailyAnimes.Anime = new List<AnimeWithEmoji>(_animleConect.AnimeWithEmoji.OrderBy((item) => rnd.Next()).Take(15));
                    dailyAnimes.Anime.ForEach((a) =>
                    {
                        int gameType = rnd.Next(0, 3);
                        a.Type = UtilityService.GetTypeByNumber(gameType);
                    });
                    _cacheManager.SetCacheItem("daily", dailyAnimes, TimeSpan.FromDays(1));
                }
                else
                {

                    if (user.GameContests.Any(u => u.gameGuid == dailyAnimes.Id))
                    {
                        simpleResponse.Response = "You have already played this game";

                        return BadRequest(simpleResponse);
                    }
                }

                var data = UtilityService.Serialize(dailyAnimes);
                var bytes = Encoding.UTF8.GetBytes(data);
                return Ok(Convert.ToBase64String(bytes));
            }
            return Unauthorized(Response);

        }

        [HttpPost]
        [EnableRateLimiting("fixed")]
        [Route("contest")]
        public async Task<IActionResult> DailyResult([FromBody] DailyGameResult gameResult )
        {
            SimpleResponse simpleResponse = new();
            if (HttpContext.Items["user"] is User user)
            {
            ContestGame dailyAnimes = _cacheManager.GetCachedItem<ContestGame>("daily");

            if (!user.GameContests.Any(u => u.gameGuid == dailyAnimes.Id))
            {
                GameContest game = new GameContest();
                game.gameGuid = new Guid(gameResult.GameId);
                game.Points = gameResult.Result;
                game.Type = gameResult.Type;
                game.TimePlayed = DateTime.Now;
                game.User = user;
                user.GameContests.Add(game);
                await _animleConect.GameContests.AddAsync(game);
                await _animleConect.SaveChangesAsync();

                simpleResponse.Response = "Game saved";

                return Ok(simpleResponse);
            }
           
            simpleResponse.Response = "You have Already Played this game!";
            return BadRequest(simpleResponse.Response);
            }
            else
            {
                simpleResponse.Response = "User not found";
                return NotFound(Response);
            }


        }


        [HttpGet]
        [EnableRateLimiting("fixed")]
        [Route("filter")]
        public async Task<IActionResult> SearchAnime([FromQuery] string q)
        {
            List<AnimeFilter> filteredList = _animleConect.AnimeWithEmoji.Where(x => x.JapaneseTitle.ToLower().Contains(q) || x.Title.ToLower().Contains(q))
                .OrderBy(x => x.JapaneseTitle.Length)

               .Select(x => new AnimeFilter
               {

                   Id = x.Id,
                   Title = x.Title,
                   Thumbnail = x.Thumbnail,
                   MyanimeListId = x.MyanimeListId,
                   JapaneseTitle = x.JapaneseTitle,
               })
              .Take(4).ToList();
            return Ok(filteredList);
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        [Route("Random")]
        public IActionResult Random()
        {
            Random rnd = new Random();
            List<AnimeFilter> anim = _animleConect.AnimeWithEmoji.ToList().OrderBy(item => rnd.Next()).Take(10).Select(x => new AnimeFilter
            {
                Id = x.Id,
                Title = x.Title,
                Thumbnail = x.Thumbnail,
                Description = x.Description,
                Image = x.Image,
                properties = x.properties,
                EmojiDescription = x.EmojiDescription,
                MyanimeListId = x.MyanimeListId,
            }).ToList();
            anim.ForEach((a) =>
           {
               int random = rnd.Next(0, anim.Count - 1);
               int gameType = rnd.Next(0, 4);
               a.Type = UtilityService.GetTypeByNumber(gameType);
           });

            return Ok(anim);
        }

        [HttpGet]
        [EnableRateLimiting("fixed")]
        [Route("emoji-quiz")]
        public IActionResult EmojiQuiz()
        {
            Random rnd = new Random();
            List<AnimeFilter> anim = _animleConect.AnimeWithEmoji.ToList().OrderBy(item => rnd.Next()).Take(10).Select(x => new AnimeFilter
            {
                Id = x.Id,
                Title = x.Title,
                Thumbnail = x.Thumbnail,
                Description = x.Description,
                Image = x.Image,
                properties = x.properties,
                EmojiDescription = x.EmojiDescription,
                MyanimeListId = x.MyanimeListId,
            }).ToList();
            anim.ForEach((a) =>
            {
                int random = rnd.Next(0, anim.Count - 1);
                int gameType = rnd.Next(0, 4);
                a.Type = UtilityService.GetTypeByNumber(gameType);
            });

            return Ok(anim);
        }
    }
}


