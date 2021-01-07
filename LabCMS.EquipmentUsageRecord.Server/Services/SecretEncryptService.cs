using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace LabCMS.EquipmentUsageRecord.Server.Services
{
    public class SecretEncryptService
    {
        private readonly IConfiguration _configuration;
        private string _Key => _configuration["key"];
        private RijndaelManaged _RM => new()
        {
            Key = Encoding.UTF8.GetBytes(_Key),
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        public SecretEncryptService(IConfiguration configuration)
        { _configuration = configuration; }
        public string Encrypt(string value)
        {
            byte[] valueBuffer = Encoding.UTF8.GetBytes(value);
            ICryptoTransform cTransform = _RM.CreateEncryptor();
            byte[] resultBuffer = cTransform.TransformFinalBlock(valueBuffer, 0, valueBuffer.Length);
            return Convert.ToBase64String(resultBuffer);
        }

        public string Decrypt(string base64Payload)
        {
            byte[] payloadBuffer = Convert.FromBase64String(base64Payload);
            ICryptoTransform cTransform = _RM.CreateDecryptor();
            byte[] resultBuffer = cTransform.TransformFinalBlock(payloadBuffer, 0, payloadBuffer.Length);
            return Encoding.UTF8.GetString(resultBuffer);
        }
    }
}
