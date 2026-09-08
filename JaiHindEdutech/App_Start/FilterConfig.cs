using JaiHindEdutech.Infrastructure;
using System.Web.Mvc;

namespace JaiHindEdutech
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ApplicationHandleErrorAttribute());
        }
    }
}
