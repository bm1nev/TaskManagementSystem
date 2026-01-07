using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Infrastructure.Persistence;

namespace TaskManagementSystem.Infrastructure.Seeding;

public static class DbSeeder
{
    private static readonly Guid AdminUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid DemoProjectId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid DemoProjectOwnerMembershipId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    public static async Task SeedAsync(
        AppDbContext db,
        IPasswordHasher passwordHasher)
    {
        // ADMIN USER
        var admin = await db.Users
            .FirstOrDefaultAsync(u => u.Email == "admin@tms.local");

        if (admin is null)
        {
            admin = new User(
                email: "admin@tms.local",
                passwordHash: passwordHasher.Hash("admin123!"),
                role: UserRole.Admin
            );

            admin.Id = AdminUserId;
            admin.Activate();

            db.Users.Add(admin);
            await db.SaveChangesAsync();
        }
        else
        {
            if (admin.Role != UserRole.Admin)
                admin.ChangerRole(UserRole.Admin);

            if (!admin.IsActive)
                admin.Activate();

            admin.SetPasswordHash(passwordHasher.Hash("admin123!"));

            await db.SaveChangesAsync();
        }

        // DEMO PROJECT
        var demoProject = await db.Projects
            .FirstOrDefaultAsync(p => p.Id == DemoProjectId);

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

        // OWNER MEMBERSHIP
        var ownerMembershipExists = await db.ProjectMembers.AnyAsync(pm =>
            pm.ProjectId == demoProject.Id &&
            pm.UserId == admin.Id);

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
