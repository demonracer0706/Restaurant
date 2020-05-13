namespace Restaurant.Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Product")]
    public partial class Product
    {
        [Key]
        [StringLength(50)]
        public string ID_Product { get; set; }

        [StringLength(50)]
        public string ID_Type { get; set; }

        [StringLength(50)]
        public string ProductName { get; set; }

        public long Price { get; set; }

        [StringLength(200)]
        public string Image { get; set; }
    }
}
