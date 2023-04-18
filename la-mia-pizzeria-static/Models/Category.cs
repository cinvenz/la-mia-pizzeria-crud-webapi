using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Plase provide a post Title.")]
        [StringLength(100, ErrorMessage = "Title must have less than 100 characters.")]
        public string Title { get; set; } = string.Empty;

        public IEnumerable<Pizza>? Pizze { get; set; }
    }
    
}
