using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FingerprintsData;
using FingerprintsModel;
using Fingerprints.Filters;
using System.Threading;
using Fingerprints.ViewModel;
using System.Globalization;
using System.IO;
using System.Configuration;
using Fingerprints.CustomClasses;
using System.Text;
using System.Data;
namespace Fingerprints.Controllers
{
    public class NurseController : Controller
    {
        /*roleid=f87b4a71-f0a8-43c3-aea7-267e5e37a59d(Super Admin)
        roleid=a65bb7c2-e320-42a2-aed4-409a321c08a5(GenesisEarth Administrator)
        roleid=a31b1716-b042-46b7-acc0-95794e378b26(Health/Nurse)
        roleid=2d9822cd-85a3-4269-9609-9aabb914d792(HR Manager)
        roleid=94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d(Family Service Worker)
        roleid=e4c80fc2-8b64-447a-99b4-95d1510b01e9(Home Visitor)
        roleid=82b862e6-1a0f-46d2-aad4-34f89f72369a(teacvher)
        roleid=b4d86d72-0b86-41b2-adc4-5ccce7e9775b(CenterManager)
        roleid=9ad1750e-2522-4717-a71b-5916a38730ed(Health Manager)
        */
        FamilyData _family = new FamilyData();
        NurseData _nurse = new NurseData();
        public ActionResult Index()
        {
            return View();
        }
      //  [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult ViewCenterDetails(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]) || Convert.ToString(Request.QueryString["id"]) != "0")
                    id = FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["id"].ToString());
                if (!string.IsNullOrEmpty(Request.QueryString["Name"]))
                    ViewBag.name = FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["Name"].ToString());
                ViewBag.userlist = _family.Getallclients(id, Session["AgencyID"].ToString(), Session["UserID"].ToString());
                Nurse obj = new Nurse();
                obj.CenterID = id;
                return View(obj);
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
       // [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult ChildDetails(string id = "0")
        {
            try
            {
                id = FingerprintsModel.EncryptDecrypt.Decrypt64(id);
            
                int yakkrid = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["Type"]) || Convert.ToString(Request.QueryString["Type"]) != "0")
                {
                    yakkrid = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["Type"].ToString()));
                    TempData["yakkrid"] = yakkrid;
                }
                Nurse _familyinfo = _nurse.GetData_AllDropdown(Session["AgencyID"].ToString(), Session["UserID"].ToString());
                if (id == "0")
                {

                    ViewBag.mode = 0;
                    ViewBag.lang = _familyinfo.langList;
                    TempData["familyinfo"] = ViewBag.lang;
                    ViewBag.RaceSubCategory = _familyinfo.raceCategory;
                    TempData["RaceSubcategory"] = ViewBag.RaceSubCategory;
                    ViewBag.Race = _familyinfo.raceList;
                    TempData["Race"] = ViewBag.Race;
                    ViewBag.Relationship = _familyinfo.relationship;
                    TempData["Relationship"] = ViewBag.Relationship;
                    TempData["Programtype"] = _familyinfo.AvailableProgram;
                }
                else
                {
                    Nurse obj = new Nurse();
                    obj = _nurse.EditFamilyInfo(id, yakkrid, Session["AgencyID"].ToString(), Session["UserID"].ToString(), Session["RoleID"].ToString());
                    Session["Docsstorage"] = obj.docstorage.ToString();
                    ViewBag.AvailableProg = obj.AvailableProgram;
                    TempData["AvailableProgram"] = ViewBag.AvailableProg;
                    _familyinfo._Screening = obj._Screening;
                    if (_familyinfo._Screening != null)
                    {
                        ViewBag.tabpageScreening = "1";
                    }
                    obj.SchoolList = _familyinfo.SchoolList;
                    obj.ImmunizationRecords = _familyinfo.ImmunizationRecords;
                    obj.AvailableDental = _familyinfo.AvailableDental;
                    obj.AvailableDiagnosedDisease = _familyinfo.AvailableDiagnosedDisease;
                    obj.AvailableDisease = _familyinfo.AvailableDisease;
                    obj.AvailableEHS = _familyinfo.AvailableEHS;
                    obj.AvailableChildDrink = _familyinfo.AvailableChildDrink;//Changes
                    obj.AvailableChildDietFull = _familyinfo.AvailableChildDietFull;
                    obj.AvailableChildVitamin = _familyinfo.AvailableChildVitamin;
                    obj.AvailableService = _familyinfo.AvailableService;
                    obj.AvailablePrblms = _familyinfo.AvailablePrblms;
                    // empty the response from other users
                    obj.AcceptApplicant = "";
                    obj.AcceptDesc = "";
                    obj.RejectDesc = "";
                    if (obj.FamilyHousehold.Income1 == null)
                        obj.FamilyHousehold.Income1 = GenerateIncomeList();
                    if (obj.FamilyHousehold.Income2 == null)
                        obj.FamilyHousehold.Income2 = GenerateIncomeList1();
                    // obj.FamilyHousehold.Income1 = obj.Income1;
                    // = _familyinfo.Income1;
                    obj.Income2 = _familyinfo.Income2;
                    Session["HouseholdID"] = id;
                    ViewBag.mode = 1;
                    ViewBag.PMConditions = _familyinfo.PMCondtnList;
                    TempData["PMConditions"] = ViewBag.PMConditions;
                    ViewBag.lang = _familyinfo.langList;
                    TempData["familyinfo"] = ViewBag.lang;
                    ViewBag.RaceSubCategory = _familyinfo.raceCategory;
                    TempData["RaceSubcategory"] = ViewBag.RaceSubCategory;
                    ViewBag.Race = _familyinfo.raceList;
                    TempData["Race"] = ViewBag.Race;
                    ViewBag.Relationship = _familyinfo.relationship;
                    TempData["Relationship"] = ViewBag.Relationship;
                    TempData["Programtype"] = _familyinfo.AvailableProgram;
                    TempData["Schooldistrict"] = _familyinfo.SchoolList;
                    TempData["ImmunizationRecords"] = _familyinfo.ImmunizationRecords;
                    //Child Health History
                    TempData["AvailableDisease"] = _familyinfo.AvailableDisease;
                    TempData["AvailableDiagnosedDisease"] = _familyinfo.AvailableDiagnosedDisease;
                    TempData["AvailableDental"] = _familyinfo.AvailableDental;
                    TempData["AvailableEHS"] = _familyinfo.AvailableEHS;
                    TempData["AvailableChildDrink"] = _familyinfo.AvailableChildDrink;
                    TempData["AvailableService"] = _familyinfo.AvailableService;
                    TempData["AvailablePrblms"] = _familyinfo.AvailablePrblms;
                    ViewBag.diet = _familyinfo.dietList;
                    TempData["DietInfo"] = ViewBag.diet;
                    ViewBag.food = _familyinfo.foodList;
                    TempData["foodInfo"] = ViewBag.food;
                    ViewBag.ChildFeed = _familyinfo.CFeedList;
                    TempData["ChildFeed"] = ViewBag.ChildFeed;
                    ViewBag.hungry = _familyinfo.ChungryList;
                    TempData["hungry"] = ViewBag.hungry;
                    TempData["AvailableChildDietFull"] = _familyinfo.AvailableChildDietFull;
                    TempData["AvailableChildVitamin"] = _familyinfo.AvailableChildVitamin;
                    ViewBag.ChildFormula = _familyinfo.CFormulaList;
                    TempData["ChildFormula"] = ViewBag.ChildFormula;
                    ViewBag.ChildCereal = _familyinfo.CFeedCerealList;
                    TempData["ChildFormula"] = ViewBag.ChildCereal;
                    ViewBag.ChildReferal = _familyinfo.CReferalCriteriaList;
                    TempData["ChildFormula"] = ViewBag.ChildReferal;
                    ViewBag.ChildCereal = _familyinfo.CFeedCerealList;
                    TempData["ChildCereal"] = ViewBag.ChildCereal;
                    ViewBag.ChildReferal = _familyinfo.CReferalCriteriaList;
                    TempData["ChildReferal"] = ViewBag.ChildReferal;
                    TempData["Customscreening"] = obj.customscreening;
                    //TempData["centers"] = _familyinfo.HrcenterList;
                    //End
                    if (TempData["DeleteParent"] != null && TempData["DeleteParent"].ToString() == "1")
                    {
                        ViewBag.tabpage = "3";
                        ViewBag.message = " Parent/Guardian deleted successfully.";
                    }
                    return View(obj);
                }
              
                return View(_familyinfo);

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
       // [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        [HttpPost]
        public ActionResult ChildDetails(Nurse info, Nurse.PostedPMProblems PostedPostedPrblms, Nurse.PostedPMService PostedPostedService,
            Nurse.PostedProgram PostedPostedPrograms, Nurse.PostedDisease PostedPostedDisease,
           Nurse.PostedDiagnosedDisease PostedPostedDiagnosedDisease, Nurse.PostedChildEHS PostedPostedMedicalEHS,
            Nurse.PostedChildEHS PostedPostedEHS, Nurse.PostedChildVitamin PostedPostedChildVitamin, Nurse.PostedChildDiet PostedPostedChildDietFull,
          Nurse.PostedChildDrink PostedPostedChildDrink, string Command, Screening _screen, List<FamilyHousehold.ImmunizationRecord> Imminization)
        {
            Nurse _familyinfo = new Nurse();
        
            _familyinfo = info;
            _familyinfo.AvailableProgram = (List<Nurse.Programdetail>)TempData["AvailableProgram"];
            //Changes
            StringBuilder _stringnew = new StringBuilder();
            if (PostedPostedPrograms.ProgramID != null)
            {
                foreach (string str in PostedPostedPrograms.ProgramID)
                {
                    _stringnew.Append(str + ",");
                }
                info.CProgramType = _stringnew.ToString().Substring(0, _stringnew.Length - 1);
            }
            //End
            if (Command == "addparentpmquesinfo")
            {
                StringBuilder _string = new StringBuilder();
                StringBuilder _string1 = new StringBuilder();

                StringBuilder _familychildinfo = new StringBuilder();
                if (PostedPostedService.PMServiceID != null)
                {
                    foreach (string str in PostedPostedService.PMServiceID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._Pregnantmotherpmservices = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedService.PMServiceID1 != null)
                {
                    foreach (string str in PostedPostedService.PMServiceID1)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._Pregnantmotherpmservices1 = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedPrblms.PMPrblmID != null)
                {
                    foreach (string str in PostedPostedPrblms.PMPrblmID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._Pregnantmotherproblem = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedPrblms.PMPrblmID1 != null)
                {
                    foreach (string str in PostedPostedPrblms.PMPrblmID1)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._Pregnantmotherproblem1 = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                string message = string.Empty;
                if (info.ParentOriginalId == 0)//parent 1
                    message = _nurse.addParentInfo(info, 1, Guid.Parse(Session["UserID"].ToString()), (Session["AgencyID"].ToString()));
                else
                    message = _nurse.addParentInfo(info, 1, Guid.Parse(Session["UserID"].ToString()), (Session["AgencyID"].ToString()));//parent 2
                _familyinfo = info;
                if (message == "1")
                {
                    ViewBag.tabpage = "3";
                    ViewBag.mode = 1;
                    ViewBag.message = "Record updated successfully.";
                }
            }

            if (Command == "addchildinfo" || Command == "SaveScreening")
            {
                #region Screening
                if (_screen.Physical != null)
                {
                    _screen.PhysicalFileName = _screen.Physical.FileName;
                    _screen.PhysicalFileExtension = Path.GetExtension(_screen.Physical.FileName);
                    BinaryReader b = new BinaryReader(_screen.Physical.InputStream);
                    _screen.PhysicalImageByte = b.ReadBytes(_screen.Physical.ContentLength);
                }
                else
                {
                    _screen.PhysicalImageByte = _screen.PhysicalImagejson == null ? null : Convert.FromBase64String(_screen.PhysicalImagejson);

                }
                if (_screen.Dental != null)
                {
                    _screen.DentalFileName = _screen.Dental.FileName;
                    _screen.DentalFileExtension = Path.GetExtension(_screen.Dental.FileName);
                    BinaryReader b = new BinaryReader(_screen.Dental.InputStream);
                    _screen.DentalImageByte = b.ReadBytes(_screen.Dental.ContentLength);
                }
                else
                {
                    _screen.DentalImageByte = _screen.DentalImagejson == null ? null : Convert.FromBase64String(_screen.DentalImagejson);

                }
                if (_screen.Vision != null)
                {
                    _screen.VisionFileName = _screen.Vision.FileName;
                    _screen.VisionFileExtension = Path.GetExtension(_screen.Vision.FileName);
                    BinaryReader b = new BinaryReader(_screen.Vision.InputStream);
                    _screen.VisionImageByte = b.ReadBytes(_screen.Vision.ContentLength);
                }
                else
                {
                    _screen.VisionImageByte = _screen.VisionImagejson == null ? null : Convert.FromBase64String(_screen.VisionImagejson);

                }
                if (_screen.Hearing != null)
                {
                    _screen.HearingFileName = _screen.Hearing.FileName;
                    _screen.HearingFileExtension = Path.GetExtension(_screen.Hearing.FileName);
                    BinaryReader b = new BinaryReader(_screen.Hearing.InputStream);
                    _screen.HearingImageByte = b.ReadBytes(_screen.Hearing.ContentLength);
                }
                else
                {
                    _screen.HearingImageByte = _screen.HearingImagejson == null ? null : Convert.FromBase64String(_screen.HearingImagejson);

                }
                if (_screen.Develop != null)
                {
                    _screen.DevelopFileName = _screen.Develop.FileName;
                    _screen.DevelopFileExtension = Path.GetExtension(_screen.Develop.FileName);
                    BinaryReader b = new BinaryReader(_screen.Develop.InputStream);
                    _screen.DevelopImageByte = b.ReadBytes(_screen.Develop.ContentLength);
                }
                else
                {
                    _screen.DevelopImageByte = _screen.DevelopImagejson == null ? null : Convert.FromBase64String(_screen.DevelopImagejson);

                }
                if (_screen.Speech != null)
                {
                    _screen.SpeechFileName = _screen.Speech.FileName;
                    _screen.SpeechFileExtension = Path.GetExtension(_screen.Speech.FileName);
                    BinaryReader b = new BinaryReader(_screen.Speech.InputStream);
                    _screen.SpeechImageByte = b.ReadBytes(_screen.Speech.ContentLength);
                }
                else
                {
                    _screen.SpeechImageByte = _screen.SpeechImagejson == null ? null : Convert.FromBase64String(_screen.SpeechImagejson);

                }
                if (_screen.ScreeningAccept != null)
                {
                    _screen.ScreeningAcceptFileName = _screen.ScreeningAccept.FileName;
                    _screen.ScreeningAcceptFileName = Path.GetExtension(_screen.ScreeningAccept.FileName);
                    BinaryReader b = new BinaryReader(_screen.ScreeningAccept.InputStream);
                    _screen.ScreeningAcceptImageByte = b.ReadBytes(_screen.ScreeningAccept.ContentLength);
                }
                else
                {
                    _screen.ScreeningAcceptImageByte = _screen.ScreeningAcceptImageByte == null ? null : Convert.FromBase64String(_screen.ScreeningAcceptImagejson);

                }


                #endregion
                if (info.AvailableProgram.Count > 0)
                {
                    foreach (var item1 in info.AvailableProgram)
                    {
                        // string ReferenceID = item1.ReferenceId;
                        if (item1.ReferenceId == "1")//EHS
                        {
                            info.ChildReferenceProgramID = item1.ReferenceId;
                        }
                        if (item1.ReferenceId == "2")//HS
                        {
                            info.ChildReferenceProgramID = item1.ReferenceId;
                        }
                    }
                }


                StringBuilder _familychildinfo = new StringBuilder();
                if (info.ChildReferenceProgramID == "2")
                {
                    _familychildinfo.Clear();
                    if (PostedPostedDisease.DiseaseID != null)
                    {
                        foreach (string str in PostedPostedDisease.DiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildDirectBloodRelativeHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.DiagnoseDiseaseID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.DiagnoseDiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildDiagnosedConditionsHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildMedicalTreatmentHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.ChronicHealthConditionsID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.ChronicHealthConditionsID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildChronicHealthConditionsHs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                }
                if (info.ChildReferenceProgramID == "1")
                {
                    _familychildinfo.Clear();
                    if (PostedPostedDisease.DiseaseID != null)
                    {
                        foreach (string str in PostedPostedDisease.DiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildDirectBloodRelativeEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.DiagnoseDiseaseID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.DiagnoseDiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildDiagnosedConditionsEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedMedicalEHS.ChildEHSID != null)
                    {
                        foreach (string str in PostedPostedMedicalEHS.ChildEHSID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildChronicHealthConditionsEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedEHS.ChildEHSID != null)
                    {
                        foreach (string str in PostedPostedEHS.ChildEHSID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildChronicHealthConditions1Ehs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.MedicalDiagnoseDiseaseID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildMedicalTreatmentEhs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                    _familychildinfo.Clear();
                    if (PostedPostedDiagnosedDisease.ChronicHealthConditionsID != null)
                    {
                        foreach (string str in PostedPostedDiagnosedDisease.ChronicHealthConditionsID)
                        {
                            _familychildinfo.Append(str + ",");
                        }
                        info._ChildChronicHealthConditions2Ehs = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                    }
                }
                _familychildinfo.Clear();
                if (PostedPostedChildVitamin.CDietInfoID != null)
                {
                    foreach (string str in PostedPostedChildVitamin.CDietInfoID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildChildVitaminSupplement = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedChildDietFull.CDietInfoID != null)
                {
                    foreach (string str in PostedPostedChildDietFull.CDietInfoID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDiet = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                _familychildinfo.Clear();
                if (PostedPostedChildDrink.CDrinkID != null)
                {
                    foreach (string str in PostedPostedChildDrink.CDrinkID)
                    {
                        _familychildinfo.Append(str + ",");
                    }
                    info._ChildDrink = _familychildinfo.ToString().Substring(0, _familychildinfo.Length - 1);
                }
                string message = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["Type"]) || Convert.ToString(Request.QueryString["Type"]) != "0")
                    info.Yakkrid = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["Type"].ToString()));
                if (info.HealthQuesId == 0 || info.NutritionQuesId == 0)
                    message = _nurse.addHealthInfo(info, 0, Guid.Parse(Session["UserID"].ToString()), (Session["AgencyID"].ToString()), _screen, Imminization, (Session["RoleID"].ToString()));
                else
                    message = _nurse.addHealthInfo(info, 1, Guid.Parse(Session["UserID"].ToString()), (Session["AgencyID"].ToString()), _screen, Imminization, (Session["RoleID"].ToString()));
                _familyinfo = info;
                if (message == "1")
                {
                    ViewBag.tabpage = "4";
                    ViewBag.message = "Record added successfully.";
                    //  ViewBag.result = "Sucess";
                }
                if (message == "2")
                {
                    ViewBag.tabpage = "4";
                    ViewBag.message = "Record updated successfully.";
                    //  ViewBag.result = "Sucess";
                }
            }
            if (Command == "addacceptinfo")
            {
                int pendingcount = 0;
                if (!string.IsNullOrEmpty(Request.QueryString["Centerid"]) || Convert.ToString(Request.QueryString["Centerid"]) != "0")
                    info.CenterID = Request.QueryString["Centerid"].ToString();
                if (!string.IsNullOrEmpty(Request.QueryString["centername"]) || Convert.ToString(Request.QueryString["centername"]) != "0")
                    info.CenterName = Request.QueryString["centername"].ToString();
                string message = string.Empty;
                if (Request.QueryString["id"] != null)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["id"]) || Convert.ToString(Request.QueryString["id"]) != "0")
                        info.ClientID = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["id"].ToString()));
                    if (!string.IsNullOrEmpty(Request.QueryString["Type"]) || Convert.ToString(Request.QueryString["Type"]) != "0")
                        info.Yakkrid = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["Type"].ToString()));
                }
                if (info.AcceptID == 0)
                {
                    if (info.ChildId == 0)
                    {
                        info.ClientID = info.ParentID;
                    }
                    else
                    {
                        info.ClientID = info.ChildId;
                    }
                    message = _nurse.addAcceptInfo(out pendingcount, info, 0, Guid.Parse(Session["UserID"].ToString()), (Session["AgencyID"].ToString()), (Session["RoleID"].ToString()));
                }
                else
                {
                    message = _nurse.addAcceptInfo(out pendingcount, info, 1, Guid.Parse(Session["UserID"].ToString()), (Session["AgencyID"].ToString()), (Session["RoleID"].ToString()));
                }
                _familyinfo = info;

                if (pendingcount != 0)
                {

                    if (message == "1")
                    {
                        //_nurse.InsertAcceptReason(info, Session["AgencyID"].ToString(), Session["RoleId"].ToString());
                        TempData["message"] = "Applicant accepted successfully. ";
                        return Redirect("~/Nurse/ViewCenterDetails/?id=" + info.CenterID + "&Name=" + info.CenterName);
                    }
                    if (message == "2")
                    {
                        TempData["message"] = "Applicant rejected successfully.";
                        return Redirect("~/Nurse/ViewCenterDetails/?id=" + info.CenterID + "&Name=" + info.CenterName);
                    }
                    if (message == "3")
                    {
                        return Redirect("~/Nurse/ViewCenterDetails/?id=" + info.CenterID + "&Name=" + info.CenterName);
                    }
                    if(message=="4")
                    {
                        TempData["message"] = "Application already reviewed by other user.";
                        return Redirect("~/Nurse/ViewCenterDetails/?id=" + info.CenterID + "&Name=" + info.CenterName);

                    }
                }
                else
                {
                    if (message == "1")
                    {
                        //_nurse.InsertAcceptReason(info, Session["AgencyID"].ToString(), Session["RoleId"].ToString());
                        TempData["message"] = "Applicant accepted successfully. ";
                        return Redirect("~/Yakkr/YakkrDetails");
                    }
                    if (message == "2")
                    {
                        TempData["message"] = "Applicant rejected successfully.";
                        return Redirect("~/Yakkr/YakkrDetails");
                    }
                    if (message == "3")
                    {
                        return Redirect("~/Yakkr/YakkrDetails");
                    }
                    if (message == "4")
                    {
                        TempData["message"] = "Application already reviewed by other user.";
                        return Redirect("~/Yakkr/YakkrDetails");
                    }


                }

            }
            if (Command == "HealthReview")
            {
                string message = string.Empty;
                if (Request.QueryString["id"] != null)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["id"]) || Convert.ToString(Request.QueryString["id"]) != "0")
                        info.ClientID = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["id"].ToString()));
                    if (!string.IsNullOrEmpty(Request.QueryString["Type"]) || Convert.ToString(Request.QueryString["Type"]) != "0")
                        info.Yakkrid = Convert.ToInt32(FingerprintsModel.EncryptDecrypt.Decrypt64(Request.QueryString["Type"].ToString()));
                }
                message = _nurse.HealthReview(info, 0, Guid.Parse(Session["UserID"].ToString()), (Session["AgencyID"].ToString()));
                _familyinfo = info;
                if (message == "1")
                {

                    TempData["message"] = "Client health reviewed successfully.";
                    return Redirect("~/Yakkr/YakkrDetails");
                }
                else if (message == "2")
                {
                    TempData["message"] = "Client health already reviewed.";
                    return Redirect("~/Yakkr/YakkrDetails");

                }
                else
                    ViewBag.message = "Error occured please try again.";

            }
            ViewBag.lang = TempData["familyinfo"];
            ViewBag.Race = TempData["Race"];
            ViewBag.RaceSubCategory = TempData["RaceSubcategory"];
            ViewBag.Relationship = TempData["Relationship"];
            ViewBag.diet = TempData["DietInfo"];
            ViewBag.food = TempData["foodInfo"];
            ViewBag.hungry = TempData["hungry"];
            ViewBag.ChildFeed = TempData["ChildFeed"];
            ViewBag.hungry = TempData["hungry"];
            ViewBag.ChildFormula = TempData["ChildFormula"];
            ViewBag.ChildReferal = TempData["ChildReferal"];
            ViewBag.ChildCereal = TempData["ChildCereal"];
            ViewBag.PMConditions = TempData["PMConditions"];
            _familyinfo.AvailableProgram = (List<Nurse.Programdetail>)TempData["AvailableProgram"];
            _familyinfo.ImmunizationRecords = (List<FamilyHousehold.ImmunizationRecord>)TempData["ImmunizationRecords"];
            _familyinfo.AvailableDisease = (List<Nurse.ChildDirectBloodRelative>)TempData["AvailableDisease"];
            _familyinfo.AvailableDiagnosedDisease = (List<Nurse.ChildDiagnosedDisease>)TempData["AvailableDiagnosedDisease"];
            _familyinfo.AvailableDental = (List<Nurse.ChildDental>)TempData["AvailableDental"];
            _familyinfo.AvailableService = (List<Nurse.PMService>)TempData["AvailableService"];
            _familyinfo.AvailablePrblms = (List<Nurse.PMProblems>)TempData["AvailablePrblms"];
            _familyinfo.SchoolList = (List<SchoolDistrict>)TempData["Schooldistrict"];
            _familyinfo.AvailableChildDrink = (List<Nurse.ChildDrink>)TempData["AvailableChildDrink"];
            _familyinfo.AvailableChildDietFull = (List<Nurse.ChildDietFull>)TempData["AvailableChildDietFull"];
            _familyinfo.AvailableChildVitamin = (List<Nurse.ChildVitamin>)TempData["AvailableChildVitamin"];
            _familyinfo.AvailableEHS = (List<Nurse.ChildEHS>)TempData["AvailableEHS"];
            TempData.Keep();
            return View(_familyinfo);
        }
       // [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult ScreeningIntake()
        {
            return View();
        }
        public PartialViewResult FamilyHouseHold(string tabName, int houseHoldId)
        {
            Nurse list = new Nurse();
            ViewBag.tabName = tabName;
            // try
            // {
            list.getList = _nurse.GetFileCabinet(tabName, Session["AgencyID"].ToString(), houseHoldId);
            //}
            //catch(Exception ex)
            //{
            //    clsError.WriteException(ex);
            //}
            return PartialView(list);
        }
        private List<FamilyHousehold.calculateincome> GenerateIncomeList()
        {
            List<FamilyHousehold.calculateincome> IncomeList = new List<FamilyHousehold.calculateincome>();
            IncomeList.Add(new FamilyHousehold.calculateincome());
            return IncomeList;

        }
        private List<FamilyHousehold.calculateincome1> GenerateIncomeList1()
        {
            List<FamilyHousehold.calculateincome1> IncomeList = new List<FamilyHousehold.calculateincome1>();
            IncomeList.Add(new FamilyHousehold.calculateincome1());
            return IncomeList;

        }
       // [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        [JsonMaxLengthAttribute]
        public JsonResult listParentPhoneDetails1(string HouseHoldId = "0", string ParentID = "0")
        {
            try
            {

                NurseData obj = new NurseData();
                var listPhone = obj.PhoneDetails(HouseHoldId, ParentID, Session["AgencyID"].ToString()).ToList();
                return Json(new { listPhone });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
      //  [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        [JsonMaxLengthAttribute]
        public JsonResult listParentPhoneDetails2(string HouseHoldId = "0", string ParentID = "0")
        {
            try
            {

                NurseData obj = new NurseData();
                var listPhone = obj.PhoneDetails(HouseHoldId, ParentID, Session["AgencyID"].ToString()).ToList();
                return Json(new { listPhone });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json(Ex.Message);
            }
        }
       // [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        [JsonMaxLengthAttribute]
        public JsonResult SaveNotes(string HouseHoldId = "0", string Notes = "", string mode = "")
        {
            try
            {
                string result = "0";
                var noteslist = _nurse.SaveNotes(ref result, Session["AgencyID"].ToString(), Session["UserID"].ToString(), HouseHoldId, Notes, mode);
                return Json(new { noteslist, result });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }


      //  [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        [JsonMaxLengthAttribute]
        public JsonResult GetHschild(string Childid = "0")
        {
            try
            {
                NurseData obj = new NurseData();
                return Json(obj.getHsChild(Childid, Session["AgencyID"].ToString(), Session["UserId"].ToString(), Server.MapPath("~//TempAttachment//")));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }

        }
     //   [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult ClientList()
        {

            return View();
        }
        [JsonMaxLengthAttribute]
     //   [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult ClientLists(string sortOrder, string sortDirection, string search, int pageSize, int requestedPage = 1)
        {
            try
            {
                int skip = pageSize * (requestedPage - 1);
                string totalrecord;
                var list = new NurseData().ClientLists(out totalrecord, sortOrder, sortDirection, search.TrimEnd().TrimStart(), skip, pageSize, Convert.ToString(Session["UserID"]), Convert.ToString(Session["AgencyID"]), Session["Roleid"].ToString());
                return Json(new { list, totalrecord });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("");
            }
        }
      //  [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult ScreeningClient(string id)
        {
            try
            {
               
                if (!string.IsNullOrEmpty(Request.QueryString["name"]))
                    ViewBag.Name = Convert.ToString(Request.QueryString["name"]);
                if (!string.IsNullOrEmpty(Request.QueryString["clientid"]))
                    ViewBag.id = Convert.ToString(Request.QueryString["clientid"]);
                if (!string.IsNullOrEmpty(Request.QueryString["Dob"]))
                    ViewBag.Dob = Convert.ToString(Request.QueryString["Dob"]);
                if (!string.IsNullOrEmpty(Request.QueryString["Programid"]))
                    ViewBag.Programid = Convert.ToString(Request.QueryString["Programid"]);
                if (!string.IsNullOrEmpty(Request.QueryString["dashboard"]))
                    ViewBag.dashboard = Convert.ToString( Request.QueryString["dashboard"]);
              
                if (!string.IsNullOrEmpty(Request.QueryString["Screeningid"]) && Convert.ToString(Request.QueryString["Screeningid"]) == "0")
                {
                    List<SelectListItem> _list = _nurse.GetScreening(Convert.ToString(Request.QueryString["clientid"] ),  Convert.ToString(Request.QueryString["Programid"]), Session["AgencyID"].ToString(), Session["UserID"].ToString(), Session["Roleid"].ToString());
                    if(_list!=null && _list.Count <1)
                        ViewBag.message = "No Screening found.";
                    ViewBag.screening = _list;
                    TempData["Screen"] = _list;
                    ViewBag.screeningid = "0";
                }
                if (!string.IsNullOrEmpty(Request.QueryString["Screeningid"]) && Convert.ToString(Request.QueryString["Screeningid"]) != "0")
                {
                    if (Convert.ToString(Request.QueryString["Screeningid"]) == "1")
                    {
                        ViewBag.screeningid = "1";
                    }
                    else if (Convert.ToString(Request.QueryString["Screeningid"]) == "2")
                    {
                        ViewBag.screeningid = "2";
                    }
                    else if (Convert.ToString(Request.QueryString["Screeningid"]) == "3")
                    {
                        ViewBag.screeningid = "3";
                    }
                    else if (Convert.ToString(Request.QueryString["Screeningid"]) == "4")
                    {
                        ViewBag.screeningid = "4";
                    }
                    else if (Convert.ToString(Request.QueryString["Screeningid"]) == "5")
                    {
                        ViewBag.screeningid = "5";
                    }
                    else if (Convert.ToString(Request.QueryString["Screeningid"]) == "6")
                    {
                        ViewBag.screeningid = "6";
                    }
                    else
                    {

                        ViewBag.screeningid = "7";
                    }
                    DataSet _data = _nurse.GetScreeningsbyid(Convert.ToString(Request.QueryString["clientid"]), Convert.ToString(Request.QueryString["Screeningid"]), Session["AgencyID"].ToString(), Session["UserID"].ToString(), Convert.ToString(Request.QueryString["Programid"]), Session["Roleid"].ToString());
                    if (_data != null)
                    {
                        if (_data.Tables.Count > 0 && _data.Tables[0].Rows.Count>0)
                        {
                            ViewBag.screenings = _data.Tables[0];
                            ViewBag.uploaddocument = Convert.ToBoolean(_data.Tables[0].Rows[0]["DocumentUpload"]);
                        }
                        if (_data.Tables.Count > 1 && _data.Tables[1].Rows.Count > 0)
                        {
                            ViewBag.screeningsdata = _data.Tables[1];
                            ViewBag.screeningdate =  _data.Tables[1].Rows[0]["screeningdate"].ToString()!=""? Convert.ToDateTime(_data.Tables[1].Rows[0]["screeningdate"]).ToString("MM/dd/yyyy"):"" ;
                          
                        }
                        if (_data.Tables.Count > 2 && _data.Tables[2].Rows.Count > 0)
                        {

                            List<SelectListItem> _screening = new List<SelectListItem>();
                            SelectListItem obj = null;
                            foreach (DataRow dr in _data.Tables[2].Rows)
                            {
                                obj = new SelectListItem();
                                obj.Value = dr["ScreeningID"].ToString();
                                obj.Text = dr["ScreeningName"].ToString();
                                _screening.Add(obj);
                            }
                            if (_screening != null && _screening.Count < 1)
                                ViewBag.message = "No Screening found.";
                            ViewBag.screening = _screening;
                            TempData["Screen"] = _screening;
                        }
                        if (_data.Tables.Count > 3 && _data.Tables[3].Rows.Count > 0)
                        {
                            foreach (DataRow dr in _data.Tables[3].Rows)
                            {
                                ViewBag.missingscreeningdate = dr["missingscreeningdate"].ToString() == "" ? "" : Convert.ToDateTime(dr["missingscreeningdate"]).ToString("MM/dd/yyyy");
                                ViewBag.ScreeningStatus = dr["MissingStatus"].ToString();

                            }


                        }



                    }
                    ViewBag.screening = TempData["Screen"];
                    ViewBag.selectedid = Convert.ToString(Request.QueryString["Screeningid"]);
                    TempData.Keep();

                }

                return View();

            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                ViewBag.screening = TempData["Screen"];
                return View();
            }
        }
     //   [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult GetScreeningTemplate(string Screeningid, string ClientId)
        {
            try
            {

                return Json(_nurse.GetScreeningTemplate(Screeningid, Session["AgencyID"].ToString(), Session["UserID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [HttpPost]
     //   [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult ScreeningClient(Screening _screen, FormCollection _Collections,string screeningdate,HttpPostedFileBase ScreeningDocument, string Status)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["name"]))
                    ViewBag.Name = Convert.ToString(Request.QueryString["name"]);
                if (!string.IsNullOrEmpty(Request.QueryString["clientid"]))
                    ViewBag.id = Convert.ToString(Request.QueryString["clientid"]);
                if (!string.IsNullOrEmpty(Request.QueryString["Dob"]))
                    ViewBag.Dob = Convert.ToString(Request.QueryString["Dob"]);
                if (!string.IsNullOrEmpty(Request.QueryString["dashboard"]))
                    ViewBag.dashboard = Convert.ToString(Request.QueryString["dashboard"]);
                if (!string.IsNullOrEmpty(Request.QueryString["dashboard"]))
                    ViewBag.dashboard = Convert.ToString(Request.QueryString["dashboard"]);
                if (!string.IsNullOrEmpty(Request.QueryString["Programid"]))
                    ViewBag.Programid = Convert.ToString(Request.QueryString["Programid"]);
                if (!string.IsNullOrEmpty(Request.QueryString["clientid"]) && !string.IsNullOrEmpty(Request.QueryString["Screeningid"]) && Convert.ToString(Request.QueryString["Screeningid"]) != "0")
                {
                    _screen.ClientID = Convert.ToInt32( EncryptDecrypt.Decrypt64(Request.QueryString["clientid"]));
                    _screen.Screeningid = Convert.ToInt32(Request.QueryString["Screeningid"]);
                    ViewBag.selectedid = Convert.ToString(Request.QueryString["Screeningid"]);
                     string  message= "";
                     DataSet _data = _nurse.savecustomscreening(ref message, _screen, _Collections, screeningdate, Status, Session["AgencyID"].ToString(), Session["UserID"].ToString(), ScreeningDocument, Convert.ToString(Request.QueryString["Programid"]), Session["Roleid"].ToString());
                  if (_data != null)
                  {
                      if (_data.Tables.Count > 0 && _data.Tables[0].Rows.Count > 0)
                      {
                          ViewBag.screenings = _data.Tables[0];
                          ViewBag.uploaddocument = Convert.ToBoolean(_data.Tables[0].Rows[0]["DocumentUpload"]);
                      }
                      if (_data.Tables.Count > 1 && _data.Tables[1].Rows.Count > 0)
                      {
                          ViewBag.screeningsdata = _data.Tables[1];
                          ViewBag.screeningdate = _data.Tables[1].Rows[0]["screeningdate"].ToString() != "" ? Convert.ToDateTime(_data.Tables[1].Rows[0]["screeningdate"]).ToString("MM/dd/yyyy") : "";
                        
                      }
                     
                  }
                  if(message=="1")
                  {
                      ViewBag.message = "Record saved successfully.";

                  }
                  else  if (message == "2")
                  {
                      TempData["message"] = "Screening not exist or deactivated.";
                      return Redirect("/Nurse/ScreeningClient?clientid=" + Convert.ToString(Request.QueryString["clientid"]) + "&Screeningid=0&name=" + Convert.ToString(Request.QueryString["name"]) + "&Dob=" + Convert.ToString(Request.QueryString["Dob"]) + "&Programid=" + Convert.ToString(Request.QueryString["Programid"]) + "&dashboard=" + Convert.ToString(Request.QueryString["dashboard"]));

                  }
                  else
                  {

                      ViewBag.message = "Error occured please try again later.";
                  }
                  ViewBag.screeningid = _screen.Screeningid.ToString();
                }
                ViewBag.screening = TempData["Screen"];
                TempData.Keep();
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }
        }
        [JsonMaxLengthAttribute]
     //   [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult Loadallenrolled(string Centerid = "0", string Classroom = "0")
        {
            try
            {
                var list = _nurse.Getchildscreeningroster(Centerid, Classroom, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                return Json(new { list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [JsonMaxLengthAttribute]
    //    [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult Loadallcenterscreening(string Centerid = "0")
       {
           try
           {
               return Json(_nurse.Getchildscreeningcenter(Centerid, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
           }
           catch (Exception Ex)
           {
               clsError.WriteException(Ex);
               return Json("Error occured please try again.");
           }
       }
        [JsonMaxLengthAttribute]
     //   [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult Loadchildmissingscreening(string ClassRoom,string Centerid = "0" )
       {
           try
           {
               return Json(_nurse.Getallchildmissingscreening(Centerid,ClassRoom ,Session["UserID"].ToString(), Session["AgencyID"].ToString()));
           }
           catch (Exception Ex)
           {
               clsError.WriteException(Ex);
               return Json("Error occured please try again.");
           }
       }
    //    [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult  Multientryscreening(string id)
        {

            try
            {
                string screeningid = "";
                string clientid = "";
                DataTable mscreenings=null;
                DataTable ScreeningStatus = null;
                if (!string.IsNullOrEmpty(Request.QueryString["screeningids"]))
                    screeningid = Convert.ToString(Request.QueryString["screeningids"]);
                if (!string.IsNullOrEmpty(Request.QueryString["centerid"]))
                    clientid = Convert.ToString(Request.QueryString["centerid"]);
                if (!string.IsNullOrEmpty(Request.QueryString["centername"]))
                  ViewBag.centername = Convert.ToString(Request.QueryString["centername"]);
                DataSet Ds = _nurse.GetmultiScreening(screeningid, clientid, Session["AgencyID"].ToString(), Session["UserID"].ToString(), Session["Roleid"].ToString());
                if(Ds!=null)
                {
                    if(Ds.Tables.Count >0  )
                     mscreenings = Ds.Tables[0];
                    if (Ds.Tables.Count > 1)
                        ScreeningStatus = Ds.Tables[1];
                }
                if (mscreenings != null && mscreenings.Rows.Count > 0)
                {
                    ViewBag.multiscreening = mscreenings;
                    ViewBag.ScreeningStatus = ScreeningStatus;
                }
                else
                    return Redirect("~/Home/ApplicationApprovalDashboard");
                return View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return View();
            }

        }
        [JsonMaxLengthAttribute]
    //    [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult Loadallclientscreening(string Classroomid = "0", string Centerid = "0", string Screeningid="0")
        {
            try
            {
                return Json(_nurse.Loadallclientscreening(Classroomid,Centerid,Screeningid, Session["UserID"].ToString(), Session["AgencyID"].ToString(),Session["Roleid"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [JsonMaxLengthAttribute]
    //    [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult Saveclientscreening(List<Nurse.clients> ClientScreenings)
        {
            try
            {
                return Json(_nurse.Saveclientscreening(ClientScreenings, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
   //     [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult Savemultiscreening(List<Nurse.clients> multiscreenings)
        {
            try
            {
                return Json(_nurse.Savemultiscreening(multiscreenings, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [JsonMaxLengthAttribute]
   //     [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult Loadmissingclient(string Screeningid , string Centerid)
        {
            try
            {
                return Json(_nurse.Loadmissingclient(Screeningid, Centerid, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [JsonMaxLengthAttribute]
     //   [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult Loadclients(string Classroomid = "0", string Centerid = "0", string Screeningid = "0")
        {
            try
            {
                return Json(_nurse.Loadclients(Classroomid, Centerid, Screeningid, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [JsonMaxLengthAttribute]
      //  [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult Saveclientclassscreening(List<Nurse.clients> ClientScreenings)
        {
            try
            {
                return Json(_nurse.Saveclientclassscreening(ClientScreenings, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [JsonMaxLengthAttribute]
     //   [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult Loadsavedmissingclient(string Classroomid = "0", string Centerid = "0", string Screeningid = "0")
        {
            try
            {
                return Json(_nurse.Loadsavedmissingclient(Classroomid, Centerid, Screeningid, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [JsonMaxLengthAttribute]
    //    [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult Loaddeclinedscreening(string Centerid = "0")
        {
            try
            {
                return Json(_nurse.GetDeclinedScreeningList(Centerid, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        //added on 30Dec2016
        [JsonMaxLengthAttribute]
     //   [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult LoadallenrolledCleargrid(string Centerid = "0", string Classroom = "0")
        {
            try
            {
                if (Centerid != "0")
                {
                    Centerid = EncryptDecrypt.Decrypt64(Centerid);
                }
                var list = _nurse.Getchildscreeningroster(Centerid, Classroom, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString());
                return Json(new { list });
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        //End
        [JsonMaxLengthAttribute]
      //  [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b,9ad1750e-2522-4717-a71b-5916a38730ed,a31b1716-b042-46b7-acc0-95794e378b26,c352f959-cfd5-4902-a529-71de1f4824cc")]
        public JsonResult LoadGroupCaseNoteClient(string Centerid = "0", string Classroom = "0")
        {
            try
            {
                return Json(_nurse.LoadGroupCaseNoteClient(Centerid, Classroom, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [JsonMaxLengthAttribute]
     //   [CustAuthFilter("94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d,c352f959-cfd5-4902-a529-71de1f4824cc,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult ViewGroupCaseNoteClient(string Centerid = "0", string Classroom = "0")
        {
            try
            {
                return Json(_nurse.ViewGroupCaseNoteClient(Centerid, Classroom, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [JsonMaxLengthAttribute]
     //   [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult LoadRescreening(string Centerid = "0")
        {
            try
            {
                return Json(_nurse.GetReScreeningList(Centerid, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
        [JsonMaxLengthAttribute]
     //   [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,82b862e6-1a0f-46d2-aad4-34f89f72369a,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public JsonResult LoadWithdrawn(string Centerid = "0")
        {
            try
            {
                return Json(_nurse.GetWithdrawnList(Centerid, Session["UserID"].ToString(), Session["AgencyID"].ToString(), Session["Roleid"].ToString()));
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }
    //    [CustAuthFilter("a31b1716-b042-46b7-acc0-95794e378b26,b4d86d72-0b86-41b2-adc4-5ccce7e9775b")]
        public ActionResult DownloadScreeningMatrixExcel(string Centerid,string Classroom ="")
        {
            try
            {
                 Export export = new Export();
                 Response.Clear();
                 Response.Buffer = true;
                 Response.Charset = "";
                 Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                 Response.AddHeader("content-disposition", "attachment;filename=Screening Status Report " + DateTime.Now.ToString("MM/dd/yyyy") + ".xlsx");
                 MemoryStream ms = export.ExportExcelScreeningMatrix(_nurse.Getallchildmissingscreening(Centerid, Classroom, Session["UserID"].ToString(), Session["AgencyID"].ToString()));
                 ms.WriteTo(Response.OutputStream);
                 Response.Flush();
                 Response.End();
                return  View();
            }
            catch (Exception Ex)
            {
                clsError.WriteException(Ex);
                return Json("Error occured please try again.");
            }
        }

        
    }
}
