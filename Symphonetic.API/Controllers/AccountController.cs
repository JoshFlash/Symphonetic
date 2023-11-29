using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Symphonetic.API.Controllers;

using Models;

[Route("api/[controller]")]
[ApiController]
public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,  IConfiguration Config) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterModel model)
    {
        var user = new ApplicationUser(model.UserName)
        {
            Email = model.Email
        };

        var result = await userManager.CreateAsync(user, model.Password);

        return result.Succeeded ? Ok(new { message = "User registered successfully" }) : BadRequest(result.Errors);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginModel model)
    {
        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (!result.Succeeded) return Unauthorized();
        
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user is null) return Unauthorized();
        
        var tokenString = GenerateJwtToken(user);
        return Ok(new { token = tokenString, message = "Login successful" });
    }
    
    private string GenerateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtKey = Config["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
            throw new InvalidOperationException("JWT Key is not configured correctly.");
        
        var key = Encoding.ASCII.GetBytes(jwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Email, user.Email ?? "NONE")
                // Add more claims as required
            }),
            Expires = DateTime.UtcNow.AddDays(7), // Token expiration
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}