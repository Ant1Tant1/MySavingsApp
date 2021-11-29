using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SendEmail
{
    private MailMessage mail = new MailMessage();


    public void SendEmails(string subject, string body, string[] strAttachments = null)
    {
        mail.From = new MailAddress("antoine.tantin@gmail.com");
        mail.To.Add(Engine.MailAddress);

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;//GIVE CORRECT PORT HERE
        mail.Subject = "My Savings App - " + subject;
        mail.Body = body;

        if (strAttachments != null)
        {
            foreach (string strAttachment in strAttachments)
            {
                var attachment = new Attachment(strAttachment);
                // Add time stamp information for the file.
                ContentDisposition disposition = attachment.ContentDisposition;
                disposition.CreationDate = System.IO.File.GetCreationTime(strAttachment);
                disposition.ModificationDate = System.IO.File.GetLastWriteTime(strAttachment);
                disposition.ReadDate = System.IO.File.GetLastAccessTime(strAttachment);
                mail.Attachments.Add(attachment);
            }
        }


        smtpServer.Credentials = new System.Net.NetworkCredential("antoine.tantin", "txjrbbwimibczmur") as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
        smtpServer.Send(mail);
        //smtpServer.SendAsync(mail)
        Debug.Log("success");
    }
}
