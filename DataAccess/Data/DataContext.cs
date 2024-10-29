using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccess.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<AppUser, IdentityRole<int>, int>(options)
{
	public DbSet<Category> Categories { get; set; } = default!;
	public DbSet<Product> Products { get; set; } = default!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Category>()
			.HasData([
				new Category(){
					Id = 1,
					Name = "Toys"
				},
				new Category(){
					Id = 2,
					Name = "Games"
				},
				new Category(){
					Id = 3,
					Name = "Books"
				},
				new Category(){
					Id = 4,
					Name = "Electronics"
				}
				]);
		modelBuilder.Entity<Product>()
			.HasData([
				new Product(){
					Id = 1,
					Name ="Iphone 16",
					Description = "Cellular device from Apple",
					Price = 5399.99M,
					CategoryId = 4,
					ImageURL = ""
				},
				new Product(){
					Id = 2,
					Name ="Percy Jackson 2",
					Description = "The 2nd book in the series Percy Jackson",
					Price = 129.99M,
					CategoryId = 3,
					ImageURL = ""
				}
				]);

		modelBuilder.Entity<IdentityRole<int>>()
			.HasData([
				new IdentityRole<int>()
				{
					Id = 2,
					Name = "User",
					NormalizedName = "USER",
				},
				new IdentityRole<int>(){
					Id =1,
					Name = "Admin",
					NormalizedName = "ADMIN"
				}]
			);

		var hasher = new PasswordHasher<AppUser>();
		modelBuilder.Entity<AppUser>()
			.HasData([
				new AppUser(){
					Id = 1,
					Email = "daniel@gmail.com",
					NormalizedEmail = "DANIEL@GMAIL.COM",
					UserName = "Danielh74",
					NormalizedUserName = "DANIELH74",
					SecurityStamp = Guid.NewGuid().ToString(),
					PasswordHash = hasher.HashPassword(null,"123456")
				}
				]);

		modelBuilder.Entity<IdentityUserRole<int>>()
			.HasData(
			new IdentityUserRole<int>()
			{
				RoleId = 1,
				UserId = 1,
			}
			);
	}
}
