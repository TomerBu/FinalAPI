using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinalAPI.Services
{
	public class JwtService(IConfiguration config, UserManager<AppUser> userManager)
	{
		public async Task<string> CreateToken(AppUser user)
		{
			var jwtSettings = config.GetSection("JwtSettings");
			var secretKey = jwtSettings["SecretKey"]?? throw new Exception("Secret key must be set");

			var claims = new List<Claim>() 
			{
				new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
				new Claim(JwtRegisteredClaimNames.Email, user.Email)
			};

			var isAdmin = await userManager.IsInRoleAsync(user, "Admin");

			claims.Add(new Claim(ClaimTypes.Role, "Admin"));

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
			var token = new JwtSecurityToken(
				issuer: jwtSettings["Issuer"],
				audience: jwtSettings["Audience"],
				expires: DateTime.UtcNow.AddDays(1),
				claims: claims,
				signingCredentials: creds
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
