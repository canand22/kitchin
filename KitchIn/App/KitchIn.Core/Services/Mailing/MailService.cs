using System.Net.Mail;

namespace KitchIn.Core.Services.Mailing
{
    public class MailService : IMailService
    {
        private readonly SmtpClient smtp; 
        
        public MailService()
        {
            this.smtp = new SmtpClient();
        }

        public string Send(string to, string message)
        {
            var mes = string.Empty;
            
            var mailMessage = new MailMessage { Subject = "new password", Body = message };

            mailMessage.To.Add(to);
            try
            {
                this.smtp.Send(mailMessage);
            }
            catch (SmtpException e)
            {
                mes = string.Empty; 
            }

            return mes;
        }
    }
}