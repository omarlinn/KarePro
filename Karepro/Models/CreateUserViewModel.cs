using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Karepro.Models
{
    public class CreateUserViewModel
    {
        public ApplicationUser User { get; set; }
        public ICollection<Equipo> Equipos { get; set; }
    }
}