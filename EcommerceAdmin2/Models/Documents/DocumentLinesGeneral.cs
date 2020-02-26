using EcommerceAdmin2.Models.Sistema;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Documents
{
    public class DocumentLinesGeneral
    {
        #region Propiedades
        public string DocEntry { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public DateTime DocDate { get; set; }
        public double PorcentDiscount { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public string ImageLink { get { return ConfigurationManager.AppSettings["Ecommerce_Domain"].ToString() + string.Format(@"/store/public/images/img_spl/productos/{0}/1.jpg" , ItemCode); } }
        public double Rate { get; set; }
        public double LineTotal { get; set; }
        public double LineSubTotal { get; set; }
        public double TotalFrgn { get; set; }
        public double VatPercent { get; set; }
        private DBSqlServer SqlServer { get; set; }
        private DBMysql DBMysql;
        #endregion
        #region Constructores
        public DocumentLinesGeneral()
        {

        }
        public DocumentLinesGeneral(DBSqlServer SqlServer)
        {
            this.SqlServer = SqlServer;
        }
        public DocumentLinesGeneral(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        #endregion
        #region Metodos
        public List<DocumentLinesGeneral> GetDocumentLinesEcomerce(string DocEntry)
        {
            List<DocumentLinesGeneral> ListDocumentLinesGeneral = new List<DocumentLinesGeneral>();
            string Statement = string.Format("SELECT * FROM Admin_CotizacionesDetalle  where id_cotizacion = '{0}';", DocEntry);
            try
            {
                MySqlDataReader DataReader = DBMysql.DoQuery(Statement);
                if (DataReader.HasRows)
                {
                    while (DataReader.Read())
                    {
                        ListDocumentLinesGeneral.Add(new DocumentLinesGeneral
                        {
                            ItemCode = DataReader.GetString(1),
                            Dscription = DataReader.GetString(2),
                            Quantity = (double)DataReader.GetDouble(3),
                            Currency = DataReader.GetString(6),
                            LineSubTotal = (double)DataReader.GetDouble(4),
                            LineTotal = (double)DataReader.GetDouble(5),
                            PorcentDiscount = (double)DataReader.GetDouble(7),
                        });
                    }
                    DataReader.Close();
                }
                return ListDocumentLinesGeneral;
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
        public List<DocumentLinesGeneral> GetDocumentLines(string DocEntry, string TypeDoc)
        {
            List<DocumentLinesGeneral> ListDocumentLinesGeneral = new List<DocumentLinesGeneral>();
            string sqlStatement = string.Format("EXEC Eco_GetDocumentLines @DocumentType = '{0}', @DocEntry = '{1}'", TypeDoc, DocEntry);
            try
            {
                SqlDataReader data = SqlServer.GetDataReader(sqlStatement);
                if (data.HasRows)
                {
                    while (data.Read())
                    {
                        ListDocumentLinesGeneral.Add(new DocumentLinesGeneral
                        {
                            ItemCode = data.GetString(0),
                            Dscription = data.GetString(1),
                            Quantity = double.Parse(data.GetDecimal(2) + ""),
                            Currency = data.GetString(3),
                            Price = double.Parse(data.GetDecimal(4) + ""),
                            VatPercent = double.Parse(data.GetDecimal(5) + ""),
                        });
                    }
                }
                else
                {
                    //sin registros
                }
                data.Close();
                return ListDocumentLinesGeneral;
            }
            catch (DBException ex)
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
