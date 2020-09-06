using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SalamatKoodak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SalamatKoodak.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		public ActionResult Index()
		{
		var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
			var user = UserManager.FindById(User.Identity.GetUserId());
			//ViewBag.FullName = $"{user.Name + " " + user.LastName} ";
			if(user != null)
			{

			Session["FullName"] = $"{user.Name + " " + user.LastName} ";
			}
			return View();
		}
	}
}