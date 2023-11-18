using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Gate.Models
{
    public class Cmodel:DbContext
    {
        public Cmodel() : base("Gat44")
        {
        }
        public DbSet<user> users { get; set; }
        public DbSet<p> ps { get; set; }
        public DbSet<userinpaper> userinpapers { get; set; }
        public DbSet<comm> comms { get; set; }
        public DbSet<react> Reacts { get; set; }
    }


    public class user
    {
        public int id {get; set;}
        public string fname {get; set;}
        public string lname {get; set;}
        public string photo { get; set;}
        public string username { get; set;}
        public string password { get; set;}
        public string university { get; set;}
        public string department { get; set;}

        public virtual List<p> Ps { get; set; }
        
        
        public virtual List<userinpaper> Userinpaper { get; set; }
    }

    public class p
    {
        public int id{get; set;}
        public string name {get; set;}
        public string file {get; set;}
        public string file1 { get; set; }





        public virtual user user {get; set;}
        public virtual List<comm> Comms {get; set;}
        public virtual List<userinpaper> Userinpaper {get; set;}
        public virtual List<react> Reacts { get; set; }
    }

    public class userinpaper
    {
        public int id { get; set; }
        public int idpartener { get; set; }
        public int idpaper { get; set; }
        
        
        
        
        
        
        
        
        public virtual user partener { get; set; }
        public virtual p paper { get; set; }
    }

    public class comm
    {
        public int id {get; set;}
        public string comment {get; set;}
        public int iduser { get; set; }
        public int idp { get; set; }
       
        
        
        
        
        
        
        public virtual p paper { get; set; }
        public virtual user User { get; set; }
    }

    public class react
    {
        public int id { get; set;}
        public int rea { get; set;}
        public int iduser { get; set;}
        public int idpaper { get; set;}
        public virtual user User { get; set;}
        public virtual p pa { get; set;} 
    }

}