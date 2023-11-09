using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class UserTokenDTO
    {
        public string? Token { get; set; }
        public DateTime? ExpiresIn { get; set; }
    }

    public class UserAccessDTO
    {
        [Display(Name = "Email")]
        [EmailAddress]
        [Required(ErrorMessage = "The field {0} is mandatory")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The field {0} must be between {2} and {1} characters")]
        public string Email { get; set; } = null!;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "The field {0} is mandatory")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The field {0} must be between {2} and {1} characters")]
        public string Password { get; set; } = null!;
    }

    public class UserSignUpEditorDTO : UserAccessDTO
    {
        [Display(Name = "First name")]
        [Required(ErrorMessage = "The field {0} is mandatory")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "The field {0} must be between {2} and {1} characters")]
        public string? FirstName { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "The field {0} is mandatory")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "The field {0} must be between {2} and {1} characters")]
        public string? LastName { get; set; }
    }

    public class UserLogInEditorDTO : UserAccessDTO
    {
    }

    public class UserChangePasswordEditorDTO
    {
        [Display(Name = "Current passwoed")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field {0} is mandatory")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "This field must be between {2} and {1} characters")]
        public string CurrentPassword { get; set; } = null!;

        [Display(Name = "New password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This field {0} is mandatory")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "This field must be between {2} and {1} characters")]
        public string NewPassword { get; set; } = null!;
    }
}
