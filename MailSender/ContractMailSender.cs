using Shared;
using MailKit.Net.Smtp;
using Utilities;
using MimeKit;

namespace MailSender
{
    public class ContractMailSender : IContractSender
    {
        private MailSetting _settings;
        private ILogger _logger;

        public ContractMailSender(ILogger logger, ISettingsRepository<MailSetting> repository)
        {
            _logger = logger;
            _settings = repository.GetSettings();
        }

        public async Task<bool> SendContract(string to, string subject, string contactPath)
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;

            var builder = new BodyBuilder();
            builder.Attachments.Add(contactPath);

            emailMessage.Body = builder.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_settings.Server, _settings.Port, _settings.UseSSL);
                    await client.AuthenticateAsync(_settings.FromAddress, _settings.Password);
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, ex.Message, LogLevel.Error);
                return false;
            }
        }
    }
}
