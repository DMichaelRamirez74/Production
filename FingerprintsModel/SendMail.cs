using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Xml;


namespace FingerprintsModel
{
    public class SendMail
    {
        static XmlNodeList xmlnode;
        static XmlDocument xmlDoc = new XmlDocument();
        static string subject = string.Empty;
        static string footer = string.Empty;
        static string body = string.Empty;
        public static string SendEmail(string Emailid, string Password, string username, string path, string imagepath, string link = "", string code = "", string SuperAdmin = "")
        {
            try
            {

                MailMessage Message = new MailMessage(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]), Emailid);
                if (!string.IsNullOrEmpty(username))
                {

                    xmlDoc.Load(path + "\\Admin.xml");
                    xmlnode = xmlDoc.GetElementsByTagName("Subject");
                    subject = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Footer");
                    footer = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Body");
                    body = xmlnode[0].InnerXml;
                    if (SuperAdmin.ToLower().Equals("yes"))
                    {
                        Message.Body = body.Replace("$Name$", username.TrimEnd().TrimStart()).Replace("$Emailid$", Emailid).Replace("$Password$", Password).Replace("Agency Code: $code$", String.Empty).Replace("$Link$ ", link).Replace("$Path$", imagepath);
                    }
                    else
                    {
                        Message.Body = body.Replace("$Name$", username.TrimEnd().TrimStart()).Replace("$Emailid$", Emailid).Replace("$Password$", Password).Replace("$code$", code).Replace("$Link$", link).Replace("$Path$", imagepath);
                    }
                }
                else
                {
                    xmlDoc.Load(path + "\\Password.xml");
                    xmlnode = xmlDoc.GetElementsByTagName("Subject");
                    subject = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Footer");
                    footer = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Body");
                    body = xmlnode[0].InnerXml;
                    Message.Body = body.Replace("$Name$", username.TrimEnd().TrimStart()).Replace("$Emailid$", Emailid).Replace("$Password$", Password).Replace("$Path$", imagepath);
                    //Message.Body = "Hi " + username + ", <br/><br/>You Password has been changed successfully. <br/><br/> Your login credentials <br/><br/> " +
                    // " Username (Email): " + Emailid + "<br/><br/>Password: " + Password + "<br /><br />Thank You.";
                }
                //Message.Subject = "Login details for Genesis Earth";
                Message.Subject = subject;
                Message.IsBodyHtml = true;
                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.Credentials = basicCredential;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.Send(Message);
                return "If the entered email exist an email has been send to the entered email id.";

            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }
        public static string SendEmailToSuperAdmin(string Emailid, string Password, string username, string path, string imagepath, string link = "")
        {
            try
            {

                MailMessage Message = new MailMessage(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]), Emailid);
                xmlDoc.Load(path + "\\SuperAdmin.xml");
                xmlnode = xmlDoc.GetElementsByTagName("Subject");
                subject = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Footer");
                footer = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Body");
                body = xmlnode[0].InnerXml;
                Message.Body = body.Replace("$Name$", username.TrimEnd().TrimStart()).Replace("$Emailid$", Emailid).Replace("$Password$", Password).Replace("$domain", link).Replace("$Path$", imagepath);
                Message.Subject = subject;
                Message.IsBodyHtml = true;
                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.Credentials = basicCredential;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.Send(Message);
                return "If the entered email exist an email has been send to the entered email id.";

            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }
        public static string Sendenrollmentemail(string Emailid, string agencycode, string agencyname, DateTime expiryttime, string path, string link, string imagepath)
        {
            try
            {
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]));
                //Message.Body = "Hi ,<br/><br/> Please click following link for registration <a style='text-decoration:underline!important;' href='" + path + "' >Click here for registration process </a>  .<br /><br /> Enrollment Code : " + agencycode + " <br /><br /> Thank You.";
                //Message.Subject = "Staff Registartion";

                xmlDoc.Load(path);
                xmlnode = xmlDoc.GetElementsByTagName("Subject");
                subject = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Footer");
                footer = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Body");
                body = xmlnode[0].InnerXml;
                Message.Subject = subject;
                Message.Body = body.Replace("$Name$", "").Replace("$Link$", link).Replace("$Code$", agencycode).Replace("$Agencyname$", agencyname).Replace("$ValidUpto$", expiryttime.ToString()).Replace("$Path$", imagepath);

                Message.IsBodyHtml = true;
                string[] emailids = Emailid.Split(';');
                foreach (string email in emailids)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                        Message.To.Add(new MailAddress(email.Trim()));
                }
                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.Credentials = basicCredential;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.Send(Message);
                return "If the entered email exist an email has been send to the entered email id.";
            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }
        public static string SendEmailoldnew(string emailidold, string emailidnew, string username, string emailoldnew, string path, string imagepath)
        {
            try
            {


                xmlDoc.Load(path + "\\Emailchanged.xml");
                xmlnode = xmlDoc.GetElementsByTagName("Subject");
                subject = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Footer");
                footer = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Body");
                body = xmlnode[0].InnerXml;

                string[] emailids = emailoldnew.Split(',');
                foreach (string email in emailids)
                {
                    MailMessage Message = new MailMessage(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]), email);
                    Message.Body = body.Replace("$Name$", username.TrimEnd().TrimStart()).Replace("$Emailidold$", emailidold).Replace("$Emailidnew$", emailidnew).Replace("$Path$", imagepath);
                    Message.Subject = subject;
                    Message.IsBodyHtml = true;
                    SmtpClient Client = new SmtpClient();
                    Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                    Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                    NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                    Client.UseDefaultCredentials = true;
                    Client.Credentials = basicCredential;
                    Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                    Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    Client.Send(Message);

                }
                return "If the entered email exist an email has been send to the entered email id.";
            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }
        public static string Sendverificationemail(string Emailid, string name, string path, string Template, string imagepath)
        {
            try
            {
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]));
                //Message.Body = "Hi " + name + ",<br/><br/> Please click following link for verification <a style='text-decoration: underline !important;' href='" + path + "' >Click here for further verification</a> .<br /><br /> <br /><br /> Thank You.";
                //Message.Subject = "Staff Registartion ";
                xmlDoc.Load(Template);
                xmlnode = xmlDoc.GetElementsByTagName("Subject");
                subject = xmlnode[0].InnerXml;
                // xmlnode = xmlDoc.GetElementsByTagName("Footer");
                // footer = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Body");
                body = xmlnode[0].InnerXml;
                Message.Subject = subject;
                Message.Body = body.Replace("$Name$", name.TrimEnd().TrimStart()).Replace("$Link$", path).Replace("$Path$", imagepath);
                Message.IsBodyHtml = true;
                string[] emailids = Emailid.Split(',');
                foreach (string email in emailids)
                {
                    Message.To.Add(new MailAddress(email));
                }
                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.Credentials = basicCredential;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.Send(Message);
                return "If the entered email exist an email has been send to the entered email id.";
            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }

        public static string SendEventChangedEmail(string EmailId, string template, string FromEmail)
        {
            try
            {
                using (MailMessage mailMessage = new MailMessage())

                {


                    mailMessage.From = new MailAddress(FromEmail);
                    mailMessage.Subject = "SendMail";
                    mailMessage.Body = template;
                    mailMessage.To.Add(new MailAddress(EmailId));

                    mailMessage.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);

                    smtp.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;

                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

                    NetworkCred.UserName = ConfigurationManager.AppSettings["MailServerUserName"]; //reading from web.config  

                    NetworkCred.Password = ConfigurationManager.AppSettings["MailserverPwd"]; //reading from web.config  

                    smtp.UseDefaultCredentials = true;

                    smtp.Credentials = NetworkCred;

                    smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);

                    smtp.Send(mailMessage);

                    return "If the entered email exist an email has been send to the entered email id.";
                }


            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }
        public static string SendEventChangedEmail(string EmailId, string FullName, string EventDate, string EventTime,string WorkShopName, string CenterName, string template, string imagepath,string link,string reason,string FromEmail)
        {
            try
            {
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress(FromEmail);
                //Message.Body = "Hi " + name + ",<br/><br/> Please click following link for verification <a style='text-decoration: underline !important;' href='" + path + "' >Click here for further verification</a> .<br /><br /> <br /><br /> Thank You.";
                //Message.Subject = "Staff Registartion ";
                xmlDoc.Load(template);
                xmlnode = xmlDoc.GetElementsByTagName("Subject");
                subject = xmlnode[0].InnerXml;
                // xmlnode = xmlDoc.GetElementsByTagName("Footer");
                // footer = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Body");
                body = xmlnode[0].InnerXml;
                Message.Subject = subject;
                Message.Body = body.Replace("$Name$", FullName).Replace("$WorkShopName$", WorkShopName).Replace("$EventDate$", EventDate).Replace("$EventTime$",EventTime).Replace("$CenterName$",CenterName).Replace("$url$",link).Replace("$Path$",imagepath).Replace("$ChangeReason$",reason);
                Message.IsBodyHtml = true;
                    Message.To.Add(new MailAddress(EmailId));
                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.Credentials = basicCredential;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.Send(Message);
                return "If the entered email exist an email has been send to the entered email id.";
            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }
        public static string SendHRverificationemail(string Emailid, string name, string path, string Template, string code, string imagepath)
        {
            try
            {
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]));
                //Message.Body = "Hi " + name + ",<br/><br/> Please click following link for verification <a style='text-decoration: underline !important;' href='" + path + "' >Click here for further verification</a> .<br /><br /> <br /><br /> Thank You.";
                //Message.Subject = "Staff Registartion ";
                xmlDoc.Load(Template);
                xmlnode = xmlDoc.GetElementsByTagName("Subject");
                subject = xmlnode[0].InnerXml;
                // xmlnode = xmlDoc.GetElementsByTagName("Footer");
                // footer = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Body");
                body = xmlnode[0].InnerXml;
                Message.Subject = subject;
                if (code != "")
                {
                    Message.Body = body.Replace("$Name$", name.TrimEnd().TrimStart()).Replace("$Link$", path).Replace("$Code$", code).Replace("$Path$", imagepath);
                }
                else
                {
                    Message.Body = body.Replace("$Name$", name.TrimEnd().TrimStart()).Replace("$Path$", imagepath);
                }
                Message.IsBodyHtml = true;
                Message.To.Add(new MailAddress(Emailid));
                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.Credentials = basicCredential;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.Send(Message);
                return "If the entered email exist an email has been send to the entered email id.";
            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }
        public static string SendEmailagencyuser(string Emailid, string Password, string username, string path, string agencyname, string rolename, string imagepath, string link = "", string code = "")
        {
            try
            {

                MailMessage Message = new MailMessage(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]), Emailid);
                if (!string.IsNullOrEmpty(username))
                {

                    xmlDoc.Load(path + "\\Addagencyuser.xml");
                    xmlnode = xmlDoc.GetElementsByTagName("Subject");
                    subject = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Footer");
                    footer = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Body");
                    body = xmlnode[0].InnerXml;
                    Message.Body = body.Replace("$Name$", username.TrimEnd().TrimStart()).Replace("$Emailid$", Emailid).Replace("$Password$", Password).Replace("$code$", code).Replace("$Role$", rolename).Replace("$AgencyName$", agencyname).Replace("$Link$", link).Replace("$Path$", imagepath);

                }
                else
                {
                    xmlDoc.Load(path + "\\Password.xml");
                    xmlnode = xmlDoc.GetElementsByTagName("Subject");
                    subject = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Footer");
                    footer = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Body");
                    body = xmlnode[0].InnerXml;
                    Message.Body = body.Replace("$Name$", username.TrimEnd().TrimStart()).Replace("$Emailid$", Emailid).Replace("$Password$", Password).Replace("$Path$", imagepath);
                    //Message.Body = "Hi " + username + ", <br/><br/>You Password has been changed successfully. <br/><br/> Your login credentials <br/><br/> " +
                    // " Username (Email): " + Emailid + "<br/><br/>Password: " + Password + "<br /><br />Thank You.";
                }
                //Message.Subject = "Login details for Genesis Earth";
                Message.Subject = subject;
                Message.IsBodyHtml = true;
                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.Credentials = basicCredential;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.Send(Message);
                return "If the entered email exist an email has been send to the entered email id.";

            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }
        public static string Sendchangepassword(string Emailid, string Password, string username, string path, string imagepath, string link = "", string code = "", string SuperAdmin = "")
        {
            try
            {

                MailMessage Message = new MailMessage(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]), Emailid);

                xmlDoc.Load(path + "\\Password.xml");
                xmlnode = xmlDoc.GetElementsByTagName("Subject");
                subject = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Footer");
                footer = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Body");
                body = xmlnode[0].InnerXml;
                Message.Body = body.Replace("$Name$", username.TrimEnd().TrimStart()).Replace("$Emailid$", Emailid).Replace("$Password$", Password).Replace("$Path$", imagepath);
                Message.Subject = subject;
                Message.IsBodyHtml = true;
                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.Credentials = basicCredential;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.Send(Message);
                return "If the entered email exist an email has been send to the entered email id.";

            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }


        public static string SendEmail(string Emailid, string username, string agencyname, string path, string imagepath, string link = "")
        {
            try
            {

                MailMessage Message = new MailMessage(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]), Emailid);

                xmlDoc.Load(path + "\\ResendEmailAgency.xml");
                xmlnode = xmlDoc.GetElementsByTagName("Subject");
                subject = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Footer");
                footer = xmlnode[0].InnerXml;
                xmlnode = xmlDoc.GetElementsByTagName("Body");
                body = xmlnode[0].InnerXml;
                Message.Body = body.Replace("$Name$", username.TrimEnd().TrimStart()).Replace("$Emailid$", Emailid).Replace("$Link$", link).Replace("$Path$", imagepath).Replace("$AgencyName$", agencyname);


                Message.Subject = subject;
                Message.IsBodyHtml = true;
                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.Credentials = basicCredential;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.Send(Message);
                return "If the entered email exist an email has been send to the entered email id.";

            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }

        public static string SendFPAStepsEmail(string ParentName, string ChildName, string FSWName, string ParentEmail, string Goal, string path, string link, string code = "", string SuperAdmin = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(ParentEmail))
                {
                    //string link = UrlExtensions.LinkToRegistrationProcess("/login/Loginagency");
                    MailMessage Message = new MailMessage(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]), ParentEmail);

                    xmlDoc.Load(path + "\\FPASteps.xml");
                    xmlnode = xmlDoc.GetElementsByTagName("Subject");
                    subject = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Footer");
                    footer = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Body");
                    body = xmlnode[0].InnerXml;
                    Message.Body = body.Replace("$ParentName$", ParentName.TrimEnd().TrimStart()).Replace("$ChildName$", ChildName).Replace("$FPAGoal$", Goal).Replace("$Link$", link).Replace("$FSWName$", FSWName);
                    Message.Subject = subject;
                    Message.IsBodyHtml = true;
                    SmtpClient Client = new SmtpClient();
                    Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                    Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                    NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                    Client.UseDefaultCredentials = true;
                    Client.Credentials = basicCredential;
                    Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                    Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    Client.Send(Message);
                    return "Email sent successfully.";
                }
                else
                {
                    return "Parent Email is not found.";
                }

            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }
        public static bool SendEmailToParentAndTeacher(string emailId, string message, string userEmail)
        {
            try
            {
                MailMessage mailMessage = new MailMessage(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]), emailId);
                mailMessage.Body = message;
                mailMessage.Subject = "Mail From CenterManager" + " " + userEmail;
                mailMessage.IsBodyHtml = true;

                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.Credentials = basicCredential;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                try
                {
                    System.Threading.Thread thread = new System.Threading.Thread(() => Client.Send(mailMessage));
                    thread.Start();
                }
                catch(Exception ex)
                {
                    clsError.WriteException(ex);
                }
                
                return true;



            }
            catch (Exception ex)
            {
                string exp = ex.Message;
                return false;
            }
        }

        public static bool SendBillingEmail(string emailId, string message, string userEmail, string Attachments,string cc)
        {
            try
            {
                MailMessage mailMessage = new MailMessage(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]), emailId);
                if (cc.Trim() != "")
                    mailMessage.CC.Add(cc);
                mailMessage.Body = message;
                mailMessage.Subject = "Mail  From " + " " + userEmail;
                mailMessage.IsBodyHtml = true;
                Attachments = Attachments.TrimStart(',').TrimEnd(',');
               
                if (!string.IsNullOrEmpty(Attachments))
                    if (!Attachments.Contains(','))
                        mailMessage.Attachments.Add(new Attachment(Attachments));
                else
                    {
                        string[] attachmentslist = Attachments.Split(',');
                        foreach( string path in attachmentslist)
                        {
                            mailMessage.Attachments.Add(new Attachment(path));
                        }
                    }
                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.Credentials = basicCredential;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                System.Threading.Thread thread = new System.Threading.Thread(() => Client.Send(mailMessage));
                thread.Start();
                return true;
            }
            catch (Exception ex)
            {
                string exp = ex.Message;
                return false;
            }
        }
        public static string SendEmailForSlotsPurchase( string useremail,string UName, int slots, string path, string imagepath,string ToAddress)
        {
            try
            {
                string ToEmailid = Convert.ToString(ConfigurationManager.AppSettings["FinancialManagerEmailID"]);

                MailMessage Message = new MailMessage(Convert.ToString(ConfigurationManager.AppSettings["FromAddress"]), ToEmailid);
               
                    xmlDoc.Load(path + "\\PurchaseSlots.xml");
                    xmlnode = xmlDoc.GetElementsByTagName("Subject");
                    subject = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Footer");
                    footer = xmlnode[0].InnerXml;
                    xmlnode = xmlDoc.GetElementsByTagName("Body");
                    body = xmlnode[0].InnerXml;
                //string toaddrs=  string.Join(",", ToAddress);
                Message.Body = body.Replace("$AgencyName$", UName.TrimEnd().TrimStart()).Replace("$slots$", Convert.ToString(slots)).Replace("$ExeEmail$", ToAddress.TrimEnd().TrimStart());
                    //Message.Body = "Hi Greetings, <br/><br/>You Password has been changed successfully. <br/><br/> Your login credentials <br/><br/> " +
                    // " Username (Email): " + Emailid + "<br/><br/>Password: " + Password + "<br /><br />Thank You.";
                
                //Message.Subject = "Login details for Genesis Earth";
                Message.Subject = subject;
                Message.IsBodyHtml = true;
                SmtpClient Client = new SmtpClient();
                Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
                Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
                NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
                Client.UseDefaultCredentials = true;
                Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
                Client.Credentials = basicCredential;
                Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                Client.Send(Message);
                return "If the entered email exist an email has been send to the entered email id.";

            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }
        }
        //public static string SendMailForFacilityIssue(string path, AssignFacilityStaff ToAddressDetails, AssignFacilityStaff staffobj)
        //{
        //    try
        //    {


        //        MailMessage Message = new MailMessage((ToAddressDetails.StaffEmailaddress), staffobj.ExternalEmailId);

        //        xmlDoc.Load(path + "\\ExternalFacilityTemplate.xml");
        //        xmlnode = xmlDoc.GetElementsByTagName("Subject");
        //        subject = xmlnode[0].InnerXml;
        //        xmlnode = xmlDoc.GetElementsByTagName("Footer");
        //        footer = xmlnode[0].InnerXml;
        //        xmlnode = xmlDoc.GetElementsByTagName("Body");
        //        body = xmlnode[0].InnerXml;
        //        Message.Body = body.Replace("$userdescrp$", staffobj.UserDescrption.TrimEnd().TrimStart()).Replace("$classroomname$", staffobj.ClassroomName).Replace("$centername$", staffobj.CenterName).Replace("$contactno$", ToAddressDetails.StaffContact).Replace("$emailaddress$", ToAddressDetails.StaffEmailaddress.TrimEnd().TrimStart()).Replace("$Name$", ToAddressDetails.StaffName.TrimEnd().TrimStart()).Replace("$centeraddress$", staffobj.CenterAddress.TrimEnd().TrimStart()).Replace("$RoleName$", ToAddressDetails.RoleName.TrimEnd().TrimStart());          
        //        Message.Subject = subject;
        //        Message.IsBodyHtml = true;
        //        SmtpClient Client = new SmtpClient();
        //        Client.Host = Convert.ToString(ConfigurationManager.AppSettings["MailServer"]);
        //        Client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailServerPort"]);
        //        NetworkCredential basicCredential = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["MailServerUserName"]), Convert.ToString(ConfigurationManager.AppSettings["MailserverPwd"]));
        //        Client.UseDefaultCredentials = true;
        //        Client.EnableSsl = ConfigurationManager.AppSettings["EnableSSl"].ToString().ToLower() == "true" ? true : false;
        //        Client.Credentials = basicCredential;
        //        Client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        Client.Send(Message);
        //        return "If the entered email exist an email has been send to the entered email id.";

        //    }
        //    catch (Exception Ex)
        //    {
        //        return Ex.Message;

        //    }
        //}

    }
}
