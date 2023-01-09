using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain._App {
    public class PostgreContext: DbContext {
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Account>()
                .Property(p => p.InsertedAt)
                .ValueGeneratedOnAdd()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
