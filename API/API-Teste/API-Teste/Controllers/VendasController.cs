using API_Teste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Teste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendasController: ControllerBase
    {
        private readonly EFDBContext _dbContext;

        public VendasController(EFDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Vendas>> Get()
        {
            return await _dbContext.Vendas.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Vendas>> Post(Vendas vendas)
        {
            _dbContext.Vendas.Add(vendas);
            await _dbContext.SaveChangesAsync();

            return vendas;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vendas = await _dbContext.Vendas.FindAsync(id);
            if (vendas == null)
            {
                return NotFound();
            }

            _dbContext.Vendas.Remove(vendas);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Vendas vendas)
        {
            if (id != vendas.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(vendas).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendasExists(id))
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

        private bool VendasExists(int id)
        {
            return _dbContext.Cliente.Any(e => e.Id == id);
        }
    }
}
