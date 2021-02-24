using System.Net;
using System.Net.Mail;

namespace SiteMapGeneratorTool.Helpers
{
    /// <summary>
    /// Email helper
    /// </summary>
    public class EmailHelper
    {
        // Email subject
        private const string SUBJECT = "Site Map Generation Complete";

        // Variables
        private readonly SmtpClient SmtpClient;

        // Properties
        private string UserName { get; set; }
        public string DisplayName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="userName">Email address of sender</param>
        /// <param name="displayName">Display name of sender</param>
        /// <param name="password">Password of sender</param>
        /// <param name="host">Address of sending host</param>
        /// <param name="portString">String value of outgoing port</param>
        public EmailHelper(string userName, string displayName, string password, string host, string portString)
        {
            UserName = userName;
            DisplayName = displayName;
            int.TryParse(portString, out int port);
            SmtpClient = new SmtpClient(host)
            {
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = true,
                Port = port
            };
        }

        /// <summary>
        /// Send completion notification email
        /// </summary>
        /// <param name="recipient">Email of recipient</param>
        /// <param name="domain">Domain for link</param>
        /// <param name="guid">GUID of request</param>
        public void SendEmail(string recipient, string domain, string guid)
        {
            SmtpClient.Send(new MailMessage(new MailAddress(UserName, DisplayName), new MailAddress(recipient))
            {
                Subject = SUBJECT,
                Body = @$"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                    <meta charset='utf-8' />
                    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
	                <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' integrity='sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T' crossorigin='anonymous'>
                </head>
                <div class='container'>
	                <h2>Site Map Generation Complete</h2>
	                <p>The results from your site map generation request are now available to view.</p>
	                <p><a href='https://{domain}/results?guid={guid}'>View Results</a></p>
                </div>",
                IsBodyHtml = true
            });
        }
    }
}
