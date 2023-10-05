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
using static System.Security.Claims.ClaimsIdentity;
using System.Security.Claims;

[ApiController]
[Route("api/super/")]
[Authorize(Roles = "SuperAdmin")]
public class SuperAdminController : ControllerBase
{
  private readonly UserContext _context;
  private readonly IConfiguration _configuration;
  public SuperAdminController(UserContext context , IConfiguration configuration)
  {
    _context = context;
    _configuration = configuration;
  }

  
  // [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetUser(int id)
  {
    var user = await _context.Users.FindAsync(id);

    if (user == null)
    {
      return NotFound();
    }

    return user;
  }

  [HttpGet("test")]
  public string Test()
  {
    return "Hello World!";
  }

[HttpPost("permission")]
public async Task<ActionResult<List<string>>> CreatePermission(CreatePermissionReqDto req)
{
    string myemail = (User?.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Email)?.Value;

    List<string> permissionNames = await GetMyPermissions(myemail);

    // Return the permission names
    var NewPermissionName = req.PermissionName;
    
    var allPermissionNames = await _context.Permissions.Select(p => p.Name).ToListAsync();

    if (permissionNames.Contains("root") && !allPermissionNames.Contains(NewPermissionName))
    {
        var newPermission = new Permission
        {
            Name = NewPermissionName
        };

        _context.Permissions.Add(newPermission);

        await _context.SaveChangesAsync();

        return Ok("Permission added");
    }
    else if (!permissionNames.Contains("root"))
    {
        return Unauthorized("You don't have authorization to add permission");
    }
    else
    {
        return BadRequest("Permission already exists");
    }


}

[HttpGet("permission")]
public async Task<ActionResult<List<string>>> MyPermissions()
{
    
    string myemail = (User?.Identity as ClaimsIdentity)?.FindFirst(ClaimTypes.Email)?.Value;


List<string> permissionNames = await GetMyPermissions(myemail);
return Ok(permissionNames);

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


private async Task<List<string>> GetMyPermissions(string email)
{
    var existinguser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    if (existinguser == null)
    {
        return null;
    }

    var role = await _context.Roles.FindAsync(existinguser.RoleId);

    if (role == null)
    {
        return null;
    }

    var roleName = role.Name;

    var rolePermissions = await _context.RolePermissions
        .Where(rp => rp.RoleId == role.Id)
        .ToListAsync();

    List<string> permissionNames = new List<string>();

    foreach (var rolePermission in rolePermissions)
    {
        int permissionId = rolePermission.PermissionId;

        string permissionName = await _context.Permissions
            .Where(p => p.Id == permissionId)
            .Select(p => p.Name)
            .FirstOrDefaultAsync();

        permissionNames.Add(permissionName);
    }

    return permissionNames;
}



}


