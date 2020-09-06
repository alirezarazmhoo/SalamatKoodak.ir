using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SalamatKoodak.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public string DecriptPass { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
      
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, configure>());
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<PersonelStatus> PersonelStatuses { get; set; }
        public DbSet<RelationType> RelationTypes { get; set; }
        public DbSet<Personels> Personels { get; set; }
        class configure : System.Data.Entity.Migrations.DbMigrationsConfiguration<ApplicationDbContext>
        {
            public configure()
            {
                this.AutomaticMigrationsEnabled = true;
                this.AutomaticMigrationDataLossAllowed = true;
            }
            protected override void Seed(SalamatKoodak.Models.ApplicationDbContext context)
            {
                if (!context.Cities.Any())
                {
                    context.Cities.AddOrUpdate(
                        p => p.Id, new City { Id = 1, Name = "اصفهان" },
                        new City { Id = 2, Name = "آذربایجان شرقی" },
                        new City { Id = 3, Name = "آذربایجان غربی" },
                        new City { Id = 4, Name = "اردبیل" },
                        new City { Id = 5, Name = "البرز" },
                        new City { Id = 6, Name = "ایلام" },
                        new City { Id = 7, Name = "بوشهر" },
                        new City { Id = 8, Name = "تهران" },
                        new City { Id = 9, Name = "چهارمحال و بختیاری" },
                        new City { Id = 10, Name = "خراسان جنوبی" },
                        new City { Id = 11, Name = "خراسان رضوی" },
                        new City { Id = 12, Name = "خراسان شمالی" },
                        new City { Id = 13, Name = "خوزستان" },
                        new City { Id = 14, Name = "زنجان" },
                        new City { Id = 15, Name = "سمنان" },
                        new City { Id = 16, Name = "سیستان و بلوچستان" },
                        new City { Id = 17, Name = "فارس" },
                        new City { Id = 18, Name = "قزوین" },
                        new City { Id = 19, Name = "قم" },
                        new City { Id = 20, Name = "کردستان" },
                        new City { Id = 21, Name = "کرمان" },
                        new City { Id = 22, Name = "کرمانشاه" },
                        new City { Id = 23, Name = "کهگیلویه و بویراحمد" },
                        new City { Id = 24, Name = "گلستان" },
                        new City { Id = 25, Name = "گیلان" },
                        new City { Id = 26, Name = "لرستان" },
                        new City { Id = 27, Name = "مازندران" },
                        new City { Id = 28, Name = "مرکزی" },
                        new City { Id = 29, Name = "هرمزگان" },
                        new City { Id = 30, Name = "همدان" },
                        new City { Id = 31, Name = "یزد" },
                        new City { Id = 32, Name = "NoCity" }

                        );
                }
                if (!context.Roles.Any())
                {
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                    string roleName = "Normal";

                    IdentityResult roleResult;
                    if (!RoleManager.RoleExists(roleName))
                    {
                        roleResult = RoleManager.Create(new IdentityRole(roleName));
                    }
                     roleName = "Admin";
                    if (!RoleManager.RoleExists(roleName))
                    {
                        roleResult = RoleManager.Create(new IdentityRole(roleName));
                    }
                }
                if (!context.Users.Any())
                {
                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = new ApplicationUser { UserName ="Admin", Name = "", LastName = "مدیریت کل", NationalCode ="1111111111", Email = "Admin@yahoo.com", DecriptPass = "", CityId = 32 };
                    manager.Create(user, "854921");
                    manager.AddToRole(user.Id, "Admin");
                }
                if (!context.RelationTypes.Any())
                {
                    context.RelationTypes.AddOrUpdate(
                       p => p.Id, new RelationType { Id = 1, Name = "آموزش" },
                       new RelationType { Id = 2, Name = "هیعت" },
                       new RelationType { Id = 3, Name = "فعالیت های تفریحی و ورزشی" },
                       new RelationType { Id = 4, Name = "فعالیت های جهادی " },
                       new RelationType { Id = 5, Name = "فراخوان عمومی" },
                       new RelationType { Id = 6, Name = "شناسایی چهره به چهره" },
                       new RelationType { Id = 7, Name = "عملیات" }
                       );
                }
                if (!context.PersonelStatuses.Any())
                {
                    context.PersonelStatuses.AddOrUpdate(
                       p => p.Id, new PersonelStatus { Id = 1, Name = "راهبر" },
                       new PersonelStatus { Id = 2, Name = "فعال" },
                       new PersonelStatus { Id = 3, Name = "نیمه فعال" },
                       new PersonelStatus { Id = 4, Name = "عادی" },
                       new PersonelStatus { Id = 5, Name = "متعهد و علاقه مند" }
                       );
                }
            }
        }
    }
}