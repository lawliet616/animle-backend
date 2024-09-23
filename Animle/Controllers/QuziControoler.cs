
using Animle.interfaces;
using Animle.Models;
using Animle.services;
using Animle.services.Token;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using NHibernate.Util;
using System.Security.Claims;
using System.Security.Cryptography.Xml;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Animle.Controllers
{
    [Route("quiz")]
    [ApiController]
    public class QuziControoler : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly AnimleDbContext _animleDbContext;

        public QuziControoler(TokenService tokenService, AnimleDbContext animleDbContext)
        {
            _tokenService = tokenService;
            _animleDbContext = animleDbContext;
        }

        [EnableRateLimiting("fixed")]
        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [HttpPost]
        public async Task<IActionResult> PostQuiz([FromBody] QuizCreation quiz)
        {
            if (HttpContext.Items["user"] is User user)
            {
                try
                {
                    var newQuiz = new Quiz();
                    newQuiz.Title = quiz.Title;
                    newQuiz.CreatedAt = DateTime.Now;
                    newQuiz.user = user;
                    if (newQuiz.Animes == null)
                    {
                        newQuiz.Animes = new List<AnimeWithEmoji>();
                    }
                    var animes = _animleDbContext.AnimeWithEmoji.Where(p => quiz.AnimeIds.Contains(p.MyanimeListId)).ToList();
                    animes.ForEach((a) =>
                    {
                        if (a != null)
                        {
                            newQuiz.Animes.Add(a);
                        }
                    });
                    newQuiz.Thumbnail = animes.FirstOrDefault(a => a.MyanimeListId == quiz.SelectedImageId).Thumbnail;
                
                    _animleDbContext.Quizes.Add(newQuiz);

                    await _animleDbContext.SaveChangesAsync();

                    SimpleResponse simpleResponse = new SimpleResponse();
                    simpleResponse.Response = "Quiz Created";

                    return Ok(simpleResponse);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }

            }
            else
            {
                return Unauthorized(new {Response = "Please login first!"});
            }

           

        }
        [EnableRateLimiting("fixed")]
        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [Route("like/{id}")]
        [HttpGet]
        public async Task<IActionResult> LikeQuiz(int id)
        {
            if (HttpContext.Items["user"] is User user)
            {
                try
                {
                    SimpleResponse simpleResponse = new SimpleResponse();
                    Quiz quiz = _animleDbContext.Quizes.FirstOrDefault(q => q.Id == id);
                    if (quiz == null)
                    {
                        simpleResponse.Response = "Quiz not found";
                        return BadRequest(simpleResponse);
                    }
                    QuizLikes quizLike = _animleDbContext.QuizLikes.FirstOrDefault(q => q.quizId == id && user.Id == q.UserId);
                    if (quizLike != null)
                    {
                        _animleDbContext.QuizLikes.Remove(quizLike);
                        await _animleDbContext.SaveChangesAsync();
                        simpleResponse.Response = "Quiz Removed";

                        return Ok(simpleResponse);

                    }


                    QuizLikes quizLikes = new();
                    quizLikes.LikedQuiz = quiz;
                    quizLikes.User = user;
                    _animleDbContext.QuizLikes.Add(quizLikes);
                    await _animleDbContext.SaveChangesAsync();
                    simpleResponse.Response = "Quiz Added To user";
                    return Ok(simpleResponse);


                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            else
            {
                return Unauthorized(new {Response = "You must be logged in" });
            }
              


     
               




        }

        [HttpGet]
        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [Route("likes")]
        public IActionResult RetriveUserLikes()
        {
            if (HttpContext.Items["user"] is User user)
            {
                try
                {
                    List<int> quizLikes = _animleDbContext.QuizLikes.Where(q => q.UserId == user.Id).Select(x => x.quizId).ToList();
                    return Ok(quizLikes);

                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }

            }
            else
            {
                return Unauthorized(new { Response = "You must be logged in" });
            }




        }

        [EnableRateLimiting("fixed")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var animes = await _animleDbContext.Quizes.Include(x => x.Animes).FirstOrDefaultAsync(x => x.Id == x.Id);
            Random rnd = new Random();
            animes.Animes.ForEach((a) =>
            {
                a.Type = UtilityService.GetTypeByNumber(rnd.Next(0, 3));
            });

            return Ok(animes);
        }
        [EnableRateLimiting("fixed")]
        [HttpGet]
        public IActionResult GeQuizzs()
        {
           var quryString = Request.Query;
           var query = UtilityService.GenerateQuery(quryString, _animleDbContext.Quizes.Include(x => x.user).Select(x => new QuizDto
            {
                Thumbnail = x.Thumbnail,
                Title = x.Title,
                CreatedAt = x.CreatedAt,
                Id = x.Id,
                user = new UserDto
                {
                    Name = x.user.Name,
                
                }
            }).ToList());


            return Ok(query);

        }



        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    internal class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Thumbnail { get; set; }

        public UserDto user { get; set; }
    }

 


}
