using LogistHelper.Models.Settings;
using Shared;

namespace LogistHelper.Models
{
    public class ComanyJsonRepository : JsonRepository<CompanySettings>
    {
        public ComanyJsonRepository(ILogger logger) : base(logger, "compSettings.txt")
        {
        }

        protected override CompanySettings GetDefault()
        {
            return new CompanySettings()
            {
                Name = "ООО Логист",
                InnKpp = "15262558/66552336",
                Address = "г.Москва, 3-я улица Строителей, д.25 кв. 12",
                Phones = new List<string>() { "+7(192)262-56-66" },
                Emails = new List<string>() { "logist@mail.ru" }
            };
        }
    }
}
