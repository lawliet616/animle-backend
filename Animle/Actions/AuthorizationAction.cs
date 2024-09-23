using Animle;
using Animle.services.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading.Tasks;

public class CustomAuthorizationFilter : IAsyncActionFilter
{
    private readonly TokenService _tokenService;
    private readonly AnimleDbContext _context;

    public CustomAuthorizationFilter(TokenService tokenService, AnimleDbContext context)
    {
        _tokenService = tokenService;
        _context = context;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var token = httpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(token))
        {
            context.Result = new UnauthorizedObjectResult(new { Response = "Login required" });
            return;
        }

        var claims = _tokenService.ValidateToken(token);

        if (claims == null)
        {
            context.Result = new UnauthorizedObjectResult(new { Response = "Token invalid" });
            return;
        }
        var userNameClaim = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);

        User user = _context.Users.Include(u => u.GameContests).FirstOrDefault(u => u.Name == userNameClaim.Value);
        if (user == null) {
            context.Result = new UnauthorizedObjectResult(new { Response = "Token invalid" });
            return;
        }

        httpContext.Items["user"] = user;

        await next();
    }
}
