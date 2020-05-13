namespace Restaurant.Models.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OrderTable")]
    public partial class OrderTable
    {
        public int ID { get; set; }

        public int Number { get; set; }

        [StringLength(50)]
        public string FullName { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public DateTime Date { get; set; }
    }
}
