using EcommerceAdmin2.Models.Sistema;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.BussinesPartner
{
    public class Clientes
    {
        #region Propiedades
        private DBMysql DBMysql;

        public int Id_cliente { get;  set; }
        public string Nombre { get;  set; }
        public string Apellidos { get;  set; }
        public string Telefono { get;  set; }
        public string Email { get;  set; }
        public DateTime FechaReistro { get;  set; }
        public DateTime LastLogin { get;  set; }
        public string TipoCliente { get;  set; }
        public string CardCode { get;  set; }
        public string Sociedad { get;  set; }
        public int NoDocs { get;  set; }
        #endregion
        #region Construtores
        public Clientes()
        {

        }
        public Clientes(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        #endregion
        #region Metodos
        public bool GetById(int id_cliente)
        {
            string Statement = string.Format("select * from admin_clientes where id_cliente = '{0}'", id_cliente);
            MySqlDataReader data = null;
            bool isExists = false;
            try
            {
                data = DBMysql.DoQuery(Statement);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        Id_cliente = data.IsDBNull(0) ? 0 : data.GetInt32(0);
                        Nombre = data.IsDBNull(1) ? "" : data.GetString(1);
                        Apellidos = data.IsDBNull(2) ? "" : data.GetString(2);
                        Telefono = data.IsDBNull(3) ? "" : data.GetString(3);
                        Email = data.IsDBNull(4) ? "" : data.GetString(4);
                        FechaReistro = data.IsDBNull(5) ? DateTime.Now : data.GetDateTime(5);
                        LastLogin = data.IsDBNull(6) ? DateTime.Now : data.GetDateTime(6);
                        TipoCliente = data.IsDBNull(7) ? "" : data.GetString(7);
                        CardCode = data.IsDBNull(8) ? "" : data.GetString(8);
                        Sociedad = data.IsDBNull(9) ? "" : data.GetString(9);
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
                if(data != null)
                {
                    data.Close();
                }
            }
        }
        public int GetTotalClientesByType(string ModeBussiness)
        {
            int total;
            try
            {
                string Statement = string.Format("Admin_totalClientes|ModeBussiness@VARCHAR={0}", ModeBussiness);
                total = DBMysql.ExecuteProcedureInt(Statement, "TotalClientes");
                return total;
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
                
            }
        }
        public List<Clientes> GetQuoatationsDashboard(DateTime start, DateTime end, string ModeBussiness, string tipoDocumento)
        {
            string Statement = string.Format("Admin_QuotationsDashboard|startdate@DATETIME={0}&enddate@DATETIME={1}&tipoDocumento@VARCHAR={2}&ModeBussiness@VARCHAR={3}&ModeQuery@INT={4}",
                start.ToString("yyyy-MM-dd"),
                end.ToString("yyyy-MM-dd 23:59:59"),
                tipoDocumento,
                ModeBussiness,
                1);
            MySqlDataReader data = null;
            List<Clientes> List;
            try
            {
                data = DBMysql.ExecuteStoreProcedureReader(Statement);
                List = new List<Clientes>();
                while (data.Read())
                {
                    Clientes cliente = new Clientes();
                    cliente.Id_cliente = data.IsDBNull(0) ? 0 : data.GetInt32(0);
                    cliente.Nombre = data.IsDBNull(1) ? "" : data.GetString(1);
                    cliente.Apellidos = data.IsDBNull(2) ? "" : data.GetString(2);
                    cliente.NoDocs = data.IsDBNull(3) ? 0 : data.GetInt32(3);
                    cliente.CardCode = data.IsDBNull(4) ? "" : data.GetString(4);
                    List.Add(cliente);
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
