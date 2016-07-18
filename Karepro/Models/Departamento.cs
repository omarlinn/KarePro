using System.ComponentModel.DataAnnotations;

namespace Karepro.Models
{
    public class Departamento
    {
        [Key]
        public int IdDepartamento { get; set; }
        public string Nombre { get; set; }
        public int IdInstitucion { get; set; }

        public virtual Institucion Institucion { get; set; }
    }
}