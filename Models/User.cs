using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace HobbiesExamC.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "FirstName must be 2 characters or longer!")]
        public string FirstName { get; set; }
        
        [Required]
        [MinLength(2,  ErrorMessage = "LastName must be 2 characters or longer!")]
        public string LastName { get; set; }

        [Required]
        [MinLength(3,  ErrorMessage = "UserName must be 3-15 characters!")]
        [MaxLength(15,  ErrorMessage = "UserName must be 3-15 characters!")]
        public string UserName {get; set;} // using this instead of email

        [Required]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
        public string Password { get; set; }


        // Will not be mapped to your users table!
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
        
        public List<Enthusiast> UsersHobbies {get; set;} // all the hobbies this user has


    }

}