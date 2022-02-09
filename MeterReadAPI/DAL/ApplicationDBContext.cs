using System;
using MeterReadAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadAPI.DAL

{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<CustomerAccount> CustomerAccount { get; set; }
        public DbSet<MeterReadings> MeterReadings { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory;
            var connectionStringBuilder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder { DataSource = path + "MeterAppDB.db" };           
            var connectionString = connectionStringBuilder.ToString();
            var connection = new Microsoft.Data.Sqlite.SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }

       
      
    }
}