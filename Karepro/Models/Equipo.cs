using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karepro.Models
{
    public class Equipo
    {
        [Key]
        public int IdEquipo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Perio_mantenimiento { get; set; }
        public string IdUsuario { get; set; }
        public int Cod_inst { get; set; }

        public virtual Institucion Institucion { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual ApplicationUser Usuario { get; set; }
    }
}