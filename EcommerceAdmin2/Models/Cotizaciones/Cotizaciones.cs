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
        #endregion
    }
}
