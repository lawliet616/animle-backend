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
using Microsoft.EntityFrameworkCore;

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





            int page = 1; // Page number (starting from 1)
            int limit = 500; // Number of items per page
            int offset = (page - 1) * limit;
            string apiSubUrl = $"anime/ranking?nfsw&ranking_type=bypopularity&limit={limit}&offset={offset}&fields=id,title,main_picture,alternative_titles,start_date,end_date,synopsis,mean,rank,popularity,num_list_users,num_scoring_users,nsfw,created_at,updated_at,media_type,status,genres,my_list_status,num_episodes,start_season,broadcast,source,average_episode_duration,rating,pictures,background,related_anime,related_manga,recommendations,studios,statistics";

            var malResult = await malService.ReturnAny(apiSubUrl, apiUrl);
            List<AnimeWithEmoji> animeWithEmojis = new List<AnimeWithEmoji>();
            string jsonString = malResult;
            MalApiObject animeData = JsonSerializer.Deserialize<MalApiObject>(jsonString);
            if (animeData.data != null)
            {
                animeData.data.ForEach((anime) =>
                {
                    if (!anime.node.title.ToLower().Contains("season") && !anime.node.title.Contains(" Part "))
                    {
                        AnimeWithEmoji animeWithEmoji = new AnimeWithEmoji();
                        List<string> propertyList = new List<string>();
                        propertyList.AddRange(anime.node.genres.Select(x => x.name));
                        propertyList.AddRange(anime.node.studios.Select(x => x.name));
                        propertyList.Add("start date:" + anime.node.start_date);
                        propertyList.Add("end date:" + anime.node.end_date);
                        propertyList.Add("media type:" + anime.node.media_type);
                        propertyList.Add("source:" + anime.node.source);
                        animeWithEmoji.properties = JsonSerializer.Serialize(propertyList);
                        animeWithEmoji.Title = anime.node.alternative_titles.en;
                        animeWithEmoji.MyanimeListId = anime.node.id;
                        animeWithEmoji.JapaneseTitle = anime.node.title;
                        animeWithEmoji.Image = anime.node.main_picture.large;
                        animeWithEmoji.Description = anime.node.synopsis;
                        animeWithEmoji.Thumbnail = anime.node.main_picture.large;
                        animeWithEmojis.Add(animeWithEmoji);
                    }
                });




                animeWithEmojis.ForEach((anime) =>
                {


                    _animle.AnimeWithEmoji.Add(anime);


                });




























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

            try
            {
                await _animle.UnathenticatedGames.ExecuteDeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }


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




