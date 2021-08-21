    using Microsoft.EntityFrameworkCore;
	namespace HobbiesExamC.Models
	{ 
   	 // the MyContext class representing a session with our MySQL 
   	 // database allowing us to query for or save data
    public class MyContext : DbContext 
   	 { 
        public MyContext(DbContextOptions options) : base(options) { }
        // all my tables
        public DbSet<User> Users { get; set; }
        public DbSet<Hobby> Hobbies { get; set; }
        public DbSet<Enthusiast> Enthusiasts { get; set; }
        
   	 }
	}