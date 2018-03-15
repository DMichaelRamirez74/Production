using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FingerprintsModel;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;

namespace FingerprintsData
{
  public  class MatrixData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataTable _dataTable = null;
        DataSet _dataset = null;


        public bool InsertMatrixType(Matrix matrix)
        {

                 bool isRowAffected = false;
                 string queryCommand = "INSERT";
            try
            {
            
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", matrix.UserId));
                command.Parameters.Add(new SqlParameter("@MatrixValue", matrix.MatrixValue));
                command.Parameters.Add(new SqlParameter("@MatrixType", matrix.MatrixType));
                command.Parameters.Add(new SqlParameter("@AgencyId",matrix.AgencyId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_MatrixType";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;

        }




        public bool InsertAcronym(Acronym acronym)
        {

            bool isRowAffected = false;
            string queryCommand = "INSERT";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", acronym.UserId));
                command.Parameters.Add(new SqlParameter("@AcronymName", acronym.AcronymName.ToUpper())); 
                command.Parameters.Add(new SqlParameter("@AcronymId", acronym.AcronymId));
                command.Parameters.Add(new SqlParameter("@AgencyId", acronym.AgencyId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_Acronym";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;

        }


        public bool CheckMatrixType(Matrix matrix,string queryCommand)
        {
            bool isRowAffected = false;

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserId", matrix.UserId));
                command.Parameters.Add(new SqlParameter("@MatrixValue", matrix.MatrixValue));
                command.Parameters.Add(new SqlParameter("@MatrixType", matrix.MatrixType));
                command.Parameters.Add(new SqlParameter("@AgencyId", matrix.AgencyId));
                command.Parameters.Add(new SqlParameter("@MatrixId", matrix.MatrixId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_MatrixType";
                int rowsCount = Convert.ToInt32(command.ExecuteScalar());

                if (rowsCount > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }

        public bool CheckQuestions(QuestionsModel question,string queryCommand)
        {
            bool isRowAffected = false;
            /// string queryCommand = "UPDATE";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", question.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyId", question.AgencyId));
                command.Parameters.Add(new SqlParameter("@AssessmentQuestionId", question.AssessmentQuestionId));
                command.Parameters.Add(new SqlParameter("@AssessmentGroupId", question.AssessmentGroupId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_AssessmentQuestions";
                int RowsAffected = Convert.ToInt32(command.ExecuteScalar());
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }

        public bool InsertQuestionType(QuestionsModel question)
        {

            bool isRowAffected = false;
            string queryCommand = "INSERT";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@AssessmentGroupId", question.AssessmentGroupId));
                command.Parameters.Add(new SqlParameter("@AssessmentQuestion", question.QuestionText));
                command.Parameters.Add(new SqlParameter("@AgencyId", question.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", question.UserId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_AssessmentQuestions";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;

        }

        public List<SelectListItem> GetAssessmentType(Guid? agencyId)
        {
            string queryCommand = "SETCATEGORY";
            List<SelectListItem> selectedlist = new List<SelectListItem>();
            DataSet ds = new DataSet();
            command.Connection = Connection;
            command.CommandText = "SP_AssessmentQuestions";
            command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
            command.Parameters.Add(new SqlParameter("@Command", queryCommand));
            // command.Parameters.AddWithValue("@AgencyId", agencyId);
            // command.Parameters.AddWithValue("@CommunityId", communityId);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    selectedlist.Add(new SelectListItem
                    {
                        Text = dr["AssessmentGroupType"].ToString(),
                        Value = dr["AssessmentGroupId"].ToString()

                    });
                }
            }
            return selectedlist;
        }


        public List<Matrix> GetMatrixType(out int TotalCount, Matrix matrix, string sortOrder, string sortDirection, long pageSize, long requestedPage,long skip)
        {
          
            List<Matrix> matrixList = new List<Matrix>();
            TotalCount = 0;
            try
            {
                string queryCommand = "SELECTLIST";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", matrix.AgencyId));
                command.Parameters.Add(new SqlParameter("@userId", matrix.UserId));
                command.Parameters.Add(new SqlParameter("@SortDirection", sortDirection));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@SortOrder", sortOrder));
                command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_MatrixType";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Matrix matrixmodel = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {

                            matrixmodel = new Matrix();
                            matrixmodel.MatrixId = Convert.ToInt64(dr["MatrixId"]);
                            matrixmodel.MatrixValue = Convert.ToInt64(dr["MatrixValue"]);
                            matrixmodel.MatrixType = dr["MatrixType"].ToString();
                            matrixList.Add(matrixmodel);
                        }
                    }

                }
                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {

                        TotalCount = Convert.ToInt32(dr["TotalCount"]);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return matrixList;
        }


        public List<Matrix> GetMatrixTypeList(Matrix matrix)
        {

            List<Matrix> matrixList = new List<Matrix>();

            try
            {
                string queryCommand = "SELECT";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", matrix.AgencyId));
                command.Parameters.Add(new SqlParameter("@userId", matrix.UserId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_MatrixType";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Matrix matrixmodel = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {

                            matrixmodel = new Matrix();
                            matrixmodel.MatrixId = Convert.ToInt64(dr["MatrixId"]);
                            matrixmodel.MatrixValue = Convert.ToInt64(dr["MatrixValue"]);
                            matrixmodel.MatrixType = dr["MatrixType"].ToString();
                            matrixList.Add(matrixmodel);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return matrixList;
        }
        public Acronym GetAcronym(Acronym acronym)
        {
            Acronym acronymmodel = new Acronym();

            try
            {
                string queryCommand = "SELECT";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", acronym.AgencyId));
                command.Parameters.Add(new SqlParameter("@userId", acronym.UserId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Acronym";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                           
                            acronymmodel.AcronymId = Convert.ToInt64(dr["AcronymId"]);
                            acronymmodel.AcronymName = dr["AcronymName"].ToString();
                          
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return acronymmodel;
        }




        public List<QuestionsModel> GetQuestionType(out int TotalCount,QuestionsModel question, string sortOrder, string sortDirection, int pageSize, int requestedPage,int skip)
        {

            List<QuestionsModel> questionList = new List<QuestionsModel>();
            List<QuestionsModel> questionList1 = new List<QuestionsModel>();
            List<QuestionsModel> questionList2 = new List<QuestionsModel>();
                List<QuestionsModel> modQuestionList = new List<QuestionsModel>();
            QuestionsModel questionmodel = null;
            TotalCount = 0;
            try
            {
                string queryCommand = "SELECT";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", question.AgencyId));
                command.Parameters.Add(new SqlParameter("@sortOrder", sortOrder));
                command.Parameters.Add(new SqlParameter("@sortDirection", sortDirection));
                command.Parameters.Add(new SqlParameter("@pageSize", pageSize));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_AssessmentQuestions";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                       
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            questionmodel = new QuestionsModel();
                            questionmodel.AssessmentQuestionId = Convert.ToInt64(dr["AssessmentQuestionId"]);
                            questionmodel.AssessmentQuestion = (dr["AssessmentQuestion"]).ToString();
                            questionmodel.AssessmentGroupType = (dr["AssessmentGroupType"]).ToString();
                            questionmodel.AssessmentGroupId=Convert.ToInt64(dr["AssessmentGroupId"]);
                            questionmodel.CategoryPosition = Convert.ToInt64(dr["CategoryPosition"]);
                            questionmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            if(string.IsNullOrEmpty(dr["ModifiedDate"].ToString()))
                            {
                                questionmodel.ModifiedDate = (DateTime?)null;
                            }
                            else
                            {
                            questionmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);

                            }
                            questionList.Add(questionmodel);

                        }
                    }

                }

                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                          
                            TotalCount = Convert.ToInt32(dr["TotalCount"]);

                        }
                    }

                }
                long CategoryPosition = 0;
                DateTime date1 = DateTime.Now;
                DateTime date2 = DateTime.Now;
         
                if (((sortOrder.ToUpper()=="CREATEDDATE")||(sortOrder.ToUpper()=="MODIFIEDDATE")))
                    {

                    if((sortOrder.ToUpper() == "CREATEDDATE"))
                    {
                        date1 = questionList.OrderByDescending(x => x.CreatedDate).Select(x => x.CreatedDate).First();
                        CategoryPosition = questionList.Where(x => x.CreatedDate == date1).Select(x => x.CategoryPosition).FirstOrDefault();
                        questionList2 = questionList.OrderBy(x => x.CreatedDate).Where(x => x.CategoryPosition == CategoryPosition).ToList();

                    }
                    else
                    {
                        date2 = (DateTime)questionList.OrderByDescending(x => x.ModifiedDate).Select(x => x.ModifiedDate).First();
                        CategoryPosition = questionList.Where(x => x.ModifiedDate == date2).Select(x => x.CategoryPosition).FirstOrDefault();
                        questionList2 = questionList.OrderBy(x => x.ModifiedDate).Where(x => x.CategoryPosition == CategoryPosition).ToList();
                    }
                 

                foreach(var item1 in questionList2)
                {
                    questionmodel = new QuestionsModel();
                    questionmodel.AssessmentQuestionId = item1.AssessmentQuestionId;
                    questionmodel.AssessmentQuestion = item1.AssessmentQuestion;
                    questionmodel.AssessmentGroupType = item1.AssessmentGroupType;
                    questionmodel.AssessmentGroupId = item1.AssessmentGroupId;
                    questionmodel.CategoryPosition = item1.CategoryPosition;
                    questionmodel.CreatedDate = item1.CreatedDate;
                    questionmodel.ModifiedDate = item1.ModifiedDate;
                    modQuestionList.Add(questionmodel);
                }
                    var list3 = questionList.OrderBy(x => x.CategoryPosition).Where(x => x.CategoryPosition != CategoryPosition).ToList();
                    foreach (var item2 in list3)
                {
                    questionmodel = new QuestionsModel();
                    questionmodel.AssessmentQuestionId = item2.AssessmentQuestionId;
                    questionmodel.AssessmentQuestion = item2.AssessmentQuestion;
                    questionmodel.AssessmentGroupType = item2.AssessmentGroupType;
                    questionmodel.AssessmentGroupId = item2.AssessmentGroupId;
                    questionmodel.CategoryPosition = item2.CategoryPosition;
                    questionmodel.CreatedDate = item2.CreatedDate;
                    questionmodel.ModifiedDate = item2.ModifiedDate;
                    modQuestionList.Add(questionmodel);
                }

                }
                else
                {
                    modQuestionList = questionList;
                }


            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return modQuestionList;
        }



        public List<Acronym> GetAcronymList(Acronym acronym, string sortOrder, string sortDirection, int pageSize, int requestedPage)
        {

            List<Acronym> AcronymList = new List<Acronym>();

            try
            {
               
                string queryCommand = "SELECT";                
               // command.Parameters.Clear();
               // command.Parameters.Add(new SqlParameter("@AgencyId", acronym.AgencyId));
              //  command.Parameters.Add(new SqlParameter("@sortOrder", sortOrder));
             //   command.Parameters.Add(new SqlParameter("@sortDirection", sortDirection));
             //   command.Parameters.Add(new SqlParameter("@pageSize", pageSize));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
             //   command.Connection = Connection;
             //   command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_Acronym";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {
                        Acronym Acronymmodel = null;
                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            Acronymmodel = new Acronym();
                            Acronymmodel.AcronymId = Convert.ToInt32(dr["AcronymId"]);
                            Acronymmodel.AcronymName = dr["AcronymName"].ToString();
                            //Acronymmodel.AssessmentQuestion = (dr["AssessmentQuestion"]).ToString();
                            //Acronymmodel.AssessmentGroupType = (dr["AssessmentGroupType"]).ToString();
                            //Acronymmodel.AssessmentGroupId = Convert.ToInt32(dr["AssessmentGroupId"]);
                            AcronymList.Add(Acronymmodel);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return AcronymList;
        }


        public bool CheckMatrixRef(long matrixId,Guid? agencyId)
        {

            bool isRowAffected = false;
            string queryCommand = "CHECKREF";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MatrixId", matrixId);
                command.Parameters.AddWithValue("@AgencyId", agencyId);
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_MatrixType";
                int RowsAffected = Convert.ToInt32(command.ExecuteScalar());
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }
        public bool DeleteMatrixType(long matrixId,Guid? agencyId)
        {

            bool isRowAffected = false;
            string queryCommand = "DELETE";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MatrixId", matrixId);
                command.Parameters.AddWithValue("@AgencyId", agencyId);
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_MatrixType";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;

        }

        public bool UpdateAcronym(Acronym acronym)
        {

            bool isRowAffected = false;
            string queryCommand = "UPDATE";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", acronym.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyId", acronym.AgencyId));
                command.Parameters.Add(new SqlParameter("@AcronymId", acronym.AcronymId));
                command.Parameters.Add(new SqlParameter("@AcronymName", acronym.AcronymName.ToUpper()));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_Acronym";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;

        }

        public bool CheckUpdateAcronym(Acronym acronym, string queryCommand)
        {

            bool isRowAffected = false;
            /// string queryCommand = "UPDATE";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", acronym.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyId", acronym.AgencyId));
                command.Parameters.Add(new SqlParameter("@AcronymId", acronym.AcronymId));
                command.Parameters.Add(new SqlParameter("@AcronymName", acronym.AcronymName.ToUpper()));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_Acronym";
                int RowsAffected = Convert.ToInt32(command.ExecuteScalar());
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;

        }
        public bool DeleteAcronym(long AcronymId,Guid? AgencyId)
        {
            bool isRowAffected = false;
            string queryCommand = "DELETE";
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AcronymId", AcronymId);
                command.Parameters.AddWithValue("@AgencyId", AgencyId);
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_Acronym";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;

        }


        public bool DeleteQuestionType(int id,Guid? agencyId)
        {

            bool isRowAffected = false;
            string queryCommand = "DELETE";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@AssessmentQuestionId", id);
                command.Parameters.AddWithValue("@AgencyId", agencyId);
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_AssessmentQuestions";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;

        }

        public bool UpdateMatrixType(Matrix matrix)
        {

            bool isRowAffected = false;
            string queryCommand = "UPDATE";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", matrix.UserId));
                command.Parameters.Add(new SqlParameter("@MatrixId", matrix.MatrixId));
                command.Parameters.Add(new SqlParameter("@MatrixValue", matrix.MatrixValue));
                command.Parameters.Add(new SqlParameter("@MatrixType", matrix.MatrixType));
                command.Parameters.Add(new SqlParameter("@AgencyId", matrix.AgencyId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_MatrixType";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;

        }


        public bool UpdateQuestionType(QuestionsModel question)
        {

            bool isRowAffected = false;
            string queryCommand = "UPDATE";
            try
            {
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@AgencyId", question.AgencyId));
                command.Parameters.Add(new SqlParameter("@AssessmentQuestion", question.AssessmentQuestion));
                command.Parameters.Add(new SqlParameter("@AssessmentQuestionId", question.AssessmentQuestionId.ToString()));
                command.Parameters.Add(new SqlParameter("@AssessmentGroupId", question.AssessmentGroupId));
                command.Parameters.Add(new SqlParameter("@UserId", question.UserId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_AssessmentQuestions";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;

        }



        public AnnualAssessment GetAnnualAssessment(AnnualAssessment assessment)
        {
            AnnualAssessment annualAssessment = new AnnualAssessment();

            try
            {
                string queryCommand = "SELECT";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", assessment.AgencyId));
                command.Parameters.Add(new SqlParameter("@UserId", assessment.UserId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_AnnualAssessment";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            annualAssessment.AnnualAssessmentId = Convert.ToInt64(dr["AnnualAssessmentId"]);
                            annualAssessment.AnnualAssessmentType = Convert.ToInt64(dr["AnnualAssessmentType"]);
                            annualAssessment.AgencyName = dr["AgencyName"].ToString();
                            annualAssessment.Assessment1From = Convert.ToDateTime(dr["Assessment1FromDate"].ToString()).ToString("MM/dd/yyyy");
                            annualAssessment.Assessment1To = Convert.ToDateTime(dr["Assessment1ToDate"].ToString()).ToString("MM/dd/yyyy");
                            annualAssessment.Assessment2From = Convert.ToDateTime(dr["Assessment2FromDate"].ToString()).ToString("MM/dd/yyyy");
                            annualAssessment.Assessment2To = Convert.ToDateTime(dr["Assessment2ToDate"].ToString()).ToString("MM/dd/yyyy");
                            annualAssessment.Assessment3From = Convert.ToDateTime(dr["Assessment3FromDate"].ToString()).ToString("MM/dd/yyyy");
                            annualAssessment.Assessment3To = Convert.ToDateTime(dr["Assessment3ToDate"].ToString()).ToString("MM/dd/yyyy");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return annualAssessment;
        }
        public bool InsertAnnualAssement(AnnualAssessment annualAssement)
        {
            bool isRowAffected = false;
            string queryCommand = "INSERT";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", annualAssement.UserId));
                command.Parameters.Add(new SqlParameter("@AssessmentType", annualAssement.AnnualAssessmentType));
                command.Parameters.Add(new SqlParameter("@AgencyId", annualAssement.AgencyId));
                command.Parameters.Add(new SqlParameter("@Assessment1FromDate", annualAssement.Assessment1From));
                command.Parameters.Add(new SqlParameter("@Assessment1ToDate", annualAssement.Assessment1To));
                command.Parameters.Add(new SqlParameter("@Assessment2FromDate", annualAssement.Assessment2From));
                command.Parameters.Add(new SqlParameter("@Assessment2ToDate", annualAssement.Assessment2To));
                command.Parameters.Add(new SqlParameter("@Assessment3FromDate", annualAssement.Assessment3From));
                command.Parameters.Add(new SqlParameter("@Assessment3ToDate", annualAssement.Assessment3To));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_AnnualAssessment";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;

        }


        public List<AssessmentCategory> GetAssessmentCategoryList(Guid? agencyId)
        {
            AssessmentCategory category = null;
            List<AssessmentCategory> categoryList = new List<AssessmentCategory>();
            try
            {
                string queryCommand = "SELECTLIST";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_AssessmentCategory";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            category = new AssessmentCategory();
                            category.AssessmentCategoryId = Convert.ToInt64(dr["AssessmentCategoryId"]);
                            category.Category = dr["Category"].ToString();
                            categoryList.Add(category);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return categoryList;

        }

        public List<AssessmentCategory> GetAssessmentCategory(out int  TotalCount,string sortOrder, string sortDirection, int pageSize, int requestedPage,int skip,Guid? agencyId)
        {
            AssessmentCategory category = null;
            List<AssessmentCategory> categoryList = new List<AssessmentCategory>();
             TotalCount = 0;
            
            try
            {
                string queryCommand = "SELECT";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@SortOrder", sortOrder));
                command.Parameters.Add(new SqlParameter("@SortDirection", sortDirection));
                command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                command.Parameters.Add(new SqlParameter("@RequestedPage", requestedPage));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_AssessmentCategory";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            category = new AssessmentCategory();
                            category.AssessmentCategoryId = Convert.ToInt64(dr["AssessmentCategoryId"]);
                            category.Category = dr["Category"].ToString();
                           category.CategoryPosition = Convert.ToInt64(dr["CategoryPosition"]);
                            categoryList.Add(category);
                        }
                    }

                }
                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                            TotalCount = Convert.ToInt32(dr["TotalCount"]);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return categoryList;
        }

        public bool CheckAssessmentCategory(AssessmentCategory category , string queryCommand)
        {
            bool isRowAffected = false;
            //string queryCommand = "CHECKUPDATE";
            try
            {
                
                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserId", category.UserId));
                command.Parameters.Add(new SqlParameter("@CategoryName", category.Category));
                command.Parameters.Add(new SqlParameter("@AgencyId", category.AgencyId));
                command.Parameters.Add(new SqlParameter("@AssessmentCategoryId", category.AssessmentCategoryId));
                command.Parameters.Add(new SqlParameter("@CategoryPosition", category.CategoryPosition));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));

                command.CommandText = "SP_AssessmentCategory";
                int rowsCount = Convert.ToInt32(command.ExecuteScalar());

                if (rowsCount > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }
        public bool InsertAssessmentCategory(AssessmentCategory category)
        {
            bool isRowAffected = false;
            string queryCommand = "INSERT";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", category.UserId));
                command.Parameters.Add(new SqlParameter("@CategoryName", category.Category));
                command.Parameters.Add(new SqlParameter("@CategoryPosition", category.CategoryPosition));
                command.Parameters.Add(new SqlParameter("@AgencyId", category.AgencyId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_AssessmentCategory";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }

        public bool UpdateAssessmentCategory(AssessmentCategory category)
        {
            bool isRowAffected = false;
            string queryCommand = "UPDATE";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", category.UserId));
                command.Parameters.Add(new SqlParameter("@CategoryName", category.Category));
                command.Parameters.Add(new SqlParameter("@CategoryId", category.AssessmentCategoryId));
                command.Parameters.Add(new SqlParameter("@AgencyId", category.AgencyId));
                command.Parameters.Add(new SqlParameter("@CategoryPosition", category.CategoryPosition));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_AssessmentCategory";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }
        
        public bool DeleteAssessmentCategory(AssessmentCategory category)
        {
            bool isRowAffected = false;
            string queryCommand = "DELETE";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@UserId", category.UserId));
                command.Parameters.Add(new SqlParameter("@CategoryName", category.Category));
                command.Parameters.Add(new SqlParameter("@CategoryId", category.AssessmentCategoryId));
                command.Parameters.Add(new SqlParameter("@AgencyId", category.AgencyId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_AssessmentCategory";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }


        public List<AssessmentGroup> GetAssessmentGroup(out long TotalCount,string sortOrder, string sortDirection, long pageSize, long requestedPage,long skip,Guid? agencyId)
        {
            AssessmentGroup assessmentGroup = null;
            List<AssessmentGroup> groupList = new List<AssessmentGroup>();
            List<AssessmentGroup> groupList2 = new List<AssessmentGroup>();
            List<AssessmentGroup> groupList3 = new List<AssessmentGroup>();
            TotalCount = 0;
            try
            {
                
                string queryCommand = "GETSELECT";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                command.Parameters.Add(new SqlParameter("@RequestedPage", requestedPage));
                command.Parameters.Add(new SqlParameter("@SortDirection", sortDirection));
                command.Parameters.Add(new SqlParameter("@SortOrder", sortOrder));
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_AssessmentGroup";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            assessmentGroup = new AssessmentGroup();
                            assessmentGroup.AssessmentGroupId = Convert.ToInt64(dr["AssessmentGroupId"]);
                            assessmentGroup.AssessmentCategoryId = Convert.ToInt64(dr["AssessmentCategoryId"]);
                            assessmentGroup.Category = dr["Category"].ToString();
                            assessmentGroup.AssessmentGroupType =( dr["AssessmentGroupType"]==null)?"": dr["AssessmentGroupType"].ToString();
                            assessmentGroup.IsActive = (dr["IsActive"].ToString()=="") ? false :Convert.ToBoolean( dr["IsActive"]);
                            assessmentGroup.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            assessmentGroup.ModifiedDate = (dr["ModifiedDate"].ToString() != "") ? Convert.ToDateTime(dr["ModifiedDate"].ToString()) : (DateTime?)(null);
                            assessmentGroup.CategoryPosition = Convert.ToInt64(dr["CategoryPosition"]);
                            groupList.Add(assessmentGroup);
                        }
                    }

                }
                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                           TotalCount = Convert.ToInt32(dr["TotalCount"]);
                          
                        }
                    }

                }

                long CategoryPosition = 0;
                DateTime date1 = DateTime.Now;
                DateTime date2 = DateTime.Now;
                if (((sortOrder.ToUpper() == "CREATEDDATE") || (sortOrder.ToUpper() == "MODIFIEDDATE")))
                {

                    if (sortOrder.ToUpper() == "CREATEDDATE")
                    {
                        date1 =groupList.OrderByDescending(x => x.CreatedDate).Select(x => x.CreatedDate).First();

                        CategoryPosition = groupList.Where(x => x.CreatedDate == date1).Select(x => x.CategoryPosition).FirstOrDefault();
                        groupList3 = groupList.OrderBy(x => x.CreatedDate).Where(x => x.CategoryPosition == CategoryPosition).ToList();

                    }
                    else
                    {
                        date2 = (DateTime)groupList.OrderByDescending(x => x.ModifiedDate).Select(x => x.ModifiedDate).First();
                      //var test= groupList.OrderByDescending(x => x.ModifiedDate);
                        // date = (DateTime)groupList.OrderByDescending(x => x.ModifiedDate).Select(x => x.ModifiedDate).FirstOrDefault();
                        //var date2 = (DateTime)groupList.OrderByDescending(x => x.ModifiedDate).Select(x => x.ModifiedDate).FirstOrDefault();
                        CategoryPosition = groupList.Where(x => x.ModifiedDate == date2).Select(x => x.CategoryPosition).FirstOrDefault();
                        groupList3 = groupList.OrderBy(x => x.ModifiedDate).Where(x => x.CategoryPosition == CategoryPosition).ToList();

                    }

                    foreach (var item in groupList3)
                    {
                        assessmentGroup = new AssessmentGroup();
                        assessmentGroup.AssessmentGroupId = item.AssessmentGroupId;
                        assessmentGroup.AssessmentCategoryId = item.AssessmentCategoryId;
                        assessmentGroup.Category = item.Category;
                        assessmentGroup.AssessmentGroupType = item.AssessmentGroupType;
                        assessmentGroup.IsActive = item.IsActive;
                        assessmentGroup.CreatedDate = item.CreatedDate;
                        assessmentGroup.CategoryPosition = item.CategoryPosition;
                        groupList2.Add(assessmentGroup);

                    }
                    var list3 = groupList.OrderBy(x => x.CategoryPosition).Where(x => x.CategoryPosition != CategoryPosition).ToList();

                    foreach (var item2 in list3)
                    {
                        assessmentGroup = new AssessmentGroup();
                        assessmentGroup.AssessmentGroupId = item2.AssessmentGroupId;
                        assessmentGroup.AssessmentCategoryId = item2.AssessmentCategoryId;
                        assessmentGroup.Category = item2.Category;
                        assessmentGroup.AssessmentGroupType = item2.AssessmentGroupType;
                        assessmentGroup.IsActive = item2.IsActive;
                        assessmentGroup.CreatedDate = item2.CreatedDate;
                        assessmentGroup.CategoryPosition = item2.CategoryPosition;
                        groupList2.Add(assessmentGroup);

                    }
                }
                else
                {
                    groupList2 = groupList;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return groupList2;
        }

        public bool InsertAssessmentGroup(AssessmentGroup group)
        {
            bool isRowAffected = false;
            string queryCommand = "INSERT";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserId", group.UserId));
                command.Parameters.Add(new SqlParameter("@AssessmentCategoryId", group.AssessmentCategoryId));
                command.Parameters.Add(new SqlParameter("@AgencyId", group.AgencyId));
                command.Parameters.Add(new SqlParameter("@AssessmentGroupType", group.AssessmentGroupType));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Parameters.Add(new SqlParameter("@IsActive", group.IsActive));
                command.CommandText = "SP_AssessmentGroup";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }
        public bool CheckAssessmentGroup(AssessmentGroup group,string queryCommand)
        {
            bool isRowAffected = false;
        
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserId", group.UserId));
                command.Parameters.Add(new SqlParameter("@AssessmentCategoryId", group.AssessmentCategoryId));
                command.Parameters.Add(new SqlParameter("@AgencyId", group.AgencyId));
                command.Parameters.Add(new SqlParameter("@AssessmentGroupType", group.AssessmentGroupType));
                command.Parameters.Add(new SqlParameter("@AssessmentGroupId", group.AssessmentGroupId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Parameters.Add(new SqlParameter("@IsActive", group.IsActive));
                command.CommandText = "SP_AssessmentGroup";
                int rowsCount =Convert.ToInt32( command.ExecuteScalar());
                
                if (rowsCount > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }




        public bool CheckAcronymName(Acronym name, string queryCommand)
        {
            bool isRowAffected = false;

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserId", name.UserId));                
                command.Parameters.Add(new SqlParameter("@AcronymName", name.AcronymName));
                command.Parameters.Add(new SqlParameter("@AgencyId", name.AgencyId));                                
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));                
                command.CommandText = "SP_Acronym";
                int rowsCount = Convert.ToInt32(command.ExecuteScalar());
                if (rowsCount > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }


        public bool UpdateAssessmentGroup(AssessmentGroup assessmentGroup)
        {
            bool isRowAffected = false;
            string queryCommand = "UPDATE";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserId", assessmentGroup.UserId));
                command.Parameters.Add(new SqlParameter("@AssessmentCategoryId", assessmentGroup.AssessmentCategoryId));
                command.Parameters.Add(new SqlParameter("@AgencyId", assessmentGroup.AgencyId));
                command.Parameters.Add(new SqlParameter("@AssessmentGroupType", assessmentGroup.AssessmentGroupType));
                command.Parameters.Add(new SqlParameter("@AssessmentGroupId", assessmentGroup.AssessmentGroupId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Parameters.Add(new SqlParameter("@IsActive", assessmentGroup.IsActive));
                command.CommandText = "SP_AssessmentGroup";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }


        public bool DeleteAssessmentGroup(AssessmentGroup group)
        {
            bool isRowAffected = false;
            string queryCommand = "DELETE";
            try
            {


                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AssessmentGroupId", group.AssessmentGroupId);
                command.Parameters.AddWithValue("@UserId", group.UserId);
                command.Parameters.AddWithValue("@AgencyId", group.AgencyId);
                command.Parameters.AddWithValue("@Command", queryCommand);
                command.CommandText = "SP_AssessmentGroup";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }

        public List<AssessmentGroup> GetAssessmentGroupList(Guid? agencyId)
        {
            AssessmentGroup assessmentGroup = null;
            List<AssessmentGroup> groupList = new List<AssessmentGroup>();
            try
            {

                string queryCommand = "GET_GROUP";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_AssessmentResults";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            assessmentGroup = new AssessmentGroup();
                            assessmentGroup.AssessmentGroupId = Convert.ToInt64(dr["AssessmentGroupId"]);
                            assessmentGroup.AssessmentGroupType = (dr["AssessmentGroupType"] == null) ? "" : dr["AssessmentGroupType"].ToString();
                            groupList.Add(assessmentGroup);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
               
                DataAdapter.Dispose();
                command.Dispose();
            }
            return groupList;
        }

        public bool checkAssessmentResult(AssessmentResults results,string queryCommand)
        {
            bool isRowAffected = false;

            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@UserId", results.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyId", results.AgencyId));
                command.Parameters.Add(new SqlParameter("@AssessmentGroupId", results.AssessmentGroupId));
                command.Parameters.Add(new SqlParameter("@MatrixId", results.MatrixId));
                command.Parameters.Add(new SqlParameter("@AssessmentResultId", results.AssessmentResultId));
                command.Parameters.Add(new SqlParameter("@Description", results.Description));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_AssessmentResults";
                int rowsCount = Convert.ToInt32(command.ExecuteScalar());

                if (rowsCount > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }

        public bool InsertAssessmentResult(AssessmentResults results)
        {
            bool isRowAffected = false;
            string queryCommand = "INSERT";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AssessmentGroupId", results.AssessmentGroupId));
                command.Parameters.Add(new SqlParameter("@MatrixId", results.MatrixId));
                command.Parameters.Add(new SqlParameter("@Description", results.Description));
                command.Parameters.Add(new SqlParameter("@ReferralSuggested", results.ReferralSuggested));
                command.Parameters.Add(new SqlParameter("@FPASuggessted", results.FPASuggested));
                command.Parameters.Add(new SqlParameter("@UserId", results.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyId", results.AgencyId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_AssessmentResults";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }


        public List<AssessmentResults> GetAssessmentResults( out int TotalCount,string sortOrder, string sortDirection, int pageSize, int requestedPage,int skip,Guid? agencyID)
        {
            AssessmentResults results = null;
            List<AssessmentResults> resultslist = new List<AssessmentResults>();
            List<AssessmentResults> sortList = new List<AssessmentResults>();
            List<AssessmentResults> resultslist2 = new List<AssessmentResults>();
            TotalCount = 0;
            try
            {
                string queryCommand = "SELECT";
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@SortOrder", sortOrder));
                command.Parameters.Add(new SqlParameter("@SortDirection", sortDirection));
                command.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                command.Parameters.Add(new SqlParameter("@RequestedPage", requestedPage));
                command.Parameters.Add(new SqlParameter("@skip", skip));
                command.Parameters.Add(new SqlParameter("@AgencyId", agencyID));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_AssessmentResults";
                DataAdapter = new SqlDataAdapter(command);
                _dataset = new DataSet();
                DataAdapter.Fill(_dataset);
                if (_dataset.Tables[0] != null)
                {
                    if (_dataset.Tables[0].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[0].Rows)
                        {
                            results = new AssessmentResults();
                            results.AssessmentResultId = Convert.ToInt64(dr["AssessmentResultId"]);
                            results.MatrixId = Convert.ToInt64(dr["MatrixId"]);
                            results.AssessmentGroupId = Convert.ToInt64(dr["AssessmentGroupId"]);
                            results.MatrixType = dr["MatrixType"].ToString();
                            results.MatrixValue = Convert.ToInt64(dr["MatrixValue"]);
                            results.AssessmentGroupType = dr["AssessmentGroupType"].ToString();
                            results.Description = dr["Description"].ToString();
                            results.ReferralSuggested =  Convert.ToBoolean(dr["ReferralSuggested"]);
                            results.FPASuggested =  Convert.ToBoolean(dr["FPASuggested"]);
                            results.CategoryPosition = Convert.ToInt64(dr["CategoryPosition"]);
                            if(string.IsNullOrEmpty(dr["ModifiedDate"].ToString()))
                            {
                                results.ModifiedDate = (DateTime?)null;
                            }
                            else
                            {
                                results.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);

                            }
                            results.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            resultslist.Add(results);
                        }
                    }

                }
                if (_dataset.Tables[1] != null)
                {
                    if (_dataset.Tables[1].Rows.Count > 0)
                    {

                        foreach (DataRow dr in _dataset.Tables[1].Rows)
                        {
                          
                            TotalCount = Convert.ToInt32(dr["TotalCount"]);
                        }
                    }

                }

                long CategoryPosition = 0;
                DateTime date1 = DateTime.Now;
                DateTime date2 = DateTime.Now;
                if (sortOrder.ToUpper().Contains("CREATEDDATE")|| sortOrder.ToUpper().Contains("MODIFIEDDATE"))
                { 
                 CategoryPosition = resultslist.Select(x => x.CategoryPosition).FirstOrDefault();

                    if ((sortOrder.ToUpper() == "CREATEDDATE"))
                    {
                        date1 = resultslist.OrderByDescending(x => x.CreatedDate).Select(x => x.CreatedDate).First();
                        CategoryPosition = resultslist.Where(x => x.CreatedDate == date1).Select(x => x.CategoryPosition).FirstOrDefault();
                        sortList = resultslist.OrderBy(x => x.CreatedDate).Where(x => x.CategoryPosition == CategoryPosition).ToList();

                    }
                    else
                    {
                        date2 = (DateTime)resultslist.OrderByDescending(x => x.ModifiedDate).Select(x => x.ModifiedDate).First();
                        CategoryPosition = resultslist.Where(x => x.ModifiedDate == date2).Select(x => x.CategoryPosition).FirstOrDefault();
                        sortList = resultslist.OrderBy(x => x.ModifiedDate).Where(x => x.CategoryPosition == CategoryPosition).ToList();
                    }


                    foreach (var item in sortList)
                {
                    results = new AssessmentResults();
                    results.AssessmentResultId = item.AssessmentResultId;
                    results.MatrixId = item.MatrixId;
                    results.AssessmentGroupId = item.AssessmentGroupId;
                    results.MatrixType = item.MatrixType;
                    results.MatrixValue = item.MatrixValue;
                    results.AssessmentGroupType = item.AssessmentGroupType;
                    results.Description = item.Description;
                    results.ReferralSuggested = item.ReferralSuggested;
                    results.FPASuggested = item.FPASuggested;
                    results.CategoryPosition = item.CategoryPosition;
                    results.ModifiedDate = item.ModifiedDate;
                    results.CreatedDate = item.CreatedDate;
                    resultslist2.Add(results);

                }

                    var list2 = resultslist.OrderBy(x => x.CategoryPosition).Where(x => x.CategoryPosition != CategoryPosition).ToList();

                    foreach (var item1 in list2)
                {
                    results = new AssessmentResults();
                    results.AssessmentResultId = item1.AssessmentResultId;
                    results.MatrixId = item1.MatrixId;
                    results.AssessmentGroupId = item1.AssessmentGroupId;
                    results.MatrixType = item1.MatrixType;
                    results.MatrixValue = item1.MatrixValue;
                    results.AssessmentGroupType = item1.AssessmentGroupType;
                    results.Description = item1.Description;
                    results.ReferralSuggested = item1.ReferralSuggested;
                    results.FPASuggested = item1.FPASuggested;
                    results.CategoryPosition = item1.CategoryPosition;
                    results.ModifiedDate = item1.ModifiedDate;
                    results.CreatedDate = item1.CreatedDate;
                    resultslist2.Add(results);

                }
                }
                else
                {
                    resultslist2 = resultslist;
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            finally
            {
                DataAdapter.Dispose();
                command.Dispose();
            }
            return resultslist2;
        }

      
        public int GetAssementResultCount()
        {
            int totalcount = 0;
            try
            {
                string queryCommand = "TOTALRECORD";
                command.Parameters.Clear();
                Connection.Open();
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "SP_AssessmentResults";
                var totalrecord = command.ExecuteScalar();
                totalcount = Convert.ToInt32(totalrecord);
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
            return totalcount;
        }

        public bool UpdateAssessmentResult(AssessmentResults results)
        {
            bool isRowAffected = false;
            string queryCommand = "UPDATE";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@AssessmentGroupId", results.AssessmentGroupId));
                command.Parameters.Add(new SqlParameter("@MatrixId", results.MatrixId));
                command.Parameters.Add(new SqlParameter("@Description", results.Description));
                command.Parameters.Add(new SqlParameter("@AssessmentResultId", results.AssessmentResultId));
                command.Parameters.Add(new SqlParameter("@ReferralSuggested", results.ReferralSuggested));
                command.Parameters.Add(new SqlParameter("@FPASuggessted", results.FPASuggested));
                command.Parameters.Add(new SqlParameter("@UserId", results.UserId));
                command.Parameters.Add(new SqlParameter("@AgencyId", results.AgencyId));
                command.Parameters.Add(new SqlParameter("@Command", queryCommand));
                command.CommandText = "SP_AssessmentResults";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }

        public bool DeleteAssessmentResults(long resultId, Guid userId, Guid? agencyId)
        {
            bool isRowAffected = false;
            string queryCommand = "DELETE";
            try
            {

                if (Connection.State == ConnectionState.Open)
                    Connection.Close();
                Connection.Open();
                command.Connection = Connection;
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AssessmentResultId", resultId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@AgencyId", agencyId);
                command.Parameters.AddWithValue("@Command", queryCommand);
                command.CommandText = "SP_AssessmentResults";
                int RowsAffected = command.ExecuteNonQuery();
                if (RowsAffected > 0)
                    isRowAffected = true;
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
            return isRowAffected;
        }

    }
}
