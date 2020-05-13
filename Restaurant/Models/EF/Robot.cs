namespace Restaurant.Models.EF
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Robot")]
    public partial class Robot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_Robot { get; set; }

        public bool Status { get; set; }

        public int Number { get; set; }
    }
}
