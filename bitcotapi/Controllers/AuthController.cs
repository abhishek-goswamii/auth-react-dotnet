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
using Microsoft.AspNetCore.Identity;
using BCrypt.Net;
using MyServices;

[ApiController]
[Route("api/")]
public class AuthController : ControllerBase
{
  private readonly IEmailSender _emailSender;
  private readonly UserContext _context;
  private readonly IConfiguration _configuration;

  public AuthController(UserContext context, IConfiguration configuration, IEmailSender emailSender)
{
    _context = context;
    _configuration = configuration;
    _emailSender = emailSender;
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


  
  [HttpPost("register")]
  public async Task<ActionResult<User>> RegisterUser(UserRequestDto user)
  {
    var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

    if (existingUser != null)
    {
        return BadRequest("User with the same email already exists");
    }

    var role = await _context.Roles.FindAsync(3);

    if (role == null)
    {
        return BadRequest("Role with id 3 not present");
    }
    
    // var passwordHash = BCrypt.HashPassword(user.Password);

    // var newUser = new User
    // {
    //     Name = user.Name,
    //     Email = user.Email,
    //     Password = passwordHash,
    //     Address = user.Address,
    //     RoleId = 3,
    //     Role = role

    // };

    // _context.Users.Add(newUser);
    // await _context.SaveChangesAsync();

    var token = CreateTokenForVerification(user.Name,user.Email,user.Password);

    var receiver = user.Email;
    var subject = "Test Email";
    var message = "< href='http://localhost:5173/verify?token=" + token + "'>Click here to verify your account";
    
    await _emailSender.SendEmailAsync(receiver,subject,message);
    
    return StatusCode(200, " a verification link is sent to your email please click on it to verify");
    // return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, new { newUser.Id, newUser.Name, newUser.Email });
  }



  


    [HttpPost("login")]
  public async Task<ActionResult<UserLoginResponseDto>> LoginUser(UserLoginDto user)
    {
        try
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser == null || !BCrypt.Verify(user.Password, existingUser.Password))
            {
                return BadRequest("Invalid email or password");
            }

            if (existingUser == null)
            {
                return BadRequest("Invalid email or password");
            }

          
          string role = "";

          if(existingUser.Email == "admin@admin.com"){
          
          role = "Admin";

          }else if(existingUser.Email == "superadmin@superadmin.com"){

          role = "SuperAdmin";
          
          }else{
          
          role = "Client";

          }

          var token = CreateToken(existingUser , role);

            return new UserLoginResponseDto
            {
                Id = existingUser.Id,
                Name = existingUser.Name,
                Email = existingUser.Email,
                Address = existingUser.Address,
                Token = token
            };
        }
        catch (Exception ex)
        {

           Console.WriteLine(ex.ToString());
          return StatusCode(500, $"An error occurred while logging in: {ex.Message}");
        }
    }




[HttpPost("forgot-password")]
public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
{
    try
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);

        if (user == null)
        {
            return BadRequest("User with the provided email does not exist");
        }

        var token = CreateToken(user,"Client");

        var receiver = user.Email;
        var subject = "Password Reset";
        var message = $"<a href='http://localhost:5173/reset-password?token={token}'>Click here to reset your password</a>";

        await _emailSender.SendEmailAsync(receiver, subject, message);

        return StatusCode(200, "A password reset link has been sent to your email. Please check your inbox.");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
        return StatusCode(500, $"An error occurred while processing the forgot password request: {ex.Message}");
    }
}











  private string CreateToken(User user ,  string role)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

  private string CreateTokenForVerification(string name , string email , string password)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Email, email),
                new Claim("Password", password),
                new Claim(ClaimTypes.Name, name ),
                new Claim(ClaimTypes.Role, "Verify")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    








}

