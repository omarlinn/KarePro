
using System.ComponentModel.DataAnnotations;

namespace Karepro.Models
{
    public class Institucion
    {
        [Key]
        public int IdInstitucion { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Descripcion { get; set; }
    }
}


