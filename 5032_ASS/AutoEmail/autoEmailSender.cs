using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;

namespace _5032_ASS.AutoEmail
{
    public class autoEmailSender
    {    
        public async Task SendAsync(String toEmail, String date)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("caobaichuan1122@gmail.com", "8878598cao"),
                EnableSsl = true,
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("caobaichuan1122@gmail.com"),
                Subject = "Confirm",
                Body = "Congratulations on your appointment date: "+date,
                IsBodyHtml = true,
            };
            var attachment = new System.Net.Mail.Attachment("C:\\Users\\Ansen\\Pictures\\timg.jpg", MediaTypeNames.Image.Jpeg);
            mailMessage.Attachments.Add(attachment);
            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }

    }
}