namespace Restaurant.Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("InvoiceDetail")]
    public partial class InvoiceDetail
    {
        [Key]
        public int ID_Detail { get; set; }

        [StringLength(50)]
        public string ID_Product { get; set; }

        [StringLength(50)]
        public string ID_Invoice { get; set; }

        public int Quantity { get; set; }

        public long TotalPrice { get; set; }
    }
}
