using Microsoft.AspNetCore.Identity;
namespace Symphonetic.API.Models;

public class ApplicationUser(string UserName, string Email, UserInfo? Info) : IdentityUser(UserName)
{
    
}
