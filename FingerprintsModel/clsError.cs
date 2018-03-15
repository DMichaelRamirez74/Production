using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FingerprintsModel
{
    public static class clsError
    {

        public static void WriteException(Exception Exp)
        {
            try
            {
                string strLogFilePath = AppDomain.CurrentDomain.BaseDirectory + "Exceptions\\";
                if (!Directory.Exists(strLogFilePath))
                {
                    Directory.CreateDirectory(strLogFilePath);
                }

                StreamWriter txtwriter = new StreamWriter(strLogFilePath + "\\ErrorLog.txt", true);
                try
                {
                    string strDateTime = string.Empty;
                    strDateTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                    txtwriter.WriteLine("Error :" + strDateTime);
                    txtwriter.WriteLine(" 1. " + Exp.Message);
                    txtwriter.WriteLine(" 2. " + Exp.InnerException);
                    txtwriter.WriteLine(" 3. " + Exp.StackTrace);
                    txtwriter.WriteLine(" 4. " + Exp.Source);
                    txtwriter.WriteLine("==============================================================================================================");
                }
                catch { }
                finally
                {
                    txtwriter.Flush();
                    txtwriter.Close();
                    txtwriter.Dispose();
                }
            }
            catch (Exception)
            {

            }
        }


        public static void WriteError(string Exp)
        {
            string strLogFilePath = AppDomain.CurrentDomain.BaseDirectory + "Exceptions\\";
            if (!Directory.Exists(strLogFilePath))
            {
                Directory.CreateDirectory(strLogFilePath);
            }

            StreamWriter txtwriter = new StreamWriter(strLogFilePath + "\\ErrorLog.txt", true);
            try
            {
                string strDateTime = string.Empty;
                strDateTime = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                txtwriter.WriteLine("Error :" + strDateTime);
                txtwriter.WriteLine(" 1. " + Exp);

                txtwriter.WriteLine("==============================================================================================================");
            }
            catch { }
            finally
            {
                txtwriter.Flush();
                txtwriter.Close();
                txtwriter.Dispose();
            }
        }
        public static string GetUniqueFilePath(string filepath)
        {
            try
            {
                if (File.Exists(HttpContext.Current.Server.MapPath(filepath)))
                {
                    string folder = Path.GetDirectoryName(filepath);
                    string filename = Path.GetFileNameWithoutExtension(filepath);
                    string extension = Path.GetExtension(filepath);
                    int number = 1;
                    System.Text.RegularExpressions.Match regex = System.Text.RegularExpressions.Regex.Match(filepath, @"(.+) \((\d+)\)\.\w+");

                    if (regex.Success)
                    {
                        filename = regex.Groups[1].Value;
                        number = int.Parse(regex.Groups[2].Value);
                    }

                    do
                    {
                        number++;
                        filepath = Path.Combine(folder, string.Format("{0} ({1}){2}", filename, number, extension));
                    }
                    while (File.Exists(HttpContext.Current.Server.MapPath(filepath)));
                }

            }
            catch (Exception ex)
            {
                WriteException(ex);
            }
            return filepath;
        }
    }
}
