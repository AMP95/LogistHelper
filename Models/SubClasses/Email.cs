using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Email
    {
        [Key]
        public Guid Id { get; set; }    
        public string Address { get; set; }
    }
}
