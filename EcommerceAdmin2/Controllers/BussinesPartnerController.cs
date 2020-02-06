using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAdmin2.Models.Sistema;
using EcommerceAdmin2.Models.Empleado;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using EcommerceAdmin2.Models.BussinesPartner;
using EcommerceAdmin2.Models;
using EcommerceAdmin2.Models.Filters;
using MySql.Data.MySqlClient;
using EcommerceAdmin2.Models.Produto;

namespace EcommerceAdmin2.Controllers
{
    [FilterSessionValid]
    public class BussinesPartnerController : Controller
    {
        private ResponseList<BussinesPartner> responseList;
        private Response response;

        [AccessViewSession]
        public IActionResult Index()
        {
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            using (Usuario usuario = new Usuario())
            {
                if(usuario.ValidPermisControlView(USR_IdSplinnet, 9))
                {
                    return View();
                }
                else if(usuario.ValidPermisControlView(USR_IdSplinnet, 10))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("NoAccess", "ErrorPages");
                }
            }
        }
        [AccessViewSession]
        public IActionResult List()
        {
            int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
            using (Usuario usuario = new Usuario())
            {
                if (usuario.ValidPermisControlView(USR_IdSplinnet, 9))
                {
                    return View();
                }
                else if (usuario.ValidPermisControlView(USR_IdSplinnet, 10))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("NoAccess", "ErrorPages");
                }
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ListUserLoged()
        {
            try
            {
                bool AccessBySalesEmp = false;
                bool AccessAll = false;
                int USR_IdSplinnet = (int)HttpContext.Session.GetInt32("USR_IdSplinnet");
                using (DBMysql dBMysql = new DBMysql("Splinet"))
                {
                    dBMysql.OpenConnection();
                    using (Usuario usuario = new Usuario(dBMysql))
                    {
                        // acceso a solo clientes por ejecutivo
                        AccessBySalesEmp = usuario.AccessToAction(USR_IdSplinnet, 9);
                        // acceso a todos los clientes
                        AccessAll = usuario.AccessToAction(USR_IdSplinnet, 10);
                    }
                    // obtener informacion de todos los clientes sin importar el ejecutivo
                    if (!AccessBySalesEmp && AccessAll)
                    {
                        responseList = new ResponseList<BussinesPartner> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<BussinesPartner>() };
                        using (DBSqlServer DBSqlServer = new DBSqlServer())
                        {
                            bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                            using (BussinesPartner BussinesPartner = new BussinesPartner(DBSqlServer))
                            {
                                responseList.Records = BussinesPartner.SelectAllactive();
                            }
                            DBSqlServer.CloseDataBaseAccess();
                        }
                        return Ok(responseList);
                    }
                    // obtener informacion de clientes por ejecutivo
                    else if (AccessBySalesEmp && !AccessAll)
                    {
                        using (Empleado empleado = new Empleado(dBMysql))
                        {
                            empleado.GetEmpleado(USR_IdSplinnet);
                            empleado.GetIdSapDB(USR_IdSplinnet);
                            using (DBSqlServer DBSqlServer = new DBSqlServer())
                            {
                                responseList = new ResponseList<BussinesPartner> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = new List<BussinesPartner>() };
                                bool IsConnectionDB = DBSqlServer.OpenDataBaseAccess();
                                using (BussinesPartner BussinesPartner = new BussinesPartner(DBSqlServer))
                                {
                                    BussinesPartner.GetBussinesPartnersBySalesEmp(empleado.Id_sap, responseList.Records);
                                }
                                DBSqlServer.CloseDataBaseAccess();
                            }
                            return Ok(responseList);
                        }
                    }
                    else
                    {
                        return BadRequest("Sin acceso a ninguna seccion, configuración de permisos erronea");
                    }
                    dBMysql.CloseConnection();
                }
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
        [AccessViewSession]
        [HttpGet]
        public IActionResult Dashboard360(string id)
        {
            ViewData["CardCode"] = id;
            return View();
        }
        [AccessDataSession]
        [HttpPost]
        public IActionResult DataGetBussinesPartner(string CardCode)
        {
            try
            {
                using (DBSqlServer DBSqlServer = new DBSqlServer())
                {
                    DBSqlServer.OpenDataBaseAccess();
                    BussinesPartner bussinesPartner = new BussinesPartner(DBSqlServer);
                    bool foundbp = bussinesPartner.GetBussinesPartner(CardCode);
                    DBSqlServer.CloseDataBaseAccess();
                    Response<BussinesPartner> response = new Response<BussinesPartner> { Code = 0,Description="The bussines partner was found", Objeto = bussinesPartner, Type = "success" };
                    return Ok(response);
                }
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
        [AccessDataSession]
        [HttpPost]
        public IActionResult DataGetArticulosTopByQuantity(string CardCode)
        {
            try
            {
                using (DBSqlServer DBSqlServer = new DBSqlServer())
                {
                    DBSqlServer.OpenDataBaseAccess();
                    Articulos articulo = new Articulos(DBSqlServer);
                    
                    ResponseList<Articulos> response = new ResponseList<Articulos> {
                        Code = 0,
                        Description = "The items was found",
                        Records = articulo.GetArticulosTopByQuantity(CardCode),
                        Type = "success"
                    };
                    DBSqlServer.CloseDataBaseAccess();
                    return Ok(response);
                }
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
        [AccessDataSession]
        [HttpPost]
        public IActionResult DataGetArticulosTopByPrice(string CardCode)
        {
            try
            {
                using (DBSqlServer DBSqlServer = new DBSqlServer())
                {
                    DBSqlServer.OpenDataBaseAccess();
                    Articulos articulo = new Articulos(DBSqlServer);
                    
                    ResponseList<Articulos> response = new ResponseList<Articulos>
                    {
                        Code = 0,
                        Description = "The items was found",
                        Records = articulo.GetArticulosTopByPrice(CardCode),
                        Type = "success"
                    };
                    DBSqlServer.CloseDataBaseAccess();
                    return Ok(response);
                }
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

    }
}