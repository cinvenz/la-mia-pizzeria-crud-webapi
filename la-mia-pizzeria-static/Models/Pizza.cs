using la_mia_pizzeria_static.Attributes;
using System.ComponentModel.DataAnnotations;


	namespace la_mia_pizzeria_static.Models
{
	public class Pizza
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Inserisci un nome per la pizza.")]
		[StringLength(50, ErrorMessage = "Il Nome non può essere più lungo di 50 caratteri.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Inserisci una descrizione per la pizza.")]
		[StringLength(250, ErrorMessage = "la descrizione della pizza non può superare i 250 caratteri.")]
		[MoreThanOneWord(5, ErrorMessage = "la descrizione della pizza deve contenere almeno 5 parole.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Inserisci un immagine per la Pizza.")]
        public string Image { get; set; } = string.Empty;

        public byte[]? ImageFile { get; set; }

        public string ImgSrc => ImageFile is null
            ? Image
            : $"data:image/png;base64,{Convert.ToBase64String(ImageFile)}";


        [Required(ErrorMessage = "inserisci un prezzo per la pizza.")]
		[Range(1, 100, ErrorMessage = "Il prezzo deve essere compreso tra 1 e 100 euro.")]
		public double Price { get; set; }

        [NonZero(ErrorMessage = "Scegli una categoria.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

		public IEnumerable<Ingredient>? Ingredients { get; set; }	
    }
}


