using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Karepro.Models
{
    public class NivelUrgencia
    {
        [Key]
        public int IdUrgencia { get; set; }
        
        public String Nivel { get; set; }       
       
    }
}

