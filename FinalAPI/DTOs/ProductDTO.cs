using DataAccess.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalAPI.DTOs;

public class ProductDTO
{
	public int Id { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
	public decimal Price { get; set; }
	public required string ImageUrl { get; set; }
	public required string Category { get; set; }
}

public static class ProductExtensions
{
	public static ProductDTO ToDto(this Product p)
	{
		return new ProductDTO()
		{
			Category = p.Category.Name,
			Id = p.Id,
			Name = p.Name,
			Description = p.Description,
			Price = p.Price,
			ImageUrl = p.ImageURL
		};
	}
}
