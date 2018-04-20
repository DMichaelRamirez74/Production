using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FingerprintsModel;
using System.Web.Mvc;
using System.IO;
using System.Web;

namespace FingerprintsData
{
    public class MyProfileData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataSet _dataset = null;
        public void GetProfile(DataSet _dataset, MyProfile _Profile)
        {
            PrimaryLanguages language = new PrimaryLanguages();
            _Profile.LangList = new List<PrimaryLanguages>();
            if (_dataset != null)
            {
                if (_dataset.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {

                        _Profile.TBDate = dr["TBDate"].ToString() != "" ? Convert.ToDateTime(dr["TBDate"]).ToString("MM/dd/yyyy") : "";
                        _Profile.TBResults = dr["TBResults"].ToString();
                        _Profile.CDDate = dr["CDDate"].ToString() != "" ? Convert.ToDateTime(dr["CDDate"]).ToString("MM/dd/yyyy") : "";
                        _Profile.CDResults = dr["CDResults"].ToString();
                        _Profile.MSDate = dr["MSDate"].ToString() != "" ? Convert.ToDateTime(dr["MSDate"]).ToString("MM/dd/yyyy") : "";
                        _Profile.FBDate = dr["FBDate"].ToString() != "" ? Convert.ToDateTime(dr["FBDate"]).ToString("MM/dd/yyyy") : "";
                        _Profile.BCIDate = dr["BCIDate"].ToString() != "" ? Convert.ToDateTime(dr["BCIDate"]).ToString("MM/dd/yyyy") : "";
                        _Profile.NCDate = dr["NCDate"].ToString() != "" ? Convert.ToDateTime(dr["NCDate"]).ToString("MM/dd/yyyy") : "";
                        _Profile.Degree = dr["Degree"].ToString();
                        _Profile.Institution = dr["Institution"].ToString();
                        _Profile.Major = dr["Major"].ToString();
                        _Profile.DegreeDate = dr["DegreeDate"].ToString();
                        _Profile.Degree2 = dr["Degree2"].ToString();
                        _Profile.Institution2 = dr["Institution2"].ToString();
                        _Profile.Major2 = dr["Major2"].ToString();
                        _Profile.DegreeDate2 = dr["DegreeDate2"].ToString();
                        _Profile.Degree3 = dr["Degree3"].ToString();
                        _Profile.Institution3 = dr["Institution3"].ToString();
                        _Profile.Major3 = dr["Major3"].ToString();
                        _Profile.DegreeDate3 = dr["DegreeDate3"].ToString();
                        _Profile.HighestDegree = dr["HighestDegree"].ToString();
                        _Profile.Type = dr["Type"].ToString();
                        _Profile.Type2 = dr["Type2"].ToString();
                        _Profile.Type3 = dr["Type3"].ToString();
                        _Profile.MSIFileUploaded = dr["MSImageFileName"].ToString();
                        _Profile.FBFileUploaded = dr["FBImageFileName"].ToString();
                        _Profile.BCIFileUploaded = dr["BCIImageFileName"].ToString();
                        _Profile.NCFileUploaded = dr["NCImageFileName"].ToString();
                        _Profile.UserID = dr["userid"].ToString();

                    }

                }

                if (_dataset.Tables[1].Rows.Count > 0)
                {

                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {
                        language = new PrimaryLanguages();
                        language.LanguageId = Convert.ToInt32(dr["LanguageID"]);
                        language.LanguageName = Convert.ToString(dr["PrimaryLanguage"]);
                        language.OtherLanguage = Convert.ToString(dr["OtherLanguage"]);
                        language.IsSpoken = ((dr["IsSpoken"]) == DBNull.Value) ? false : true;
                        _Profile.LangList.Add(language);
                    }

                }
            }


        }
        public MyProfile Getprofile(string UserID, string AgencyId)
        {
            MyProfile _Profile = new MyProfile();
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserID", UserID));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_getProfile";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                GetProfile(_dataset, _Profile);
                return _Profile;
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return _Profile;
            }
        }
        public string deleteEducation(int edu, MyProfile _Profile)
        {
            string result = string.Empty;

            if (Connection.State == ConnectionState.Open)
                Connection.Close();
            Connection.Open();
            command.Connection = Connection;
            command.Parameters.Add(new SqlParameter("@UserID", _Profile.UserID));
            command.Parameters.Add(new SqlParameter("@Edu", edu));
            command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_deleteProfileEDU";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            GetProfile(_dataset, _Profile);
            result = command.Parameters["@result"].Value.ToString();
            Connection.Close();
            command.Dispose();
            return result;

        }

        public string deleteEmployment(string ID, int emp, MyProfile _Profile)
        {
            string result = string.Empty;

            if (Connection.State == ConnectionState.Open)
                Connection.Close();
            Connection.Open();
            command.Connection = Connection;
            command.Parameters.Add(new SqlParameter("@UserID", ID));
            command.Parameters.Add(new SqlParameter("@emp", emp));
            command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "SP_deleteProfileEMP";
            DataAdapter = new SqlDataAdapter(command);
            _dataset = new DataSet();
            DataAdapter.Fill(_dataset);
            result = command.Parameters["@result"].Value.ToString();
            GetProfile(_dataset, _Profile);

            Connection.Close();
            command.Dispose();
            return result;

        }
        public string SaveProfile(string ID, MyProfile _Profile)
        {
            string result = string.Empty;
            try
            {


                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                string savetype = _Profile.profileindex;
                if (savetype == "1")
                {
                    _Profile.hidtab = "#addEducation";
                    command.Parameters.Add(new SqlParameter("@UserID", ID));
                    command.Parameters.Add(new SqlParameter("@Institution", _Profile.Institution));
                    command.Parameters.Add(new SqlParameter("@Major", _Profile.Major));
                    command.Parameters.Add(new SqlParameter("@Degree", _Profile.Degree));
                    command.Parameters.Add(new SqlParameter("@Type", _Profile.Type));
                    if (String.IsNullOrEmpty(_Profile.DegreeDate))
                    {
                        command.Parameters.Add(new SqlParameter("@DegreeDate", DBNull.Value));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@DegreeDate", _Profile.DegreeDate));
                    }
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_addProfileEDU";
                }
                if (savetype == "2")
                {
                    _Profile.hidtab = "#addHealth";
                    command.Parameters.Add(new SqlParameter("@UserID", ID));
                    if (String.IsNullOrEmpty(_Profile.TBDate))
                    {
                        command.Parameters.Add(new SqlParameter("@TBDate", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@TBResults", DBNull.Value));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@TBDate", _Profile.TBDate));
                        command.Parameters.Add(new SqlParameter("@TBResults", _Profile.TBResults));
                    }

                    if (String.IsNullOrEmpty(_Profile.CDDate))
                    {
                        command.Parameters.Add(new SqlParameter("@CDDate", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@CDResults", DBNull.Value));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@CDDate", _Profile.CDDate));
                        command.Parameters.Add(new SqlParameter("@CDResults", _Profile.CDResults));
                    }

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_addProfileHEALTH";

                }
                if (savetype == "3")
                {


                    _Profile.hidtab = "#addEmployment";
                    command.Parameters.Add(new SqlParameter("@UserID", ID));
                    if (String.IsNullOrEmpty(_Profile.MSDate))
                    {
                        command.Parameters.Add(new SqlParameter("@MSDate", DBNull.Value));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@MSDate", _Profile.MSDate));
                    }
                    if (String.IsNullOrEmpty(_Profile.BCIDate))
                    {
                        command.Parameters.Add(new SqlParameter("@BCIDate", DBNull.Value));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@BCIDate", _Profile.BCIDate));
                    }
                    if (String.IsNullOrEmpty(_Profile.FBDate))
                    {
                        command.Parameters.Add(new SqlParameter("@FBDate", DBNull.Value));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@FBDate", _Profile.FBDate));
                    }
                    if (String.IsNullOrEmpty(_Profile.NCDate))
                    {
                        command.Parameters.Add(new SqlParameter("@NCDate", DBNull.Value));
                    }
                    else
                    {
                        command.Parameters.Add(new SqlParameter("@NCDate", _Profile.NCDate));
                    }


                    if (_Profile.MSIfile != null)
                    {
                        _Profile.MSIFileName = _Profile.MSIfile.FileName;
                        _Profile.MSIFileExtension = Path.GetExtension(_Profile.MSIfile.FileName);
                        BinaryReader m = new BinaryReader(_Profile.MSIfile.InputStream);
                        _Profile.MSIFileData = m.ReadBytes(_Profile.MSIfile.ContentLength);
                        command.Parameters.Add(new SqlParameter("@MSImage", _Profile.MSIFileData));
                        command.Parameters.Add(new SqlParameter("@MSImageFileName", Path.GetFileName(_Profile.MSIFileName)));
                        command.Parameters.Add(new SqlParameter("@MSImageFileExt", _Profile.MSIFileExtension));
                    }
                    else if (_Profile.MSIFileUploaded != null)
                    {
                        _Profile.MSIFileExtension = Path.GetExtension(_Profile.MSIFileUploaded);
                        byte[] MSIByte = System.Text.ASCIIEncoding.Default.GetBytes(_Profile.MSIFileUploaded);
                        command.Parameters.Add(new SqlParameter("@MSImage", MSIByte));
                        command.Parameters.Add(new SqlParameter("@MSImageFileName", _Profile.MSIFileUploaded));
                        command.Parameters.Add(new SqlParameter("@MSImageFileExt", _Profile.MSIFileExtension));
                    }
                    else
                    {
                        command.Parameters.Add("@MSImage", System.Data.SqlDbType.VarBinary).Value = DBNull.Value;
                        command.Parameters.Add(new SqlParameter("@MSImageFileName", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@MSImageFileExt", DBNull.Value));
                    }


                    if (_Profile.FBfile != null)
                    {
                        _Profile.FBFileName = _Profile.FBfile.FileName;
                        _Profile.FBFileExtension = Path.GetExtension(_Profile.FBfile.FileName);
                        BinaryReader f = new BinaryReader(_Profile.FBfile.InputStream);
                        _Profile.FBFileData = f.ReadBytes(_Profile.FBfile.ContentLength);
                        command.Parameters.Add(new SqlParameter("@FBImage", _Profile.FBFileData));
                        command.Parameters.Add(new SqlParameter("@FBImageFileName", Path.GetFileName(_Profile.FBFileName)));
                        command.Parameters.Add(new SqlParameter("@FBImageFileExt", _Profile.FBFileExtension));
                    }
                    else if (_Profile.FBFileUploaded != null)
                    {
                        _Profile.FBFileExtension = Path.GetExtension(_Profile.FBFileUploaded);
                        byte[] FBByte = System.Text.ASCIIEncoding.Default.GetBytes(_Profile.FBFileUploaded);
                        command.Parameters.Add(new SqlParameter("@FBImage", FBByte));
                        command.Parameters.Add(new SqlParameter("@FBImageFileName", _Profile.FBFileUploaded));
                        command.Parameters.Add(new SqlParameter("@FBImageFileExt", _Profile.FBFileExtension));
                    }
                    else
                    {
                        command.Parameters.Add("@FBImage", System.Data.SqlDbType.VarBinary).Value = DBNull.Value;
                        command.Parameters.Add(new SqlParameter("@FBImageFileName", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@FBImageFileExt", DBNull.Value));
                    }


                    if (_Profile.BCIfile != null)
                    {
                        _Profile.BCIFileName = _Profile.BCIfile.FileName;
                        _Profile.BCIFileExtension = Path.GetExtension(_Profile.BCIfile.FileName);
                        BinaryReader b = new BinaryReader(_Profile.BCIfile.InputStream);
                        _Profile.BCIFileData = b.ReadBytes(_Profile.BCIfile.ContentLength);
                        command.Parameters.Add(new SqlParameter("@BCIImage", _Profile.BCIFileData));
                        command.Parameters.Add(new SqlParameter("@BCIImageFileName", Path.GetFileName(_Profile.BCIFileName)));
                        command.Parameters.Add(new SqlParameter("@BCIImageFileExt", _Profile.BCIFileExtension));
                    }
                    else if (_Profile.BCIFileUploaded != null)
                    {
                        _Profile.BCIFileExtension = Path.GetExtension(_Profile.BCIFileUploaded);
                        byte[] BCIByte = System.Text.ASCIIEncoding.Default.GetBytes(_Profile.BCIFileUploaded);
                        command.Parameters.Add(new SqlParameter("@BCIImage", BCIByte));
                        command.Parameters.Add(new SqlParameter("@BCIImageFileName", _Profile.BCIFileUploaded));
                        command.Parameters.Add(new SqlParameter("@BCIImageFileExt", _Profile.BCIFileExtension));
                    }
                    else
                    {
                        command.Parameters.Add("@BCIImage", System.Data.SqlDbType.VarBinary).Value = DBNull.Value;
                        command.Parameters.Add(new SqlParameter("@BCIImageFileName", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@BCIImageFileExt", DBNull.Value));
                    }


                    if (_Profile.NCfile != null)
                    {
                        _Profile.NCFileName = _Profile.NCfile.FileName;
                        _Profile.NCFileExtension = Path.GetExtension(_Profile.NCfile.FileName);
                        BinaryReader n = new BinaryReader(_Profile.NCfile.InputStream);
                        _Profile.NCFileData = n.ReadBytes(_Profile.NCfile.ContentLength);
                        command.Parameters.Add(new SqlParameter("@NCImage", _Profile.NCFileData));
                        command.Parameters.Add(new SqlParameter("@NCImageFileName", Path.GetFileName(_Profile.NCFileName)));
                        command.Parameters.Add(new SqlParameter("@NCImageFileExt", _Profile.NCFileExtension));
                    }
                    else if (_Profile.NCFileUploaded != null)
                    {
                        _Profile.NCFileExtension = Path.GetExtension(_Profile.NCFileUploaded);
                        byte[] NCByte = System.Text.ASCIIEncoding.Default.GetBytes(_Profile.NCFileUploaded);
                        command.Parameters.Add(new SqlParameter("@NCImage", NCByte));
                        command.Parameters.Add(new SqlParameter("@NCImageFileName", _Profile.NCFileUploaded));
                        command.Parameters.Add(new SqlParameter("@NCImageFileExt", _Profile.NCFileExtension));
                    }
                    else
                    {

                        command.Parameters.Add("@NCImage", System.Data.SqlDbType.VarBinary).Value = DBNull.Value;
                        command.Parameters.Add(new SqlParameter("@NCImageFileName", DBNull.Value));
                        command.Parameters.Add(new SqlParameter("@NCImageFileExt", DBNull.Value));
                    }



                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_addProfileEMP";

                }

                if (savetype == "4")
                {
                    _Profile.hidtab = "#addLanguage";
                    command.Parameters.Add(new SqlParameter("@UserID", ID));
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[3] {
                    new DataColumn("LanguageId", typeof(string)),
                      new DataColumn("IsSpoken",typeof(bool)),
                       new DataColumn("OtherLanguage",typeof(string))
                    });
                    foreach (PrimaryLanguages lang in _Profile.LangList)
                    {
                        if (lang.LanguageId != 0 && lang.IsSpoken)
                        {
                            dt.Rows.Add(lang.LanguageId, lang.IsSpoken,lang.OtherLanguage);

                        }
                    }
                    command.Parameters.Add(new SqlParameter("@PrimaryLanguage", dt));

                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "USP_AddStaffPrimaryLanguage";

                }
                command.Parameters.AddWithValue("@result", "").Direction = ParameterDirection.Output;
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                result = command.Parameters["@result"].Value.ToString();
                GetProfile(_dataset, _Profile);


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);

            }
            finally
            {
                Connection.Close();
                command.Dispose();
            }
            return result;

        }
        public List<Role> Getallroles( string userid)
        {
            List<Role> RoleList = new List<Role>();
            try
            {
                
                command.Parameters.Add(new SqlParameter("@userid", userid));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "Sp_getalternateroles";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null && _dataset.Tables[0].Rows.Count > 0)
                {
                    Role info = new Role();
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {
                        info = new Role();
                        info.RoleId = dr["Roleid"].ToString();
                        info.RoleName = dr["rolename"].ToString();
                        info.Defaultrole = Convert.ToBoolean(dr["defualtrole"]);
                        RoleList.Add(info);
                    }

                }
               
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                if (Connection != null)
                    Connection.Close();
              
            }
            return RoleList;
        }


    }
}
