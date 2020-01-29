using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Sistema
{
    public class Modulo
    {
        #region Propiedades
        public int Id {  set; get; }
        public int IdUser {  set; get; }
        public string Description {  set; get; }
        public bool isAccessAdmin {  set; get; }
        public List<Acciones> Acciones {  set; get; }
        private DBMysql DBMysql;
        #endregion
        #region Constructores
        public Modulo()
        {

        }
        public Modulo(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        #endregion
        #region Metodos
        public List<Modulo> SelectAll()
        {
            try
            {
                string Statement = string.Format("SELECT * FROM t01_admin_objeto;");
                List<Modulo> Lista = new List<Modulo>();
                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                {
                    while (DataReader.Read())
                    {
                        Modulo modulo = new Modulo();
                        modulo.Id = (int)DataReader.GetUInt32(0);
                        modulo.Description = DataReader.IsDBNull(1) ? "" : DataReader.GetString(1);
                        Lista.Add(modulo);
                    }
                }
                DataReader.Close();
                Lista.ToList().ForEach(p => p.Acciones = new Acciones(DBMysql).SelectAll(p.Id));
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
        #endregion

    }
}
