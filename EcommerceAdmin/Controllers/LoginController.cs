using EcommerceAdmin.Models.Empleado;
using EcommerceAdmin.Models.Sistema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceAdmin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create(Usuario Usuario)
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
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return RedirectToAction("LoginFailed", "Login", Usuario);
                        }
                    }
                    else
                    {
                        Usuario.SetMessage(new Response { Code = -100, Description = "Por favor intenta de nuevo", Type = "Error" });
                        return RedirectToAction("LoginFailed", "Login", Usuario);
                    }

                }
            }
            catch (Exception ex)
            {
                Usuario.SetMessage(new Response { Code = -100, Description = ex.Message, Type = "Error" });
                return RedirectToAction("LoginFailed", "Login", Usuario);
            }
        }

        // POST: Login/Create
        [HttpPost]
        public ActionResult Create()
        {
            //int Response;
            //try
            //{
            //    using (DBMysql dBMysql = new DBMysql())
            //    {
            //        dBMysql.OpenConnection();
            //        if (dBMysql.Connection != null)
            //        {
            //            Usuario.SetConnectionMysql(dBMysql);
            //            Response = Usuario.DoLogin();
            //            if (Response == 0)
            //            {
            //                return RedirectToAction("Index", "Home");
            //            }
            //            else
            //            {
            //                return RedirectToAction("LoginFailed", "Login", Usuario);
            //            }
            //        }
            //        else
            //        {
            //            Usuario.SetMessage(new Response { Code = -100, Description = "Por favor intenta de nuevo", Type = "Error" });
            //            return RedirectToAction("LoginFailed", "Login", Usuario);
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    Usuario.SetMessage(new Response { Code = -100, Description = ex.Message, Type = "Error" });
            //    return RedirectToAction("LoginFailed", "Login", Usuario);
            //}
            return View();
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
