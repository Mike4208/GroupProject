namespace GroupProject.Migrations
{
    using GroupProject.Models;
    using GroupProject.Data;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "GroupProject.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {

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
            manufacturers.ForEach(s => context.Manufacturers.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            new List<Product>
            {
                new Product { Name = "mobile1", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "Images/iphone12.png" },
                new Product { Name = "mobile2", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "Images/iphone12.png" },
                new Product { Name = "mobile3", CategoryID = categories.SingleOrDefault(g => g.Name == "Mobile").ID,
                    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Apple").ID, ProductImage = "Images/iphone12.png" },
                new Product { Name = "FA506IU-HN156T", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "Images/asus.jpg" },
                new Product { Name = "FA706IU-H7006T", CategoryID = categories.SingleOrDefault(g => g.Name == "Laptop").ID,
                    Price = 8.99M, ManufacturerID = manufacturers.Single(a => a.Name == "Asus").ID, ProductImage = "Images/asus.jpg" },
            }.ForEach(s => context.Products.AddOrUpdate(a => a.Name, s));
        }
    }
}
