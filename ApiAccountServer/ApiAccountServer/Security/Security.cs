using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace ApiAccountServer.Security;

public class Security
{
    private static readonly IPasswordHasher<object> _passwordHasher = new PasswordHasher<object>();
    private const string _secretKey = "pancht-256-secret-must-be-32byte";
    private static readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

    public static string EncryptPassword(string pw)
    {
        //var hashed = _passwordHasher.HashPassword(null, pw);
        var hashed = BCrypt.Net.BCrypt.HashPassword(pw);

        return hashed;
    }

    public static bool VerifyPassword(string pw, string hashedPw)
    {
        try
        {
            return BCrypt.Net.BCrypt.Verify(pw, hashedPw);
            //return _passwordHasher.VerifyHashedPassword(null, hashedPw, pw) == PasswordVerificationResult.Success;
        }
        catch (SaltParseException ex)
        {
            Console.WriteLine("Invalid salt version: " + ex.Message);
            return false;
        }
    }

    public static string GenerateAuthToken(string id)
    {
        var claims = new[]{
            new Claim(ClaimTypes.Email, id)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(6),
            SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }
}
