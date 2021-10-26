
using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Shop;

namespace Shop.Controllers
{

    [Route("products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var model = await context
                .Products
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
            return Ok(model);
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> GetProductsByCategory(int id, [FromServices] DataContext context)
        {
            var model = await context
                .Products
                .Include(p => p.Category)
                .AsNoTracking()
                .Where(p => p.CategoryId == id)
                .ToListAsync();
            return Ok(model);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context)
        {
            var model = await context
                .Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            return Ok(model);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Product>> Create([FromBody] Product model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();

                return Ok(model);

            }
            catch
            {
                return BadRequest(new { message = "Server error" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<Product>> Update(int id, [FromBody] Product model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.Id != id)
                return NotFound(new { message = "Categoria nao encontrada" });

            try
            {
                context.Entry<Product>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Concurrency problems" });
            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Server" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<Product>> Delete(int id, [FromServices] DataContext context)
        {
            var model = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (model == null)
                return NotFound(new { message = "Categoria nao encontrada" });

            try
            {
                context.Products.Remove(model);
                await context.SaveChangesAsync();
                return Ok();

            }
            catch (System.Exception)
            {
                return BadRequest(new { message = "Server" });
            }
        }
    }
}
