using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceAdmin1.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            InitAppController(filterContext);
            base.OnActionExecuting(filterContext);
        }

        // Redirect to login page if user's session is not valid.
        public void InitAppController(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session == null)
            {
                filterContext.Result = RedirectToAction("Index", "Login");
            }
            
        }
    }
}