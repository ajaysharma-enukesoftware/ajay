using IVPD.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Models
{
    public class RevenueContext : DbContext
    {
        public RevenueContext()
        {
        }
        public RevenueContext(DbContextOptions<RevenueContext> options)
         : base(options)
        {
        }
        public virtual DbSet<LIBTESOUR_THDOCUF> LIBTESOUR_THDOCUF { get; set; }
        public virtual DbSet<LIBTESOUR_TSARTIF> LIBTESOUR_TSARTIF { get; set; }
        // public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<EntityTransaction> EntityTransaction { get; set; }
        public virtual DbSet<BusEntidadeContacto> BusEntidadeContacto { get; set; }

        public virtual DbSet<BusEntTipoContacto> BusEntTipoContacto { get; set; }
        public virtual DbSet<Currency> Currency { get; set; }

        public virtual DbSet<LIBTESOUR_TSTIPDF> LIBTESOUR_TSTIPDF { get; set; }
        public virtual DbSet<ConsultCurrentAccount> ConsultCurrentAccount { get; set; }
        public virtual DbSet<LIBTESOUR_TSSALDF> LIBTESOUR_TSSALDF { get; set; }
        public virtual DbSet<PendingInvoices> PendingInvoices { get; set; }
        public virtual DbSet<DetailDocumentInfo> DetailDocumentInfo { get; set; }
        public virtual DbSet<LIBTESOUR_TSCIVAF> LIBTESOUR_TSCIVAF { get; set; }
        public virtual DbSet<LIBTESOUR_THDOCLF> LIBTESOUR_THDOCLF { get; set; }
        public virtual DbSet<BoxOpening> BoxOpening { get; set; }
        public virtual DbSet<BoxDetails> BoxDetails { get; set; }
        public virtual DbSet<CashValues> CashValues { get; set; }
        public virtual DbSet<TransactionDetails> TransactionDetails { get; set; }


        public virtual DbSet<TaxType> TaxType { get; set; }
        public virtual DbSet<OpeningClosedAmount> OpeningClosedAmount { get; set; }
        public virtual DbSet<IssueDocumentDetails> IssueDocumentDetails { get; set; }
        public virtual DbSet<EntityAccounts> EntityAccounts { get; set; }
    //    public virtual DbSet<DocumentType> DocumentType { get; set; }
        public virtual DbSet<BillingAddress> BillingAddress { get; set; }

        public virtual DbSet<DebitDetails> DebitDetails { get; set; }
       
        public virtual DbSet<CollectionRevenue> CollectionRevenue { get; set; }
        public virtual DbSet<AllTransactions> AllTransactions { get; set; }
        public virtual DbSet<AllTransactions1> AllTransactions1 { get; set; }

        public virtual DbSet<BoxClasp> BoxClasp { get; set; }
        public virtual DbSet<Cashier> Cashier { get; set; }
        public virtual DbSet<BusEntidade> BusEntidade { get; set; }
        public virtual DbSet<TransactionMethod> TransactionMethod { get; set; }
       // public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AlottedServices> AlottedServices { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=REVENUEDB");
            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.Query<MGBalance>().ToView("v_SaldosMG");
            modelBuilder.Entity<ConsultCurrentAccount>().HasNoKey();
            modelBuilder.Entity<LIBTESOUR_TSSALDF>().HasNoKey();
            modelBuilder.Entity<LIBTESOUR_TSTIPDF>().HasNoKey();
            modelBuilder.Entity<PendingInvoices>().HasNoKey();
            modelBuilder.Entity<DetailDocumentInfo>().HasNoKey();
            modelBuilder.Entity<LIBTESOUR_THDOCUF>().HasNoKey();
            modelBuilder.Entity<LIBTESOUR_TSCIVAF>().HasNoKey();
            modelBuilder.Entity<LIBTESOUR_THDOCLF>().HasNoKey();
            modelBuilder.Entity<BoxOpening>().HasNoKey();
            modelBuilder.Entity<BoxDetails>().HasNoKey();
            modelBuilder.Entity<BoxClasp>().HasNoKey();
            modelBuilder.Entity<CashValues>().HasNoKey();
            modelBuilder.Entity<EntityTransaction>().HasNoKey();
            modelBuilder.Entity<TaxType>().HasNoKey();
            modelBuilder.Entity<EntityAccounts>().HasNoKey();
           // modelBuilder.Entity<DocumentType>().HasNoKey();
            modelBuilder.Entity<CollectionRevenue>().HasNoKey();
            modelBuilder.Entity<Cashier>().HasNoKey();

        }

    }
    
}
