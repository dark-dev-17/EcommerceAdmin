using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAdmin2.Models.Address;
using EcommerceAdmin2.Models.Filters;
using EcommerceAdmin2.Models.Sistema;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace EcommerceAdmin2.Controllers
{
    public class AddressesController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDataSession]
        public IActionResult DataGetShipToAddressesIdEcom(int id)
        {
            try
            {
                using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                {
                    //open connection ton database ecommerce
                    dBMysql1.OpenConnection();
                    //start object cotizaciones with the connection starts 
                    AddressEcom address = new AddressEcom(dBMysql1);
                    bool result = address.GetShipToAddresses(id);
                    dBMysql1.CloseConnection();
                    if (result)
                    {
                        Response<AddressEcom> responseAddress = new Response<AddressEcom> { Code = 0, Description = "Informacion obtenida", Type = "success", Objeto = address };
                        return Ok(responseAddress);
                    }
                    else
                    {
                        throw new Exception("No existe la direccion de envio");
                    }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDataSession]
        public IActionResult DataGetBillToAddressesIdEcom(int id)
        {
            try
            {
                using (DBMysql dBMysql1 = new DBMysql("Ecommerce"))
                {
                    //open connection ton database ecommerce
                    dBMysql1.OpenConnection();
                    //start object cotizaciones with the connection starts 
                    AddressEcom address = new AddressEcom(dBMysql1);
                    bool result = address.GetBillToAddresses(id);
                    dBMysql1.CloseConnection();
                    if (result)
                    {
                        Response<AddressEcom> responseAddress = new Response<AddressEcom> { Code = 0, Description = "Informacion obtenida", Type = "success", Objeto = address };
                        return Ok(responseAddress);
                    }
                    else
                    {
                        throw new Exception("No existe la direccion de envio");
                    }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDataSession]
        public IActionResult DataGetBillToAddressesIdSAP(string CardCode, string AddressName)
        {
            try
            {
                using (DBSqlServer dBSqlServer = new DBSqlServer())
                {
                    //open connection ton database ecommerce
                    dBSqlServer.OpenDataBaseAccess();
                    //start object cotizaciones with the connection starts 
                    AddressSap address = new AddressSap(dBSqlServer);
                    address.GetBillToAddresses(CardCode).Where(add => add.AddressName == AddressName).ToList().ForEach(add => address = add);
                    dBSqlServer.CloseDataBaseAccess();
                    Response<AddressSap> responseAddress = new Response<AddressSap> { Code = 0, Description = "Informacion obtenida", Type = "success", Objeto = address };
                    return Ok(responseAddress);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessDataSession]
        public IActionResult DataGetShipToAddressesIdSAP(string CardCode, string AddressName)
        {
            try
            {
                using (DBSqlServer dBSqlServer = new DBSqlServer())
                {
                    //open connection ton database ecommerce
                    dBSqlServer.OpenDataBaseAccess();
                    //start object cotizaciones with the connection starts 
                    AddressSap address = new AddressSap(dBSqlServer);
                    address.GetShipToAddresses(CardCode).Where(add => add.AddressName == AddressName).ToList().ForEach(add => address = add);
                    dBSqlServer.CloseDataBaseAccess();
                    Response<AddressSap> responseAddress = new Response<AddressSap> { Code = 0, Description = "Informacion obtenida", Type = "success", Objeto = address };
                    return Ok(responseAddress);
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