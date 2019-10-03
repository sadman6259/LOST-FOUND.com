using LOF.Models;
using System.Web;
using System.Web.Mvc;

namespace LOF
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
           // filters.Add(new AuthenticationSampleFilter());
        }
    }
}
