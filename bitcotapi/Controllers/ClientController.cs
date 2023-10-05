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
[Route("api/client/")]
[Authorize(Roles = "Client")]
public class ClientController : ControllerBase
{
  private readonly UserContext _context;
  private readonly IConfiguration _configuration;
  public ClientController(UserContext context , IConfiguration configuration)
  {
    _context = context;
    _configuration = configuration;
  }


[HttpGet("")]
public async Task<ActionResult<List<string>>> Home()
{
        return Ok("Hello from client");
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


}


