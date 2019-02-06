using Microsoft.EntityFrameworkCore;
using Prakrishta.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Prakrishta.Data.Sample
{
    public class DatabaseContext : DbContext
    {
        private string connectionString = $"Data Source = (LocalDB)\\MSSQLLocalDB;AttachDbFilename={MDFDirectory}\\TestDatabase.mdf;Integrated Security=True;Connect Timeout=30";


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static string MDFDirectory
        {
            get
            {
                var directoryPath = AppDomain.CurrentDomain.BaseDirectory;
                return Path.GetFullPath(Path.Combine(directoryPath, "..//..//..//DataFiles"));
            }
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
