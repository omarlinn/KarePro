

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

        public string IdTecnico { get; set; }
        
        public string Descripcion { get; set; } 
        public int IdInstitucion { get; set; }

        public virtual Institucion Institucion { get; set; }
        public virtual Equipo Equipo { get; set; }
        public virtual NivelUrgencia Nivel_urgencia { get; set; }

        [ForeignKey("IdTecnico")]
        public virtual ApplicationUser Tecnico { get; set; }

    }

    public class AsignarAveriaViewModel
    {

        public string idTecnico { get; set; }
        public int idAveria { get; set; }

        public Averia Averia { get; set; }
        public ApplicationUser Tecnico { get; set; }

    }
}