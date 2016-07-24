using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Karepro.Models
{
    public class EstadoAveria
    {
        [Key]
        public int IdEstado { get; set; }
        public string Estado { get; set; }
    }
}