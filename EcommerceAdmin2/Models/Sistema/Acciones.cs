using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Sistema
{
    public class Acciones
    {
        #region Propiedades
        public int Id {  set; get; }
        public string Description {  set; get; }
        public bool isAccess {  set; get; }
        private DBMysql DBMysql;
        #endregion
        #region Constructores
        public Acciones()
        {

        }
        public Acciones(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        #endregion
        #region Metodos
        public void SetConnectionMYsql(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        public List<Acciones> SelectAll(int idModulo)
        {
            try
            {
                string Statement = string.Format("SELECT * FROM t02_admin_acciones where t01_pk01 = '{0}'", idModulo);
                List<Acciones> Lista = new List<Acciones>();
                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                while (DataReader.Read())
                {
                    Acciones acciones = new Acciones();
                    acciones.Id = (int)DataReader.GetUInt32(0);
                    acciones.Description = DataReader.IsDBNull(1) ? "" : DataReader.GetString(1);
                    acciones.isAccess = false;
                    Lista.Add(acciones);
                }
                DataReader.Close();
                return Lista;
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
        public bool CheckPermissToUser(int idUser, int IdAccion)
        {
            try
            {
                bool access = false;
                string Statement = string.Format("SELECT * FROM t03_permisos where clienteKey = '{0}' and t02_pk01 = '{1}' and t03_f001 = '1';", idUser, IdAccion);
                List<Acciones> Lista = new List<Acciones>();
                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                if (DBMysql.CountDataReader(DataReader) == 1)
                    access = true;
                else
                    access = false;
                DataReader.Close();
                return access;
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
        public bool ChangePermissToUser(int idUser, int IdAccion, int permiss)
        {
            try
            {
                bool access = false;
                string Statement = string.Format("AdminEcom_changeAccessActions|ID_ACTION@INT={0}&ID_USER@INT={1}&ACCESS@INT={2}", IdAccion, idUser, permiss);
                bool DataReader = DBMysql.ExecuteProcedure(Statement,"");
                return access;
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
