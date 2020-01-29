using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceAdmin2.Models.Empleado;
using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

namespace EcommerceAdmin2.Controllers
{
    public class EmpleadoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DataListAll()
        {
            try
            {
                ResponseList<Empleado> responseList = new ResponseList<Empleado> { Code = 0, Description = "Autorization to access", Type = "Suscess" };
                // conectar abase de  datos splittel
                DBMysql dBMysql = new DBMysql("Splinet");
                dBMysql.OpenConnection();
                responseList.Records = new Empleado(dBMysql).SelectAllSplit();
                // obtener articulos dados de alta en ecommercce
                dBMysql.CloseConnection();
                return Ok(responseList);
            }
            catch (DBException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MySqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //public IActionResult GetInformation()
        //{
        //    var response = Json(new { });
        //    try
        //    {
        //        using (DBMysql dBMysql = new DBMysql("Splitel"))
        //        {
        //            dBMysql.OpenConnection();
        //            if (dBMysql.Connection != null)
        //            {
        //                using (Empleado Empleado = new Empleado(dBMysql))
        //                {
        //                    int id = (int) HttpContext.Session.GetInt32("User_id");
        //                    int valueProcess = Empleado.GetEmpleado(id);
        //                    if (valueProcess == 0)
        //                    {
        //                        response = Json(new { Error = false, Description = "Información obtenida", Type = "Success", Code = 0, Data = Empleado });
        //                    }
        //                    else
        //                    {
        //                        response = Json(new { Error = true, Description = "Empleado no encontrado", Type = "Warnign", Code = 200 });
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                response = Json(new { Error = true, Description = "Ups!!, Por favor vuelve a intentarlo ", Type = "Warnign", Code = 200 });
        //            }
        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        response = Json(new { Error = true, Description = "Ups!!, Por favor vuelve a intentarlo ", Type = "Warnign", Code = 200 });
        //    }
        //    return response;
        //}
    }
}