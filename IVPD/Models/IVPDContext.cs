using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IVPD.Models
{
    public class IVPDContext:DbContext
    {

        public IVPDContext()
        {
        }

        public IVPDContext(DbContextOptions<IVPDContext> options)
            : base(options)
        {
        }
        public virtual DbSet<UserTypes> UserTypes { get; set; }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<GroupPermission> GroupPermissions { get; set; }
        public virtual DbSet<Schedules> Schedules { get; set; }
        public virtual DbSet<Parcel> Parcela { get; set; }
        public virtual DbSet<MatrixArticle> ARTIGO { get; set; }

        public virtual DbSet<Country> Country { get; set; }

        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Parish> Parish { get; set; }
        public virtual DbSet<SINONIMO> SINONIMO { get; set; }
        public virtual DbSet<PlotSituation> PlotSituation { get; set; }
        public virtual DbSet<LegalSituation> SITLEGAL { get; set; }
        public virtual DbSet<LanguageKeys> LanguageKeys { get; set; }
        public virtual DbSet<AuditLog> AuditLog { get; set; }
        public virtual DbSet<ParcelExplorer> EXPLORPARC { get; set; }
        public virtual DbSet<CASTAPARC> CASTAPARC { get; set; }
        public virtual DbSet<ParcelProperty> PROPPARC { get; set; }
        public virtual DbSet<Levantamento> LEVANTAMENTO { get; set; }
        public virtual DbSet<LegalFramework> ENQLEGAL { get; set; }
        public virtual DbSet<MatrixList> MatrixList { get; set; }
        public virtual DbSet<Entity> Entity { get; set; }
        public virtual DbSet<Estatuto> Estatuto { get; set; }
        public virtual DbSet<BusEntFreguesia> Freguesia { get; set; }
        public virtual DbSet<BusEntConcelho> CONCELHO { get; set; }
        public virtual DbSet<BusEntDistrito> Distrito { get; set; }
        public virtual DbSet<ClassPlantation> ClassePlant { get; set; }
        public virtual DbSet<SitucaoDaParcela> SitucaoDaParcela { get; set; }

        public virtual DbSet<LitigationSituation> SITLITIGIO { get; set; }

        public virtual DbSet<ExplorerType> TIPOEXPLOR { get; set; }

        public virtual DbSet<DouroPort> DouroPort { get; set; }
        public virtual DbSet<colors> Cor { get; set; }
        public virtual DbSet<Synonyms> Synonyms { get; set; }
        public virtual DbSet<Casta> Casta { get; set; }

        public virtual DbSet<Rules> Rules { get; set; }
       // public virtual DbSet<Plantacao> Plantacao { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=IVPD");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Parcel>().HasKey("NUMPARC", "VERSAO");
            modelBuilder.Entity<LegalFramework>().HasKey("IDENQLEGAL", "IDDTPLANTACAO");
            modelBuilder.Entity<SINONIMO>().HasKey("IDSINONIMO", "CODCASTA");
            modelBuilder.Entity<BusEntConcelho>().HasKey(table => new {
                table.Coddis,
                table.Codcon
            });
        }

       
    }
}
