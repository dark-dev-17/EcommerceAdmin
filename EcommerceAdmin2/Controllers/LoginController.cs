using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAdmin2.Models.Empleado;
using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using EcommerceAdmin2.Models;

namespace EcommerceAdmin2.Controllers
{
    
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromBody]Usuario Usuario)
        {
            var response = Json(new { }) ;
            if (ModelState.IsValid)
            {
                try
                {
                    using (DBMysql dBMysql = new DBMysql("Splinet"))
                    {
                        dBMysql.OpenConnection();
                        Usuario.SetConnectionMysql(dBMysql);
                        int Response = Usuario.DoLogin();
                        if (Response == 0)
                        {
                            StartSessions(Usuario.GetId(), dBMysql);
                            response = Json(new { Error = false, Description = "Login success", Type = "Success", Code = 0 });
                        }
                        else
                        {
                            response = Json(new { Error = true, Description = "Usuario o contraseña incorrecta", Type = "Info", Code = 100 });
                        }
                    }
                }
                catch (DBException ex)
                {
                    response = Json(new { Error = true, Description = ex.Message, Type = "Danger", Code = -100 });
                }
                catch (MySqlException ex)
                {
                    response = Json(new { Error = true, Description = ex.Message, Type = "Danger", Code = -100 });
                }
                catch (Exception ex)
                {
                    response = Json(new { Error = true, Description = ex.Message, Type = "Danger", Code = -100 });
                }
            }
            else
            {
                response = Json(new { Error = false, Description = "Campos vacios", Type = "Success", Code = 0 });
            }
            return response;

        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult GetSession()
        {
            if (HttpContext.Session.GetInt32("User_id") != null)
            {
                return Json(new { Error = false, Description = "The session is active", Type = "Success", Code = 0, SessionID = HttpContext.Session.GetInt32("User_id") });
            }
            else
            {
                return Json(new { Error = true, Description = "Sin sessión activa", Type = "Info", Code = 100 });
            }
        }
        private void StartSessions(int id, DBMysql DBMysql)
        {
            using (Empleado Empleado = new Empleado(DBMysql))
            {
                Empleado.GetEmpleado(id);
                HttpContext.Session.SetInt32("USR_IdSplinnet", Empleado.IdSplinnet);
                HttpContext.Session.SetString("USR_Nombre", Empleado.Nombre);
                HttpContext.Session.SetString("USR_ApellidoPaterno", Empleado.ApellidoPaterno);
                HttpContext.Session.SetString("USR_Apellidomaterno", Empleado.Apellidomaterno);
                HttpContext.Session.SetString("USR_Correo", Empleado.Correo);
                HttpContext.Session.SetInt32("USR_IdArea", Empleado.IdArea);
                HttpContext.Session.SetString("USR_Sociedad", Empleado.Sociedad);
            }
        }
    }
}