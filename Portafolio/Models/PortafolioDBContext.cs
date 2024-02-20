
using Microsoft.EntityFrameworkCore;
using Portafolio.Models.BD1;

namespace Portafolio.Models
{
    public class PortafolioDBContext : DbContext
    {
        public PortafolioDBContext(DbContextOptions<PortafolioDBContext> options) : base(options)
        {
        }

        public DbSet<Cuenta> Cuenta => Set<Cuenta>();
        public DbSet<Usuario> Usuario => Set<Usuario>();
        public DbSet<InfHome> InfHome => Set<InfHome>();
        public DbSet<Proyectos> Proyectos => Set<Proyectos>();
        public DbSet<Lenguaje> Lenguaje => Set<Lenguaje>();
        public DbSet<Especialidad> Especialidad => Set<Especialidad>();
        public DbSet<RedSocial> RedSocial => Set<RedSocial>();
        public DbSet<TipRedSocial> TipRedSocial => Set<TipRedSocial>();


    }
}
