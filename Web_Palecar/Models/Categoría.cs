using System.ComponentModel.DataAnnotations;

namespace Web_Palecar.Models
{
    public class Categoría
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Debe indicar el nombre de la categoría.")]  
        public string NombreCategoria { get; set; }

        [Required(ErrorMessage = "Debe indicar el número de orden.")]
        [Range(1,int.MaxValue, ErrorMessage ="Debe indicar un número mayor a 0")]
        public int MostrarOrden { get; set; }
    }
    
}
