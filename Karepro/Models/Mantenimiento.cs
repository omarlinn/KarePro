using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Karepro.Models
{
    public class Mantenimiento
    {
        public Mantenimiento()
        {
            this.Insumos = new HashSet<Insumo>();
        }
        [Key]
        public int IdMantenimiento { get; set; }
        public int IdAveria { get; set; }
        public string Descripcion { get; set; }
        public int IdInstitucion { get; set; }
        [ForeignKey("IdInstitucion")]
        public virtual Institucion Institucion { get; set; }
        [ForeignKey("IdAveria")]
        public virtual Averia Averia { get; set; }
        public virtual ICollection<Insumo> Insumos { get; set; }
    }
}
