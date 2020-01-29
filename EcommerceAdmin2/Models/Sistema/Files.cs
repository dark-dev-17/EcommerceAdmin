using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EcommerceAdmin2.Models.Sistema
{
    public class Files
    {
        public string Ruta { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        private string FTP_server { set; get; }
        private string FTP_user { set; get; }
        private string FTP_password { set; get; }
        private string FTP_Directory { set; get; }
        private string Site { set; get; }
        #region Constructores
        public Files(string FTP_Directory = "public_html/store/", string site = @"http://fibremex.co/")
        {
            this.FTP_server = "ftp.fibremex.co/";
            this.FTP_user = "fibreco";
            this.FTP_password = "spt.f1br3m3X.f1b.CO458#";
            this.FTP_Directory = FTP_Directory;
            this.Site = site;
        }
        public Files()
        {
            this.FTP_server = "ftp.fibremex.co/";
            this.FTP_user = "fibreco";
            this.FTP_password = "spt.f1br3m3X.f1b.CO458#";
        }
        #endregion
        #region Metodos
        public MemoryStream DownloadFile(string filePath, string fileName)
        {
            try
            {
                string Uri = "ftp://" + FTP_server + "/public_html/" + filePath;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(Uri));
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(FTP_user, FTP_password);
                request.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                using (MemoryStream stream = new MemoryStream())
                {
                    //Download the File.
                    response.GetResponseStream().CopyTo(stream);

                    response.Close();
                    return stream;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        } 
        public List<Files> Getfiles(string pattern)
        {
            string Uri = "ftp://" + FTP_server + FTP_Directory + "/" + pattern;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(Uri));
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(FTP_user, FTP_password);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = null;
            StreamReader reader = null;
            //StreamWriter writeStream = null;
            try
            {
                List<Files> lista = new List<Files>();
                string.Format("");
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);
                while (reader.Peek() >= 0)
                {
                    string nameFile = reader.ReadLine();
                    lista.Add(new Files { Ruta  = Site + nameFile, Name = nameFile });
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                //if (writeStream != null)
                //{
                //    writeStream.Close();
                //}
            }
        }
        public void DeleteFile(string path)
        {
            string Uri = "ftp://" + FTP_server + "/" + path;
            FtpWebRequest request = null;
            FtpWebResponse response = null;
            try
            {
                request = (FtpWebRequest)WebRequest.Create(new Uri(Uri));
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(FTP_user, FTP_password);
                response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        public void UpdateFile(string path, IFormFile FormFile)
        {
            string Uri = "ftp://" + FTP_server + "/" + path;
            FtpWebRequest request = null;
            FtpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = (FtpWebRequest)WebRequest.Create(new Uri(Uri));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(FTP_user, FTP_password);
                // Copy the contents of the file to the request stream.
                //StreamReader sourceStream = new StreamReader(FormFile.InputStream);
                //byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                //sourceStream.Close();
                //request.ContentLength = fileContents.Length;
                //Stream requestStream = request.GetRequestStream();
                //requestStream.Write(fileContents, 0, fileContents.Length);
                //requestStream.Close();
                // response = (FtpWebResponse)request.GetResponse();
                using (Stream ftpStream = request.GetRequestStream())
                {
                    FormFile.CopyTo(ftpStream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }

        }
        #endregion
    }
}
