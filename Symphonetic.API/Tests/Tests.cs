using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Symphonetic.API.Data;
using Symphonetic.API.Models;

namespace Symphonetic.API.Tests;

public static class Tests
{
    public static class Jwt
    {
        public static void TestValidation(IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "TestUser"),
                    // Add other claims as needed
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            Console.WriteLine($"Generated Token: {tokenString}");

            // Validate Token
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                tokenHandler.ValidateToken(tokenString, validationParameters, out _);
                Console.WriteLine("Token is valid.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
            }
        }
    }

    public static class SqlServer
    {
        public static async Task TestDatabaseOperations(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Create
            var newEntity = new UserInfo( "Test", "Name", new DateOnly(1999, 09, 09) );
            context.UserInfos.Add(newEntity);
            await context.SaveChangesAsync();
            Console.WriteLine($"Created new entity with ID: {newEntity.Id}");

            // Read
            var readEntity = await context.UserInfos.FindAsync(newEntity.Id);
            if (readEntity is null) throw new Exception("Failed to find new Entity");
                
            Console.WriteLine($"Read entity with " +
                              $"ID: {readEntity.Id}, " +
                              $"Name: {readEntity.FirstName} {readEntity.LastName}, " +
                              $"DoB: {readEntity.DateOfBirth}");

            // Update
            context.Entry(readEntity).State = EntityState.Detached;
            
            var updatedEntity = readEntity with { FirstName = "Updated Name" };
            context.UserInfos.Update(updatedEntity);
            await context.SaveChangesAsync();
            Console.WriteLine($"Updated entity with ID: {updatedEntity.Id}, First Name: {updatedEntity.FirstName}");

            // Delete
            context.Entry(updatedEntity).State = EntityState.Detached;
            
            context.UserInfos.Remove(updatedEntity);
            await context.SaveChangesAsync();
            Console.WriteLine($"Deleted entity with ID: {updatedEntity.Id}");
        }

    }
    
}

