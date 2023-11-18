using Gate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Gate.Controllers
{
    public class CommentController : ApiController
    {
        private Cmodel db = new Cmodel();

        public string post()
        {
            try
            {
                int iduser = Convert.ToInt32(HttpContext.Current.Request.Form["iduser"]);
                int idpost = Convert.ToInt32(HttpContext.Current.Request.Form["idpost"]);
                string comment = HttpContext.Current.Request.Form["comment"];

                comm comm = new comm();
                comm.iduser = iduser;
                comm.idp = idpost;
                comm.comment = comment;
                comm.User = db.users.Find(iduser);
                comm.paper = db.ps.Find(idpost);
                db.comms.Add(comm);
                db.SaveChanges();
            }
            catch
            {
                return "badjop";
            }

            return "goodjop";
        }
    }
}
