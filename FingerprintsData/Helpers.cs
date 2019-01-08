using FingerprintsModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

                            if (encry.Contains(prop.Name)) {

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


    }
}
