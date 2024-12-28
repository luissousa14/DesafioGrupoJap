using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using GrupoJap.Models;

namespace GrupoJap.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ConnectionDatabase"))
                        .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var dieselGuid = Guid.NewGuid();

            modelBuilder.Entity<TipoCombustivel>().HasData(
                new TipoCombustivel { Id = dieselGuid, Descritivo = "Diesel" },
                new TipoCombustivel { Id = Guid.NewGuid(), Descritivo = "Gasolina" },
                new TipoCombustivel { Id = Guid.NewGuid(), Descritivo = "GPL" });

            var clienteGuid = Guid.NewGuid();
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente { Id = clienteGuid, NomeCompleto = "Luis Borges Sousa", Email = "luisousa2002@gmail.com", Telefone = "925195028", CartaConducao = true });

            var veiculoGuid = Guid.NewGuid();
            modelBuilder.Entity<Veiculo>().HasData(
                new Veiculo { Id = veiculoGuid, Marca = "Ford", Modelo = "Fiesta", Matricula = "88-JZ-33", AnoFabrico = 2010, TipoCombustivelId = dieselGuid });

            modelBuilder.Entity<ContratoAluguer>().HasData(
                new ContratoAluguer { Id = Guid.NewGuid(), ClienteId = clienteGuid, VeiculoId = veiculoGuid, DataInicio = DateTime.Now, DataFim = DateTime.Now.AddDays(5) });
        }

        public DbSet<Cliente> Cliente { get; set; } = default!;
        public DbSet<Veiculo> Veiculo { get; set; } = default!;
        public DbSet<TipoCombustivel> TipoCombustivel { get; set; } = default!;
        public DbSet<ContratoAluguer> ContratoAluguer { get; set; } = default!;
    }
}
