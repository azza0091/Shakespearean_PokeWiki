using System.Web;
using System.Web.Mvc;

namespace Shakespearean_PokeWiki
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
