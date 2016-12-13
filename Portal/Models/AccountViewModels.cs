using System.ComponentModel.DataAnnotations;

namespace Portal.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display( Name = "Email" )]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType( DataType.Password )]
        [Display( Name = "Current password" )]
        public string OldPassword { get; set; }

        [Required]
        [StringLength( 100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6 )]
        [DataType( DataType.Password )]
        [Display( Name = "New password" )]
        public string NewPassword { get; set; }

        [DataType( DataType.Password )]
        [Display( Name = "Confirm new password" )]
        [Compare( "NewPassword", ErrorMessage = "The new password and confirmation password do not match." )]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [DataType( DataType.EmailAddress )]
        [Display( Name = "Email" )]
        [EmailAddress( ErrorMessage = "E-mail address has incorrect format" )]
        public string UserName { get; set; }

        [Required]
        [DataType( DataType.Password )]
        [Display( Name = "Password" )]
        public string Password { get; set; }

        [Display( Name = "Remember me?" )]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [DataType( DataType.EmailAddress )]
        [Display( Name = "Email" )]
        [EmailAddress( ErrorMessage = "E-mail address has incorrect format" )]
        public string UserName { get; set; }

        [Required]
        [StringLength( 100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6 )]
        [DataType( DataType.Password )]
        [Display( Name = "Password" )]
        public string Password { get; set; }

        [DataType( DataType.Password )]
        [Display( Name = "Confirm password" )]
        [Compare( "Password", ErrorMessage = "The password and confirmation password do not match." )]
        public string ConfirmPassword { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be from {2} to {1} characters long", MinimumLength = 2)]
        [Display( Name = "Name" )]
        public string First_Name { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be from {2} to {1} characters long", MinimumLength = 2)]
        [Display( Name = "Surname" )]
        public string Second_Name { get; set; }

        [StringLength(20, ErrorMessage = "The {0} cannot be more than {1} characters long.")]
        [Display( Name = "Patronymic" )]
        public string Middle_Name { get; set; }

        [DataType( DataType.PhoneNumber )]
        public string PhoneNumber { get; set; }

        [Required]
        [Display( Name = "User Status" )]
        public string Person_Type { get; set; }
        public virtual Picture Picture { get; set; }
    }
}
