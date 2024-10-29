using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public required string Name { get; set; }

		[Required]
		public required string Description { get; set; }

		[Required, Column(TypeName = "money")]
		public required decimal Price { get; set; }
		public string? ImageURL { get; set; }

		//Navigation Props:

		public int CategoryId { get; set; }
		public Category Category { get; set; }
	}
}