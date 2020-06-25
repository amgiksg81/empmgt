namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClientFunctionality : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        ClientName = c.String(maxLength: 100),
                        ClientCountry = c.String(maxLength: 100),
                        ClientAddress = c.String(maxLength: 500),
                        ClientSkypeID = c.String(maxLength: 100),
                        ClientEmailID = c.String(maxLength: 100),
                        ContactNo = c.String(maxLength: 50),
                        WhatsappNo = c.String(maxLength: 50),
                        ClientCompanyName = c.String(maxLength: 200),
                        ClientCompanyEmailID = c.String(maxLength: 100),
                        ClientCompanyWebsite = c.String(maxLength: 100),
                        TaxNo = c.String(maxLength: 100),
                        GSTNo = c.String(maxLength: 100),
                        PANNo = c.String(maxLength: 100),
                        PaypalAddress = c.String(maxLength: 100),
                        TimeZone = c.String(maxLength: 100),
                        FirstHiredUpworkID = c.String(maxLength: 100),
                        HiringDate = c.DateTime(),
                        HiredBy = c.String(maxLength: 100),
                        AddedOnSkype = c.String(maxLength: 100),
                        AddedOnEmail = c.String(maxLength: 100),
                        Description = c.String(maxLength: 500),
                        ReferenceByName = c.String(maxLength: 50),
                        ReferenceByContactNo = c.String(maxLength: 50),
                        ReferenceByEmailSkype = c.String(maxLength: 100),
                        ProfileImage = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(),
                        UpdatedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ClientId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Clients");
        }
    }
}
