using Quartz;
using Animle.Models;
using Animle.services.Cache;
using Animle.interfaces;
using Animle.SignalR;
using Animle.services;
using System.Text.Json;
using Newtonsoft.Json;
using System.Security.Claims;
using Animle.classes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Animle.services.Quartz
{

    public class MonthlyJob : IJob
    {
        private RequestCacheManager cacheManager;

        private AnimleDbContext _animle;


        private SignalrAnimeService _signalrAnimeService;

        public MonthlyJob(RequestCacheManager requestCacheManager, AnimleDbContext animle, SignalrAnimeService signalrAnimeService)
        {
            cacheManager = requestCacheManager;
            _animle = animle;
            _signalrAnimeService = signalrAnimeService;
        }


        public async Task Execute(IJobExecutionContext context)
        {


            Random rnd = new Random();


                List<AnimeWithEmoji> animes = _animle.AnimeWithEmoji.ToList();

                cacheManager.SetCacheItem("monthly", animes, TimeSpan.FromDays(31));

                _signalrAnimeService.SetList(animes);

                List<AnimeWithEmoji> weeklyAnime = new List<AnimeWithEmoji>(animes.OrderBy((item) => rnd.Next()).Take(25));

                ContestGame weeklyGame = new();
                weeklyGame.Type = "weekly";
                weeklyGame.Anime = weeklyAnime;
                weeklyGame.Anime.ForEach((a) =>

                {
                    int gameType = rnd.Next(0, 3);
                    AnimeWithEmoji animeWithEmoji = a;
                    a.Type = UtilityService.GetTypeByNumber(gameType);
                });

                cacheManager.SetCacheItem("weekly", weeklyGame, TimeSpan.FromDays(7));


                List<AnimeWithEmoji> dailyAnime = new List<AnimeWithEmoji>(animes.OrderBy((item) => rnd.Next()).Take(15));

                ContestGame dailyGame = new();
                dailyGame.Type = "daily";
                dailyGame.Anime = dailyAnime;
                dailyGame.Anime.ForEach((a) =>

                {
                    int gameType = rnd.Next(0, 3);
                    AnimeWithEmoji animeWithEmoji = a;
                    a.Type = UtilityService.GetTypeByNumber(gameType);
                });

                cacheManager.SetCacheItem("daily", dailyGame, TimeSpan.FromDays(1));



            }


        }

    } 


    public class DailyJob : IJob
    {

        private RequestCacheManager cacheManager;

        private AnimleDbContext _animle;


        public DailyJob(RequestCacheManager requestCacheManager, AnimleDbContext animle)
        {
            cacheManager = requestCacheManager;
            _animle = animle;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            Random rnd = new Random();

            var cachedAnime = cacheManager.GetCachedItem<List<AnimeWithEmoji>>("monthly");
            List<AnimeWithEmoji> dailyAnimes = new List<AnimeWithEmoji>();
            ContestGame dailyGame = new();
            dailyGame.Type = "daily";

            if (cachedAnime != null)
            {
                dailyAnimes = new List<AnimeWithEmoji>(cachedAnime.OrderBy((item) => rnd.Next()).Take(15));
                dailyAnimes.ForEach((a) =>

                {
                    int gameType = rnd.Next(0, 3);
                    a.Type = UtilityService.GetTypeByNumber(gameType);

                });
            }
            else
            {
                dailyAnimes = new List<AnimeWithEmoji>(_animle.AnimeWithEmoji.OrderBy((item) => rnd.Next()).Take(15));
                dailyAnimes.ForEach((a) =>

                {
                    int gameType = rnd.Next(0, 3);

                    a.Type = UtilityService.GetTypeByNumber(gameType);
                });
            }
            cacheManager.SetCacheItem("daily", dailyGame, TimeSpan.FromDays(1));

        }
    }

    public class WeeklyJob : IJob
    {

        private RequestCacheManager cacheManager;
        private AnimleDbContext _animle;


        public WeeklyJob(RequestCacheManager requestCacheManager, AnimleDbContext anime)
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




