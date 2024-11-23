using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        public int PostalCode { get;set; }
        [MaxLength(50)]
        public string Region { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Street { get; set; }
        [MaxLength(10)]
        public string Building { get; set; }
        [MaxLength(5)]
        public string Corpuse { get; set; }
        [MaxLength(5)]
        public string Office { get; set; }
    }
}
