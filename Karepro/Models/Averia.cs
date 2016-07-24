

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karepro.Models
{
    public class Averia
    {
        [Key]
        public int IdAveria { get; set; }
        public int IdEquipo { get; set; }

        public int IdUrgencia { get; set; }
        public virtual NivelUrgencia Nivel_urgencia { get; set; }
        public string Descripcion { get; set; } 
        public int IdInstitucion { get; set; }
        public virtual Institucion Institucion { get; set; }
        public virtual Equipo Equipo { get; set; }
    }
}