using Data;
using Microsoft.AspNetCore.Mvc;
using Models;
using Microsoft.EntityFrameworkCore;
using Dto;
namespace bitcotapi.Controllers;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/")]
public class OthersController : ControllerBase
{
  private readonly UserContext _context;
  private readonly IConfiguration _configuration;
  public OthersController(UserContext context , IConfiguration configuration)
  {
    _context = context;
    _configuration = configuration;
  }


[Authorize(Roles = "Verify")]
[HttpGet("verify")]
public async Task<ActionResult<ClientProfileDto>> VerifyAccount()
{
    string myemail = (User?.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Email)?.Value;
    string mypassword = (User?.Identity as ClaimsIdentity)?.FindFirst("Password")?.Value;
    string myname = (User?.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Name)?.Value;

    var passwordHash = BCrypt.HashPassword(mypassword);

    var existinguser = await _context.Users.FirstOrDefaultAsync(u => u.Email == myemail);
    if (existinguser != null)
    {
        return BadRequest("User with this email already exists");
    }

    var role = await _context.Roles.FindAsync(3);

    if (role == null)
    {
        return BadRequest("Role with id 3 not present");
    }

    var newUser = new User
    {
        Name = myname,
        Email = myemail,
        Password = passwordHash,
        Address = "none",
        RoleId = 3,
        Role = role
    };

    _context.Users.Add(newUser);
    await _context.SaveChangesAsync();

    return StatusCode(200, " Account created and verified successfully");
}

[Authorize]
[HttpPost("reset-password")]
public async Task<ActionResult<ClientProfileDto>> VerifyAccount(ResetPasswordDto req)
{
    string myemail = (User?.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Email)?.Value;
    string mypassword = req.Password;

    var passwordHash = BCrypt.HashPassword(mypassword);

    var existinguser = await _context.Users.FirstOrDefaultAsync(u => u.Email == myemail);
    
    var role = await _context.Roles.FindAsync(3);

    if (role == null)
    {
        return BadRequest("Role with id 3 not present");
    }

    existinguser.Password = passwordHash;
    await _context.SaveChangesAsync();

    return StatusCode(200, " Password reset successful");
}




}


