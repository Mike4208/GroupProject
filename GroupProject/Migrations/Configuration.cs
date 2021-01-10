namespace GroupProject.Migrations
{
    using GroupProject.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GroupProject.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "GroupProject.Models.ApplicationDbContext";
        }

        protected override void Seed(GroupProject.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var categories = new List<Category>
            {
                new Category { Name = "Laptop" },
                new Category { Name = "Mobile" },
                new Category { Name = "Tablet" }
            };

            categories.ForEach(s => context.Categories.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            var manufacturers = new List<Manufacturer>
            {
                new Manufacturer { Name = "Apple" },
                new Manufacturer { Name = "Hp" },
                new Manufacturer { Name = "Samsung" },
                new Manufacturer { Name = "Asus" },
                new Manufacturer { Name = "Huawei" },
                new Manufacturer { Name = "Lenovo" },
                new Manufacturer { Name = "Toshiba" },
                new Manufacturer { Name = "Dell" },
                new Manufacturer { Name = "Cubot" },
                new Manufacturer { Name = "Xiaomi" }
            };
            manufacturers.ForEach(s => context.Manufactorers.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            new List<Product>
            {
                new Product { Name = "64gb", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobiles").ID,
                    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "Images/iphone12.png" },
                new Product { Name = "64gb", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobiles").ID,
                    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "Images/iphone12.png" },
                new Product { Name = "128gb", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobiles").ID,
                    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "Images/iphone12.png" },
                new Product { Name = "FA506IU-HN156T", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptops").ID,
                    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "Images/asus.jpg" },
                new Product { Name = "FA706IU-H7006T", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptops").ID,
                    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "Images/asus.jpg" },
                new Product { Name = "21-b0003nv AiO", CategoryID = categories.SingleOrDefault(g => g.Name == "Pc").ID,
                    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Hp").ID, ProductImage = "Images/hp.jpg" }
            }.ForEach(a => context.Products.AddOrUpdate(a));
        }
    }
}
