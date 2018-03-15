using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FingerprintsModel;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.Web.Mvc;

namespace FingerprintsData
{
    public class MatrixReportData
    {
        SqlConnection Connection = connection.returnConnection();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter DataAdapter = null;
        DataTable _dataTable = null;
        DataSet _dataset = null;



        public ArrayList GetMatrixReportData(out List<MasterMatrixSummary> masterMatrix, out List<ChangePercentage> changePercentageList, out string programYear, Guid agencyId, Guid userId, Guid roleId, string year)
        {
            string queryCommand = "SELECT";
            MatrixSummary summary = null;
            MasterMatrixSummary masterSummary = null;
            List<MatrixSummary> summaryList = new List<MatrixSummary>();
            List<MatrixSummary> summaryList1 = new List<MatrixSummary>();
            List<MasterMatrixSummary> masterSummaryList = new List<MasterMatrixSummary>();
            ArrayList list1 = new ArrayList();
            ArrayList list2 = new ArrayList();
            List<long> catIdList = new List<long>();
            List<long> aNoList = new List<long>();

            changePercentageList = new List<ChangePercentage>();
            ChangePercentage changePercentage = null;
            programYear = string.Empty;
            const string upArrow = "/images/dw-arw.png";
            const string downArrow = "/images/tp-arw.png";

            const string fontColorDecrease = "rgb(231, 76, 60)";
            const string fontColorIncrease = "rgb(46, 204, 113)";
            const string fontColorBlack = "rgb(0, 0, 0)";
            masterMatrix = new List<MasterMatrixSummary>();
            _dataset = new DataSet();
            command.Connection = Connection;
            command.CommandText = "USP_MatrixSummary";
            command.Parameters.Add(new SqlParameter("@AgencyId", agencyId));
            command.Parameters.Add(new SqlParameter("@ProgramTypeYear", year));
            command.Parameters.Add(new SqlParameter("@UserId", userId));
            command.Parameters.Add(new SqlParameter("@RoleId", roleId));
            command.Parameters.Add(new SqlParameter("@Command", queryCommand));

            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);

            da.Fill(_dataset);
            if (_dataset.Tables[0] != null)
            {
                if (_dataset.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[0].Rows)
                    {

                        summary = new MatrixSummary();

                        summary.AgencyId = new Guid(dr["AgencyId"].ToString());
                        summary.TotalAnswered = Convert.ToInt64(dr["TotalAnswered"]);
                        summary.TotalClients = Convert.ToInt64(dr["TotalClients"]);
                        summary.TotalPoints = Convert.ToInt64(dr["TotalPoints"]);
                        summary.TotalRatio = Convert.ToDouble(dr["TotalRatio"]);
                        summary.AssessmentCategoryId = Convert.ToInt64(dr["AssessmentCategoryId"]);
                        summary.AssessmentGroupId = Convert.ToInt64(dr["AssessmentGroupId"]);
                        summary.AssessmentGroupType = dr["AssessmentGroupType"].ToString();
                        summary.AssessmentNumber = Convert.ToInt64(dr["AssessmentNumber"]);
                        summary.Category = dr["Category"].ToString();
                        summary.ProgramYear = string.IsNullOrEmpty(dr["ProgramYear"].ToString()) ? "" : dr["ProgramYear"].ToString();
                        summary.CategoryPosition = Convert.ToInt64(dr["CategoryPosition"]);
                        summary.MaxMatrixValue = Convert.ToInt64(dr["MaxMatrixValue"]);
                        summaryList.Add(summary);
                    }

                    programYear = summaryList.Where(x => x.ProgramYear != "").Select(x => x.ProgramYear).Distinct().FirstOrDefault().ToString();
                    var convlist = summaryList.GroupBy(x => x.AssessmentGroupId).ToList();
                    double as1value = 0;
                    double as2value = 0;
                    double as3value = 0;

                    foreach (var item in convlist)
                    {

                        changePercentage = new ChangePercentage();
                        foreach (var item2 in item)
                        {
                            if (item2.AssessmentNumber == 1)
                            {
                                as1value = item2.TotalRatio;
                            }
                            if (item2.AssessmentNumber == 2)
                            {
                                as2value = item2.TotalRatio;
                            }
                            if (item2.AssessmentNumber == 3)
                            {
                                as3value = item2.TotalRatio;
                            }
                            changePercentage.AssessmentGroupId = item2.AssessmentGroupId;
                            changePercentage.AssessmentCategoryId = item2.AssessmentCategoryId;
                            changePercentage.AssessmentGroupType = item2.AssessmentGroupType;
                            changePercentage.Category = item2.Category;
                            changePercentage.CategoryPosition = item2.CategoryPosition;
                        }

                        if (as3value != 0 && as1value != 0)
                        {
                            changePercentage.ChangePercent = ((as1value - as3value) / (as1value)) * 100;

                        }
                        else if (as1value != 0 && as2value != 0)
                        {
                            changePercentage.ChangePercent = ((as1value - as2value) / (as1value)) * 100;
                        }

                        if (changePercentage.ChangePercent > 0)
                        {
                            changePercentage.ArrowType = downArrow;
                            changePercentage.FontColor = fontColorDecrease;

                        }
                        else if (changePercentage.ChangePercent < 0)
                        {
                            changePercentage.ArrowType = upArrow;
                            changePercentage.FontColor = fontColorIncrease;
                        }
                        else if (changePercentage.ChangePercent == 0 && ((as1value != 0 && as2value != 0) || (as1value != 0 && as3value != 0)))
                        {

                            changePercentage.ArrowType = upArrow;
                            changePercentage.FontColor = fontColorIncrease;
                        }
                        else if (as1value == 0 && as2value == 0 && as3value == 0)
                        {
                            changePercentage.ArrowType = string.Empty;
                            changePercentage.FontColor = fontColorBlack;
                            changePercentage.ChangePercent = 0;
                        }
                        else if (as1value == 0 && as2value == 0 || as3value == 0)
                        {
                            changePercentage.ArrowType = string.Empty;
                            changePercentage.FontColor = fontColorBlack;
                            changePercentage.ChangePercent = 0;
                        }
                        else if (as3value == 0 && as2value == 0 || as1value == 0)
                        {
                            changePercentage.ArrowType = string.Empty;
                            changePercentage.FontColor = fontColorBlack;
                            changePercentage.ChangePercent = 0;
                        }


                        changePercentage.ChangePercent = Math.Abs(changePercentage.ChangePercent);
                        changePercentageList.Add(changePercentage);
                    }


                    catIdList = summaryList.Select(x => x.AssessmentCategoryId).Distinct().ToList();
                    aNoList = summaryList.Select(x => x.AssessmentNumber).Distinct().ToList();

                    foreach (var id in catIdList)
                    {
                        summaryList1 = summaryList.Where(x => x.AssessmentCategoryId == id).ToList();

                        list2.Add(summaryList1);
                    }

                }
            }
            if (_dataset.Tables[1] != null)
            {
                if (_dataset.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in _dataset.Tables[1].Rows)
                    {

                        masterSummary = new MasterMatrixSummary();
                        masterSummary.AssessmentNumberMaster = Convert.ToInt64(dr["AssessmentNumber"]);
                        masterSummary.FamiliesEntered = Convert.ToInt64(dr["FamiliesEntered"]);
                        masterSummary.PercentFamilyEntered = string.IsNullOrEmpty(dr["PercentFamilyEntered"].ToString()) ? 0 : Convert.ToDouble(dr["PercentFamilyEntered"]);
                        masterSummaryList.Add(masterSummary);
                    }
                }

            }



            masterMatrix = masterSummaryList;

            return list2;
        }

        /// <summary>
        /// Description for Matrix Summary
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="clientId"></param>
        /// <param name="agencyid"></param>
        /// <returns></returns>

        public List<SelectListItem> GetActiveProgramYear(Guid agencyId)
        {
            MatrixScore score = new MatrixScore();
            DataSet ds = null;
            List<SelectListItem> activeYearList = new List<SelectListItem>();
            ds = new DataSet();
            string queryCommand = "GETYEAR";
            command.Connection = Connection;
            command.CommandText = "USP_MatrixSummary";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@AgencyId", agencyId);
            command.Parameters.AddWithValue("@Command", queryCommand);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (!string.IsNullOrEmpty(dr["ActiveProgramYear"].ToString()))
                    {
                        activeYearList.Add(new SelectListItem
                        {
                            Text = dr["ActiveProgramYear"].ToString()
                        });
                    }
                }
                //score.ActiveYearList = activeYearList;
            }
            return activeYearList;
        }


        /// <summary>
        /// Description for Matrix Summary
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        /// 
        public List<AssessmentResults> GetDescriptionsummary(int groupId, Guid agencyid)
        {
            List<AssessmentResults> resultList = new List<AssessmentResults>();
            AssessmentResults results = null;
            try
            {
                string queryCommand = "GETDESCRIPTIONSUMMARY";
                DataSet ds = new DataSet();
                command.Connection = Connection;
                command.CommandText = "USP_MatrixSummary";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AssessmentGroupId", groupId);
                //  command.Parameters.AddWithValue("@ClientId", clientId);
                command.Parameters.AddWithValue("@AgencyId", agencyid);
                command.Parameters.AddWithValue("@Command", queryCommand);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        results = new AssessmentResults();
                        results.AssessmentResultId = Convert.ToInt64(dr["AssessmentResultId"]);
                        results.Description = dr["Description"].ToString();
                        results.MatrixValue = Convert.ToInt64(dr["MatrixValue"]);
                        // results.MatrixId = (dr["AssessmentNumber"].ToString() == "") ? 0 : Convert.ToInt32(dr["AssessmentNumber"]);
                        resultList.Add(results);
                    }
                }
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
            return resultList;
        }


        /// <summary>
        /// Get Questions For Matrix Summary
        /// </summary>
        /// <param name="matrixScore"></param>
        /// <returns></returns>
        public QuestionsModel GetSummaryquestions(long groupId)
        {
            QuestionsModel assessmentQuestions = null;
            try
            {
                string queryCommand = "GETQUESTIONSSUMMARY";
                DataSet ds = new DataSet();
                command.Connection = Connection;
                command.CommandText = "USP_MatrixSummary";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@AssessmentGroupId", groupId);
                //command.Parameters.AddWithValue("@AgencyId", groupId);
                command.Parameters.AddWithValue("@Command", queryCommand);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        assessmentQuestions = new QuestionsModel();
                        assessmentQuestions.AssessmentQuestionId = Convert.ToInt64(dr["AssessmentQuestionId"]);
                        assessmentQuestions.AssessmentQuestion = dr["AssessmentQuestion"].ToString();
                        assessmentQuestions.AssessmentGroupId = Convert.ToInt64(dr["AssessmentGroupId"]);
                    }
                }
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
            return assessmentQuestions;
        }


    }
}
