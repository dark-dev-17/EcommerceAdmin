using EcommerceAdmin2.Models.Sistema;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Produto
{
    public class Categoria
    {
        public int Id { get;  set; }
        public string Id_categoria { get; set; }
        public string Description { get;  set; }
        public int Total { get;  set; }
        private DBMysql DBMysql;
        #region Construtores
        public Categoria()
        {

        }
        public Categoria(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        #endregion
        #region Metodos
        public List<Categoria> GetTopCategoria(DateTime start, DateTime end, string ModeBussiness)
        {
            // SELECT COUNT(*) AS TOTAL FROM cotizacion_encabezado where estatus = 'P';
            string Statement = string.Format("Admin_topCategoriasCotizaciones|startdate@DATETIME={0}&enddate@DATETIME={1}&ModeBussiness@VARCHAR={2}", start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd 23:59:59"), ModeBussiness);
            MySqlDataReader DataReader = null;
            List<Categoria> Articulos = null;
            try
            {
                DataReader = DBMysql.ExecuteStoreProcedureReader(Statement);
                Articulos = new List<Categoria>();
                while (DataReader.Read())
                {
                    Categoria categoria = new Categoria();
                    categoria.Id_categoria = DataReader.IsDBNull(0) ? "" : (DataReader.GetString(0) + "");
                    categoria.Description = DataReader.IsDBNull(1) ? "" : DataReader.GetString(1);
                    categoria.Total = DataReader.IsDBNull(2) ? 0 : (int)DataReader.GetDouble(2);
                    Articulos.Add(categoria);
                }
                return Articulos;
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
                if(DataReader != null)
                {
                    DataReader.Close();
                }
            }
        }
        public List<Categoria> GetQuoatationsDashboard(DateTime start, DateTime end, string ModeBussiness, string tipoDocumento)
        {
            string Statement = string.Format("Admin_QuotationsDashboard|startdate@DATETIME={0}&enddate@DATETIME={1}&tipoDocumento@VARCHAR={2}&ModeBussiness@VARCHAR={3}&ModeQuery@INT={4}",
                start.ToString("yyyy-MM-dd"),
                end.ToString("yyyy-MM-dd 23:59:59"),
                tipoDocumento,
                ModeBussiness,
                2);
            MySqlDataReader data = null;
            List<Categoria> List;
            try
            {
                data = DBMysql.ExecuteStoreProcedureReader(Statement);
                List = new List<Categoria>();
                while (data.Read())
                {
                    Categoria categoria = new Categoria();
                    categoria.Id_categoria = data.IsDBNull(0) ? "" : (data.GetString(0) + "");
                    categoria.Description = data.IsDBNull(1) ? "" : data.GetString(1);
                    categoria.Total = data.IsDBNull(2) ? 0 : (int)data.GetDouble(2);
                    List.Add(categoria);
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
