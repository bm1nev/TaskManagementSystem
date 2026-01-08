using TaskManagementSystem.Domain.Common;

namespace TaskManagementSystem.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    
    public string? Description { get; private set; }
    
    public Guid OwnerId { get; private set; }
    
    private Project() { }

    public Project(string name, Guid ownerId, string? description = null)
    {
        SetName(name);
        
        if (ownerId == Guid.Empty)
            throw new ArgumentException("OwnerId is required.", nameof(ownerId));
        
        OwnerId = ownerId;
        Description = NormalizeOptional(description);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name is required.", nameof(name));
        
        Name = name.Trim();
    }

    public void SetDescription(string? description)
    {
        Description = NormalizeOptional(description);
    }

    private static string? NormalizeOptional(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        
        return value.Trim();
    }

    public void Update(string name, string? description)
    {
        name = (name ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Project name is required.", nameof(name));
        
        if (name.Length > 200)
            throw new ArgumentException("Project name is too long.", nameof(name));

        Name = name;

        if (string.IsNullOrWhiteSpace(description))
        {
            Description = null;
        }
        else
        {
            description = description.Trim();
            if (description.Length > 2000)
                throw  new ArgumentException("Project description is too long.", nameof(description));
            
            Description = description;
        }
    }
    
}
