namespace final_project_be.Emails
{
    public class EmailModel
    {

        // Receiver
        public List<string> To { get; }
        public List<string> Cc { get; }
        public List<String> Bcc { get; }

        // Content
        public string Subject { get; }
        public string? Body { get; }

        public EmailModel(List<string> to,
                          string subject,
                          string? body = null,
                          List<string>? cc = null,
                          List<string>? bcc = null)
        {
            To = to;
            Cc = cc ?? new List<string>();
            Bcc = bcc ?? new List<string>();

            Subject = subject;
            Body = body;
        }
    }
}
