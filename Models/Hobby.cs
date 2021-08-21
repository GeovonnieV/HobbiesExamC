using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace HobbiesExamC.Models
{
    public class Hobby
    {
        [Key]
        public int HobbyId {get; set;}

        [Required(ErrorMessage = "Must have a hobby name")]
        public string HobbyName {get; set;}

        [Required(ErrorMessage = "Must have a description")]
        public string HobbyDescription {get; set;}

        public int UserId {get; set;} // got to have the Id of who made this hobby
    
        public List<Enthusiast> UsersInThisHobby {get; set;} // all the Users who have this hobby


    }

}