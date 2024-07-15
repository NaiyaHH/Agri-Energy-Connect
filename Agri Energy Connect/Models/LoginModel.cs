using System.ComponentModel.DataAnnotations;

namespace Agri_Energy_Connect.Models
{
    public class LoginModel
    {//The following code was adapted from Varsity College Durban North's GitHub
        //https://github.com/PROG7311-VCDN-2024/MathApp/blob/master/MathApp/Models/LoginModel.cs
        //ebadamZA
        //https://github.com/ebadamZA
        //The following code was adapted:
        //public class LoginModel
        //{
        //    [Required]
        //    [EmailAddress]
        //    public string Email { get; set; }
        //    [Required]
        //    public string Password { get; set; }
        //}
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string UserRole { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
