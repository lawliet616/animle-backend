using Animle.Interfaces;
using Animle.Models;
using Animle.interfaces;
using Microsoft.EntityFrameworkCore;
using Animle.Helpers;

namespace Animle.Services
{
    public interface IAnimeService
    {
        Task<DailyChallenge> GetDailyChallengeAsync(User user);
        Task<SimpleResponse> SubmitGameResultAsync(string type, User user, DailyGameResult gameResult);
        Task<List<AnimeFilter>> FilterAnimeAsync(string query);
        List<AnimeFilter> GetRandomAnimes();
        dynamic GetEmojiQuiz(User user);
    }
    public class AnimeService : IAnimeService
    {
        private readonly AnimleDbContext _dbContext;
        private readonly IRequestCacheManager _cacheManager;

        public AnimeService(AnimleDbContext dbContext, IRequestCacheManager cacheManager)
        {
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        public async Task<DailyChallenge> GetDailyChallengeAsync(User user)
        {
            var dailyChallenge = _cacheManager.GetCachedItem<DailyChallenge>("daily");
         

            if (_dbContext.GameContests.Any(g => g.UserId == user.Id && g.DailyChallengeId == dailyChallenge.Id))
            {
                return null;
            }

            return dailyChallenge;
        }

        public async Task<SimpleResponse> SubmitGameResultAsync(string type, User user, DailyGameResult gameResult)
        {
            if (type == "daily")
            {
                var dailyChallenge = _cacheManager.GetCachedItem<DailyChallenge>("daily");
           

                if (_dbContext.GameContests.Include(u=> u.User).Any(g => g.UserId == user.Id && g.DailyChallengeId == dailyChallenge.Id))
                {
                    return new SimpleResponse { IsSuccess = false, Response = "You have already played this game." };
                }
                var gameContest = new GameContest
                {
                    User = user,
                    UserId = user.Id,
                    TimePlayed = DateTime.Now,
                    Result = gameResult.Result,
                    Challenge = dailyChallenge,
                    DailyChallengeId = dailyChallenge.Id
                };
                _dbContext.ChangeTracker.Clear();
                _dbContext.GameContests.Attach(gameContest);
                await _dbContext.SaveChangesAsync();

                return new SimpleResponse { IsSuccess = true, Response = "Game saved" };
            }

            return new SimpleResponse { IsSuccess = false, Response = "Invalid game type" };
        }

        public async Task<List<AnimeFilter>> FilterAnimeAsync(string query)
        {
            return _dbContext.AnimeWithEmoji
                .Where(x => x.JapaneseTitle.ToLower().Contains(query) || x.Title.ToLower().Contains(query))
                .OrderBy(x => x.JapaneseTitle.Length)
                .Select(x => new AnimeFilter
                {
                    Id = x.Id,
                    Title = x.Title,
                    Thumbnail = x.Thumbnail,
                    MyanimeListId = x.MyanimeListId,
                    JapaneseTitle = x.JapaneseTitle,
                })
                .Take(4)
                .ToList();
        }

        public List<AnimeFilter> GetRandomAnimes()
        {
            var rnd = new Random();
            var anim = _dbContext.AnimeWithEmoji.ToList()
                .OrderBy(item => rnd.Next())
                .Take(10)
                .Select(x => new AnimeFilter
                {
                    Id = x.Id,
                    Title = x.Title,
                    Thumbnail = x.Thumbnail,
                    Description = x.Description,
                    Image = x.Image,
                    properties = x.properties,
                    EmojiDescription = x.EmojiDescription,
                    MyanimeListId = x.MyanimeListId,
                })
                .ToList();

            anim.ForEach(a =>
            {
                int random = rnd.Next(0, anim.Count - 1);
                int gameType = rnd.Next(0, 4);
                a.Type = UtilityService.GetTypeByNumber(gameType);
            });

            return anim;
        }

        public dynamic GetEmojiQuiz(User user)
        {
            var guessGame = _cacheManager.GetCachedItem<GuessGame>("dailyGuess");
            var us = _dbContext.Users.Include(x => x.UserGuessGames).FirstOrDefault(u => u.Id == user.Id);

            if (us?.UserGuessGames.Any(u => u.GuessGameId == guessGame.Id) == true)
            {
                return null;
            }

            var resObject = new { Id = guessGame.Id, AnimeId = guessGame.Anime.Id, guessGame.Anime.EmojiDescription };


            return resObject;
        }
    }
}
