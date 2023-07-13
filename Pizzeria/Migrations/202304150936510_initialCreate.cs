namespace Pizzeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ordine",
                c => new
                    {
                        IDordine = c.Int(nullable: false, identity: true),
                        Quantita = c.Int(nullable: false),
                        Note = c.String(),
                        IndirizzoSpedizione = c.String(),
                        OrdineConsegnato = c.Boolean(),
                        OrdineInConsegnato = c.Boolean(),
                        DataOrdine = c.DateTime(storeType: "date"),
                        IdPizza = c.Int(nullable: false),
                        IdUtente = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IDordine)
                .ForeignKey("dbo.Pizza", t => t.IdPizza)
                .ForeignKey("dbo.Utente", t => t.IdUtente)
                .Index(t => t.IdPizza)
                .Index(t => t.IdUtente);
            
            CreateTable(
                "dbo.Pizza",
                c => new
                    {
                        IDpizza = c.Int(nullable: false, identity: true),
                        NomePizza = c.String(nullable: false),
                        Foto = c.String(nullable: false, maxLength: 50),
                        Prezzo = c.Decimal(nullable: false, precision: 10, scale: 2),
                        Ingredienti = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IDpizza);
            
            CreateTable(
                "dbo.Utente",
                c => new
                    {
                        IDutente = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 25),
                        Cognome = c.String(nullable: false, maxLength: 25),
                        Ruolo = c.String(maxLength: 25),
                        Username = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.IDutente);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ordine", "IdUtente", "dbo.Utente");
            DropForeignKey("dbo.Ordine", "IdPizza", "dbo.Pizza");
            DropIndex("dbo.Ordine", new[] { "IdUtente" });
            DropIndex("dbo.Ordine", new[] { "IdPizza" });
            DropTable("dbo.Utente");
            DropTable("dbo.Pizza");
            DropTable("dbo.Ordine");
        }
    }
}
