using SalamatKoodak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SalamatKoodak.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index(int? page, string searchString,int? CityId)
        {
            //492cc8fa - 468b - 457a - 86a4 - adc4de1344d1
            List<ApplicationUser> users =await db.Users.Where(p=>p.Roles.Select(x=>x.RoleId).Contains("3054772e-bacc-4a9f-bc8e-c47cc44bfed1")).Include(s => s.City).ToListAsync();

            if (!String.IsNullOrEmpty(searchString) || CityId !=null)
            {
                if (CityId != null)
                {
                   users = users.Where(s => s.CityId == CityId).ToList();
                    ViewBag.Count = users.Count;
                }
                else
                {
                 users = users.Where(s => s.Name.Contains(searchString) ||
                 s.LastName.Contains(searchString) ||
                 s.UserName.Contains(searchString)).ToList();
                 ViewBag.Count = users.Count;
                }
            }
            else
            {
              page = 1;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.Count = users.Count;
            return View(users.OrderBy(s=>s.Id).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public async Task<JsonResult> LoadCityes()
        {
            List<City> cities =await db.Cities.Where(s=>s.Id !=32).ToListAsync();
            return Json(new { success = true  , list = cities } , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task< JsonResult> Remove(string UserId)
        {
            ApplicationUser user = db.Users.Find(UserId);
            if(user == null)
            {
                return Json(new { success = false, responseText = "کاربرمورد نظر یافت نشد" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if(await db.Personels.AnyAsync(s=> s.ApplicationUserId == user.Id))
                {

                    return Json(new { success = false, responseText = "اطلاعات این کاربر در سیستم ثبت شده و قابل حذف نمی باشد" }, JsonRequestBehavior.AllowGet);

                }
                db.Users.Remove(user);
       await     db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "سیستم قادر به پاسخ گویی نیست" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public  JsonResult GetUserInfo(string UserId)
        {
            List<EditModel> edit = new List<EditModel>();
            ApplicationUser user = db.Users.Find(UserId);
            if (user == null)
            {
                return Json(new { success = false, responseText = "کاربرمورد نظر یافت نشد" }, JsonRequestBehavior.AllowGet);
            }  
            edit.Add(new EditModel() { Key = "Name"  , Value = user.Name });
            edit.Add(new EditModel() { Key = "LastName", Value = user.LastName });
            edit.Add(new EditModel() { Key = "NationalCode", Value = user.NationalCode });
            edit.Add(new EditModel() { Key = "DecriptPass", Value = user.DecriptPass });
            edit.Add(new EditModel() { Key = "UserName", Value = user.UserName });
            edit.Add(new EditModel() { Key = "PasswordHash", Value = user.DecriptPass });
            edit.Add(new EditModel() { Key = "UserId", Value = user.Id });
            return Json(new { success = true , listItem = edit.ToList(), cityid = user.CityId }, JsonRequestBehavior.AllowGet);
        }

    }
}