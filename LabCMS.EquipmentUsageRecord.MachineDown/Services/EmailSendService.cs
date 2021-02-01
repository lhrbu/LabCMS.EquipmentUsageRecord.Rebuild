using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Services
{
    public class EmailSendService:IDisposable
    {
        private readonly SmtpClient _smtpClient;
        public EmailSendService(SmtpClient smtpClient)
        { _smtpClient = smtpClient;}

        public async ValueTask SendEmailAsync(IEnumerable<string> fromAddresses,IEnumerable<string> toAddresses,
            string subject,string payload)
        {
            MimeMessage email = new (fromAddresses,toAddresses,subject);
            email.Body = new TextPart(TextFormat.Html){Text = payload};
            await _smtpClient.SendAsync(email);
        }

        public void Dispose()
        {
            _smtpClient.Disconnect(true);
            _smtpClient.Dispose();
        }
    }
}