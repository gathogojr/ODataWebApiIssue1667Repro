namespace Repro.Lib.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ManagerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.ManagerId)
                .Index(t => t.ManagerId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ProjectId = c.Int(nullable: false),
                        MilestoneId = c.Int(),
                        SupervisorId = c.Int(),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Milestones", t => t.MilestoneId)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.SupervisorId)
                .Index(t => t.ProjectId)
                .Index(t => t.MilestoneId)
                .Index(t => t.SupervisorId);
            
            CreateTable(
                "dbo.Milestones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ProjectId = c.Int(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ManagerId = c.Int(nullable: false),
                        Start = c.DateTime(nullable: false),
                        End = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.ManagerId, cascadeDelete: true)
                .Index(t => t.ManagerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "SupervisorId", "dbo.Employees");
            DropForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Tasks", "MilestoneId", "dbo.Milestones");
            DropForeignKey("dbo.Milestones", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Projects", "ManagerId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "ManagerId", "dbo.Employees");
            DropIndex("dbo.Projects", new[] { "ManagerId" });
            DropIndex("dbo.Milestones", new[] { "ProjectId" });
            DropIndex("dbo.Tasks", new[] { "SupervisorId" });
            DropIndex("dbo.Tasks", new[] { "MilestoneId" });
            DropIndex("dbo.Tasks", new[] { "ProjectId" });
            DropIndex("dbo.Employees", new[] { "ManagerId" });
            DropTable("dbo.Projects");
            DropTable("dbo.Milestones");
            DropTable("dbo.Tasks");
            DropTable("dbo.Employees");
            DropTable("dbo.Companies");
        }
    }
}
