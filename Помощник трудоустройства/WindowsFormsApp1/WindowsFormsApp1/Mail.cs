using System;
using System.Net;
using System.Net.Mail;

namespace WindowsFormsApp1
{
    class Mail
    {
        public void SendMail()    //процедура отправки электронного письма с файлом
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("wersat@mail.ru");
                mail.To.Add(new MailAddress("wersat007@yandex.ru"));
                mail.Subject = "Log файл";
                mail.Body = "log file";
                if (!string.IsNullOrEmpty("rezume.docx"))
                    mail.Attachments.Add(new Attachment("rezume.docx"));
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.mail.ru";
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("wersat@mail.ru".Split('@')[0], "*Raider_002");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }
    }
}
