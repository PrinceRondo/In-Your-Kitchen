using InYourFridge.Data;
using MimeKit;
using System;
using System.Linq;
using MailKit.Net.Smtp;
using InYourFridge.Models;

namespace InYourFridge.Helper
{
    public class Mailer
    {
        private readonly AppDbContext dbContext;
        private readonly Utility utility;

        public Mailer(AppDbContext dbContext, Utility utility)
        {
            this.dbContext = dbContext;
            this.utility = utility;
        }
        public void SendMail(string link, string mailTo, string subject, string name)
        {
            try
            {
                var config = dbContext.MailConfigurations.Where(x => x.Id == 2).FirstOrDefault();
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("In Your Fridge", config.MailFromAddress);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(name, mailTo);
                message.To.Add(to);

                message.Subject = "Fast Lane Registration";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = "<h1>Fast Lane Email Confirmation</h1><br/><h3>Click <a href=" + link + ">here</a> to confirm your email</h3>";
                bodyBuilder.TextBody = "Click this link " + link + " to confirm your email";

                //bodyBuilder.Attachments.Add(path);
                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                
                client.Connect(config.SMTPHostName, Convert.ToInt16(config.SMTPPort),config.EnableSSL);
                client.Authenticate(config.MailUsername, config.MailPassword);

                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
            }
            catch (Exception ex)
            {
                //save error to db
                ErrorLog errorLog = new ErrorLog
                {
                    ErrorDate = DateTime.Now,
                    ErrorMessage = ex.Message,
                    ErrorSource = ex.Source,
                    ErrorStackTrace = ex.StackTrace
                };
                dbContext.ErrorLogs.Add(errorLog);
                dbContext.SaveChanges();
                //save error to file
                utility.SaveErrorMessage(ex);
            }
        }
    }
}
