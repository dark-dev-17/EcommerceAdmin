using EcommerceAdmin2.Models.Sistema;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Cotizaciones
{
    public class Cotizacion
    {
        #region Propiedades
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public DateTime DocDate { get; set; }
        public double DocTotal { get; set; }
        public double DocSubTotal { get; set; }
        public double DocIva { get; set; }
        public double DocRate { get; set; }
        public string DocType { get; set; }
        public string TypeCustomer { get; set; }
        public string CardCode { get; set; }
        public string Cardname { get; set; }
        public string DocCur { get; set; }
        public string TrackNo { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string ShipTo { get; set; }
        public string BillTo { get; set; }
        public string CFDIUser { get; set; }
        public int PorcentDisaccount { get; set; }
        public int Id_cliente { get; set; }
        public int TransportationCode { get; set; }
        public string DocNumEcommerce { get; set; }
        private DBMysql DBMysql;
        #endregion
        #region Constructores
        public Cotizacion()
        {

        }
        public Cotizacion(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        #endregion
        #region Metodos
        public bool GetById(int DocNumEcommerce)
        {
            string Statement = string.Format("select * from cotizacion_encabezado where id = '{0}'", DocNumEcommerce);
            MySqlDataReader data = null;
            bool isExists = false;
            try
            {
                data = DBMysql.DoQuery(Statement);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        Id_cliente = data.IsDBNull(1) ? 0 : data.GetInt32(1);
                        DocSubTotal = data.IsDBNull(2) ? 0 : data.GetDouble(2);
                        DocIva = data.IsDBNull(3) ? 0 : data.GetDouble(3);
                        DocTotal = data.IsDBNull(4) ? 0 : data.GetDouble(4);
                        DocDate = data.IsDBNull(7) ? DateTime.Now : data.GetDateTime(7);
                        Status = data.IsDBNull(9) ? "" : data.GetString(9);
                        PaymentMethod = data.IsDBNull(10) ? "" : data.GetString(10);
                        DocCur = data.IsDBNull(11) ? "" : data.GetString(11);
                        ShipTo = data.IsDBNull(12) ? "" : data.GetString(12);
                        BillTo = data.IsDBNull(13) ? "" : data.GetString(13);
                        TransportationCode = data.IsDBNull(15) ? 0 : data.GetInt32(15);
                        DocRate = data.IsDBNull(16) ? 0 : data.GetDouble(16);
                        CFDIUser = data.IsDBNull(18) ? "" : data.GetString(18);
                        PorcentDisaccount = data.IsDBNull(19) ? 0 : data.GetInt32(19);
                        if(DocCur != "USD")
                        {
                            DocIva = DocIva * DocRate;
                            DocTotal = DocTotal * DocRate;
                            DocSubTotal = DocSubTotal * DocRate;
                        }
                    }
                    isExists = true;
                }
                return isExists;
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (data != null)
                {
                    data.Close();
                }
            }
        }
        public int GetNoCotizaciones(DateTime start, DateTime end, string ModeBussiness)
        {
            // SELECT COUNT(*) AS TOTAL FROM cotizacion_encabezado where estatus = 'P';
            string Statement = string.Format("AdminNoDocuments|startdate@DATETIME={0}&enddate@DATETIME={1}&tipoDocumento@VARCHAR={2}&ModeBussiness@VARCHAR={3}", start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd 23:59:59"), "COTIZACION", ModeBussiness);
            try
            {
                int Result = 0;
                Result = DBMysql.ExecuteProcedureInt(Statement, "totalDoc");
                return Result;
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetNoPedidos(DateTime start, DateTime end, string ModeBussiness)
        {
            // 2020-01-06 03:14:08
            // 2020-01-06 03:14:08
            string Statement = string.Format("AdminNoDocuments|startdate@DATETIME={0}&enddate@DATETIME={1}&tipoDocumento@VARCHAR={2}&ModeBussiness@VARCHAR={3}", start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd 23:59:59"), "PEDIDO", ModeBussiness);
            try
            {
                int Result = 0;
                Result = DBMysql.ExecuteProcedureInt(Statement, "totalDoc");
                return Result;
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetTotalPedidos(DateTime start, DateTime end,string Currency,string ModeBussiness)
        {
            // 
            string Statement = string.Format("Admin_TotalPedidos|startdate@DATETIME={0}&enddate@DATETIME={1}&moneda@VARCHAR={2}&ModeBussiness@VARCHAR={3}", start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd 23:59:59"), Currency, ModeBussiness);
            try
            {
                double Result = 0;
                Result = DBMysql.ExecuteProcedureDouble(Statement, "totalCost");
                return Result;
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Cotizacion> GetOrdersInPendingByBP(string CardCode)
        {
            string Statement = string.Format("Admin_GetOrderssInPending|CardCode@VARCHAR={0}&ModeQuery@VARCHAR={1}", CardCode, "Single");
            MySqlDataReader data = null;
            List<Cotizacion> List;
            try
            {
                data = DBMysql.ExecuteStoreProcedureReader(Statement);
                List = new List<Cotizacion>();
                while (data.Read())
                {
                    Cotizacion cotizacion = new Cotizacion();
                    cotizacion.DocNumEcommerce = data.GetUInt32(0) + "";
                    cotizacion.DocDate = data.GetDateTime(1);
                    cotizacion.DocTotal = data.GetDouble(2); 
                    cotizacion.DocCur = data.GetString(3);
                    cotizacion.CardCode = data.GetString(4);
                    List.Add(cotizacion);
                }
                return List;
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(data != null)
                {
                    data.Close();
                }
            }
        }
        public List<Cotizacion> GetOrdersInPending()
        {
            string Statement = string.Format("Admin_GetOrderssInPending|CardCode@VARCHAR={0}&ModeQuery@VARCHAR={1}", "a", "Multiple");
            MySqlDataReader data = null;
            List<Cotizacion> List;
            try
            {
                data = DBMysql.ExecuteStoreProcedureReader(Statement);
                List = new List<Cotizacion>();
                while (data.Read())
                {
                    Cotizacion cotizacion = new Cotizacion();
                    cotizacion.DocNumEcommerce = data.GetUInt32(0) + "";
                    cotizacion.DocDate = data.GetDateTime(1);
                    cotizacion.DocTotal = data.GetDouble(2);
                    cotizacion.DocCur = data.GetString(3);
                    cotizacion.CardCode = data.GetString(4);
                    List.Add(cotizacion);
                }
                return List;
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (data != null)
                {
                    data.Close();
                }
            }
        }
        public List<Cotizacion> GetQuoatations()
        {
            string Statement = string.Format("Admin_GetQuoatations|CardCode@VARCHAR={0}&ModeQuery@VARCHAR={1}", "a", "Multiple");
            MySqlDataReader data = null;
            List<Cotizacion> List;
            try
            {
                data = DBMysql.ExecuteStoreProcedureReader(Statement);
                List = new List<Cotizacion>();
                while (data.Read())
                {
                    Cotizacion cotizacion = new Cotizacion();
                    cotizacion.DocNumEcommerce = data.IsDBNull(0) ? "" : (data.GetInt32(0) + "");
                    cotizacion.CardCode = data.IsDBNull(8) ? "" : data.GetString(8);
                    cotizacion.TypeCustomer = data.IsDBNull(10) ? "" : data.GetString(10);
                    cotizacion.Status = data.IsDBNull(11) ? "" : data.GetString(11);
                    cotizacion.Cardname = "";
                    cotizacion.DocSubTotal = data.IsDBNull(2) ? 0 : data.GetDouble(2);
                    cotizacion.DocIva = data.IsDBNull(3) ? 0 : data.GetDouble(3);
                    cotizacion.DocTotal = data.IsDBNull(4) ? 0 : data.GetDouble(4);
                    cotizacion.DocDate = data.GetDateTime(5);
                    cotizacion.DocRate = data.GetDouble(6);
                    cotizacion.DocCur = "USD";
                    List.Add(cotizacion);
                }
                return List;
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (data != null)
                {
                    data.Close();
                }
            }
        }
        public List<Cotizacion> GetQuoatations(string CardCode)
        {
            string Statement = string.Format("Admin_GetQuoatations|CardCode@VARCHAR={0}&ModeQuery@VARCHAR={1}", CardCode, "Single");
            MySqlDataReader data = null;
            List<Cotizacion> List;
            try
            {
                data = DBMysql.ExecuteStoreProcedureReader(Statement);
                List = new List<Cotizacion>();
                while (data.Read())
                {
                    Cotizacion cotizacion = new Cotizacion();
                    cotizacion.DocNumEcommerce = data.IsDBNull(0) ? "" : (data.GetInt32(0) + "");
                    cotizacion.CardCode = data.IsDBNull(8) ? "" : data.GetString(8);
                    cotizacion.TypeCustomer = data.IsDBNull(10) ? "" : data.GetString(10);
                    cotizacion.Status = data.IsDBNull(11) ? "" : data.GetString(11);
                    cotizacion.Cardname = "";
                    cotizacion.DocSubTotal = data.IsDBNull(2) ? 0 : data.GetDouble(2);
                    cotizacion.DocIva = data.IsDBNull(3) ? 0 : data.GetDouble(3);
                    cotizacion.DocTotal = data.IsDBNull(4) ? 0 : data.GetDouble(4);
                    cotizacion.DocDate = data.GetDateTime(5);
                    cotizacion.DocRate = data.GetDouble(6);
                    cotizacion.DocCur = "USD";
                    List.Add(cotizacion);
                }
                return List;
            }
            catch (DBException ex)
            {
                throw ex;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (data != null)
                {
                    data.Close();
                }
            }
        }
        
        #endregion
    }
}
