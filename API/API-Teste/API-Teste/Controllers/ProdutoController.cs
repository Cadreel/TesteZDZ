using API_Teste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Teste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController: ControllerBase
    {
        private readonly EFDBContext _dbContext;

        public ProdutoController(EFDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Produto>> Get()
        {
            return await _dbContext.Produto.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Produto>> Post(Produto produto)
        {
            _dbContext.Produto.Add(produto);
            await _dbContext.SaveChangesAsync();

            return produto;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var produto = await _dbContext.Produto.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            _dbContext.Produto.Remove(produto);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(produto).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return _dbContext.Produto.Any(e => e.Id == id);
        }
    }
}
