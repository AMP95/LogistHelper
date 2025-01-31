using LogistHelper.Models.Settings;
using MailSender;
using Shared;

namespace LogistHelper.Models
{
    public class MailJsonRepository : JsonRepository<MailSetting>
    {
        public MailJsonRepository(ILogger logger) : base(logger, "mailSettings.txt")
        {
        }

        protected override MailSetting GetDefault()
        {
            return new MailSetting()
            {
                FromAddress = "chunya95@mail.ru",
                FromName = "Логист, ООО",
                Password = "qK8HQgAAT5WFTaMEwX8d",
                Server = "smtp.mail.ru",
                Port = 465,
                UseSSL = true,
            };
        }
    }
}
