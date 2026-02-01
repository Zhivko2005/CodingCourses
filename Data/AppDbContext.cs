using System;
using Microsoft.EntityFrameworkCore;
using CodingCourses.Common.Entities; 

namespace CodingCourses.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<CourseCategory> CourseCategories { get; set; } = null!;
    public DbSet<Lesson> Lessons { get; set; } = null!;
    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<Enrollment> Enrollments { get; set; } = null!;
    public DbSet<Submission> Submissions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<CourseCategory>()
            .HasKey(cc => new { cc.CourseId, cc.CategoryId });

        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.UserId, e.CourseId });
    }
}
