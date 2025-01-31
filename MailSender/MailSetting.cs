namespace MailSender
{
    public class MailSetting
    {
        
        public string FromAddress { get; set; }
        public string FromName { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
    }
}
