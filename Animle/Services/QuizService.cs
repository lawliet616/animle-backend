using Animle.Controllers;
using Animle.Helpers;
using Animle.interfaces;
using Animle.Models;
using Animle;
using Microsoft.EntityFrameworkCore;


public class QuizDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Thumbnail { get; set; }

    public UserDto user { get; set; }
}

public interface IQuizService
{
    Task<Quiz> CreateQuizAsync(QuizCreation quiz, User user);
    Task<string> LikeQuizAsync(int id, User user);
    Task<List<int>> RetrieveUserLikesAsync(User user);
    Task<Quiz> GetQuizByIdAsync(int id);
    Task<ListResponse<QuizDto>> GetQuizzesAsync(IQueryCollection queryString);
}

public class QuizService : IQuizService
{
    private readonly AnimleDbContext _context;

    public QuizService(AnimleDbContext context)
    {
        _context = context;
    }

    public async Task<Quiz> CreateQuizAsync(QuizCreation quiz, User user)
    {
        var newQuiz = new Quiz
        {
            Title = quiz.Title,
            CreatedAt = DateTime.Now,
            user = user,
            Animes = _context.AnimeWithEmoji
                .Where(p => quiz.AnimeIds.Contains(p.MyanimeListId))
                .ToList(),
            Thumbnail = _context.AnimeWithEmoji
                .FirstOrDefault(a => a.MyanimeListId == quiz.SelectedImageId)?.Thumbnail
        };

        _context.Quizes.Add(newQuiz);
        await _context.SaveChangesAsync();
        return newQuiz;
    }

    public async Task<string> LikeQuizAsync(int id, User user)
    {
        var quiz = await _context.Quizes.FindAsync(id);
        if (quiz == null) return "Quiz not found";

        var existingLike = await _context.QuizLikes
            .FirstOrDefaultAsync(q => q.quizId == id && q.UserId == user.Id);

        if (existingLike != null)
        {
            _context.QuizLikes.Remove(existingLike);
            await _context.SaveChangesAsync();
            return "Quiz Removed";
        }

        var quizLike = new QuizLikes { LikedQuiz = quiz, User = user };
        _context.QuizLikes.Add(quizLike);
        await _context.SaveChangesAsync();
        return "Quiz Added To user";
    }

    public async Task<List<int>> RetrieveUserLikesAsync(User user)
    {
        return await _context.QuizLikes
            .Where(q => q.UserId == user.Id)
            .Select(x => x.quizId)
            .ToListAsync();
    }

    public async Task<Quiz> GetQuizByIdAsync(int id)
    {
        return await _context.Quizes
            .Include(x => x.Animes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ListResponse<QuizDto>> GetQuizzesAsync(IQueryCollection queryString)
    {
        var query = UtilityService.GenerateQuery(queryString,
            _context.Quizes.Include(x => x.user).Select(x => new QuizDto
            {
                Thumbnail = x.Thumbnail,
                Title = x.Title,
                CreatedAt = x.CreatedAt,
                Id = x.Id,
                user = new UserDto
                {
                    Name = x.user.Name
                }
            }).ToList());

        return query;
    }


}
