using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Diagnostics;

namespace la_mia_pizzeria_static.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class PizzaController : Controller
    {
        private readonly ILogger<PizzaController> _logger;
        private readonly PizzaContext _context;
        public PizzaController(ILogger<PizzaController> logger, PizzaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var pizze = _context.Pizze.Include(p => p.Category).ToArray();

            return View(pizze);
        }

        public IActionResult Detail(int id)
        {
			var pizze = _context.Pizze
			   .Include(p => p.Category)
			   .Include(p => p.Ingredients)
			   .SingleOrDefault(p => p.Id == id);

			if (pizze is null)
            {
                return View("NotFound", "Post not found.");
                //return NotFound($"Pizza with id {id} not found.");
            }

            return View(pizze);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //var pizza = new Pizza
            //{
            //    Image = "https://picsum.photos/200/300"
            //};

            //return View(pizza);
            
            var formModel = new PizzaFormModel
            {
                Categories = _context.Categories.ToArray(),
                Ingredients = _context.Ingredients.Select(i => new SelectListItem(i.Title, i.Id.ToString())).ToArray(),
            };

            return View(formModel);
            
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(PizzaFormModel form)
		{
            //if (!ModelState.IsValid)
            //{
            //	return View(pizza);
            //}

            //using var ctx = new PizzaContext();
            //         ctx.Pizze.Add(pizza);   
            //         ctx.SaveChanges();  
            //         return RedirectToAction("Index");
        if (!ModelState.IsValid)
        {
            form.Categories = _context.Categories.ToArray();
				form.Ingredients = _context.Ingredients.Select(i => new SelectListItem(i.Title, i.Id.ToString())).ToArray();
				return View(form);
        }

        form.SetImageFileFromFormFile();
		form.Pizza.Ingredients = form.SelectedIngredients.Select(st => _context.Ingredients.First(i => i.Id == Convert.ToInt32(st))).ToList();

		_context.Pizze.Add(form.Pizza);
        _context.SaveChanges();

        return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
		{
			var pizza = _context.Pizze.Include(p => p.Ingredients).FirstOrDefault(p => p.Id == id);

			if (pizza is null)
			{
				return View("NotFound");
			}
            var formModel = new PizzaFormModel
            {
                Pizza = pizza,
                Categories = _context.Categories.ToArray(),
				Ingredients = _context.Ingredients.ToArray().Select(t => new SelectListItem(
				    t.Title,
				    t.Id.ToString(),
				    pizza.Ingredients!.Any(_i => _i.Id == t.Id))
			).ToArray()
			};

			formModel.SelectedIngredients = formModel.Ingredients.Where(i => i.Selected).Select(i => i.Value).ToList();
			return View(formModel);
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Update(int id, PizzaFormModel form)
		{
			if (!ModelState.IsValid)
			{
                form.Categories = _context.Categories.ToArray();
				form.Ingredients = _context.Ingredients.Select(i => new SelectListItem(i.Title, i.Id.ToString())).ToArray();
				return View(form);
			}


            var pizzaToUpdate = _context.Pizze.Include(p =>p.Ingredients).FirstOrDefault(p => p.Id == id);

            if (pizzaToUpdate is null)
            {
                return View("NotFound");
            }

			form.SetImageFileFromFormFile();
			form.Pizza.Name = pizzaToUpdate.Name;
			form.Pizza.Description = pizzaToUpdate.Description;
		    form.Pizza.Price = pizzaToUpdate.Price;
			form.Pizza.Category = pizzaToUpdate.Category;
			form.Pizza.ImageFile = pizzaToUpdate.ImageFile;
			pizzaToUpdate.Ingredients = form.SelectedIngredients.Select(st => _context.Ingredients.First(i => i.Id == Convert.ToInt32(st))).ToList();

			_context.SaveChanges();

            return RedirectToAction("Index");
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			var pizzaToDelete = _context.Pizze.FirstOrDefault(p => p.Id == id);

			if (pizzaToDelete is null)
			{
				return View("NotFound");
			}

			_context.Pizze.Remove(pizzaToDelete);
			_context.SaveChanges();

			return RedirectToAction("Index");
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

