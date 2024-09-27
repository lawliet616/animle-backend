using Quartz;
using Animle.Models;
using Animle.services;
using Animle.SignalR;
using Microsoft.EntityFrameworkCore;
using Animle.Helpers;
using Animle.Interfaces;
using Animle.interfaces;

namespace Animle.services.Quartz
{

    public class MonthlyJob : IJob
    {
        private IRequestCacheManager cacheManager;

        private AnimleDbContext _animle;


        private SignalrAnimeService _signalrAnimeService;

        public MonthlyJob(IRequestCacheManager requestCacheManager, AnimleDbContext animle, SignalrAnimeService signalrAnimeService)
        {
            cacheManager = requestCacheManager;
            _animle = animle;
            _signalrAnimeService = signalrAnimeService;
        }


        public async Task Execute(IJobExecutionContext context)
        {


            Random rnd = new Random();


            List<AnimeWithEmoji> animes = _animle.AnimeWithEmoji.Take(453).ToList();

            cacheManager.SetCacheItem("monthly", animes, TimeSpan.FromDays(31));

            _signalrAnimeService.SetList(animes);

            List<AnimeWithEmoji> weeklyAnime = new List<AnimeWithEmoji>(animes.OrderBy((item) => rnd.Next()).Take(25));


            List<AnimeWithEmoji> dailyAnime = new List<AnimeWithEmoji>(animes.OrderBy((item) => rnd.Next()).Take(15));
            var todaysQuiz = _animle.DailyChallenges.Include(x => x.Animes).FirstOrDefault(d => d.TimeCreated == DateTime.Now.Date);
            if (todaysQuiz == null) {
                DailyChallenge dailyGame = new();
                dailyGame.Type = "daily";
                dailyGame.Animes = dailyAnime;
                dailyGame.TimeCreated = DateTime.Now.Date;
                try
                {
                    _animle.DailyChallenges.Add(dailyGame);
                     await _animle.SaveChangesAsync();
                    todaysQuiz = dailyGame;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

            for (int i = 0; i < todaysQuiz.Animes.Count; i++)
            {
                int gameType = rnd.Next(0, 4);
                todaysQuiz.Animes.ElementAt(i).Type = UtilityService.GetTypeByNumber(gameType);
            }

            cacheManager.SetCacheItem("daily", todaysQuiz, TimeSpan.FromDays(1));


            var todaysGuess = _animle.GuessGames.FirstOrDefault(d => d.TimeCreated == DateTime.Now.Date);

            if (todaysGuess == null)
            {
                try
                {
                    GuessGame guessGame = new();
                    List<AnimeWithEmoji> filteredAnimes = animes
                        .Where(anime => !dailyAnime.Contains(anime))
                        .OrderBy(item => rnd.Next())
                        .ToList();
                    Console.WriteLine(filteredAnimes.Count);
                    AnimeWithEmoji animeWithEmoji = filteredAnimes.OrderBy((item) => rnd.Next()).Take(1).ElementAt(0);
                    guessGame.Anime = animeWithEmoji;
                    guessGame.AnimeWithEmojiId = animeWithEmoji.Id;
                    _animle.GuessGames.Add(guessGame);
                    await _animle.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());   
                }
            

            }
            cacheManager.SetCacheItem("dailyGuess", todaysGuess, TimeSpan.FromDays(1));




        }

    }


    public class DailyJob : IJob
    {

        private IRequestCacheManager cacheManager;

        private AnimleDbContext _animle;


        public DailyJob(IRequestCacheManager requestCacheManager, AnimleDbContext animle)
        {
            cacheManager = requestCacheManager;
            _animle = animle;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Random rnd = new Random();

            var cachedAnime = cacheManager.GetCachedItem<List<AnimeWithEmoji>>("monthly");
            List<AnimeWithEmoji> dailyAnimes = new List<AnimeWithEmoji>();
            DailyChallenge dailyGame = new();
            dailyGame.Type = "daily";

            if (cachedAnime != null)
            {
                dailyAnimes = new List<AnimeWithEmoji>(cachedAnime.OrderBy((item) => rnd.Next()).Take(15));
            }
            else
            {
                dailyAnimes = new List<AnimeWithEmoji>(_animle.AnimeWithEmoji.Take(453).OrderBy((item) => rnd.Next()).Take(15));

            }
            Console.WriteLine("miatő");

            var todaysQuiz = _animle.DailyChallenges.FirstOrDefault(d => d.TimeCreated == DateTime.Now.Date);
            if (todaysQuiz == null)
            {
                try
                {
                    dailyGame.Animes = dailyAnimes;
                    _animle.DailyChallenges.Add(dailyGame);
                    await _animle.SaveChangesAsync();
                    todaysQuiz = dailyGame;
                } catch (Exception ex) {

                    Console.WriteLine(ex.Message);
                }

            }
            for (int i = 0; i< todaysQuiz.Animes.Count;i++)
            {
                
                    int gameType = rnd.Next(0, 4);
                    todaysQuiz.Animes.ElementAt(i).Type = UtilityService.GetTypeByNumber(gameType);
            
            }
            Console.WriteLine("miatő");
            cacheManager.SetCacheItem("daily", todaysQuiz, TimeSpan.FromDays(1));

            try
            {
                await _animle.UnathenticatedGames.ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }

            var todaysGuess = _animle.GuessGames.FirstOrDefault(d => d.TimeCreated == DateTime.Now.Date);

            if (todaysGuess == null)
            {
                try
                {
                    GuessGame guessGame = new();
                    List<AnimeWithEmoji> filteredAnimes = cachedAnime
                        .Where(anime => !dailyAnimes.Contains(anime))
                        .OrderBy(item => rnd.Next())
                        .ToList();
                    AnimeWithEmoji animeWithEmoji = filteredAnimes.OrderBy((item) => rnd.Next()).Take(1).ElementAt(0);
                    guessGame.Anime = animeWithEmoji;
                    guessGame.AnimeWithEmojiId = animeWithEmoji.Id;


                    _animle.GuessGames.Add(guessGame);
                    await _animle.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                }


            }
            cacheManager.SetCacheItem("dailyGuess", todaysGuess, TimeSpan.FromDays(1));


        }
    }

    public class WeeklyJob : IJob
    {

        private IRequestCacheManager cacheManager;
        private AnimleDbContext _animle;


        public WeeklyJob(IRequestCacheManager requestCacheManager, AnimleDbContext anime)
        {
            cacheManager = requestCacheManager;
            _animle = anime;
        }
        public async Task Execute(IJobExecutionContext context)
        {

            var cachedAnime = cacheManager.GetCachedItem<List<AnimeWithEmoji>>("monthly");
            Random rnd = new Random();
            List<AnimeWithEmoji> dailyAnimes = new List<AnimeWithEmoji>();
            ContestGame dailyGame = new();
            dailyGame.Type = "weekly";

            if (cachedAnime != null)
            {
                dailyAnimes = new List<AnimeWithEmoji>(cachedAnime.OrderBy((item) => rnd.Next()).Take(25));
                dailyAnimes.ForEach((a) =>

                {
                    int gameType = rnd.Next(0, 3);
                    a.Type = UtilityService.GetTypeByNumber(gameType);
                });
            }
            else
            {
                dailyAnimes = new List<AnimeWithEmoji>(_animle.AnimeWithEmoji.OrderBy((item) => rnd.Next()).Take(25));
                dailyAnimes.ForEach((a) =>

                {
                    int gameType = rnd.Next(0, 3);
                    a.Type = UtilityService.GetTypeByNumber(gameType);
                });
            }
            cacheManager.SetCacheItem("weekly", dailyGame, TimeSpan.FromDays(7));
        }
    }
}




