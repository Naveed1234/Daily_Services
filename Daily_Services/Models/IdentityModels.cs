using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Daily_Services.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Phone_Number { get; set; }

        public Nullable<System.DateTime> DateOfBirth { get; set; }

        public int? Gender_Id { get; set; }
        public int? Category_Id { get; set; }
        public int? SubCategory_Id { get; set; }
        public int? Qualification_Id { get; set; }
        public string Image_Path { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

        public string Postal_Code { get; set; }
        public string CNIC_No { get; set; }
        public bool ServiceOffer { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("Category_Id")]
        public virtual Category Category { get; set; }
        [ForeignKey("Gender_Id")]
        public virtual Gender Gender { get; set; }
        [ForeignKey("Qualification_Id")]
        public virtual Qualification Qualification { get; set; }
        [ForeignKey("SubCategory_Id")]
        public virtual SubCategory SubCategory { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }


    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string RoleName) : base(RoleName) { }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> GetSubCategories { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Daily_Services.Models.RoleViewModel> RoleViewModels { get; set; }

        //public System.Data.Entity.DbSet<Daily_Services.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}
