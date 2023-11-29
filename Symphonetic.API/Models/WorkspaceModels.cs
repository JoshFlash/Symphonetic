using System.ComponentModel.DataAnnotations;
namespace Symphonetic.API.Models;

public record Project(string Name, string Description, DateOnly CreationDate)
{
    [Key] public Guid Id { get; set; } = new();

    public ApplicationUser? Owner { get; set; }

    public ICollection<Ticket> Tickets { get; } = new List<Ticket>();
}

public record Ticket(string ProjectName)
{
    [Key] public Guid Id { get; set; } = new();
}