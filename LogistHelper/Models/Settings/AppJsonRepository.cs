using Shared;

namespace LogistHelper.Models.Settings
{
    public class AppJsonRepository : JsonRepository<OtherSettings>
    {
        public AppJsonRepository(ILogger logger) : base(logger, "appSettings.txt")
        {
        }

        protected override OtherSettings GetDefault()
        {
            return new OtherSettings()
            {
                DefaultPrinterName = "Microsoft Print to PDF"
            };
        }
    }
}
