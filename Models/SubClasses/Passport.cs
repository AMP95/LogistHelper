using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Passport
    {
        [Key]
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public string PassportNumber { get; set; }
        public string Issuer { get; set; }
        public string IssuerCode { get; set; }
        public DateTime DateOfIssue { get; set; }
    }
}
