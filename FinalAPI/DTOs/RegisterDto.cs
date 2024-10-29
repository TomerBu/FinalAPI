using DataAccess.Models;
using System.ComponentModel.DataAnnotations;

namespace FinalAPI.DTOs;

public class RegisterDto
{
	[Required]
	[MinLength(2), MaxLength(20)]
	public required string UserName { get; set; }

	[Required]
	[EmailAddress]
	public required string Email { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public required string Password { get; set; }
}

public static class RegisterDtoExtensions
{
	public static AppUser ToUser(this RegisterDto dto)
	{
		return new AppUser
		{
			Email = dto.Email,
			UserName = dto.UserName,
		};
	}
}
