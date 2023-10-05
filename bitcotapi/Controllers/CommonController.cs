using Data;
using Microsoft.AspNetCore.Mvc;
using Models;
using Microsoft.EntityFrameworkCore;
using Dto;
namespace bitcotapi.Controllers;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;

[ApiController]
[Route("api/")]
[Authorize]
public class CommonController : ControllerBase
{
  private readonly UserContext _context;
  private readonly IConfiguration _configuration;
  public CommonController(UserContext context , IConfiguration configuration)
  {
    _context = context;
    _configuration = configuration;
  }


[HttpGet("profile")]
public async Task<ActionResult<ClientProfileDto>> MyProfile()
{
    string myemail = (User?.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Email)?.Value;

    var existinguser = await _context.Users.FirstOrDefaultAsync(u => u.Email == myemail);

    if (existinguser == null)
    {
        return NotFound();
    }

    var clientProfile = new ClientProfileDto
    {
        Name = existinguser.Name,
        Email = existinguser.Email,
        Address = existinguser.Address
    };

    return Ok(clientProfile);
}


[HttpPost("changepassword")]
public async Task<ActionResult<ClientProfileDto>> ChangePassword(ChangePasswordDto req)
{
    string myemail = (User?.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Email)?.Value;

    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == myemail);

    if (existingUser == null)
    {
        return NotFound();
    }

    if (!VerifyPassword(existingUser.Password, req.OldPassword))
    {
        return BadRequest("Invalid old password");
    }

    // Update password
    existingUser.Password = HashPassword(req.NewPassword);

    // Save changes to the database
    await _context.SaveChangesAsync();

    // You may return a response or a DTO here if needed
    return Ok("password changed successfully");
}

private bool VerifyPassword(string hashedPassword, string inputPassword)
{
    return BCrypt.Verify(inputPassword, hashedPassword);
}

private string HashPassword(string password)
{
    return BCrypt.HashPassword(password);
}



}


