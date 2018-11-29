﻿using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DAL.Models.Interfaces;

namespace DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public string CurrentUserId { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<DocumentCategory> DocumentCategories { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Eviction> Evictions { get; set; }
        public DbSet<EvictionDetail> EvictionDetails { get; set; }
        public DbSet<Agent> Agents { get; set; }


        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasMany(u => u.Claims).WithOne().HasForeignKey(c => c.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationUser>().HasMany(u => u.Roles).WithOne().HasForeignKey(r => r.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationRole>().HasMany(r => r.Claims).WithOne().HasForeignKey(c => c.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ApplicationRole>().HasMany(r => r.Users).WithOne().HasForeignKey(r => r.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Customer>().Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Customer>().HasIndex(c => c.Name);
            builder.Entity<Customer>().Property(c => c.Email).HasMaxLength(100);
            builder.Entity<Customer>().Property(c => c.PhoneNumber).IsUnicode(false).HasMaxLength(30);
            builder.Entity<Customer>().Property(c => c.City).HasMaxLength(50);
            builder.Entity<Customer>().ToTable($"App{nameof(this.Customers)}");

            builder.Entity<DocumentCategory>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Entity<DocumentCategory>().Property(p => p.Description).HasMaxLength(500);
            builder.Entity<DocumentCategory>().ToTable($"App{nameof(this.DocumentCategories)}");

            builder.Entity<Document>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Document>().HasIndex(p => p.Name);
            builder.Entity<Document>().Property(p => p.Description).HasMaxLength(500);
            builder.Entity<Document>().Property(p => p.Icon).IsUnicode(false).HasMaxLength(256);
            builder.Entity<Document>().HasOne(p => p.Parent).WithMany(p => p.Children).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Document>().ToTable($"App{nameof(this.Documents)}");

            builder.Entity<Eviction>().Property(o => o.Comments).HasMaxLength(500);
            builder.Entity<Eviction>().ToTable($"App{nameof(this.Evictions)}");

            builder.Entity<Agent>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Entity<Agent>().HasIndex(p => p.Name);
            builder.Entity<Agent>().ToTable($"App{nameof(this.Agents)}");

            builder.Entity<EvictionDetail>().ToTable($"App{nameof(this.EvictionDetails)}");
        }




        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                DateTime now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUserId;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUserId;
            }
        }
    }
}
