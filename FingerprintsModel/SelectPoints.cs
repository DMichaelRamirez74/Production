using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class SelectPoints
    {
       public Int32 SPID { get; set; }
       //Program Type
       public string RefProgName { get; set; }
       public string AgencyID { get; set; }
       public string ReferenceProg { get; set; }
       public List<ReferenceProgInfo> refList = new List<ReferenceProgInfo>();
       public bool IsLocked { get; set; }
       public class ReferenceProgInfo
       {
           public string Id { get; set; }
           public string Name { get; set; }
       }
       //end
       //Family Household
       public int SingleParent { get; set; }
       public int TwoParent { get; set; }
       public int English { get; set; }
       public int Other { get; set; }
       public int HomelessYes { get; set; }
       public int HomelessNo { get; set; }
       public int TANF { get; set; }
       public int SSI { get; set; }
       public int SNAP { get; set; }
       public int WIC { get; set; }
       public int None { get; set; }
       public int Teenager { get; set; }
       //Guardian1
       public int Age20 { get; set; }
       public int Age30over { get; set; }
       public int MilitaryStatusNone { get; set; }
       public int MilitaryStatusActive { get; set; }
       public int MilitaryStatusVeteran { get; set; }
       public int CurrentlyWorkYes { get; set; }
       public int CurrentlyWorkNo { get; set; }
       public int JobTrainingyes { get; set; }
       public int JobTrainingno { get; set; }
       //Guardian2
       public int G2Teenager { get; set; }
       public int G2Age20 { get; set; }
       public int G2Age30over { get; set; }
       public int G2MilitaryStatusNone { get; set; }
       public int G2MilitaryStatusActive { get; set; }
       public int G2MilitaryStatusVeteran { get; set; }
       public int G2CurrentlyWorkYes { get; set; }
       public int G2CurrentlyWorkNo { get; set; }
       public int G2JobTrainingyes { get; set; }
       public int G2JobTrainingno { get; set; }
       //Child

       public int Ageless10weeks { get; set; }
       public int Agegreater10weeks { get; set; }
       public int Age3Months { get; set; }
       public int Age6Months { get; set; }
       public int Age1yr { get; set; }
       public int Age2yr { get; set; }
       public int Age3yr { get; set; }
       public int Age4yr { get; set; }
       public int Age5yr { get; set; }
       public int Age6yr { get; set; }
       public int Age6yrorgreater { get; set; }
       public int MedicalHomeYes { get; set; }
       public int MedicalHomeNo { get; set; }
       public int DentalHomeYes { get; set; }
       public int DentalHomeNo { get; set; }
       public int InsuranceYes { get; set; }
       public int InsuranceNo { get; set; }
       public int SuspecteddocumentofdisabiltyYes  { get; set; }
       public int SuspecteddocumentofdisabiltyNo { get; set; }
       public int SuspecteddisabiltyYes { get; set; }
       public int SuspecteddisabiltyNo { get; set; }
       public int DocumentofdisabiltyYes { get; set; }
       public int DocumentofdisabiltyNo { get; set; }
       public int ChildWlfareYes { get; set; }
       public int ChildWlfareNo { get; set; }
       public int FosterChildYes { get; set; }
       public int FosterChildNo { get; set; }
       public int DualCustYes { get; set; }
       public int DualCustNo { get; set; }
       //Poverty Points
       public int poverty0to25 { get; set; }
       public int poverty26to50 { get; set; }
       public int poverty51to75 { get; set; }
       public int poverty76to100 { get; set; }
       public int poverty100to130 { get; set; }
       public int povertygreater130 { get; set; }
       //PM
       public int PMInsuranceYes { get; set; }
       public int PMInsuranceNo { get; set; }
       public int PMMedicalHomeYes { get; set; }
       public int PMMedicalHomeNo { get; set; }
       public int Trimester1 { get; set; }
       public int Trimester2 { get; set; }
       public int Trimester3 { get; set; }
       //public int DualCustNo { get; set; }
       //public int DualCustNo { get; set; }
       //public int DualCustNo { get; set; }
       public List<CustomQuestion> CustomQues = new List<CustomQuestion>();
       public class CustomQuestion
       {
           public int CQID { get; set; }
           public string ProgramType { get; set; }
           public string QuesYes { get; set; }
           public string Question { get; set; }
           public string QuesNo { get; set; }
           public string point { get; set; }
           public string totalcustompoint { get; set; }

       }


    }
}
