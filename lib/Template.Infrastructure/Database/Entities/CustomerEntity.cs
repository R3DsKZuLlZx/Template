namespace Template.Infrastructure.Database.Entities;

public class CustomerEntity
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }
}
