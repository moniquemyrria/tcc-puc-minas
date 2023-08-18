using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Kodigos.API.Utils.Core
{
    public class SendEmail
    {
        public static async Task<bool> SendEmailText(IConfiguration configuration, string EmailTo, string Title, string Html,
            List<ModelAnexos> anexos, string ToCopias)
        {

            // create email message
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(configuration.GetSection("Smtp:User").Value.ToString()));


            //email.To.Add(MailboxAddress.Parse(EmailTo));

            foreach (var item in EmailTo.Split(";"))
            {
                if (item.Trim() != "" && item.Contains('@'))
                {
                    email.To.Add(MailboxAddress.Parse(item));
                }
            }


            var emailsCopia = ToCopias;  //configuration.GetSection("Smtp:EmailsCopiaCompras").Value.ToString();
            if (emailsCopia != null && emailsCopia.Trim() != null)
            {
                var emailsCopias = emailsCopia.Split(";");
                foreach (var item in emailsCopias)
                {
                    //if (item.Trim() != "" && item.Contains("@inventuspower.com"))
                    if (item.Trim() != "")
                        {
                        email.Cc.Add(MailboxAddress.Parse(item));
                    }
                }

            }


            email.Subject = Title;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = Html
            };

            //Anexar arquivos no corpo do email
            if (anexos != null && anexos.Count > 0)
            {
                foreach (var file in anexos)
                {
                    bodyBuilder.Attachments.Add(file.Title, Convert.FromBase64String(file.Document.Split(";base64,")[1]));
                }
            }

            email.Body = bodyBuilder.ToMessageBody();

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(configuration.GetSection("Smtp:Host").Value.ToString(),
                    int.Parse(configuration.GetSection("Smtp:Port").Value.ToString()), SecureSocketOptions.StartTls);

            smtp.Authenticate(configuration.GetSection("Smtp:User").Value.ToString(), configuration.GetSection("Smtp:Pass").Value.ToString());
            smtp.Send(email);

            smtp.Disconnect(true);

            return true;
        }
    }
}
