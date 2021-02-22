using Attendance50.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attendance50.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Student> Students { set; get; }
        public DbSet<Group> Groups { set; get; }
        public DbSet<Check> Checks { set; get; }
        public DbSet<CheckStudent> CheckStudents { set; get; }
        public DbSet<FlowStudent> FlowStudents { set; get; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Составные ключи
            modelBuilder.Entity<CheckStudent>()
                .HasKey(e => new { e.CheckId, e.StudentId });
            modelBuilder.Entity<FlowStudent>()
                .HasKey(e => new { e.FlowId, e.StudentId });
        }

    }
}
