using System.ComponentModel.DataAnnotations;

namespace Web_Palecar.Models
{
    public class Categoría
    {
        [Key]
        public int Id { get; set; }
        public string NombreCategoria { get; set; }
        public int MostrarOrden { get; set; }
    }
    
}
