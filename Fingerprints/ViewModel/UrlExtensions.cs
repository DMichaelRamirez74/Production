using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fingerprints.ViewModel
{
    public static class UrlExtensions
    {
        public static string BuildAbsolutePath(string serverUrl, bool forceHttps)
        {
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;

            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;

            newUrl = (forceHttps ? "https" : originalUri.Scheme) + "://" + originalUri.Authority + newUrl;
            return newUrl;
        }

        public static string BuildAbsolute(string relativeUri)
        {
            // get current uri
            Uri uri = HttpContext.Current.Request.Url;

            // build absolute path
            string app = HttpContext.Current.Request.ApplicationPath;

            if (!app.EndsWith("/"))
                app += "/";

            relativeUri = relativeUri.TrimStart('/');

            // return the absolute path
           // return HttpUtility.UrlPathEncode(String.Format("http://{0}:{1}{2}", uri.Host, app, relativeUri));
            return HttpUtility.UrlPathEncode(String.Format("http://{0}{1}{2}", uri.Authority, app, relativeUri));
            // return HttpUtility.UrlPathEncode(String.Format("http://{0}{1}{2}", uri.Host, app, relativeUri));
        }
        public static string ToAvtarImages(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                return "";

            }
            else
            {

                //  return "/StaffRegistration/AvtarImages/" + fileName;
                return BuildAbsolute("/AvtarImages/" + fileName);
            }
        }

        public static string LinkToRegistrationProcess(string fileName)
        {
            return BuildAbsolute(fileName);
        }
    }
}