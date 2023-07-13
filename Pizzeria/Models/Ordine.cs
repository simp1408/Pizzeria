namespace Pizzeria.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordine")]
    public partial class Ordine
    {
        [Key]
        public int IDordine { get; set; }

        public int Quantita { get; set; }

        public string Note { get; set; }

        public string IndirizzoSpedizione { get; set; }
        public bool? OrdineConsegnato { get; set; }

        public bool? OrdineInConsegnato { get; set; }


        [Column(TypeName = "date")]
        public DateTime? DataOrdine { get; set; }

        public int IdPizza { get; set; }

        public int IdUtente { get; set; }

        public virtual Pizza Pizza { get; set; }

        public virtual Utente Utente { get; set; }
    }
}
