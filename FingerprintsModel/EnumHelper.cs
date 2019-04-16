using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public static class EnumHelper
    {
        /// <summary>
        /// Generic method to get the description of Enumeration
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        //public static string GetDescription(Enum value)
        //{
        //    return
        //        value.GetType()
        //        .GetMember(value.ToString())
        //        .FirstOrDefault()?
        //        .GetCustomAttribute<DescriptionAttribute>()?
        //        .Description;
        //}



        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }
		
		
		
		
		/// <summary>
        /// Extension method to return an enum value of type T for the given string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T GetEnumByStringValue<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }



        /// <summary>
        /// Extension method to convert Enumeration enum to JSON array 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string ConvertToJson(Type e)
        {

            var ret = "{";

            foreach (var val in Enum.GetValues(e))
            {

                var name = Enum.GetName(e, val);

                ret += name + ":" + ((int)val).ToString() + ",";

            }

            
            ret += "}";
            return ret;

        }

    }

}
