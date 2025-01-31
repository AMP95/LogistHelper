namespace LogistHelper.Models
{
    public class CompanySettings
    {
        public string Name { get; set; }
        public string InnKpp { get; set; }
        public string Address { get; set; }
        public List<string> Phones { get; set; }
        public List<string> Emails { get; set; }
    }
}
