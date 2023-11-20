using Microsoft.AspNetCore.Identity;
namespace Symphonetic.API.Models;

public class ApplicationUser(string UserName) : IdentityUser(UserName)
{
    public UserInfo? Info { get; set; }
}
