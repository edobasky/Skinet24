using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _Context;

        public ProductsController(StoreContext storeContext)
        {
            _Context = storeContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct() {
            return await _Context.Products.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id) {
            var product = await _Context.Products.FindAsync(id);

            if (product is null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody]Product product) {
             _Context.Products.Add(product);

            await _Context.SaveChangesAsync();

            return product;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product) {
            if (product.Id != id || !ProductExists(id)) return BadRequest("Cannot update this product");

            _Context.Entry(product).State = EntityState.Modified;

            await _Context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id) {
            var product = await _Context.Products.FindAsync(id);

            if (product is null) return NotFound();

            _Context.Products.Remove(product);

            await _Context.SaveChangesAsync();
            return NoContent();
        }

        private bool ProductExists(int id) {
            return _Context.Products.Any(x => x.Id == id);
        }
    }
}
