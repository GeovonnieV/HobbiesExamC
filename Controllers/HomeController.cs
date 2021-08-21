using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HobbiesExamC.Models;
// Other using statements
namespace HobbiesExamC.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            _context = context;
        }

        // Registration Page
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // show login page
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            // get the user Id in session
            int? userId = HttpContext.Session.GetInt32("userId");

            // grab the current user
            var UserInDb = _context.Users.FirstOrDefault(user => user.UserId == userId);
            ViewBag.SpecificUser = UserInDb;

            // Get all the Hobbies in the Hobbies table
            ViewBag.AllHobbies = _context.Hobbies
                .Include(hobby => hobby.UsersInThisHobby);

            // 


            return View();
        }

        // Create hobby display 
        [HttpGet("CreateHobby")]
        public IActionResult CreateHobby()
        {
            // need the logged in users Id for the hobby 
            // get the user Id in session
            // grab the current user
            ViewBag.LoggedInUser = (int)HttpContext.Session.GetInt32("userId");

            return View();
        }


        // One Hobby display
        [HttpGet("OneHobby/{hobbyId}")]
        public IActionResult OneHobby(int hobbyId)
        {
            // gets the selected hobby from DB
            ViewBag.SpecificHobby = _context.Hobbies
                .FirstOrDefault(hobby => hobby.HobbyId == hobbyId);

            // users that have this hobby
            // give me the users
            ViewBag.UsersThatLikeThisHobby = _context.Users
                .Include(user => user.UsersHobbies)
                // where the HobbyId in usersHobbies == to the hobbyID
                .Where(user => user.UsersHobbies.Any(u => u.HobbyId == hobbyId)).ToList();

            // Need the users ID 
            ViewBag.LoggedInUser = (int)HttpContext.Session.GetInt32("userId");

            return View();
        }




        // Post Actions

        // Register Post
        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser(User user)
        {
            // Check initial ModelState
            if (ModelState.IsValid)
            {
                // If a User exists with provided userName
                if (_context.Users.Any(u => u.UserName == user.UserName))
                {
                    // Manually add a ModelState error to the Username field, with provided
                    // error message
                    ModelState.AddModelError("UserName", "Username already in use!");

                    // You may consider returning to the View at this point
                    return View("Index");
                }
                // if everything is okay save the user to the DB
                // Initializing a PasswordHasher object, providing our User class as its type
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            // other code
            return View("Index");
        }

        // Login Post 
        [HttpPost("LoginPost")]
        public IActionResult LoginPost(LoginUser userSubmission)
        {
            if (ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = _context.Users.FirstOrDefault(u => u.UserName == userSubmission.UserName);
                // If no user exists with provided email
                if (userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("UserName", "Invalid UserName");
                    return View("Login");
                }

                // Initialize hasher object
                var hasher = new PasswordHasher<LoginUser>();

                // verify provided password against hash stored in db
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

                // result can be compared to 0 for failure
                if (result == 0)
                {
                    // handle failure (this should be similar to how "existing email" is handled)
                    ModelState.AddModelError("Password", "Not the right password cops are being called!");
                    return View("Login");
                }
                // assign user ID to sessions userId
                HttpContext.Session.SetInt32("userId", userInDb.UserId);
                // If everything is good go to the Dashboard view page 
                // pass in the user we found in the db into it
                return RedirectToAction("Dashboard");
            }
            // go back to login if fails
            return View("Login");
        }

        // Create hobby post 
        [HttpPost("CreateHobbyPost")]
        public IActionResult CreateHobbyPost(Hobby newHobby)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newHobby);
                _context.SaveChanges();
                // go to dashboard to see all hobbies 
                return RedirectToAction("Dashboard");

            }
            else
            {

                ViewBag.LoggedInUser = (int)HttpContext.Session.GetInt32("userId");
            
                return View("CreateHobby");
            }

        }

        // Adding a user to our Hobbies list
        [HttpPost("AddUserToHobby")]
        public IActionResult AddUserToHobby(Enthusiast newEnthusiast)
        {
            if (ModelState.IsValid)
            {
                // check if this user is already in this hobbies liked users
                //     var IsUserInhobby = _context.Users
                //    .Include(user => user.UsersHobbies)
                //    .Where(user => user.UsersHobbies.All(u => u.UserId == newEnthusiast.UserId));
                // if the user does not have this hobby liked we can add it 
                // if (IsUserInhobby == null)
                // {
                _context.Enthusiasts.Add(newEnthusiast);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");

                // }

            }
            return RedirectToAction("Dashboard");
        }


    }
}