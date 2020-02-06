using EcommerceAdmin2.Models.Sistema;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Produto
{
    public class Articulos
    {
        #region Propiedades
        public int Id { get; private set; }
        public string ItemCode { get; private set; }
        public string Description { get; private set; }
        public string LargeDescription { get; private set; }
        public Categoria Category { get; private set; }
        public SubCategoria SubCategory { get; private set; }
        public FichaTecnica FichaTecnica { get; private set; }
        public string DataSheetPath { get; private set; }
        public double UnitPrice { get; private set; }
        public double Discount { get; private set; }
        public double Stock { get; private set ; }
        public double Quantity { get; private set ; }
        public bool IsActiveEcomerce { get; private set; }
        private DBSqlServer SqlServer { get; set; }
        private DBMysql DBMysql;
        #endregion
        #region Constructores
        public Articulos()
        {

        }
        public Articulos(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        public Articulos(DBSqlServer SqlServer)
        {
            this.SqlServer = SqlServer;
        }
        #endregion
        #region Metodos  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        public bool FindByID(string ItemCode)
        {
            string Statement = string.Format("SELECT * FROM Admin_producto_categoria_subcategoria where codigo = '{0}';", ItemCode);
            try
            {
                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                if (DataReader.HasRows)
                {
                    while (DataReader.Read())
                    {
                        this.Id = (int)DataReader.GetUInt32(0);
                        this.ItemCode = DataReader.IsDBNull(1) ? "" : DataReader.GetString(1);
                        this.Description = DataReader.IsDBNull(2) ? "" : DataReader.GetString(2);
                        this.UnitPrice = DataReader.IsDBNull(3) ? 0 : DataReader.GetFloat(3);
                        this.Discount = DataReader.IsDBNull(15) ? 0 : DataReader.GetFloat(15);//
                        this.Stock = DataReader.IsDBNull(4) ? 0 : DataReader.GetFloat(4);
                        this.IsActiveEcomerce = DataReader.IsDBNull(16) ? false : DataReader.GetString(16) == "si" ? true : false;//
                        this.Category = new Categoria { Description = DataReader.IsDBNull(6) ? "" : DataReader.GetString(6), Id = DataReader.IsDBNull(5) ? 0 : DataReader.GetInt32(5) };
                        this.SubCategory = new SubCategoria { Description = DataReader.IsDBNull(10) ? "" : DataReader.GetString(10), Id = DataReader.IsDBNull(8) ? 0 : DataReader.GetInt32(8) };
                        this.LargeDescription = DataReader.IsDBNull(14) ? "Sin descripción" : DataReader.GetString(14);
                    }
                    DataReader.Close();
                    this.FichaTecnica = new FichaTecnica(DBMysql).GetDataSheet(ItemCode);
                    return true;
                }
                else
                {
                    return false;
                }
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Articulos> SelectAll()
        {
            List<Articulos> Lista = new List<Articulos>();
            string Statement = string.Format("SELECT * FROM Admin_producto_categoria_subcategoria;");
            try
            {
                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                if (DataReader.HasRows)
                {
                    while (DataReader.Read())
                    {
                        Articulos artic = new Articulos();
                        artic.Id = (int)DataReader.GetUInt32(0);
                        artic.ItemCode = DataReader.IsDBNull(1) ? "" : DataReader.GetString(1);
                        artic.Description = DataReader.IsDBNull(2) ? "" : DataReader.GetString(2);
                        artic.UnitPrice = DataReader.IsDBNull(3) ? 0 : DataReader.GetFloat(3);
                        artic.Discount = DataReader.IsDBNull(15) ? 0 : DataReader.GetFloat(15);//
                        artic.Stock = DataReader.IsDBNull(4) ? 0 : DataReader.GetFloat(4);
                        artic.IsActiveEcomerce = DataReader.IsDBNull(16) ? false : DataReader.GetString(16) == "si" ? true : false;//
                        artic.Category = new Categoria { Description = DataReader.IsDBNull(6) ? "" : DataReader.GetString(6), Id = DataReader.IsDBNull(5) ? 0 : DataReader.GetInt32(5) };
                        artic.SubCategory = new SubCategoria { Description = DataReader.IsDBNull(10) ? "" : DataReader.GetString(10), Id = DataReader.IsDBNull(8) ? 0 : DataReader.GetInt32(8) };
                        artic.LargeDescription = DataReader.IsDBNull(14) ? "Sin descripción" : DataReader.GetString(14);
                        artic.FichaTecnica = new FichaTecnica { Id = DataReader.IsDBNull(12) ? 0 : DataReader.GetInt32(12), Ruta = DataReader.IsDBNull(13) ? "" : DataReader.GetString(13) };
                        Lista.Add(artic);
                    }
                }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="active"></param>
        /// <param name="ItemCode"></param>
        /// <returns></returns>
        public bool ActiveEcomerce(bool active,string ItemCode)
        {
            string Statement = string.Format("update catalogo_productos set activo = '{0}'  where codigo = '{1}';",(active ? "si" : "no"), ItemCode);
            try
            {
                int Result = DBMysql.DoNonQuery(Statement);
                if(Result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemCode"></param>
        /// <param name="LargeDescription"></param>
        /// <returns></returns>
        public bool UpdateLargeDescription(string ItemCode, string LargeDescription)
        {
            string Statement = string.Format("update `catalogo_descripciones` as t01 set t01.desc_larga = '{0}'  where t01.id_desc_larga = (select id_desc_larga from catalogo_productos where codigo = '{1}')", LargeDescription, ItemCode);
            try
            {
                int Result = DBMysql.DoNonQuery(Statement);
                if (Result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        public List<Articulos> GetArticulosTopByQuantity(string CardCode)
        {
            string statement = string.Format("EXEC Eco_GetTop5ItemsByQuantityByCustomer @CardCode = '{0}'", CardCode);
            SqlDataReader data = null;
            List<Articulos> List = null;
            try
            {
                data = SqlServer.GetDataReader(statement);
                if (!data.HasRows)
                {
                    throw new DBException("No existen registros");
                }
                List = new List<Articulos>();
                while (data.Read())
                {
                    List.Add(new Articulos
                    {
                        ItemCode = data.GetString(0),
                        Description = data.GetString(1),
                        Quantity = double.Parse(data.GetDecimal(2) + ""),
                    }); ;
                }
                return List;
            }
            catch (DBException ex)
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
        public List<Articulos> GetArticulosTopByPrice(string CardCode)
        {
            string statement = string.Format("EXEC Eco_GetTop5ItemsByPriceByCustomer @CardCode = '{0}'", CardCode);
            SqlDataReader data = null;
            List<Articulos> List = null;
            try
            {
                data = SqlServer.GetDataReader(statement);
                if (!data.HasRows)
                {
                    throw new DBException("No existen registros");
                }
                List = new List<Articulos>();
                while (data.Read())
                {
                    List.Add(new Articulos
                    {
                        ItemCode = data.GetString(0),
                        Description = data.GetString(1),
                        UnitPrice = double.Parse(data.GetDecimal(2) + ""),
                    }); ;
                }
                return List;
            }
            catch (DBException ex)
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
        #endregion
    }
}
