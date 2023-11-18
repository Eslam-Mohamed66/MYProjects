namespace Gate.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class e1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.comms",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        comment = c.String(),
                        iduser = c.Int(nullable: false),
                        idp = c.Int(nullable: false),
                        paper_id = c.Int(),
                        User_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.p", t => t.paper_id)
                .ForeignKey("dbo.users", t => t.User_id)
                .Index(t => t.paper_id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.p",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        file = c.String(),
                        file1 = c.String(),
                        user_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.users", t => t.user_id)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.reacts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        rea = c.Int(nullable: false),
                        iduser = c.Int(nullable: false),
                        idpaper = c.Int(nullable: false),
                        pa_id = c.Int(),
                        User_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.p", t => t.pa_id)
                .ForeignKey("dbo.users", t => t.User_id)
                .Index(t => t.pa_id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        fname = c.String(),
                        lname = c.String(),
                        photo = c.String(),
                        username = c.String(),
                        password = c.String(),
                        university = c.String(),
                        department = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.userinpapers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        idpartener = c.Int(nullable: false),
                        idpaper = c.Int(nullable: false),
                        paper_id = c.Int(),
                        partener_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.p", t => t.paper_id)
                .ForeignKey("dbo.users", t => t.partener_id)
                .Index(t => t.paper_id)
                .Index(t => t.partener_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.comms", "User_id", "dbo.users");
            DropForeignKey("dbo.reacts", "User_id", "dbo.users");
            DropForeignKey("dbo.userinpapers", "partener_id", "dbo.users");
            DropForeignKey("dbo.userinpapers", "paper_id", "dbo.p");
            DropForeignKey("dbo.p", "user_id", "dbo.users");
            DropForeignKey("dbo.reacts", "pa_id", "dbo.p");
            DropForeignKey("dbo.comms", "paper_id", "dbo.p");
            DropIndex("dbo.userinpapers", new[] { "partener_id" });
            DropIndex("dbo.userinpapers", new[] { "paper_id" });
            DropIndex("dbo.reacts", new[] { "User_id" });
            DropIndex("dbo.reacts", new[] { "pa_id" });
            DropIndex("dbo.p", new[] { "user_id" });
            DropIndex("dbo.comms", new[] { "User_id" });
            DropIndex("dbo.comms", new[] { "paper_id" });
            DropTable("dbo.userinpapers");
            DropTable("dbo.users");
            DropTable("dbo.reacts");
            DropTable("dbo.p");
            DropTable("dbo.comms");
        }
    }
}
