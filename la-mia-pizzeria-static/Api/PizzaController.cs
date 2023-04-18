using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly PizzaContext _context;

        public PizzaController(PizzaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPizze([FromQuery] string? name)
        {
            var pizze = _context.Pizze
                .Where(p => name == null || p.Name.ToLower().Contains(name.ToLower()))
                .ToList();

            return Ok(pizze);
        }

        [HttpGet("{id}")]
        public IActionResult GetPizza(int id)
        {
            var pizza = _context.Pizze.FirstOrDefault(p => p.Id == id);

            if (pizza is null)
            {
                return NotFound();
            }

            return Ok(pizza);
        }

        [HttpPost]
        public IActionResult CreatePizza(Pizza pizza)
        {
            _context.Pizze.Add(pizza);
            _context.SaveChanges();

            return Ok(pizza);
        }

        [HttpPut("{id}")]
        public IActionResult PutPizza(int id, [FromBody] Pizza pizza)
        {
            var savedPizza = _context.Pizze.FirstOrDefault(p => p.Id == id);

            if (savedPizza is null)
            {
                return NotFound();
            }

            savedPizza.Name = pizza.Name;
            savedPizza.Description = pizza.Description;
            savedPizza.Price = pizza.Price;
            savedPizza.Category = pizza.Category;
            savedPizza.ImageFile = pizza.ImageFile;

            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePizza(int id)
        {
            var savedPizza = _context.Pizze.FirstOrDefault(p => p.Id == id);

            if (savedPizza is null)
            {
                return NotFound();
            }

            _context.Pizze.Remove(savedPizza);
            _context.SaveChanges();

            return Ok();
        }
    }
}
