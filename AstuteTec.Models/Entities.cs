using System;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AstuteTec.Models
{
    public partial class Entities : DbContext
    {
        public static string ConnectionString { get; set; }

        public DbSet<Dictionary> Dictionary { get; set; }

        public DbSet<DictionaryItem> DictionaryItem { get; set; }

        public DbSet<User> User { get; set; }

        public Entities(DbContextOptions<Entities> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //判断当前数据库是Oracle 需要手动添加Schema(DBA提供的数据库账号名称)
            if (this.Database.IsOracle())
            {
                modelBuilder.HasDefaultSchema("NJQHWATER");
            }
            base.OnModelCreating(modelBuilder);

            #region Dictionary

            //modelBuilder.Entity<Dictionary>()
            // .HasOne(d => d.Domain).WithMany()
            // .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<DictionaryItem>()
            //  .HasOne(d => d.Domain).WithMany()
            //  .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region User

            //modelBuilder.Entity<User>()
            //  .HasOne(d => d.Domain).WithMany()
            //  .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<User>()
            //    .HasOne(d => d.Organization)
            //    .WithMany(d=>d.User)
            //    .HasForeignKey(d=>d.OrganizationId)
            //    .OnDelete(DeleteBehavior.Restrict);

            #endregion

        }

        public static Entities CreateContext(string mySqlConnectionString = null)
        {
            if (string.IsNullOrWhiteSpace(mySqlConnectionString))
            {
                mySqlConnectionString = ConnectionString;
            }
            var optionBuilder = new DbContextOptionsBuilder<Entities>();
            //oracle数据库
            //使用oracle 11g请使用该下面方法
            //optionBuilder.UseOracle(mySqlConnectionString, b => b.UseOracleSQLCompatibility(OracleVersion));
            
            //使用oracle 12c请使用该下面方法
            optionBuilder.UseOracle(mySqlConnectionString);


            //sql server数据库
            //optionBuilder.UseSqlServer(mySqlConnectionString);
            var context = new Entities(optionBuilder.Options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
