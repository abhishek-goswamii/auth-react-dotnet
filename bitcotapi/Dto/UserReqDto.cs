using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class UserRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Address { get; set; }

    }


      public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Token { get; set; }
}    


public class CreatePermissionReqDto{

    public string PermissionName {get; set;}
    
}
public class ClientProfileDto{

    public string Name {get; set;}
    public string Email {get; set;}
    public string Address {get; set;}
    
}

public class ChangePasswordDto{
    public string OldPassword {get; set;}
    public string NewPassword {get; set;}
}

public class ForgotPasswordDto
{
    public string Email { get; set; }
}

public class ResetPasswordDto
{
    public string Password { get; set; }
}


}
