namespace Karepro.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Karepro.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Karepro.Models.ApplicationDbContext context)
        {
            //Anadir los 3 roles por defecto
            if (!context.Roles.Any(r => r.Name == "Administrador"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Administrador" };
                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Usuario"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role2 = new IdentityRole { Name = "Usuario" };
                manager.Create(role2);
            }

            if (!context.Roles.Any(r => r.Name == "Tecnico"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role3 = new IdentityRole { Name = "Tecnico" };
                manager.Create(role3);
            }

            //Anadir el usuario administrador por defecto
            if (!context.Users.Any(u => u.UserName == "admin@karepro.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser
                {
                    Name = "El",
                    LastName = "Aminitreirol",
                    UserName = "admin@karepro.com",
                    Email = "admin@karepro.com"
                };


                manager.Create(user, "holamundo");
                manager.AddToRole(user.Id, "Administrador");
            }

            if(!context.NivelUrgencia.Any(u => u.Nivel == "Baja"))
            {
                context.NivelUrgencia.Add(new NivelUrgencia { Nivel = "Baja" });
            }

            if (!context.NivelUrgencia.Any(u => u.Nivel == "Media"))
            {
                context.NivelUrgencia.Add(new NivelUrgencia { Nivel = "Media" });
            }

            if (!context.NivelUrgencia.Any(u => u.Nivel == "Alta"))
            {
                context.NivelUrgencia.Add(new NivelUrgencia { Nivel = "Alta" });
            }
        }
    }
}
