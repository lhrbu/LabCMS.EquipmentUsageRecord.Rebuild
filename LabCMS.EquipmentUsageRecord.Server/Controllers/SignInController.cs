using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Raccoon.Devkits.JwtAuthorization;
using Raccoon.Devkits.JwtAuthroization.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.EquipmentUsageRecord.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly CookieJwtEncoder _cookieJwtEncoder;
        private readonly IConfiguration _configuration;
        public SignInController(
            CookieJwtEncoder cookieJwtEncoder,
            IConfiguration configuration)
        { 
            _cookieJwtEncoder = cookieJwtEncoder;
            _configuration = configuration;
        }

        [HttpPost]
        public void Post()=>
            _cookieJwtEncoder.EncodeHttpContext(HttpContext,
                "usage_records", 
                new Dictionary<string, object>
                {
                    {"role","admin" },
                    {"exp", 1610485450}
                });
           
        
    }
}
