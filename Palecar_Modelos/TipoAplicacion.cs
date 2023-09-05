using System.ComponentModel.DataAnnotations;

namespace Palecar_Modelos
{
    public class TipoAplicacion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe indicar el nombre ddl tipo de aplicación.")]
        public string Nombre { get; set; }
    }
}
