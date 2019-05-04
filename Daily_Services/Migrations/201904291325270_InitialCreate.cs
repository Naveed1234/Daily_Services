namespace Daily_Services.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Category_Id = c.Int(nullable: false, identity: true),
                        Category_Type = c.String(),
                    })
                .PrimaryKey(t => t.Category_Id);
            
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        SubCategory_Id = c.Int(nullable: false, identity: true),
                        SubCategory_Type = c.String(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.SubCategory_Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        First_Name = c.String(),
                        Last_Name = c.String(),
                        Phone_Number = c.String(),
                        DateOfBirth = c.DateTime(),
                        Gender_Id = c.Int(),
                        Category_Id = c.Int(),
                        SubCategory_Id = c.Int(),
                        Qualification_Id = c.Int(),
                        Image_Path = c.String(),
                        Address = c.String(),
                        Description = c.String(),
                        Postal_Code = c.String(),
                        CNIC_No = c.String(),
                        ServiceOffer = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .ForeignKey("dbo.Genders", t => t.Gender_Id)
                .ForeignKey("dbo.Qualifications", t => t.Qualification_Id)
                .ForeignKey("dbo.SubCategories", t => t.SubCategory_Id)
                .Index(t => t.Gender_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.SubCategory_Id)
                .Index(t => t.Qualification_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        Gender_Id = c.Int(nullable: false, identity: true),
                        Gender_Type = c.String(),
                    })
                .PrimaryKey(t => t.Gender_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Qualifications",
                c => new
                    {
                        Qualification_Id = c.Int(nullable: false, identity: true),
                        Qualification_Type = c.String(),
                    })
                .PrimaryKey(t => t.Qualification_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUsers", "SubCategory_Id", "dbo.SubCategories");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Qualification_Id", "dbo.Qualifications");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Gender_Id", "dbo.Genders");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.SubCategories", "Category_Id", "dbo.Categories");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "Qualification_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "SubCategory_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Category_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Gender_Id" });
            DropIndex("dbo.SubCategories", new[] { "Category_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Qualifications");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Genders");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.SubCategories");
            DropTable("dbo.Categories");
        }
    }
}
