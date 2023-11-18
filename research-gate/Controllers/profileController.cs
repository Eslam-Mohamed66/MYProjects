using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gate.Models;


namespace Gate.Controllers
{
    public class profileController : Controller
    {

        private Cmodel db = new Cmodel();

        // GET: profile
        public ActionResult Index()
        {
            user user = db.users.Find(Convert.ToInt32(Session["userid"]));
            ViewBag.users = db.users.ToList();
            return View(user);
        }
        
        public ActionResult addpaper()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addpaper(FormCollection form, HttpPostedFileBase file,HttpPostedFileBase file1)
        {
            p p = new p();
            p.name = form["name"].ToString();

            HttpPostedFileBase postedFile = Request.Files["file"];
            HttpPostedFileBase postedFile1 = Request.Files["file1"];
            
            string path = Server.MapPath("~/Uploadsfile/");
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            postedFile.SaveAs(path+Path.GetFileName(postedFile.FileName));
            postedFile.SaveAs(path + Path.GetFileName(postedFile1.FileName));

            p.file = "/Uploadsfile/" + Path.GetFileName(postedFile.FileName);
            p.file1 = "/Uploadsfile/" + Path.GetFileName(postedFile1.FileName);

            p.user = db.users.Find(Convert.ToInt32(Session["userid"]));

            db.ps.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Allperson()
        {
            return View(db.users.ToList());
        }


        [HttpPost]
        public ActionResult some(FormCollection form)
        {
            string name = form["name"].ToString();



            return View(db.users.Where(m=>m.fname == name || 
                                       m.lname == name || 
                                       m.username == name || 
                                       m.university == name || 
                                       m.department == name
            ).ToList());
        }





        public ActionResult one_person(int? id)
        {
            int idi = Convert.ToInt32(id);
            
            user user = db.users.Find(Convert.ToInt32(Session["userid"]));
            ViewBag.name = user.fname + "  " + user.lname;
            ViewBag.photo = user.photo;
            return View(db.users.Find(idi));
        }

        [HttpPost]
        public ActionResult add_user_to_paper(int? id,FormCollection form)
        {

            int idp = Convert.ToInt32(id);
            int iduser = Convert.ToInt32(form["user"]);
            
            db.userinpapers.RemoveRange(
                db.userinpapers.Where(
                    m=> m.idpartener==iduser && m.idpaper == idp));
            
            db.SaveChanges();

            userinpaper u_p = new userinpaper();
            u_p.paper = db.ps.Find(idp);
            u_p.idpaper = idp;
            u_p.idpartener = iduser;
            u_p.partener = db.users.Find(iduser);
            
            db.userinpapers.Add(u_p);
            db.SaveChanges();

            return RedirectToAction("index");
        }

        public ActionResult viewpaper(int? id)
        {
            user user = db.users.Find(Convert.ToInt32(Session["userid"]));
            ViewBag.name = user.fname + "  " + user.lname;
            ViewBag.photo = user.photo;

            return View(db.ps.Find(Convert.ToInt32(id)));
        }

        public ActionResult logout()
        {
            return RedirectToAction("index","Home");
        }

    }
}