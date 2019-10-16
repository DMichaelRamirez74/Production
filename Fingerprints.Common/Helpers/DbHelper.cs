
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
//using System.Web.Script.Serialization;


namespace Fingerprints.Common
{
    public static class DbHelper
    {




        ////  public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        //public static List<T> DataTableToList<T>(this DataTable table, List<string> encry) where T : class, new()
        //{
        //    try
        //    {
        //        List<T> list = new List<T>();
        //        foreach (var row in table.AsEnumerable())
        //        {
        //            T obj = new T();
        //            foreach (var prop in obj.GetType().GetProperties())
        //            {
        //                try
        //                {
        //                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
        //                    if (encry.Contains(prop.Name))
        //                    {
        //                        propertyInfo.SetValue(obj, Fingerprints.Common.CryptoEncryptDecrypt.Encrypt(Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType).ToString()), null);
        //                        //if (propertyInfo.PropertyType == typeof(System.Int64)) {  //if long value
        //                        //    propertyInfo.SetValue(obj, EncryptDecrypt.Encrypt64(Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType).ToString()), null);
        //                        //} else {
        //                        //    propertyInfo.SetValue(obj, EncryptDecrypt.Encrypt(Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType).ToString()), null);
        //                        //}
        //                    }
        //                    else
        //                    {
        //                        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
        //                    }
        //                }
        //                catch
        //                {
        //                    continue;
        //                }
        //            }
        //            list.Add(obj);
        //        }
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}



        public static List<T> DataTableToList<T>(this DataTable table, List<string> encry) where T : new()
        {
            Type t = typeof(T);

            // Create a list of the entities we want to return
            List<T> returnObject = new List<T>();

            // Iterate through the DataTable's rows
            foreach (DataRow dr in table.Rows)
            {
                // Convert each row into an entity object and add to the list
                T newRow = dr.ConvertToEntity<T>(encry);



                returnObject.Add(newRow);
            }

            // Return the finished list
            return returnObject;
        }


        public static T ConvertToEntity<T>(this DataRow tableRow, List<string> encry) where T : new()
        {
            // Create a new type of the entity I want
            Type t = typeof(T);
            T returnObject = new T();

            foreach (DataColumn col in tableRow.Table.Columns)
            {
                string colName = col.ColumnName;

                // Look for the object's property with the columns name, ignore case
                PropertyInfo pInfo = t.GetProperty(colName.ToLower(),
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // did we find the property ?
                if (pInfo != null)
                {
                    object val = tableRow[colName];

                    // is this a Nullable<> type
                    bool IsNullable = (Nullable.GetUnderlyingType(pInfo.PropertyType) != null);
                    if (IsNullable || val==DBNull.Value)
                    {
                        if (val is System.DBNull)
                        {
                            val = null;
                        }
                        else
                        {
                            // Convert the db type into the T we have in our Nullable<T> type
                            val = Convert.ChangeType
                    (val, Nullable.GetUnderlyingType(pInfo.PropertyType));


                        }
                    }
                    else
                    {
                        // Convert the db type into the type of the property in our entity
                        val = Convert.ChangeType(val.GetType() == typeof(Guid) ? val.ToString() : val, pInfo.PropertyType);
                    }
                    // Set the value of the property with the value from the db

                    if (encry.IndexOf(colName) > -1)
                    {
                        pInfo.SetValue(returnObject, CryptoEncryptDecrypt.Encrypt(val.ToString()), null);

                    }
                    else
                    {
                        pInfo.SetValue(returnObject, val, null);

                    }

                }
            }

            // return the entity object with values
            return returnObject;
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

        public static DataTable ToUserDefinedDataTable<T>(this IList<T> data, List<string> column, List<string> decryptedcolmn)
        {





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
                    PropertyDescriptor prop = props.Find(column[i].ToString(), false);
                    if (decryptedcolmn.Contains(column[i].ToString()))
                    {
                        values[i] = Fingerprints.Common.CryptoEncryptDecrypt.Decrypt(prop.GetValue(item).ToString());
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





        public static IEnumerable<Object[]> DataRecord(this System.Data.IDataReader source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            while (source.Read())
            {
                Object[] row = new Object[source.FieldCount];
                source.GetValues(row);
                yield return row;
            }
        }


    }
}
