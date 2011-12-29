<%@ Assembly Name="System.Data, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" %>
<%@ Assembly Name="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ WebHandler Language="C#"  Class="MriyaStaffWebparts.ShowPhoto" %>

using System;
using System.Web;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using Microsoft.SharePoint;

namespace MriyaStaffWebparts
{
    public class ShowPhoto : IHttpHandler
    {
        private int _nId = 0;
        private string _sNoImageUrl = "";
        private string _sConnectionString = "";

        private const string _sEncodeParams = "InntSo873lect23ution";
        
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["id"] != null)
                int.TryParse(context.Request.QueryString["id"].ToString(), out _nId);
            if (context.Request.QueryString["npi"] != null)
                _sNoImageUrl = System.Web.HttpUtility.UrlDecode(context.Request.QueryString["npi"].ToString());
            if (context.Request.QueryString["cs"] != null && context.Request.QueryString["cs"].ToString().Trim().Length > 0)
            {
                try
                {
                    _sConnectionString = System.Web.HttpUtility.UrlDecode(context.Request.QueryString["cs"].ToString());
                    _sConnectionString = DecryptString(_sConnectionString.Replace(' ', '+'), _sEncodeParams);
                }
                catch(Exception ex)
                {
                    context.Response.Write(ex.ToString());
                    _sConnectionString = "";
                }
            }

            if (_sConnectionString.Trim().Length < 1 || _nId < 1)
            {
                context.Response.Redirect(_sNoImageUrl);
                return;
            }


            ReadImage(context);

            // Debug purposes only
            //
            //SPSite siteColl = SPContext.Current.Site;
            //SPWeb site = SPContext.Current.Web;
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("HttpHandler from the site " +
            //                       site.Title +
            //                       " at " +
            //                       site.Url);
            //context.Response.Write("\nID = " + _nId.ToString());
            //context.Response.Write("\nNPI = " + _sNoImageUrl);
            //context.Response.Write("\nConnectionString = " + _sConnectionString);
        }
        

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private void ReadImage(HttpContext context)
        {
            SqlConnection connection = new SqlConnection(_sConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM UserPhoto WHERE Id = @Id",
                connection);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = _nId;
            SqlDataReader reader;
            byte[] im = null;
            string fname = "";
            string fext = "";
            int width = 0;
            int height = 0;
            int size = 0;
                
            try
            {
                connection.Open();
                reader = cmd.ExecuteReader();
            }
            catch(Exception ex)
            {
                context.Response.Write(ex.ToString());
                return;
            }

            if (reader.Read())
            {
                if (reader["PhotoBinary"] != null && reader["PhotoBinary"] != DBNull.Value)
                {
                    im = (byte[])reader["PhotoBinary"];
                }
                if (reader["PhotoFileName"] != null && reader["PhotoFileName"] != DBNull.Value)
                {
                    fname = reader["PhotoFileName"].ToString();
                }
                if (reader["PhotoFileExtension"] != null && reader["PhotoFileExtension"] != DBNull.Value)
                {
                    fext = reader["PhotoFileExtension"].ToString();
                }
                if (reader["PhotoSize"] != null && reader["PhotoSize"] != DBNull.Value)
                {
                    try { size = Convert.ToInt32(reader["PhotoSize"]); }
                    catch { size = 0; }
                }
                if (reader["PhotoWidth"] != null && reader["PhotoWidth"] != DBNull.Value)
                {
                    try { width = Convert.ToInt32(reader["PhotoWidth"]); }
                    catch { width = 0; }
                }
                if (reader["PhotoHeight"] != null && reader["PhotoHeight"] != DBNull.Value)
                {
                    try { height = Convert.ToInt32(reader["PhotoHeight"]); }
                    catch { height = 0; }
                }
                
                reader.Close();
            }
            connection.Close();

            if (im != null)
            {
                string imagetype = fext.Replace(".", "").Trim().ToLower();
                
                context.Response.Clear();
                context.Response.ContentType = "image/" + imagetype;
                context.Response.OutputStream.Write(im, 0, im.Length);
                context.Response.End();
            }
            else
            {
                context.Response.Redirect(_sNoImageUrl);
            }
        }


        private static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }
                
        
    }
}