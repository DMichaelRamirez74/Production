﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
     public  class Slots
    {
        public int SlotId { get; set; }
        public string Slot { get; set; }
        public string ExistingSlot { get; set; }
        public string AgencyId { get; set; }
        public int ProgramId { get; set; }
        public string ProgramType { get; set; }
        public string ProgramYear { get; set; }
        public string AgencyName { get; set; }
        public string Createddate { get; set; }
     
    }
    public class LSlots
    {
        public List<Slots> Slots { get; set; }
        public List<FamilyHousehold.Programdetail> Programlist { get; set; }

    }

    public class AddSlotsEmail
    {

        public string Subject { get; set; }
        public string Body { get; set; }


        public string SenderName { get; set; }
        public string SenderRole { get; set; }
        public string SenderPhone { get; set; }
        public string ExecutiveEmailList { get; set; }

  

        public string Name { get; set; }
        public string slots { get; set; }

    }
  
}
