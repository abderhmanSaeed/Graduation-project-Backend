using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using GraduationProjectCSITINET.Models;
using Microsoft.IdentityModel.Tokens;

namespace GraduationProjectCSITINET.Controllers
{

    public class UsersController : ApiController
    {
        private GP db = new GP();



        //SignUpUSer

        [HttpPost]
        [Route("api/user/signup")]
        public HttpResponseMessage Register(User user)
        {
            user.RoleID = 2;
            var checkEmail = db.Users.Where(ww => ww.Email == user.Email).SingleOrDefault();
            if (checkEmail != null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Email already exist");
            }
            db.Users.Add(user);
            db.SaveChanges();
            return Request.CreateResponse(new { user.ID, user.Username, user.ProfilePicture, user.Adress.Adress1, user.Email });
        }

        //Login User
        [HttpPost]
        [Route("api/user/login")]
        public IHttpActionResult login(string email, string password)
        {
            var user = db.Users.Where(ww => ww.Email == email && ww.Password == password).Select(ww => new
            {
                ww.ID,
                ww.Email,
                ww.RoleID,
                ww.Username
            }).SingleOrDefault();
            if (user == null)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            string key = "my_secret_key_12345"; //Secret key which will be used later during validation    
            var issuer = "http://testwebsite.com";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("valid", "1"));
            permClaims.Add(new Claim("userid", user.ID.ToString()));
            permClaims.Add(new Claim("userRole", user.RoleID.ToString()));
            permClaims.Add(new Claim("name", user.Username.ToString()));
            permClaims.Add(new Claim(ClaimTypes.Role, user.RoleID.ToString()));
            var token = new JwtSecurityToken(issuer, //Issure    
                    issuer,  //Audience    
                    permClaims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { data =  jwt_token.ToString() });
        }

        [HttpGet]
        [Authorize]
        [Route("api/user/get")]
        public IHttpActionResult getUser()
        {
            var identity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            var id = Convert.ToInt32(claims.ToArray()[2].Value);
            var x= db.Users.Where(ww => ww.ID == id).Select(ww => new { ww.ID, ww.Email, ww.Adress.Adress1, ww.ProfilePicture, ww.RoleType.Rtype, ww.Username, ww.Phone }).SingleOrDefault();
            return Ok(x);
        }
        [HttpPost]
        [Route("api/user/register")]
        public IHttpActionResult signup()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new Exception();


            
            var httpRequest = HttpContext.Current.Request;
            User newuser = new User() { 
            Username = httpRequest.Form.Get("name"),
            Email= httpRequest.Form.Get("email"),
            Password= httpRequest.Form.Get("password"),
            AddressID = int.Parse(httpRequest.Form.Get("address")),
            RoleID = int.Parse(httpRequest.Form.Get("RoleID")),
            Phone= httpRequest.Form.Get("mobile"),

            };

            if (httpRequest.Files.Count > 0)
            {
                var file = httpRequest.Files[0];
                var mimeTime = file.ContentType.Split('/')[0];
                if (mimeTime != "image") newuser.ProfilePicture = "profile.jpg";
                else
                {
                    var fileName = "service-" + Guid.NewGuid().ToString() + file.FileName;
                    var filepath = HttpContext.Current.Server.MapPath(@"~/Public/img/user/" + fileName);
                    file.SaveAs(filepath);
                    newuser.ProfilePicture= fileName;
                }
            }
            var user = db.Users.SingleOrDefault(ww => ww.Email == newuser.Email);
            var address = db.Adresses.Where(ww => ww.Adress1 == newuser.Adress.Adress1).Select(ww => ww.Adress1);

            if (user != null)
            {
                return BadRequest();
            }
            if (address == null)
            {
                db.Adresses.Add(newuser.Adress);
                db.SaveChanges();
                var addrID = db.Adresses.SingleOrDefault(ww => ww.Adress1 == user.Adress.Adress1).ID;

                newuser.AddressID = addrID;
                db.Users.Add(newuser);
                return Ok(newuser);


            }
            else
            {
                db.Users.Add(newuser);
                db.SaveChanges();
                return Ok(newuser);
            }



        }

    }
}