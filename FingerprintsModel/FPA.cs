using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class FPA
    {
        public long FPAID { get; set; }
        public string EncriptedFPAID { get; set; }
        public string Test { get; set; }
        public Guid AgencyId { get; set; }
        public Guid StaffId { get; set; }
        public Int64 ClientId { get; set; }
        public string EncyrptedClientId { get; set; }//Changes
        public string Goal { get; set; }
        public string ChildName { get; set; }
        public string ParentName1 { get; set; }
        public string ParentName2 { get; set; }
        public string SignatureData { get; set; }
        public string FSWName { get; set; }
        public string ParentEmailId1 { get; set; }
        public string ParentEmailId2 { get; set; }
        public string Category { get; set; }
        public bool IsEmail1 { get; set; }
        public bool IsEmail2 { get; set; }
        public int GoalStatus { get; set; }
        public string GoalDate { get; set; }
        public string CompletionDate { get; set; }
        public string ActualGoalCompletionDate { get; set; }
        public int month { get; set; }
        public int week { get; set; }
        public string Domain { get; set; }
        public string AgencyLogo { get; set; }
        public string DomainDesc { get; set; }//Changes
        public string CategoryDesc { get; set; }
        public string Element { get; set; }
        public string ElemDesc { get; set; }
        public int GoalFor { get; set; }
        public bool IsSingleParent { get; set; }
        //public string Parent1 { get; set; }
        //public string Parent2 { get; set; }
        public List<ElementInfo> elemList = new List<ElementInfo>();
        public List<CategoryInfo> cateList = new List<CategoryInfo>();
        public List<DomainInfo> domList = new List<DomainInfo>();
        public List<FPASteps> GoalSteps = new List<FPASteps>();
        public class DomainInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        public class CategoryInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
      
       
    }
    public class ElementInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
