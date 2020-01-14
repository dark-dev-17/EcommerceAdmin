using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAdmin2.Models.Empleado;
using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAdmin2.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult LoginFailed()
        {
            return View();
        }
        // POST: Login/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([FromBody]Usuario Usuario)
        {
            if (ModelState.IsValid)
            {
                int Response;
                try
                {
                    using (DBMysql dBMysql = new DBMysql())
                    {
                        dBMysql.OpenConnection();
                        if (dBMysql.Connection != null)
                        {
                            Usuario.SetConnectionMysql(dBMysql);
                            Response = Usuario.DoLogin();
                            if (Response == 0)
                            {
                                HttpContext.Session.SetString("username", Usuario.User);
                                return Json(new { Error = false, Description = "Login success" + HttpContext.Session.GetString("username"), Type = "Success", Code = 0 });
                            }
                            else
                            {
                                return Json(new { Error = true, Description = "Login failed", Type = "Info", Code = 100 });
                            }
                        }
                        else
                        {
                            return Json(new { Error = true, Description = "Login failed", Type = "Warnign", Code = 200 });
                        }

                    }
                }
                catch (Exception ex)
                {
                    return Json(new { Error = true, Description = ex.ToString(), Type = "Danger", Code = -100 });
                }
            }
            else
            {
                return Json(new { Error = false, Description = "Campos vacios", Type = "Success", Code = 0 });
            }

        }

        // GET: Login/Edit/5
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}