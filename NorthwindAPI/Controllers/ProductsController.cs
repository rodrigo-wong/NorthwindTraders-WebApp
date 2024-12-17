using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;

namespace NorthwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public ProductsController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Products/ByCategory/{id}
        [HttpGet("ByCategory/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(int id)
        {
            try
            {
                if (_context.Products == null)
                {
                    return NotFound("No products found in the database.");
                }

                var products = await _context.Products
                                    .Where(p => p.CategoryId == id && !p.Discontinued)
                                    .ToListAsync();

                if (!products.Any())
                {
                    return NotFound($"No products found in category with ID {id}.");
                }

                return products;
            }
            catch (Exception ex)
            {
                // Log the exception (if you have a logging mechanism)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Products/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                if (_context.Products == null)
                {
                    return NotFound("No products found in the database.");
                }

                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found.");
                }

                return product;
            }
            catch (Exception ex)
            {
                // Log the exception (if you have a logging mechanism)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
