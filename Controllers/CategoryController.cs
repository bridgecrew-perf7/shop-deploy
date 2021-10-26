
using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Microsoft.AspNetCore.Authorization;

[Route("categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
    {
        var category = await context.Categories.AsNoTracking().ToListAsync();
        return Ok(category);
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
    {
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return Ok(category);
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Category>> Create([FromBody] Category category, [FromServices] DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return Ok(category);

        }
        catch
        {
            return BadRequest(new { message = "Server error" });
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "employee")]
    public async Task<ActionResult<Category>> Update(int id, [FromBody] Category category, [FromServices] DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (category.Id != id)
            return NotFound(new { message = "Categoria nao encontrada" });

        try
        {
            context.Entry<Category>(category).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(category);
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
    public async Task<ActionResult<Category>> Delete(int id, [FromServices] DataContext context)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            return NotFound(new { message = "Categoria nao encontrada" });

        try
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok();

        }
        catch (System.Exception)
        {
            return BadRequest(new { message = "Server" });
        }
    }
}