using EcommerceAdmin2.Models.Sistema;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAdmin2.Models.Address
{
    public class AddressEcom
    {
        #region Propiedades
        public string Street { get; set; }
        public string StreetNoExt { get; set; }
        public string StreetNoInt { get; set; }
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
        public string Comments { get; set; }
        public ContactPerson ContactPerson { get; set; }
        private DBMysql DBMysql;
        #endregion
        #region Constructores
        public AddressEcom()
        {

        }
        public AddressEcom(DBMysql DBMysql)
        {
            this.DBMysql = DBMysql;
        }
        #endregion
        #region Metodos
        public bool GetShipToAddresses(int id)
        {
            string sqlStatement = string.Format("select * from datos_envio where id = '{0}'", id);
            MySqlDataReader data = null;
            bool IsExists = false;
            try
            {
                data = DBMysql.DoQuery(sqlStatement);
                if (data.HasRows)
                {
                    IsExists = true;
                    while (data.Read())
                    {
                        Street = data.IsDBNull(6) ? "" : data.GetString(6);
                        StreetNoExt = data.IsDBNull(7) ? "" : data.GetString(7);
                        StreetNoInt = data.IsDBNull(8) ? "" : data.GetString(8);
                        Block = data.IsDBNull(13) ? "" : data.GetString(13) + "";
                        County = data.IsDBNull(12) ? "" : data.GetString(12) + "";
                        Comments = data.IsDBNull(14) ? "" : data.GetString(14) + "";
                        ZipCode = data.IsDBNull(9) ? "" : data.GetString(9);
                        State = data.IsDBNull(10) ? "" : data.GetString(10) + "";
                        //bp.FederalTaxID = data.IsDBNull(7) ? "" : data.GetString(7) + "";
                        City = data.IsDBNull(11) ? "" : data.GetString(11) + "";
                        //bp.CardName = data.IsDBNull(9) ? "" : data.GetString(9) + "";
                        //bp.Default = data.GetString(10) + "" == "default" ? true : false;
                        ContactPerson = new ContactPerson();
                        ContactPerson.Name = data.IsDBNull(2) ? "" : data.GetString(2) + "" + data.GetString(3);
                        ContactPerson.Telphone = data.IsDBNull(4) ? "" : data.GetString(4) + "";
                        ContactPerson.Email = data.IsDBNull(5) ? "" : data.GetString(5) + "";
                    }
                }
                
                return IsExists;
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
        public bool GetBillToAddresses(int id)
        {
            string sqlStatement = string.Format("select * from datos_facturacion where id = '{0}'", id);
            MySqlDataReader data = null;
            bool IsExists = false;
            try
            {
                data = DBMysql.DoQuery(sqlStatement);
                if (data.HasRows)
                {
                    IsExists = true;
                    while (data.Read())
                    {
                        Street = data.IsDBNull(5) ? "" : data.GetString(5);
                        StreetNoExt = data.IsDBNull(6) ? "" : data.GetString(6);
                        StreetNoInt = data.IsDBNull(7) ? "" : data.GetString(7);
                        Block = data.IsDBNull(13) ? "" : data.GetString(13) + "";
                        County = data.IsDBNull(12) ? "" : data.GetString(12) + "";
                        ZipCode = data.IsDBNull(8) ? "" : data.GetString(8);
                        State = data.IsDBNull(9) ? "" : data.GetString(9) + "";
                        FederalTaxID = data.IsDBNull(4) ? "" : data.GetString(4) + "";
                        City = data.IsDBNull(11) ? "" : data.GetString(11) + "";
                        CardName = data.IsDBNull(2) ? "" : data.GetString(2) + "";
                        //bp.Default = data.GetString(10) + "" == "default" ? true : false;
                    }
                }
                
                return IsExists;
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
