using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.MachineDown.Models
{
    public class SmtpClientFactory
    {
        public SmtpClient Create()
        {
            SmtpClient smtpClient = new();
            smtpClient.Connect("smtpapac.hella.com", 25, SecureSocketOptions.None);
            smtpClient.Authenticate("liha52", "2112358LHR/");
            //smtpClient.Connect("smtp.163.com", 25, SecureSocketOptions.None);
            //smtpClient.Authenticate("lihaoran228@163.com", "1234qwer");
            return smtpClient;
        }
    }
}
