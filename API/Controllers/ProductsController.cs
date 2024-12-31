using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type,string? sort) {
            var spec = new ProductSpecification(brand,type, sort);

            var products = await repo.ListAsync(spec);

           return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id) {
            var product = await repo.GetByIdAsync(id);

            if (product is null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody]Product product)
        {
             repo.Add(product);

             if (await repo.SaveAllAsync()) {
                return CreatedAtAction("GetProduct", new {id = product.Id, product});
             }

            return BadRequest("Failed to create product");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product) {
            if (product.Id != id || !ProductExists(id)) return BadRequest("Cannot update this product");

            repo.Update(product);

            if (await repo.SaveAllAsync()) {
                return NoContent();
            }

            return BadRequest("Problem updating product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id) {
            var product = await repo.GetByIdAsync(id);

            if (product is null) return NotFound();

            repo.Remove(product);

            if (await repo.SaveAllAsync()){
                return NoContent();
            }
            
            return BadRequest("Problem deleting product");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands() {
            return Ok();
        } 

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes() {
            return Ok();
        } 

        private bool ProductExists(int id) {
            return repo.Exists(id);
        }
    }
}
