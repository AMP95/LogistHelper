using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long Inn { get; set; }
        public long Kpp { get; set; }


        [ForeignKey(nameof(Address))]
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }


        public virtual ICollection<Phone> Phones { get;set; }
        public virtual ICollection<Email> Emails { get; set; }
    }
}
