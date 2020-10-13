using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IVPD.Models
{
    public class VindimaContext : DbContext
    {
        public VindimaContext()
        {
        }
        public VindimaContext(DbContextOptions<VindimaContext> options)
         : base(options)
        {
        }
        public virtual DbSet<RecocileProducer> RecocileProducer { get; set; }
        public virtual DbSet<ProductionAuthorization> ProductionAuthorization { get; set; }
        public virtual DbSet<MGBalance> MGBalance { get; set; }
        public virtual DbSet<EMTPMG> EMTPMG { get; set; }
        public virtual DbSet<ENTIDADES> ENTIDADES { get; set; }
        public virtual DbSet<REGCARTAS> REGCARTAS { get; set; }
        public virtual DbSet<FRBLIN> FRBLIN { get; set; }

        public virtual DbSet<RegistrationImpression> RegistrationImpression { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=VINDIMA");
            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           // modelBuilder.Query<MGBalance>().ToView("v_SaldosMG");


            modelBuilder.Entity<RecocileProducer>().HasKey(table => new {
                table.CONCANO,
                table.CONCDAT
            });
            modelBuilder.Entity<ProductionAuthorization>().HasKey(table => new {
                table.NRAP,
                table.ENTNUM,
                table.NUMPARC

            });
            modelBuilder.Entity<ENTIDADES>().HasNoKey();
            modelBuilder.Entity<MGBalance>().HasNoKey();
            modelBuilder.Entity<FRBLIN>().HasNoKey();
            modelBuilder.Entity<RegistrationImpression>().HasNoKey();

            

        }
    }
}
