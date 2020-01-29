using EcommerceAdmin2.Models.Sistema;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Produto
{
    public class FichaTecnica
    {
        public int Id { get; set; }
        public string Ruta { get; set; }
        private DBMysql DBMysql;
        public FichaTecnica()
        {

        }
        public FichaTecnica(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        public FichaTecnica GetDataSheet(string ItemCode)
        {
            string Statement = string.Format("select * from Admin_ProductoFichaTecnica  where codigo = '{0}';", ItemCode);
            try
            {
                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                while (DataReader.Read())
                {
                    Id = (int)DataReader.GetUInt32(0);
                    Ruta = DataReader.IsDBNull(2) ? "" : DataReader.GetString(2) + ".pdf";
                }
                Ruta = @"http://fibremex.co/store/public/images/img_spl/" + Ruta;
                return this;
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
    }
}
