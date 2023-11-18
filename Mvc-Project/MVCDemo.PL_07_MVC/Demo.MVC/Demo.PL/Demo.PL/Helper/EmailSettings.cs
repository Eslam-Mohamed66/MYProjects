using Demo.DAL.Entities;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public class EmailSettings
    {
        public static void SendEmail(Email  email)
        {
            var client = new SmtpClient("smtp.ethereal.email", 587);

            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("terrance41@ethereal.email", "4AAcym6phtS5zXzKJ9");

            client.Send("terrance41@ethereal.email", email.To , email.Title, email.Body);
        }
    }
}
