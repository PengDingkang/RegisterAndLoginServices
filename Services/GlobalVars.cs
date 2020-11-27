using System.Configuration;

namespace RegisterAndLoginServices.Services
{
    public class GlobalVars
    {
        // You need to add app.config to the projest's directory manually
        public static readonly string connStr = ConfigurationManager.ConnectionStrings["UserDB"].ToString();
        public static readonly string secret = ConfigurationManager.AppSettings["Secret"].ToString();
        public static readonly string domain = ConfigurationManager.AppSettings["Domain"].ToString();
    }
}
