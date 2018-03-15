using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FingerprintsModel
{
    public class MyProfile
    {
        public string eduindex { get; set; }
        public string profileindex { get; set; }
        public string ClientName { get; set; }
        public int TypeScreening { get; set; }
        public int AgencyID { get; set; }
        public int ClientID { get; set; }
        public string roleID { get; set; }
        public string UserID { get; set; }
        public string TBDate { get; set; }
        public string TBResults { get; set; }
        public string CDDate { get; set; }
        public string CDResults { get; set; }
        public string MSDate { get; set; }
        public string FBDate { get; set; }
        public string BCIDate { get; set; }
        public string NCDate { get; set; }
        public string Institution { get; set; }
        public string Major { get; set; }
        public string Degree { get; set; }
        public string DegreeDate { get; set; }
        public string Type { get; set; }
        public string Institution2 { get; set; }
        public string Major2 { get; set; }
        public string Degree2 { get; set; }
        public string DegreeDate2 { get; set; }
        public string Type2 { get; set; }
        public string Institution3 { get; set; }
        public string Major3 { get; set; }
        public string Degree3 { get; set; }
        public string DegreeDate3 { get; set; }
        public string Type3 { get; set; }
        public string HighestDegree { get; set; }
        public string EduDueDate { get; set; }
        public string hidtab { get; set; }
        public string MSIFileName { get; set; }
        public string MSIFileExtension { get; set; }
        public byte[] MSIFileData { get; set; }
        public HttpPostedFileBase MSIfile { get; set; }
        public HttpPostedFileBase FBfile { get; set; }
        public string FBFileName { get; set; }
        public string FBFileExtension { get; set; }
        public byte[] FBFileData { get; set; }
        public HttpPostedFileBase BCIfile { get; set; }
        public string BCIFileName { get; set; }
        public string BCIFileExtension { get; set; }
        public byte[] BCIFileData { get; set; }
        public HttpPostedFileBase NCfile { get; set; }
        public string NCFileName { get; set; }
        public string NCFileExtension { get; set; }
        public byte[] NCFileData { get; set; }
        public string MSIFileUploaded { get; set; }
        public string FBFileUploaded { get; set; }
        public string BCIFileUploaded { get; set; }
        public string NCFileUploaded { get; set; }
    }
}
