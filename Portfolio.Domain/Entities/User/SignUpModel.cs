using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Domain.Entities.User
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&#])[A-Za-z\\d@$!%*?&#]{5,}$",
            ErrorMessage = "Password must be at least 5 characters long, include an uppercase letter, a lowercase letter, a number, and a special character.")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Re-enter Password is required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [MustBeTrue(ErrorMessage = "You must accept the terms and conditions.")]
        public bool AcceptTerms { get; set; }
    }
    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is bool boolValue)
            {
                return boolValue;
            }
            return false;
        }
    }

}
