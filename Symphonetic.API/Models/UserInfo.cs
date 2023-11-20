using System.ComponentModel.DataAnnotations;
namespace Symphonetic.API.Models;

public record UserInfo(string FirstName, string LastName, DateOnly DateOfBirth)
{
    [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
}
