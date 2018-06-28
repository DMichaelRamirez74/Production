using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
    public class NewProgramYearTransition
    {


        public NewProgramYearTransition()
        {
            this.EndOfProgramYearDashboard = new NewProgramYearTransitionDashboard();
        }


        public NewProgramYearTransitionDashboard EndOfProgramYearDashboard { get; set; }

    }



    public class NewProgramYearTransitionDashboard
    {
        public NewProgramYearTransitionDashboard()
        {
            
            this.Centers = new NewProgramYearTransitionCounts();
            this.Classrooms = new NewProgramYearTransitionCounts();
            this.Funds = new NewProgramYearTransitionCounts();
            this.ProgramTypes = new NewProgramYearTransitionCounts();
            this.Seats = new EndOfYearSlotsSeats();
            this.Slots = new EndOfYearSlotsSeats();
            this.Staffs = new NewProgramYearTransitionCounts();
        }

        public NewProgramYearTransitionCounts ProgramTypes { get; set; }
        public NewProgramYearTransitionCounts Centers { get; set; }
        public NewProgramYearTransitionCounts Classrooms{ get; set;}

        public NewProgramYearTransitionCounts Funds { get; set; }

        public NewProgramYearTransitionCounts Staffs { get; set; }
        public EndOfYearSlotsSeats Seats { get; set; }

        public EndOfYearSlotsSeats Slots { get; set; }

    }

    public class NewProgramYearTransitionCounts
    {
        public long Total { get; set; }
        public long Active { get; set; }
    }

    public class EndOfYearSlotsSeats {

        public long Total{get;set;}

        public long Occupied { get; set; }

        public long Expiring { get; set; }

        public long Opened { get; set; }
    }




}
