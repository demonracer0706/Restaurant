namespace Restaurant.Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Table")]
    public partial class Table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Number { get; set; }

        [StringLength(50)]
        public string ID_Zone { get; set; }

        [StringLength(50)]
        public string TableName { get; set; }

        public bool Status { get; set; }
    }
}
