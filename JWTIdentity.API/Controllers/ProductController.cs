using JWTIdentity.API.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTIdentity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(AppDbContext context) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await context.Products.ToListAsync();
            return Ok(products);
        }

    }
}
