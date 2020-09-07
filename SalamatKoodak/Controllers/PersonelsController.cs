using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SalamatKoodak.Models;
using System.Data.Entity;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using SalamatKoodak.Utility;
using Stimulsoft.Report.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SalamatKoodak.Controllers
{
    [Authorize]
    public class PersonelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async Task<ActionResult> Index(int? page , string searchString, int? CityId)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user =await UserManager.FindByIdAsync(User.Identity.GetUserId());
            int pageSize = 10;
            var personels = new object();
            int pageNumber = 0;   
            try
            {
                if (User.IsInRole("Admin"))
                {
                    if (!String.IsNullOrEmpty(searchString) || CityId != null)
                    {
                        if (CityId != null)
                        {
                            pageNumber = (page ?? 1);
                            personels = db.Personels.Where(s => s.Mobile == searchString || s.IdTelegram.Contains(searchString)).Where(s => s.CityId == CityId).Include(s => s.PersonelStatus).Include(s => s.RelationType).OrderBy(s => s.Id).ToPagedList(pageNumber, pageSize);
                            ViewBag.Count = await db.Personels.Where(s => s.Mobile.Contains(searchString) || s.IdTelegram.Contains(searchString)).Where(s => s.CityId == CityId).CountAsync();
                            return View(personels);
                        }
                        else
                        {
                            pageNumber = (page ?? 1);
                            personels = db.Personels.Where(s => s.Mobile.Contains( searchString) || s.IdTelegram.Contains(searchString)).Include(s => s.PersonelStatus).Include(s => s.RelationType).OrderBy(s => s.Id).ToPagedList(pageNumber, pageSize);
                            ViewBag.Count = await db.Personels.Where(s => s.Mobile.Contains( searchString) || s.IdTelegram.Contains(searchString)).CountAsync();
                            return View(personels);
                        }
                    }
                    pageNumber = (page ?? 1);
                    ViewBag.Count = await db.Personels.CountAsync();
                    return View(db.Personels.Include(s => s.PersonelStatus).Include(s => s.RelationType).OrderBy(s => s.Id).ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        pageNumber = (page ?? 1);
                        personels = db.Personels.Where(s => s.Mobile.Contains(searchString) || s.IdTelegram.Contains(searchString)).Include(s => s.PersonelStatus).Include(s => s.RelationType).Where(s => s.ApplicationUserId == user.Id && s.CityId == user.CityId).OrderBy(s => s.Id).ToPagedList(pageNumber, pageSize);
                        ViewBag.Count = await db.Personels.Where(s => s.Mobile.Contains(searchString) || s.IdTelegram.Contains(searchString)).CountAsync();
                        return View(personels);
                    }
                    pageNumber = (page ?? 1);
                    ViewBag.Count = await db.Personels.Where(s => s.ApplicationUserId == user.Id && s.CityId == user.CityId).CountAsync();
                    return View(db.Personels.Include(s => s.PersonelStatus).Include(s => s.RelationType).Where(s => s.ApplicationUserId == user.Id && s.CityId == user.CityId).OrderByDescending(s => s.CreatedDate).ToPagedList(pageNumber, pageSize));
                }
            }
            catch(Exception ex)
            {
                return Content("خطای بر قراری ارتباط ، لطفا مجددا تلاش نمایید");
            }
        }
        [HttpGet]
        [Authorize]

        public async Task<JsonResult> LoadRelationType()
        {
            List<RelationType> relationTypes =await db.RelationTypes.ToListAsync();
            return Json(new { success = true, list = relationTypes }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize]

        public async Task<JsonResult> LoadStatus()
        {
            List<PersonelStatus> personelStatuses =await db.PersonelStatuses.ToListAsync();
            return Json(new { success = true, list = personelStatuses }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]

        public async Task<JsonResult> Register(Personels model , int? PersonelId)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user =await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, responseText = "مقادیر ورودی ناقص است " }, JsonRequestBehavior.AllowGet);
            }
            model.ApplicationUserId = user.Id;
            model.CityId =user.CityId;
            try
            {
                if(PersonelId != null)
                {
                    Personels personelitem =await db.Personels.Where(s => s.Id == PersonelId.Value).FirstOrDefaultAsync();
                    if(personelitem is null)
                    {
                        return Json(new { success = false, responseText = "ایتم مورد نظر یافت نشد" }, JsonRequestBehavior.AllowGet);
                    }
                    if(personelitem.Code !=model.Code)
                    {
                        if(await db.Personels.Where(s=>s.Code == model.Code).CountAsync() >0)
                        {
                        return Json(new { success = false, responseText = $"کد {model.Code} تکراری است" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if(personelitem.NetWorkType != model.NetWorkType)
                    {
                        personelitem.NetWorkType = model.NetWorkType;
                        if(model.NetWorkType == 0)
                        {
                            personelitem.NetWorkTypeName = "شبکه اجتماعی";
                        }
                        else
                        {
                            personelitem.NetWorkTypeName = "شبکه انتشار";

                        }
                    }
                    personelitem.IdTelegram = model.IdTelegram;
                    personelitem.LastName = model.LastName;
                    personelitem.Mobile = model.Mobile;
                    personelitem.Name = model.Name;
                    personelitem.NationalCode = model.NationalCode;
                    personelitem.PersonelStatusId = model.PersonelStatusId;
                    personelitem.RelationTypeId = model.RelationTypeId;
                    personelitem.Code = model.Code;
                }
                else
                {
                    if (await db.Personels.Where(s => s.Code == model.Code).CountAsync() >0)
                    {
                        return Json(new { success = false, responseText = $"کد {model.Code} تکراری است" }, JsonRequestBehavior.AllowGet);
                    }
                    model.CreatedDate = DateTime.Now;
                    if (model.NetWorkType == 0)
                    {
                        model.NetWorkTypeName = "شبکه اجتماعی";
                    }
                    else
                    {
                        model.NetWorkTypeName = "شبکه انتشار";

                    }
                    db.Personels.Add(model);
                }
              await  db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return Json(new { success = false, responseText = "سیستم قادر به پاسخ گویی نیست" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]

        public async Task<JsonResult> Remove(int PersonelId)
        {
            Personels item =await db.Personels.FindAsync(PersonelId);
            if (item == null)
            {
                return Json(new { success = false, responseText = "ایتم مورد نظر یافت نشد" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                db.Personels.Remove(item);
              await  db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "سیستم قادر به پاسخ گویی نیست" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize]

        public async Task<ActionResult> Print(int? type , int? RelationTypeId , int? PersonelStatusId)
        {
         var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
         var user =await UserManager.FindByIdAsync(User.Identity.GetUserId());
            try
            {
          StiReport report = new StiReport();            
         report.Load(Server.MapPath("~/Content/Report/stiPersonelsReport.mrt"));
         StiComponentsCollection components = report.GetComponents();
         ((StiText)components["txtCurrentDate"]).Text.Value = string.Format("{0} - {1}", DateTime.Now.ToString("HH:mm:ss"), DateTime.Now.ToPersianDateTime());
                if(user != null)
                {
                ((StiText)components["txtusername"]).Text.Value = $"{user.Name + " "+user.LastName}";               
                }
            if (User.IsInRole("Admin"))
            {
              var dataset =await db.Personels.Include(s => s.PersonelStatus).Include(s=>s.PersonelStatusId).Include(s => s.RelationType).Select(s=>new { s.Code,s.CityId   , s.Name , s.LastName , s.Mobile, s.IdTelegram , s.NationalCode , RelationType= s.RelationType.Name , PersonelStatus= s.PersonelStatus.Name , s.Id , s.RelationTypeId , s.PersonelStatusId,s.NetWorkTypeName}).OrderBy(s =>s.Id).ToListAsync();
                    if(RelationTypeId !=null && PersonelStatusId != null)
                    {
                        dataset = dataset.Where(s => s.RelationTypeId == RelationTypeId && s.PersonelStatusId == PersonelStatusId).ToList();
                    }
                    if (RelationTypeId != null)
                    {
                        dataset = dataset.Where(s => s.RelationTypeId == RelationTypeId).ToList();
                    }
                     if(PersonelStatusId != null)
                    {
                        dataset = dataset.Where(s => s.PersonelStatusId == PersonelStatusId).ToList();
                    }
              report.Compile();
                if (dataset.Count == 0)
                {
                 TempData["Error"] = "داده ای جهت چاپ یافت نشد";
                 return RedirectToAction(nameof(Index));
                }
                var dataTable = dataset.ToDataTable();
              dataTable.TableName = "DataSource1";
              report.RegData(dataTable);
            }
            else
            {
                var dataset =await db.Personels.Include(s => s.PersonelStatus).
                Include(s => s.RelationType).Select(s => new { s.Code, s.CityId,s.ApplicationUserId,s.Name, s.LastName, s.Mobile, s.IdTelegram, s.NetWorkTypeName,
                s.NationalCode, RelationType = s.RelationType.Name, PersonelStatus = 
                s.PersonelStatus.Name, s.Id,
                s.RelationTypeId,
                s.PersonelStatusId
                }).Where(s => s.ApplicationUserId == user.Id && s.CityId == user.CityId)
                .OrderBy(s => s.Id).ToListAsync();

                    if (RelationTypeId != null && PersonelStatusId != null)
                    {
                        dataset = dataset.Where(s => s.RelationTypeId == RelationTypeId && s.PersonelStatusId == PersonelStatusId).ToList();
                    }
                    if (RelationTypeId != null)
                    {
                        dataset = dataset.Where(s => s.RelationTypeId == RelationTypeId).ToList();
                    }
                    if (PersonelStatusId != null)
                    {
                        dataset = dataset.Where(s => s.PersonelStatusId == PersonelStatusId).ToList();
                    }
                 report.Compile();      
                if(dataset.Count == 0)
                {
                        TempData["Error"] = "داده ای جهت چاپ یافت نشد";
                        return RedirectToAction(nameof(Index));
                }
                var dataTable = dataset.ToDataTable();
                dataTable.TableName = "DataSource1";
                report.RegData(dataTable);     
            }
         if (type == 0)
         {
             return StiMvcReportResponse.ResponseAsPdf(report);
         }
         else
         {
             return StiMvcReportResponse.ResponseAsExcel2007(report);
         }
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
        [HttpPost]
        [Authorize]

        public JsonResult GetPersonelInfo(int PersonelId)
        {
            List<EditModel> edit = new List<EditModel>();
            Personels personel =  db.Personels.Find(PersonelId) ;
            if (personel == null)
            {
                return Json(new { success = false, responseText = "کاربرمورد نظر یافت نشد" }, JsonRequestBehavior.AllowGet);
            }
            edit.Add(new EditModel() { Key = "Code", Value = personel.Code });
            edit.Add(new EditModel() { Key = "Name", Value = personel.Name });
            edit.Add(new EditModel() { Key = "LastName", Value = personel.LastName });
            edit.Add(new EditModel() { Key = "NationalCode", Value = personel.NationalCode });
            edit.Add(new EditModel() { Key = "Mobile", Value = personel.Mobile });
            edit.Add(new EditModel() { Key = "IdTelegram", Value = personel.IdTelegram });
            edit.Add(new EditModel() { Key = "PersonelId", Value = personel.Id.ToString() });       
            return Json(new { success = true, listItem = edit.ToList(), relationtypeid = personel.RelationTypeId ,
               statusid = personel.PersonelStatusId  , networktype= personel.NetWorkType}, JsonRequestBehavior.AllowGet);
        }
    }
}