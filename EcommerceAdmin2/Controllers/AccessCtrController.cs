using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAdmin2.Models.Filters;
using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace EcommerceAdmin2.Controllers
{
    public class AccessCtrController : Controller
    {
        [AccessView(IdAction = 0)]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [AccessData(IdAction = 0)]
        public IActionResult DataGetAccionsByUser(int user)
        {
            try
            {
                // conectar abase de  datos splittel
                DBMysql dBMysql = new DBMysql("Splinet");
                dBMysql.OpenConnection();
                List<Modulo> modelo = new Modulo(dBMysql).SelectAll();
                ResponseList<Modulo> response = new ResponseList<Modulo> { Code = 0, Description = "Autorization to access", Type = "Suscess", Records = modelo };
                modelo.ForEach(p1 => {
                    p1.IdUser = user;
                    p1.Acciones.ForEach(a => a.isAccess = new Acciones(dBMysql).CheckPermissToUser(p1.IdUser, a.Id));
                }
                );
                dBMysql.CloseConnection();
                return Ok(response);
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
        [HttpPost]
        [AccessData(IdAction = 0)]
        [ValidateAntiForgeryToken]
        public IActionResult DataChangePermissions([FromBody]List<Modulo> Permissions)
        {
            try
            {
                // conectar abase de  datos splittel
                DBMysql dBMysql = new DBMysql("Splinet");
                dBMysql.OpenConnection();
                Permissions.ForEach(m => m.Acciones.ForEach(a => {
                    a.SetConnectionMYsql(dBMysql);
                    a.ChangePermissToUser(m.IdUser, a.Id, (a.isAccess ? 1 : 0));
                }));
                Response response = new Response{ Code = 0, Description = "Autorization to access", Type = "Suscess"};
                //modelo.ForEach(p1 => p1.Acciones.ForEach(a => a.isAccess = new Acciones(dBMysql).CheckPermissToUser(user, a.Id)));
                dBMysql.CloseConnection();
                return Ok(response);
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