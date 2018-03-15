using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FingerprintsModel;

namespace FingerprintsData
{
    public class RoleData
    {
        public List<Role> RoleList()
        {
            List<Role> _rolelist = new List<Role>();
            try
            {
                DataTable dt = null;
                using (SqlConnection Connection = connection.returnConnection())
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        dt = new DataTable();
                        command.Connection = Connection;
                        command.CommandText = "Sp_Sel_RoleList";
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt);
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Role obj = new Role();
                        obj.RoleId = Convert.ToString(dr["roleid"].ToString());
                        obj.RoleName = dr["roleName"].ToString();
                        _rolelist.Add(obj);
                    }
                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }

            return _rolelist;
        }
    }
}
