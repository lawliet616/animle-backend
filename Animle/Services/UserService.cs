using Animle.interfaces;
using Animle.services.Token;
using Animle;
using Microsoft.EntityFrameworkCore;
using Animle.Models;

public interface IUserService
{
    Task<User> CreateUserAsync(User user);
    Task<User> AuthenticateUserAsync(LoginInfos loginInfos);
    Task<User> GetUserByIdAsync(int userId);

    Task<List<GameContest>> GetDailyLeaderBoard(string type);
}

public class UserService : IUserService
{
    private readonly AnimleDbContext _context;
    private readonly TokenService _tokenService;

    public UserService(AnimleDbContext context, TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        user.Password = PasswordManager.HashPassword(user.Password);
        user.Rating = 1000;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> AuthenticateUserAsync(LoginInfos loginInfos)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == loginInfos.Name);
        if (user != null && PasswordManager.VerifyPassword(loginInfos.Password, user.Password))
        {
            return user;
        }
        return null;
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<List<GameContest>> GetDailyLeaderBoard(string type)
    {
        return _context.GameContests.Include(g => g.User).OrderBy(g => g.Result).Take(25).ToList();    
            
      }

}
