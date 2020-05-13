namespace Restaurant.Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductType")]
    public partial class ProductType
    {
        [Key]
        [StringLength(50)]
        public string ID_Type { get; set; }

        [StringLength(50)]
        public string TypeName { get; set; }
    }
}
