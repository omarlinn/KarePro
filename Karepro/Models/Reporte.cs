using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Karepro.Models
{
    public class Reporte
    {
        [Key]
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdAveria { get; set; }
        public string Descripcion { get; set; }
        public virtual Averia Averia { get; set; }
      
    }
}