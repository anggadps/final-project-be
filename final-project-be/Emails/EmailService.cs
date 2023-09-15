using MailKit.Net.Smtp;
using MimeKit;
using RazorEngineCore;
using System.Text;


namespace final_project_be.Emails
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _fromDisplayName;
        private readonly string _from;
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly int _port;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _fromDisplayName = _configuration.GetSection("EmailSettings:FromDisplayName").Value;
            _from = _configuration.GetSection("EmailSettings:From").Value;
            _host = _configuration.GetSection("EmailSettings:Host").Value;
            _username = _configuration.GetSection("EmailSettings:Username").Value;
            _password = _configuration.GetSection("EmailSettings:Password").Value;
            _port = Convert.ToInt32(_configuration.GetSection("EmailSettings:Port").Value);
        }

        public async Task<bool> SendAsync(EmailModel mailModel, CancellationToken ct)
        {
            try
            {
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress(_fromDisplayName, _from));
                mail.Sender = new MailboxAddress(_fromDisplayName, _from);

                foreach (string mailAddress in mailModel.To)
                    mail.To.Add(MailboxAddress.Parse(mailAddress));

                foreach (string mailAddress in mailModel.Cc)
                    mail.To.Add(MailboxAddress.Parse(mailAddress));

                foreach (string mailAddress in mailModel.Bcc)
                    mail.To.Add(MailboxAddress.Parse(mailAddress));

                var body = new BodyBuilder();
                mail.Subject = mailModel.Subject;
                body.HtmlBody = mailModel.Body;
                mail.Body = body.ToMessageBody();

                using var smtp = new System.Net.Mail.SmtpClient();

                object value = await smtp.ConnectAsync(_host, _port, true, ct);

                await smtp.AuthenticateAsync(_username, _password, ct);
                await smtp.SendAsync(mail, ct);
                await smtp.DisconnectAsync(true, ct);

                return true;
            }
            catch
            {
                throw;
            }
        }

        public string LoadTemplate(string template)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string templateDir = Path.Combine(baseDir, "Emails/Template");
            string templatePath = Path.Combine(templateDir, $"{template}.cshtml");

            using FileStream fs = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader sr = new StreamReader(fs, Encoding.Default);

            string mailTemplate = sr.ReadToEnd();
            sr.Close();

            return mailTemplate;
        }

        public string GetMailTemplate<T>(string emailTemplate, T model)
        {
            string mailTemplate = LoadTemplate(emailTemplate);

            IRazorEngine engine = new RazorEngine();
            IRazorEngineCompiledTemplate modifyTemplate = engine.Compile(mailTemplate);


            return modifyTemplate.Run(model);
        }
    }
}
