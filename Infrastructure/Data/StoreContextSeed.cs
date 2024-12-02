using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
  public static async Task SeedAsync(StoreContext context)
  {
    if (!context.Products.Any())
    {
      var productsData = await File.ReadAllTextAsync("C:/APPLICATIONS/LEARNING_PROJECTS/E-COMMERCE/API_CLIENT/skinet/Infrastructure/Data/SeedData/products.json");
      var products = JsonSerializer.Deserialize<List<Product>>(productsData);

      if (products is null) return;

      context.Products.AddRange(products); 

      await context.SaveChangesAsync();
    }
  }
}
