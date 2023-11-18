using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Gate.Models;

namespace Gate.Controllers
{

    public class ReactController : ApiController
    {
        private Cmodel db = new Cmodel();

        public string post()
        {
            try
            {
                int iduser = Convert.ToInt32(HttpContext.Current.Request.Form["iduser"]);
                int idp = Convert.ToInt32(HttpContext.Current.Request.Form["idpost"]);
                int rea = Convert.ToInt32(HttpContext.Current.Request.Form["react"]);

                db.Reacts.RemoveRange(
                    db.Reacts.Where(m=>m.idpaper == idp && m.iduser == iduser).ToList());
                db.SaveChanges();

                react react = new react();
                react.rea = rea;

                react.pa = db.ps.Find(idp);
                react.idpaper = idp;
                
                react.User = db.users.Find(iduser);
                react.iduser = iduser;
                
                db.Reacts.Add(react);
                db.SaveChanges();

                return "Good Job";
            }
            catch
            {
                return "Dad Job";
            }
        }
    }
}
