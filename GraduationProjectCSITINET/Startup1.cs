﻿using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Jwt;
using Owin;

[assembly: OwinStartup(typeof(GraduationProjectCSITINET.Startup1))]

namespace GraduationProjectCSITINET
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
           
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://testwebsite.com", //some string, normally web url,  
                        ValidAudience = "http://mysite.com",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_secret_key_12345"))
                    }
                });
        }
    }
}
