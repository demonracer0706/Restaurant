namespace Restaurant.Models.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Invoice")]
    public partial class Invoice
    {
        [Key]
        [StringLength(50)]
        public string ID_Invoice { get; set; }

        public int Number { get; set; }

        public DateTime Date { get; set; }

        public long Payable { get; set; }

        public bool Status { get; set; }
    }
}
