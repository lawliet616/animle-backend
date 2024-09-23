namespace Animle.services.Token
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Claims;
    using System.Collections.Generic;
    using NHibernate;
    using MySqlX.XDevAPI;

    public class TokenService
    {
        private readonly string _secretKey; 
        private readonly SymmetricSecurityKey _securityKey;

        public TokenService(string secretKey)
        {
            _secretKey = secretKey;
            _securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_secretKey));
        }

        public string CreateToken(User user)
        {

            var claims = new List<Claim>();
            claims.AddRange(new[]
          {
               new Claim(JwtRegisteredClaimNames.Sub, user.Name),
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            });

            var key = new SymmetricSecurityKey(Convert.FromBase64String(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7020",
                audience: "http://localhost:4200",
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public ClaimsPrincipal ValidateToken(string tokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = _securityKey
            };


            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(tokenString, tokenValidationParameters, out SecurityToken validatedToken);
                return claimsPrincipal;
            }
            catch (SecurityTokenException)
            {
                return null;
            }

        }

        public bool GetUserRolesAndValidate(ClaimsPrincipal claimsPrincipal, string RoleRequired)
        {
            var roles = new List<string>();

            var roleClaims = claimsPrincipal.FindAll(ClaimTypes.Role);
            foreach (var roleClaim in roleClaims)
            {
                roles.Add(roleClaim.Value);
            }
            return roles.Contains(RoleRequired);

        }
        public User GetUser(AnimleDbContext animDbContext, ClaimsPrincipal claims)
        {

            var userIdClaim = claims.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);



            return animDbContext.Users.FirstOrDefault(u => u.Id.ToString() == userIdClaim.Value || u.Name == userIdClaim.Value);


        }
    }

}
