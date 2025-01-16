using System.Text.RegularExpressions;

namespace LogistHelper.Services
{
    public static class RegexService
    {
        private static Regex _phone = new Regex("^\\+7\\([0-9]{3}\\)[0-9]{3}(-[0-9]{2}){2}$");
        private static Regex _mail = new Regex("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
        private static Regex _truck = new Regex("^[A-Z][ ]([0-9]{3})[ ]([A-Z]{2})\\/([0-9]{2,3})$");
        private static Regex _trailer = new Regex("^[A-Z]{2}[ ]([0-9]{4})\\/([0-9]{2})$");
        private static Regex _name = new Regex("^([\\w]*([-][\\w]*)?)([ ]([\\w]*([-][\\w]*)?))+$");
        private static Regex _innKpp = new Regex("^[0-9]+(\\/[0-9]+)*$");

        public static bool CheckPhone(string phone) 
        { 
            return _phone.IsMatch(phone);
        }
        public static bool CheckEmail(string mail)
        {
            return _mail.IsMatch(mail);
        }
        public static bool CheckTruck(string number)
        {
            return _truck.IsMatch(number);
        }
        public static bool CheckTrailer(string number)
        {
            return _trailer.IsMatch(number);
        }
        public static bool CheckName(string name)
        {
            return _name.IsMatch(name);
        }
        public static bool CheckInnKpp(string innkpp)
        {
            return _innKpp.IsMatch(innkpp);
        }
    }
}
