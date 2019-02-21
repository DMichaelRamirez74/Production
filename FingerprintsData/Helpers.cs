using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;

namespace FingerprintsData
{
    public static class Helpers
    {

        //  public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        public static List<T> DataTableToList<T>(this DataTable table,List<string> encry) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);

                            if (encry.Contains(prop.Name))
                            {

                                propertyInfo.SetValue(obj, EncryptDecrypt.Encrypt(Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType).ToString()), null);
                                //if (propertyInfo.PropertyType == typeof(System.Int64)) {  //if long value
                                //    propertyInfo.SetValue(obj, EncryptDecrypt.Encrypt64(Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType).ToString()), null);
                                //} else {
                                //    propertyInfo.SetValue(obj, EncryptDecrypt.Encrypt(Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType).ToString()), null);
                                //}
                            }
                            else
                            {
                                propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);

                               
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static DataTable ToUserDefinedDataTable<T>(this IList<T> data, List<string> column,List<string> decryptedcolmn)
        {

            /*
                        PropertyDescriptorCollection props =
                            TypeDescriptor.GetProperties(typeof(T));
                        DataTable table = new DataTable();
                        for (int i = 0; i < props.Count; i++)
                        {

                            PropertyDescriptor prop = props[i];
                            if (column.Contains(prop.Name))
                            {
                                table.Columns.Add(prop.Name, prop.PropertyType);
                            }
                        }
                        //object[] values = new object[props.Count];
                        object[] values = new object[column.Count];
                        foreach (T item in data)
                        {
                            for (int i = 0; i < values.Length; i++)
                            {
                                values[i] = props[i].GetValue(item);
                            }
                            table.Rows.Add(values);
                        }
                        return table;

                        */

 

          PropertyDescriptorCollection props =
              TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (column.Contains(prop.Name))
                {
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
            }
            object[] values = new object[column.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    // values[i] = props[i].GetValue(item);
                    PropertyDescriptor prop = props.Find(column[i].ToString(),false);
                    if (decryptedcolmn.Contains(column[i].ToString())) {
                        values[i] = EncryptDecrypt.Decrypt(prop.GetValue(item).ToString());
                    }
                    else
                    {
                        values[i] = prop.GetValue(item);
                    }
                }
                table.Rows.Add(values);
            }


            return table;

        }


        public static string GetBase64Png(string linesGraphicJSON, int width, int height)
        {

            return Draw2DLineGraphic(new JavaScriptSerializer().Deserialize<Signature>(linesGraphicJSON), width, height);
        }
        private static string Draw2DLineGraphic(I2DLineGraphic lineGraphic, int width, int height)
        {
            //The png's bytes 
            byte[] png = null;

            //Create the Bitmap set Width and height 
            using (Bitmap b = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    //Make sure the image is drawn Smoothly (this makes the pen lines look smoother) 
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    //Set the background to white 
                    g.Clear(Color.White);

                    //Create a pen to draw the signature with 
                    Pen pen = new Pen(Color.Black, 2);

                    //Smooth out the pen, making it rounded 
                    pen.DashCap = System.Drawing.Drawing2D.DashCap.Round;

                    //Last point a line finished at 
                    Point LastPoint = new Point();
                    bool hasLastPoint = false;

                    //Draw the signature on the bitmap 
                    foreach (List<List<double>> line in lineGraphic.lines)
                    {
                        foreach (List<double> point in line)
                        {
                            var x = (int)Math.Round(point[0]);
                            var y = (int)Math.Round(point[1]);

                            if (hasLastPoint)
                            {
                                g.DrawLine(pen, LastPoint, new Point(x, y));
                            }

                            LastPoint.X = x;
                            LastPoint.Y = y;
                            hasLastPoint = true;
                        }
                        hasLastPoint = false;
                    }
                }

                //Convert the image to a png in memory 
                using (MemoryStream stream = new MemoryStream())
                {
                    b.Save(stream, ImageFormat.Png);
                    png = stream.ToArray();
                }
            }
            return Convert.ToBase64String(png);
        }

        public class Signature : I2DLineGraphic
        {
            public List<List<List<double>>> lines { get; set; }
        }

        interface I2DLineGraphic
        {
            List<List<List<double>>> lines { get; set; }
        }

    }
}
