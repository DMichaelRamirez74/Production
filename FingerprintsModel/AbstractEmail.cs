using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerprintsModel
{
  public  abstract class AbstractEmail
    {

        abstract  protected   Task<int> SendEmailCenterClosure(params object[] list);


        abstract protected  Task<int> SendEmailUnscheduledSchoolDay(params object[] list);

       
    }
}
