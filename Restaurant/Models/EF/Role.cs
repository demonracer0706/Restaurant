namespace Restaurant.Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Role")]
    public partial class Role
    {
        [Key]
        [StringLength(50)]
        public string ID_Role { get; set; }

        [Column("Role")]
        [StringLength(50)]
        public string Role1 { get; set; }
    }
}
