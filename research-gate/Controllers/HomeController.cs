using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gate.Models;

namespace Gate.Controllers
{
    public class HomeController : Controller
    {


        private Cmodel db = new Cmodel();

        public ActionResult Index()
        {
            Session["userid"] = "0";
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            string username = form["uname"].ToString();
            string password = form["pass"].ToString();

            user user = db.users.Where(
                m => m.username == username && m.password == password
                ).FirstOrDefault();
            if(user == null)
            {
                Session["userid"] = "0";
                return View();
            }
            else
            {
                Session["userid"] = user.id.ToString();
                return RedirectToAction("Index","profile");
            }
        }


        public ActionResult signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult signup(FormCollection form , HttpPostedFileBase photo)
        {

            user user = new user();
            user.fname = form["fname"].ToString();
            user.lname = form["lname"].ToString();
            user.username = form["uname"].ToString();
            user.password = form["pass"].ToString();
            user.department = form["department"].ToString();
            user.university = form["university"].ToString();

            HttpPostedFileBase postedFile = Request.Files["photo"];

            string path = Server.MapPath("~/Uploads/");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            postedFile.SaveAs(path + Path.GetFileName(postedFile.FileName));

            user.photo = "/Uploads/" + Path.GetFileName(postedFile.FileName);

            db.users.Add(user);
            db.SaveChanges();

            return View();
        }

        public ActionResult About()
        {
            if (Session["userid"]=="0")
            {
                return RedirectToAction("index");
            }
            
            return View();
        }

        public ActionResult Contact()
        {
            if (Session["userid"] == "0")
            {
                return RedirectToAction("index");
            }

            return View();
        }
    }
}