using Arch.Core.Constants;
using Arch.Utilities.Manager;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
namespace System.Web.Mvc
{
    public class MailHelper
    {
        public static void SendMail(string title, string body, string to, List<string> cc, List<Attachment> attachementList)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(Resources.Parameter.SistemMailSmtpClient);
            mail.From = new MailAddress(Resources.Parameter.SistemMail, Resources.Parameter.SistemMailBaslik);
            mail.To.Add(to);
            foreach (var item in cc)
            {
                mail.CC.Add(item);
            }
            foreach (var item in attachementList)
            {
                mail.Attachments.Add(item);
            }
            mail.Subject = title;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(Resources.Parameter.SistemMail, EnDeCode.Decrypt(Resources.Parameter.SistemMailSifre, StaticParams.SifrelemeParametresi));
            SmtpServer.Send(mail);
        }
        public static void SendMail(string title, string body, List<string> to, string cc, List<Attachment> attachementList)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(Resources.Parameter.SistemMailSmtpClient);
            mail.From = new MailAddress(Resources.Parameter.SistemMail, Resources.Parameter.SistemMailBaslik);
            mail.CC.Add(cc);
            foreach (var item in to)
            {
                mail.To.Add(item);
            }
            foreach (var item in attachementList)
            {
                mail.Attachments.Add(item);
            }
            mail.Subject = title;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(Resources.Parameter.SistemMail, EnDeCode.Decrypt(Resources.Parameter.SistemMailSifre, StaticParams.SifrelemeParametresi));
            SmtpServer.Send(mail);
        }

        public static void SendMail(string title, string body, List<string> tos)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(Resources.Parameter.SistemMailSmtpClient);
            mail.From = new MailAddress(Resources.Parameter.SistemMail, Resources.Parameter.SistemMailBaslik);
            foreach (var item in tos)
            {
                mail.To.Add(item);
            }
            mail.Subject = title;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(Resources.Parameter.SistemMail, EnDeCode.Decrypt(Resources.Parameter.SistemMailSifre, StaticParams.SifrelemeParametresi));
            SmtpServer.Send(mail);
        }
    }
}
