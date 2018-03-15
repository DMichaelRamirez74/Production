using Fingerprints.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;

namespace Fingerprints.Controllers
{
    public class MatrixController : Controller
    {
        /*roleid=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
          roleid=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
          roleid=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
          roleid=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
          roleid=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
          roleid=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
          roleid=82b862e6-1a0f-46d2-aad4-34f89f72369a(teacher)
          roleid=b4d86d72-0b86-41b2-adc4-5ccce7e9775b(CenterManager)
          roleid=9ad1750e-2522-4717-a71b-5916a38730ed(Health Manager)
          */
        //[CustAuthFilter("b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult Matrix()
        {
            ViewBag.Layout = (Session["AgencyId"] != null) ? "~/Views/Shared/AgencyAdminLayout.cshtml" : "~/Views/Shared/SuperAdminLayout.cshtml";
            return View();
        }


        public ActionResult MatrixType()
        {
            ViewBag.Layout = (Session["AgencyId"] != null) ? "~/Views/Shared/AgencyAdminLayout.cshtml" : "~/Views/Shared/SuperAdminLayout.cshtml";

            return View();
        }

        [HttpPost]
        public JsonResult AddMatrxiType(long matrixvalue, string matrixtype)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {
                string querycommand = "CHECKMATRIX";
                Matrix matrix = new FingerprintsModel.Matrix();
                matrix.MatrixValue = matrixvalue;
                matrix.MatrixType = matrixtype.Trim();
                matrix.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                matrix.UserId = new Guid(Session["UserID"].ToString());

               isResult= new MatrixData().CheckMatrixType(matrix,querycommand);
                if(isResult)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
                isResult = new MatrixData().InsertMatrixType(matrix);
                if(isResult)
                {
                    returnResult = 1;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(returnResult);
        }

        [HttpPost]
        public JsonResult GetMatrixType(string sortOrder,string sortDirection,int pageSize,int requestedPage=0)
        {
            List<Matrix> matrixList = new List<Matrix>();
            Matrix matrix = new Matrix();
            int totalCount = 0;
            sortOrder = (sortOrder == "") ? "CreatedDate" : (sortOrder== "thSN")?"CreatedDate":sortOrder;
            matrix.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            matrix.UserId = new Guid(Session["UserID"].ToString());
            matrix.Status = true;
            sortDirection = (sortDirection == "") ? "DESC" : sortDirection;
            int skip = pageSize * (requestedPage - 1);
            matrixList = new MatrixData().GetMatrixType( out totalCount,matrix,sortOrder,sortDirection,pageSize,requestedPage,skip);

            return Json(new { matrixList, totalCount }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetMatrixTypeList()
        {
            List<Matrix> matrixList = new List<Matrix>();
            Matrix matrix = new Matrix();
            matrix.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            matrix.UserId = new Guid(Session["UserID"].ToString());
            matrix.Status = true;
            matrixList = new MatrixData().GetMatrixTypeList(matrix);

            return Json(matrixList, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult DeleteMatrixType(long ID)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {
               Guid? agencyid = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;

                isResult = new MatrixData().CheckMatrixRef(ID, agencyid);
                if(isResult)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
                isResult = new MatrixData().DeleteMatrixType(ID, agencyid);
                if(isResult)
                {
                    returnResult = 1;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult UpdateMatrixType( long matrixId, string matrixType, long matrixValue)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {
                string queryCommand = "CHECKUPDATE";
                Matrix matrix = new FingerprintsModel.Matrix();
                matrix.MatrixId = matrixId;
                matrix.MatrixType = matrixType;
                matrix.MatrixValue = matrixValue;
                matrix.UserId = new Guid(Session["UserID"].ToString());
                matrix.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;

                isResult = new MatrixData().CheckMatrixType(matrix, queryCommand);
                if (isResult)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
                isResult = new MatrixData().UpdateMatrixType(matrix);
                if(isResult)
                {
                    returnResult = 1;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
                //return Json("Error occured please try again.");
            }
            return Json(returnResult, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AnnualAssessment()
        {
            return View();
        }

        public JsonResult GetAnnualAssessment()
        {
            AnnualAssessment assessment = new FingerprintsModel.AnnualAssessment();
            assessment.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;

            assessment.UserId = new Guid(Session["UserID"].ToString());
            var assement = new MatrixData().GetAnnualAssessment(assessment);
            assement.UserName = Session["FullName"].ToString();
            return Json(assement, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public JsonResult AddAnnualAssessment(AnnualAssessment assessment)
        {
            bool isResult = false;

            try
            {

                assessment.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                assessment.UserId = new Guid(Session["UserID"].ToString());

                isResult = new MatrixData().InsertAnnualAssement(assessment);

            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AssessmentCategory()
        {
            return View();
        }
        

        public JsonResult GetAssessmentCategoryList()
        {
            AssessmentCategory assessmentCategory = new FingerprintsModel.AssessmentCategory();
            Guid? agencyID = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            var category = new MatrixData().GetAssessmentCategoryList(agencyID);
            return Json(category, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetAssessmentCategory(string sortOrder, string sortDirection, int pageSize = 0, int requestedPage = 1)
        {
            int totalCount = 0;
            switch (sortOrder)
            {
                case "":
                    sortOrder = "CreatedDate";
                    break;
                case null:
                    sortOrder = "CreatedDate";
                    break;
                case "thIN":
                    sortOrder = "AssessmentCatgoryId";
                    break;
                case "thCust":
                    sortOrder = "Category";
                    break;
            }
            AssessmentCategory assessmentCategory = new FingerprintsModel.AssessmentCategory();
            sortDirection = (sortDirection == "" ||sortDirection==null) ? "DESC" : sortDirection;
            int skip = pageSize * (requestedPage - 1);
            Guid? agencyID = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            var category = new MatrixData().GetAssessmentCategory(out totalCount, sortOrder,sortDirection,pageSize,requestedPage,skip, agencyID);
            return Json(new { category, totalCount }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult InsertAssessmentCategory(string categoryName,long categoryPosition)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {
                string queryCommand = "CHECKCATEGORY";
                AssessmentCategory assessmentCategory = new FingerprintsModel.AssessmentCategory();
                assessmentCategory.UserId = new Guid(Session["UserID"].ToString());
               assessmentCategory.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                assessmentCategory.Category = categoryName;
                assessmentCategory.CategoryPosition = categoryPosition;
                isResult = new MatrixData().CheckAssessmentCategory(assessmentCategory,queryCommand);
                if(isResult)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }

                isResult = new MatrixData().InsertAssessmentCategory(assessmentCategory);
                if(isResult)
                {
                    returnResult = 1;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }

                return Json(isResult, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(isResult, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public JsonResult UpdateAssessmentCategory(string categoryName,long categoryId,long position)
        {
            bool isResult = false;
            int returnResult = 0;
            string queryCommand = "CHECKUPDATE";
            AssessmentCategory assessmentCategory = new FingerprintsModel.AssessmentCategory();
            assessmentCategory.UserId = new Guid(Session["UserID"].ToString());
            assessmentCategory.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            assessmentCategory.Category = categoryName.Trim();
            assessmentCategory.AssessmentCategoryId = categoryId;
            assessmentCategory.CategoryPosition = position;
            isResult = new MatrixData().CheckAssessmentCategory(assessmentCategory,queryCommand);
            if(isResult)
            {
                returnResult = 2;
                return Json(returnResult, JsonRequestBehavior.AllowGet);
            }
            isResult = new MatrixData().UpdateAssessmentCategory(assessmentCategory);
            if(isResult)
            {
                returnResult = 1;
                return Json(returnResult, JsonRequestBehavior.AllowGet);
            }
            return Json(returnResult, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult DeleteAssessmentCategory(long categoryId)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {

           
            AssessmentCategory assessmentCategory = new FingerprintsModel.AssessmentCategory();
            assessmentCategory.UserId = new Guid(Session["UserID"].ToString());
            assessmentCategory.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            assessmentCategory.AssessmentCategoryId = categoryId;
            const string command = "CHECKCATREF";
            isResult = new MatrixData().CheckAssessmentCategory(assessmentCategory,command);
            if(isResult)
            {
                returnResult = 2;
                return Json(returnResult, JsonRequestBehavior.AllowGet);
            }
            isResult = new MatrixData().DeleteAssessmentCategory(assessmentCategory);
                {
                    returnResult = 1;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(returnResult, JsonRequestBehavior.AllowGet);

        }
        public ActionResult AssessmentGroup()
        {
            return View();
        }

        public ActionResult GetAssessmentGroup( string sortOrder,string sortDirection,int pageSize=0,int requestedPage=1)
            {
            List<AssessmentGroup> assessmentGroup = new List<FingerprintsModel.AssessmentGroup>();
            long totalCount = 0;

            try
            {
                switch(sortOrder)
                {
                    case "":
                        sortOrder = "CategoryPosition";
                        break;
                    case "thIN":
                        sortOrder = "Category";
                        break;
                    case "thCust":
                        sortOrder = "AssessmentGroupType";
                        break;
                    case "thStatus":
                        sortOrder = "AssessmentGroupType";
                        break;
                }

                sortDirection = (sortDirection == "") ? "ASC" : sortDirection;
                
                int skip = pageSize * (requestedPage - 1);
                Guid? agencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                assessmentGroup = new MatrixData().GetAssessmentGroup(out totalCount,sortOrder,sortDirection,pageSize,requestedPage,skip,agencyId);

            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
                //return Json("Error occured please try again.");
            }

            return Json(new { assessmentGroup, totalCount }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertAssessmentGroup(string groupType,long categoryId, int status)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {
                string queryCommand = "CHECK";
                AssessmentGroup assessmentGroup = new FingerprintsModel.AssessmentGroup();
                assessmentGroup.UserId = new Guid(Session["UserID"].ToString());
                assessmentGroup.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                assessmentGroup.AssessmentCategoryId = categoryId;
                assessmentGroup.IsActive = Convert.ToBoolean(status);
                assessmentGroup.AssessmentGroupType = groupType;
                isResult=  new MatrixData().CheckAssessmentGroup(assessmentGroup, queryCommand);
                if(isResult)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
                isResult = new MatrixData().InsertAssessmentGroup(assessmentGroup);
                returnResult = 1;
                return Json(returnResult, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(returnResult, JsonRequestBehavior.AllowGet);
            }
           
        }

        [HttpPost]
        public JsonResult UpdateAssessmentGroup(string groupType, long categoryId, int status,long groupId)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {

            string queryCommand = "UPDATECHECK";
            AssessmentGroup assessmentGroup = new FingerprintsModel.AssessmentGroup();
            assessmentGroup.UserId = new Guid(Session["UserID"].ToString());
            assessmentGroup.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            assessmentGroup.AssessmentCategoryId = categoryId;
            assessmentGroup.IsActive = Convert.ToBoolean(status);
            assessmentGroup.AssessmentGroupType = groupType;
            assessmentGroup.AssessmentGroupId = groupId;
            isResult = new MatrixData().CheckAssessmentGroup(assessmentGroup,queryCommand);
                if (isResult)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
                isResult = new MatrixData().UpdateAssessmentGroup(assessmentGroup);
                returnResult = 1;
                return Json(returnResult, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                return Json(returnResult, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult DeleteAssessmentGroup(long groupId)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {
                AssessmentGroup group = new FingerprintsModel.AssessmentGroup();
                group.AssessmentGroupId = groupId;
                group.UserId = new Guid(Session["UserID"].ToString());
                group.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                const string command = "CHECKGROUPREF";
                isResult = new MatrixData().CheckAssessmentGroup(group, command);
                if(isResult)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
                isResult = new MatrixData().DeleteAssessmentGroup(group);
                if(isResult)
                {
                    returnResult = 1;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
            }
            return Json(returnResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AssessmentQuestions()
        {
            return View();
        }

        public ActionResult Questions()
        {
            return View();
        }


        public ActionResult GetAssessmentType()
        {
            List<SelectListItem> assessmentType = new List<SelectListItem>();
           Guid? agencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            assessmentType = new MatrixData().GetAssessmentType(agencyId);
            return Json(assessmentType, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddQuestions(long GroupTypeVal, string GroupTypeText)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {
                QuestionsModel question = new QuestionsModel();
                question.AssessmentGroupId = GroupTypeVal;
                question.QuestionText = GroupTypeText.Trim();
                question.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                question.UserId = new Guid(Session["UserID"].ToString());
                question.AssessmentGroupId = GroupTypeVal;
                string command = "CHECKQUESTION";
                isResult = new MatrixData().CheckQuestions(question,command);
                if(isResult)
                {
                    returnResult = 2;
                    return Json(returnResult,JsonRequestBehavior.AllowGet);

                }
                isResult = new MatrixData().InsertQuestionType(question);
                if (isResult)
                {
                    returnResult = 1;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetQuestions(string sortOrder,string sortDirection,int pageSize,int requestedPage=0)
        {
            List<QuestionsModel> matrixList = new List<QuestionsModel>();
            QuestionsModel question = new QuestionsModel();
            int totalCount = 0;

            switch (sortOrder)
            {
                case "":
                    sortOrder = "CategoryPosition,AssessmentGroupType";
                    break;
                case null:
                    sortOrder = "CategoryPosition,AssessmentGroupType";
                    break;
                case "thIN":
                    sortOrder = "AssessmentGroupType";
                    break;
                case "thCust":
                    sortOrder = "AssessmentQuestion";
                    break;
                case "thStatus":
                    sortOrder = "AssessmentGroupType";
                    break;
            }

           // sortOrder = (sortOrder == "") ? "CreatedDate" : sortOrder;
            sortDirection = (string.IsNullOrEmpty(sortDirection)) ? "ASC" : sortDirection;
            question.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            question.UserId = new Guid(Session["UserID"].ToString());
            question.Status = true;
            int skip = pageSize * (requestedPage - 1);
            matrixList = new MatrixData().GetQuestionType( out totalCount,question,sortOrder,sortDirection,pageSize,requestedPage,skip);
            return Json(new { matrixList, totalCount }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult DeleteQuestions(int ID)
        {
            bool isResult = false;
            try
            {
            Guid? AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                isResult = new MatrixData().DeleteQuestionType(ID,AgencyId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult UpdateQuestions(long GroupType, string Questiontype,long QuestionId)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {
                QuestionsModel question = new QuestionsModel();
                question.AssessmentQuestion = Questiontype;
                question.AssessmentGroupId = GroupType;
                question.AssessmentQuestionId = QuestionId;
                question.UserId = new Guid(Session["UserID"].ToString());
               question.AgencyId= (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                const string command = "CHECKQUESTIONUPDATE";
                isResult = new MatrixData().CheckQuestions(question,command);
                if(isResult)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);

                }
                isResult = new MatrixData().UpdateQuestionType(question);
                if (isResult)
                {
                    returnResult = 1;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
                //return Json("Error occured please try again.");
            }
            return Json(returnResult, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AssessmentResults()
        {
            return View();
        }

       [HttpPost]

       public JsonResult GetGroupDetails()
        {
            List<AssessmentGroup> groupList = new List<FingerprintsModel.AssessmentGroup>();
             Guid? AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            groupList = new MatrixData().GetAssessmentGroupList(AgencyId);
            return Json(groupList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public JsonResult InsertAssessmentResults(long groupID, long matrixID, string Description, bool referralSuggested, bool FPASuggessted)
        {
            int returnResult = 0;
            bool isResult = false;

            try
            {
                string querycomand = "CHECKRESULT";
                AssessmentResults results = new FingerprintsModel.AssessmentResults();

                results.UserId = new Guid(Session["UserID"].ToString());
                results.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                results.MatrixId = matrixID;
                results.AssessmentGroupId = groupID;
                results.ReferralSuggested = referralSuggested;
                results.FPASuggested = FPASuggessted;
                results.Description = Description;
                isResult = new MatrixData().checkAssessmentResult(results, querycomand);
                if(isResult)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
                isResult = new MatrixData().InsertAssessmentResult(results);
                if(isResult)
                {
                    returnResult = 1;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                clsError.WriteException(ex);
                //return Json("Error occured please try again.");
            }
            return Json(returnResult, JsonRequestBehavior.AllowGet);
        }




     public ActionResult GetAssessmentResults(string sortOrder, string sortDirection, int pageSize = 0, int requestedPage = 1)
        {
            List<AssessmentResults> assessmentResults = new List<FingerprintsModel.AssessmentResults>();
            int totalCount = 0;

            try
            {
                switch (sortOrder)
                {
                    case "":
                        sortOrder = "CategoryPosition,AssessmentGroupType";
                        break;
                    case "thIN":
                        sortOrder = "AssessmentGroupType";
                        break;
                    case "thCust":
                        sortOrder = "MatrixType";
                        break;
                    case "thDesc":
                        sortOrder = "Description";
                        break;
                    case "thRefSug":
                        sortOrder = "ReferralSuggested";
                        break;
                    case "thFpaSug":
                        sortOrder = "FPASuggested";
                        break;
                    case "thSN":
                        sortOrder = "CreatedDate";
                        break;
                }

                sortDirection = (sortDirection == "") ? "ASC" : sortDirection;
                int skip = pageSize * (requestedPage - 1);
                Guid? AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                assessmentResults = new MatrixData().GetAssessmentResults( out totalCount, sortOrder, sortDirection, pageSize, requestedPage,skip, AgencyId);
                
            }

            catch (Exception ex)
            {
                clsError.WriteException(ex);
                //return Json("Error occured please try again.");
            }

            return Json(new { assessmentResults, totalCount }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAssessmentResults(long groupId, long matrixId, bool referralStatus, bool fpaStatus,string Description, long resultID)
        {

            int returnResult = 0;
            bool isResult = false;

            try
            {
                string querycomand = "CHECKUPDATE";
                AssessmentResults results = new FingerprintsModel.AssessmentResults();

                results.UserId = new Guid(Session["UserID"].ToString());
                results.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                results.MatrixId = matrixId;
                results.AssessmentGroupId = groupId;
                results.ReferralSuggested = referralStatus;
                results.FPASuggested = fpaStatus;
                results.Description = Description;
                results.AssessmentResultId = resultID;
                isResult = new MatrixData().checkAssessmentResult(results, querycomand);
                if (isResult)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
                isResult = new MatrixData().UpdateAssessmentResult(results);
                if (isResult)
                {
                    returnResult = 1;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                //return Json("Error occured please try again.");
            }
            return Json(returnResult, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteAssessmentResults(long resultId)
        {
            bool isResult = false;
            try
            {
                Guid UserId = new Guid(Session["UserID"].ToString());
                Guid? AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                isResult = new MatrixData().DeleteAssessmentResults(resultId,UserId, AgencyId);
            }
            catch (Exception ex)
            {
                clsError.WriteException(ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Acronym()
        {
            return View();
        }

        public JsonResult AddAcronym(string AcronymName, long acronymId =0)
        {
            bool isResult = false;
            try
            {
                Acronym acronym = new FingerprintsModel.Acronym();
                acronym.AcronymId = acronymId;
                acronym.AcronymName = AcronymName;
                acronym.UserId= new Guid(Session["UserID"].ToString());
                acronym.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                isResult = new MatrixData().InsertAcronym(acronym);
             
                return Json(isResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        public JsonResult GetAcronymList()
        {
           
            Acronym acronym = new Acronym();

            acronym.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            acronym.UserId = new Guid(Session["UserID"].ToString());
            acronym.Status = true;
            acronym = new MatrixData().GetAcronym(acronym);
            return Json(acronym, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAcronym()
      {
            Acronym acronym = new Acronym();
            acronym.UserId = new Guid(Session["UserID"].ToString());
            acronym.AgencyId= (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
            acronym = new MatrixData().GetAcronym(acronym);
            return Json(acronym, JsonRequestBehavior.AllowGet);

        }

        public JsonResult UpdateAcronym(long AcronymId, string AcronymName)
        {
            bool isResult = false;
            int returnResult = 0;
            try
            {
                string queryCommand = "UPDATECHECK";
                Acronym acronym = new FingerprintsModel.Acronym();
                acronym.AcronymId = AcronymId;
                acronym.AcronymName = AcronymName;
                acronym.UserId = new Guid(Session["UserID"].ToString());
                acronym.AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                isResult = new MatrixData().CheckUpdateAcronym(acronym, queryCommand);
                if (isResult)
                {
                    returnResult = 2;
                    return Json(returnResult, JsonRequestBehavior.AllowGet);
                }
                isResult = new MatrixData().UpdateAcronym(acronym);
                returnResult = 1;
                return Json(returnResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
                return Json("Error occured please try again.");
            }

        }

        public JsonResult DeleteAcronym(long ID)
        {
            bool isResult = false;
            try
            {
               Guid? AgencyId = (Session["AgencyId"] != null) ? new Guid(Session["AgencyId"].ToString()) : (Guid?)null;
                isResult = new MatrixData().DeleteAcronym(ID,AgencyId);
            }
            catch (Exception ex)
            {

                clsError.WriteException(ex);
                //return Json("Error occured please try again.");
            }
            return Json(isResult, JsonRequestBehavior.AllowGet);
        }

    }
}
