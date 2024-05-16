using API_Teste.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Teste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController: ControllerBase
    {
        private readonly EFDBContext _dbContext;

        public ClienteController(EFDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Cliente>> Get()
        {
            return await _dbContext.Cliente.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Post(Cliente cliente)
        {
            _dbContext.Cliente.Add(cliente);
            await _dbContext.SaveChangesAsync();

            return cliente;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _dbContext.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _dbContext.Cliente.Remove(cliente);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

        private bool ClienteExists(int id)
        {
            return _dbContext.Cliente.Any(e => e.Id == id);
        }
    }
}
