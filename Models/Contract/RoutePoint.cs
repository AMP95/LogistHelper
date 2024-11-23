using System.ComponentModel.DataAnnotations.Schema;

namespace Models.SubClasses
{
   

    public class RoutePoint
    {
        public Guid Id { get; set; }



        [ForeignKey(nameof(Address))]
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }


        public LoadingType Loading { get; set; }


        public virtual ICollection<Phone> Phones { get; set; }
    }
}
