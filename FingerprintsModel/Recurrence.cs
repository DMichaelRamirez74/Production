using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
   public class Recurrence
    {
        public long RecurringId { get; set; }
        public string Repeat { get; set; } //Daily, Monthly, Weekly, Yearly
        public int RepeatEvery { get; set; }//by default 1 ; else 2 or 10 day after or week after month after or year after
        public string Repeaton { get; set; }//this property is only for weekly case monday,tuesdayetc
        public string RepeatBy { get; set; }//this property is only for monthly case day of the month or day of the week of month,
        //for ex last monday or first saturday
        public string StartsOn { get; set; }//date of the day from when event will started
        public string EndsOn { get; set; }//when this recurrence will end. three cases 1. never 2.On a date 3 After no of meeting like 5 meeting 10 meeting
        public string RecurringRule { get; set; }
        public string Summery { get; set; }
    }
}
