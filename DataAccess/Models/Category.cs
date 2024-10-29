using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;

public class Category
{
	[Key]
        public int Id { get; set; }

	[Required]
	public required string Name { get; set; }

	//Navigation props:

	ICollection<Product>? Products { get; set; }
    }
