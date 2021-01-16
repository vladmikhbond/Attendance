using Attendance.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attendance.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Student> Students { set; get; }
        public DbSet<Group> Groups { set; get; }
        public DbSet<Meet> Meets { set; get; }
        public DbSet<MeetStudent> MeetStudents { set; get; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Составной первичный ключ
            modelBuilder.Entity<MeetStudent>()
                .HasKey(e => new { e.MeetId, e.StudentId });            
        }
    }
}