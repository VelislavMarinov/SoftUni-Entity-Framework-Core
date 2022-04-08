using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new ProductShopContext();

            Console.WriteLine(GetUsersWithProducts(db));

            //var userJson = File.ReadAllText("../../../Datasets/users.json");
            //var productsJson = File.ReadAllText("../../../Datasets/products.json");
            //var categoriesJson = File.ReadAllText("../../../Datasets/categories.json");
            //var categoriesProductsJson = File.ReadAllText("../../../Datasets/categories-products.json");
            //ImportUsers(db, userJson);
            //ImportProducts(db, productsJson);
            //ImportCategories(db, categoriesJson);
            //ImportCategoryProducts(db, categoriesProductsJson);

            
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var soldProducts = context.Users.Include(x => x.ProductsSold).ToList().Where(x => x.ProductsSold.Count() > 0 && x.ProductsSold.Any(x => x.BuyerId != null))
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    age = x.Age,
                    soldProducts = new
                    {
                        count = x.ProductsSold.Where(p => p.BuyerId != null).Count(),
                        products = x.ProductsSold.Where(p => p.BuyerId != null)
                        .Select(p => new { name = p.Name, price = p.Price })
                    }
                }).OrderByDescending(x => x.soldProducts.count).ToList();
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var users = new
            {
                usersCount = soldProducts.Count(),
                users = soldProducts
            };
            var soldProductsToJson = JsonConvert.SerializeObject(users, Formatting.Indented, settings);
            return soldProductsToJson;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var soldProducts = context.Users.Where(x => x.ProductsSold.Count() > 0 && x.ProductsSold.Any(x => x.Buyer != null)).Select(x => new
            {
                firstName = x.FirstName,
                lastName = x.LastName,
                soldProducts = x.ProductsSold.Where(p => p.Buyer != null).Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    buyerFirstName = p.Buyer.FirstName,
                    buyerLastName = p.Buyer.LastName
                }).ToList()
            }).OrderBy(x => x.lastName).ThenBy(x => x.firstName).ToList();

            var soldProductsToJson = JsonConvert.SerializeObject(soldProducts, Formatting.Indented);
            Console.WriteLine(soldProducts.Count());
            return soldProductsToJson;
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products.Where(x => x.Price >= 500 && x.Price <= 1000).OrderBy(x => x.Price)
                .Select(x => new { name = x.Name, price = x.Price, seller = x.Seller.FirstName + " " + x.Seller.LastName }).ToList();

            var productsToJson = JsonConvert.SerializeObject(products, Formatting.Indented);



            return productsToJson;
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {

            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

         public static string ImportProducts(ProductShopContext context, string inputJson)
         {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
         }
         public static string ImportCategories(ProductShopContext context, string inputJson)
         {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson).Where(x => x.Name != null).ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
         }
         public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
         {
            var categoriesProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoryProducts.AddRange(categoriesProducts);
            context.SaveChanges();

            return $"Successfully imported {categoriesProducts.Count}";
        }
    }
}