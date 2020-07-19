using System;
using System.Net.Mail;

namespace jvfts
{
    public class SendMail
    {
        public SendMail(string StatusMessage)
        {
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(UserSecrets.email_mail_server);

                    mail.From = new MailAddress(UserSecrets.email_address);
                    mail.To.Add(UserSecrets.email_address);
                    mail.Subject = "DB status update";
                    mail.Body = StatusMessage;

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(UserSecrets.email_address, UserSecrets.email_password);
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                Console.WriteLine(ex.ToString());
                }
           
            }
        }
    }
