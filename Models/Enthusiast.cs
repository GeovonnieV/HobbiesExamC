using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace HobbiesExamC.Models
{
    public class Enthusiast
    {
        [Key]
        public int EnthusiastId {get; set;}

        [Required]
        public int HobbyId {get; set;}

        [Required]
        public int UserId {get; set;}

        public User User {get; set;}

        public Hobby Hobby {get; set;}                 
    


    }

}