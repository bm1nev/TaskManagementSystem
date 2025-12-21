using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<TaskAssignment> TaskAssignments => Set<TaskAssignment>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ConfigureUsers(modelBuilder);
        ConfigureProjects(modelBuilder);
        ConfigureTasks(modelBuilder);
        ConfigureTaskAssignments(modelBuilder);
        ConfigureProjectMembers(modelBuilder);
    }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<User>();

        e.ToTable("Users");
        
        e.HasKey(x => x.Id);
        
        e.Property(x => x.Id)
            .IsRequired()
            .HasMaxLength(256);
        
        e.HasIndex(x => x.Email)
            .IsUnique();
        
        e.Property(x => x.PasswordHash)
            .IsRequired()
            .HasMaxLength(512);

        e.Property(x => x.IsActive)
            .IsRequired();

        e.Property(x => x.CreatedAtUtc)
            .IsRequired();
    }

    private static void ConfigureProjects(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<Project>();
        
        e.ToTable("Projects");
        
        e.HasKey(x => x.Id);
        
        e.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);
        
        e.Property(x => x.Description)
            .HasMaxLength(2500);

        e.Property(x => x.OwnerId)
            .IsRequired();

        e.Property(x => x.CreatedAtUtc)
            .IsRequired();
        
        e.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasIndex(x => x.OwnerId);
    }

    private static void ConfigureTasks(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<TaskItem>();
        
        e.ToTable("Tasks");
        
        e.HasKey(x => x.Id);

        e.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(256);
        
        e.Property(x => x.Description)
            .HasMaxLength(5000);

        e.Property(x => x.Status)
            .IsRequired();
        
        e.Property(x => x.ProjectId)
            .IsRequired();

        e.Property(x => x.DueDateUtc);
        
        e.Property(x => x.CreatedAtUtc)
            .IsRequired();
        
        e.HasOne<Project>()
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        e.HasIndex(x => x.ProjectId);
        e.HasIndex(x => x.Status);
    }

    private static void ConfigureTaskAssignments(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<TaskAssignment>();
        
        e.ToTable("TaskAssignments");
        
        e.HasKey(x => x.Id);
        
        e.Property(x => x.TaskId)
            .IsRequired();

        e.Property(x => x.UserId)
            .IsRequired();
        
        e.Property(x=> x.AssignedAtUtc)
            .IsRequired();
        
        e.Property(x=> x.CreatedAtUtc)
            .IsRequired();
        
        e.HasOne<TaskItem>()
            .WithMany()
            .HasForeignKey(x => x.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
        
        e.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        e.HasIndex(x => new { x.TaskId, x.UserId })
            .IsUnique();
        
        e.HasIndex(x=> x.UserId);
    }
    
    private static void ConfigureProjectMembers(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<ProjectMember>();
        
        e.ToTable("ProjectMembers");
        
        e.HasKey(x => x.Id);
        
        e.Property(x => x.ProjectId)
            .IsRequired();
        
        e.Property(x => x.UserId)
            .IsRequired();
        
        e.Property(x => x.Role)
            .IsRequired();

        e.Property(x => x.InvitedByUserId);
        
        e.Property(x => x.Note)
            .HasMaxLength(1024);
        
        e.Property(x => x.CreatedAtUtc)
            .IsRequired();
        
        e.HasOne<Project>()
            .WithMany()
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        e.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        e.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.InvitedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        e.HasIndex( x => new { x.ProjectId, x.UserId } )
            .IsUnique();

        e.HasIndex(x => x.UserId);
    }
}

