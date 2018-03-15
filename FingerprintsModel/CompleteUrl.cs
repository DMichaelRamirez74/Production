using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace FingerprintsModel
{
   public class CompleteUrl
    {
        public static string LinkToRegistrationProcess(string fileName)
        {
            return BuildAbsolute(fileName);
        }
        public static string BuildAbsolute(string relativeUri)
        {
            Uri uri = HttpContext.Current.Request.Url;
            string app = HttpContext.Current.Request.ApplicationPath;
            if (!app.EndsWith("/"))
                app += "/";
            relativeUri = relativeUri.TrimStart('/');
            return HttpUtility.UrlPathEncode(String.Format("http://{0}:{1}{2}{3}", uri.Host, uri.Port, app, relativeUri));
        }
    }
}
