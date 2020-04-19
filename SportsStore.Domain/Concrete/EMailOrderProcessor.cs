using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SportsStore.Domain.Concrete
{
    public class EMailSettings
    {
        public string MailToAddress = "order@example.com";
        public string MailFromAddress = "sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"C:\sports_store_emails";
    }

    public class EMailOrderProcessor : IOrderProcessor
    {
        private readonly EMailSettings emailSettings;

        public EMailOrderProcessor(EMailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("-----------------------------")
                    .AppendLine("Items:");
                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c})", line.Quantity, line.Product.Name, subtotal);
                }
                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue());
                body.AppendLine("-----------------------------")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2 ?? "")
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.Zip)
                    .AppendLine(shippingDetails.Country)
                    .AppendLine("-----------------------------");
                body.AppendFormat("Gift wrap: {0}", shippingDetails.GiftWrap ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(emailSettings.MailFromAddress, emailSettings.MailToAddress, "New order submitted", body.ToString());
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}
