using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Karepro.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public virtual Institucion Institucion { get; set; }
        public int? idInstitucion { get; set; }

        public ICollection<Equipo> Equipos { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Departamento> Departamentos { get; set; }

        public DbSet<Averia> Averias { get; set; }

        public DbSet<Equipo> Equipos { get; set; }

        public DbSet<Institucion> Instituciones { get; set; }

        public DbSet<Insumo> Insumos { get; set; }

        public DbSet<Mantenimiento> Mantenimientos { get; set; }

        public ApplicationDbContext()
            : base("KarePro", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Mantenimiento>()
                        .HasMany(e => e.Insumos)
                        .WithMany(c => c.Mantenimientos);

            modelBuilder.Entity<Mantenimiento>()
                        .HasRequired(e => e.Institucion)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Mantenimiento>()
                        .HasRequired(e => e.Averia)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Equipo>()
                        .HasRequired(e => e.Institucion)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Equipo>()
                        .HasRequired(e => e.Usuario)
                        .WithMany(e => e.Equipos)
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Departamento>()
                        .HasRequired(e => e.Institucion)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                        .HasOptional(e => e.Institucion)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Averia>()
                        .HasRequired(e => e.Institucion)
                        .WithMany()
                        .WillCascadeOnDelete(false);

            modelBuilder.Entity<Averia>()
                        .HasRequired(e => e.Equipo)
                        .WithMany()
                        .WillCascadeOnDelete(false);
        }
    }
}