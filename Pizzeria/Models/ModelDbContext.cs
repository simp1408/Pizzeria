using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Pizzeria.Models
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<Ordine> Ordine { get; set; }
        public virtual DbSet<Pizza> Pizza { get; set; }
        public virtual DbSet<Utente> Utente { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pizza>()
                .Property(e => e.Prezzo)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Pizza>()
                .HasMany(e => e.Ordine)
                .WithRequired(e => e.Pizza)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utente>()
                .HasMany(e => e.Ordine)
                .WithRequired(e => e.Utente)
                .WillCascadeOnDelete(false);
        }
    }
}
