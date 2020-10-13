using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IVPD.Models
{
    public class IVPDSBaseContext : DbContext
    {
        public IVPDSBaseContext()
        {
        }

        public IVPDSBaseContext(DbContextOptions<IVPDSBaseContext> options)
            : base(options)
        {
        }
        public virtual DbSet<BusEntidadeEstatuto> BusEntidadeEstatuto { get; set; }
        public virtual DbSet<BusEntidade> BusEntidade { get; set; }
        public virtual DbSet<BusEntFreguesia> BusEntFreguesia { get; set; }
        public virtual DbSet<BusEntConcelho> BusEntConcelho { get; set; }
        public virtual DbSet<BusEntDistrito> BusEntDistrito { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=IVPDSBase");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BusEntConcelho>().HasKey("Coddis", "Codcon");
        }
            
    }
}
