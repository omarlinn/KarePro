using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Karepro.Models
{
    public class Insumo
    {
        public Insumo()
        {
            Mantenimientos = new HashSet<Mantenimiento>();
        }

        [Key]
        public int IdInsumo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Mantenimiento> Mantenimientos { get; set; }
    }
}
