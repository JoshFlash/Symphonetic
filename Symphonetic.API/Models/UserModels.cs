using System.ComponentModel.DataAnnotations;
namespace Symphonetic.API.Models;

public record UserInfo(string FirstName, string LastName, DateOnly DateOfBirth)
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
}

public record RegisterModel(string UserName, string Email, string Password);

public record LoginModel(string Email, string Password);