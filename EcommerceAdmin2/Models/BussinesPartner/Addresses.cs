using EcommerceAdmin2.Models.Sistema;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.BussinesPartner
{
    public class Addresses
    {
        #region Propiedades
        public string Street { get; set; }
        public string StreetNo { get; set; }
        public string Block { get; set; }
        public string County { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Adress { get; set; }
        public string FederalTaxID { get; set; }
        public string CardName { get; set; }
        public bool Default { get; set; }
        public string AddressType { get; set; }
        public string AddressName { get; set; }
        private DBSqlServer SqlServer { get; set; }
        #endregion
        #region Constructores
        public Addresses()
        {

        }
        public Addresses(DBSqlServer SqlServer)
        {
            this.SqlServer = SqlServer;
        }
        #endregion
        #region Metodos
        public List<Addresses> GetAddressesShipToByBP(string CardCode)
        {
            List<Addresses> List;
            SqlDataReader data = null; ;
            string statement = string.Format("exec Eco_GetAddressByCustomer @CardCode = '{0}', @AdresType = 'S'", CardCode);
            try
            {
                data = SqlServer.GetDataReader(statement);
                List = new List<Addresses>();
                while (data.Read())
                {
                    Addresses bp = new Addresses();
                    bp.Adress = data.IsDBNull(0) ? "" : data.GetString(0) + "";
                    bp.Street = data.IsDBNull(1) ? "" : data.GetString(1) + "";
                    bp.StreetNo = data.IsDBNull(2) ? "" : data.GetString(2) + "";
                    bp.Block = data.IsDBNull(3) ? "" : data.GetString(3) + "";
                    bp.County = data.IsDBNull(4) ? "" : data.GetString(4) + "";
                    bp.ZipCode = data.IsDBNull(5) ? "" : data.GetString(5) + "";
                    bp.State = data.IsDBNull(6) ? "" : data.GetString(6) + "";
                    bp.FederalTaxID = data.IsDBNull(7) ? "" : data.GetString(7) + "";
                    bp.City = data.IsDBNull(8) ? "" : data.GetString(8) + "";
                    bp.CardName = data.IsDBNull(9) ? "" : data.GetString(9) + "";
                    bp.Default = data.GetString(10) + "" == "default" ? true : false;
                    List.Add(bp);
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
        public List<Addresses> GetAddressesBillToByBP(string CardCode)
        {
            List<Addresses> List;
            SqlDataReader data = null; ;
            string statement = string.Format("exec Eco_GetAddressByCustomer @CardCode = '{0}', @AdresType = 'B'", CardCode);
            try
            {
                data = SqlServer.GetDataReader(statement);
                List = new List<Addresses>();
                while (data.Read())
                {
                    Addresses bp = new Addresses();
                    bp.Adress = data.IsDBNull(0) ? "" : data.GetString(0) + "";
                    bp.Street = data.IsDBNull(1) ? "" : data.GetString(1) + "";
                    bp.StreetNo = data.IsDBNull(2) ? "" : data.GetString(2) + "";
                    bp.Block = data.IsDBNull(3) ? "" : data.GetString(3) + "";
                    bp.County = data.IsDBNull(4) ? "" : data.GetString(4) + "";
                    bp.ZipCode = data.IsDBNull(5) ? "" : data.GetString(5) + "";
                    bp.State = data.IsDBNull(6) ? "" : data.GetString(6) + "";
                    bp.FederalTaxID = data.IsDBNull(7) ? "" : data.GetString(7) + "";
                    bp.City = data.IsDBNull(8) ? "" : data.GetString(8) + "";
                    bp.CardName = data.IsDBNull(9) ? "" : data.GetString(9) + "";
                    bp.Default = data.GetString(10) + "" == "default" ? true : false;
                    List.Add(bp);
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
        #endregion

    }
}
