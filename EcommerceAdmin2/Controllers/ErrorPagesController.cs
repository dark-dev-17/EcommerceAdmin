using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAdmin2.Controllers
{
    public class ErrorPagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NoAccess()
        {
            return View();
        }
        public IActionResult NoAccessData()
        {
            Response response = new Response { Code = 200, Description = "You are not authorized to perform this Action", Type = "Danger" };
            return BadRequest(response);
        }
        public IActionResult NoSessionData()
        {
            Response response = new Response { Code = 200, Description = "You are logged.", Type = "Danger" };
            return BadRequest(response);
        }
    }
}