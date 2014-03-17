namespace TumblrThreadTracker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.UserBlog",
                c => new
                    {
                        UserBlogId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        BlogShortname = c.String(),
                    })
                .PrimaryKey(t => t.UserBlogId)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserThread",
                c => new
                    {
                        UserThreadId = c.Int(nullable: false, identity: true),
                        UserBlogId = c.Int(nullable: false),
                        PostId = c.String(),
                        UserTitle = c.String(),
                    })
                .PrimaryKey(t => t.UserThreadId)
                .ForeignKey("dbo.UserBlog", t => t.UserBlogId, cascadeDelete: true)
                .Index(t => t.UserBlogId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserThread", new[] { "UserBlogId" });
            DropIndex("dbo.UserBlog", new[] { "UserId" });
            DropForeignKey("dbo.UserThread", "UserBlogId", "dbo.UserBlog");
            DropForeignKey("dbo.UserBlog", "UserId", "dbo.UserProfile");
            DropTable("dbo.UserThread");
            DropTable("dbo.UserBlog");
            DropTable("dbo.UserProfile");
        }
    }
}
