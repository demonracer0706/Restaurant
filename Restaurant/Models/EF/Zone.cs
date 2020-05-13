namespace Restaurant.Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Zone")]
    public partial class Zone
    {
        [Key]
        [StringLength(50)]
        public string ID_Zone { get; set; }

        [StringLength(50)]
        public string ZoneName { get; set; }
    }
}
