using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
	{
        public const string ConnectionString = "Data Source=localhost;Initial Catalog=PizzaDati;Integrated Security=True;TrustServerCertificate=True";
        public PizzaContext(DbContextOptions<PizzaContext> options) : base(options) { }
        public PizzaContext() {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        public DbSet<Pizza> Pizze { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public void Seed()
        {
            
            var pizzaSeed = new Pizza[]
            {
            new Pizza
                {
                    Name = "Pizza Diavola",
                    Description = "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
                    Image = "https://picsum.photos/id/237/200/300",
                    Price = 4.50,
                },
            new Pizza
                {
                    Name = "Pizza Capricciosa",
                    Description = "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
                    Image = "https://picsum.photos/id/237/200/300",
                    Price = 6.80,
                },
            new Pizza
                {
                    Name = "Pizza Marinara",
                    Description = "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
                    Image = "https://picsum.photos/id/237/200/300",
                    Price = 5,
                },
            new Pizza
                {
                    Name = "Pizza Fumè",
                    Description = "Lorem, ipsum dolor sit amet consectetur adipisicing elit.",
                    Image = "https://picsum.photos/id/237/200/300",
                    Price = 5.50,
                },
            };
                 

            if (!Pizze.Any())
            {
                Pizze.AddRange(pizzaSeed);
            }

            if (!Categories.Any())
            {
                var seed = new Category[]
                {
                    new()
                    {
                        Title = "Pizze classiche",
                        Pizze = pizzaSeed,
                    },
                    new()
                    {
                        Title = "Pizze bianche",
                        
                    },
                     new()
                    {
                        Title = "Pizze vegetariane",
                        
                    },
                      new()
                    {
                        Title = "Pizze di mare",
                        
                    },
                };

                Categories.AddRange(seed);
            }

            if (!Ingredients.Any())
            {
                var seed = new Ingredient[]
                {
                    new()
                    {
                        Title = "Salame",
                        
                    },
                    new()
                    {
                        Title = "Pepe",
                    },
                     new()
                    {
                        Title = "Salsiccia",
                        Pizze = pizzaSeed,
                    },
                      new()
                    {
                        Title = "Rucola",
                    },
                };
                Ingredients.AddRange(seed);
            }

            if (!Roles.Any())
            {
                var seed = new IdentityRole[]
                {
                    new("Admin"),
                    new("User")
                };

                Roles.AddRange(seed);
            }


            if (Users.Any(u => u.Email == "admin@dev.com" || u.Email == "user@dev.com")
                && !UserRoles.Any())
            {
                var admin = Users.First(u => u.Email == "admin@dev.com");
                var user = Users.First(u => u.Email == "user@dev.com");

                var adminRole = Roles.First(r => r.Name == "Admin");
                var userRole = Roles.First(r => r.Name == "User");

                var seed = new IdentityUserRole<string>[]
                {
                    new()
                    {
                        UserId = admin.Id,
                        RoleId = adminRole.Id
                    },
                    new()
                    {
                        UserId = user.Id,
                        RoleId = userRole.Id
                    }
                };

                UserRoles.AddRange(seed);
            }
            SaveChanges();
        }
    }
}



