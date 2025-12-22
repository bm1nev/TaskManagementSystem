using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Infrastructure.Persistence;

namespace TaskManagementSystem.Infrastructure.Seeding;

public static class DbSeeder
{
    private static readonly Guid AdminUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid DemoProjectId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid DemoProjectOwnerMembershipId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    public static async Task SeedAsync(AppDbContext db)
    {
        // Admin user
        var admin = await db.Users.FirstOrDefaultAsync(u => u.Email == "admin@tms.local".Trim().ToLowerInvariant());
        if (admin is null)
        {
            var hasher = new PasswordHasher<User>();

            admin = new User(email: "admin@tms.local", passwordHash: "TEMP_HASH", role: UserRole.Admin);

            var hashed = hasher.HashPassword(admin, "аdmin123!");
            admin.SetPasswordHash(hashed);
            
            admin.Id = AdminUserId;

            db.Users.Add(admin);
            await db.SaveChangesAsync();
        }
        else
        {
            if (string.IsNullOrWhiteSpace(admin.Email))
                admin.SetEmail(admin.Email);
            
            if (admin.Role != UserRole.Admin)
                admin.ChangerRole(UserRole.Admin);
            
            if (!admin.IsActive)
                admin.Activate();

            if (string.IsNullOrWhiteSpace(admin.PasswordHash))
            {
                var hasher = new PasswordHasher<User>();
                var hashed = hasher.HashPassword(admin, "admin123!");
                admin.SetPasswordHash(hashed);
            }
            
            await db.SaveChangesAsync();
        }

        // Demo Project (owner = admin)
        var demoProject = await db.Projects.FirstOrDefaultAsync(p => p.Id == DemoProjectId);
        if (demoProject is null)
        {
            demoProject = new Project(
                name: "Demo Project",
                ownerId: admin.Id,
                description: "Seeded demo project for development/testing."
            );

            demoProject.Id = DemoProjectId;

            db.Projects.Add(demoProject);
            await db.SaveChangesAsync();
        }

        // Owner membership (ProjectMember with Role = Owner)
        var ownerMembershipExists = await db.ProjectMembers.AnyAsync(pm =>
            pm.ProjectId == demoProject.Id && pm.UserId == admin.Id);

        if (!ownerMembershipExists)
        {
            var ownerMembership = new ProjectMember(
                projectId: demoProject.Id,
                userId: admin.Id,
                role: ProjectRole.Owner,
                invitedByUserId: null,
                note: "Seeded owner membership"
            );

            ownerMembership.Id = DemoProjectOwnerMembershipId;

            db.ProjectMembers.Add(ownerMembership);
            await db.SaveChangesAsync();
        }
    }
}
