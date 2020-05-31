using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using GraduationProjectCSITINET.Models;
using System.Web;

namespace GraduationProjectCSITINET.Controllers
{
    public class AdminController : ApiController
    {
        GP db;
        public AdminController()
        {
            db = new GP();
        }
        




        //delete user by id
        [HttpDelete]
        [Route("api/admin/users/{id}")]
        public IHttpActionResult deleteUserById(int id)
        {
            var user = db.Users.Find(id);
            db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }
        //get all workers

        [HttpGet]
        [Route("api/admin/services")]
        public IHttpActionResult getAllServices()
        {
            var services = db.Services.ToList().Select(ww => new
            {

                ww.Name,
             
                ww.TypeID,
                ww.Description
            });
            return Ok(services);
        }

        [HttpPost]
        [Route("api/admin/addServiceType")]
        public IHttpActionResult addSeriveType()
        {
            var httpRequest = HttpContext.Current.Request;
            ServiceType x = new ServiceType()
            {
                Name = httpRequest.Form.Get("name")
            };
            if(httpRequest.Files.Count > 0)
            {
                var file = httpRequest.Files[0];
                var mimeTime = file.ContentType.Split('/')[0];
                if (mimeTime != "image") x.ServicePicture = "service.jpg";
                else
                {
                    var fileName = "service-"+Guid.NewGuid().ToString() + file.FileName;
                    var filepath = HttpContext.Current.Server.MapPath(@"~/Public/img/service/"+fileName);
                    file.SaveAs(filepath);
                    x.ServicePicture = fileName;
                }
            }
            db.ServiceTypes.Add(x);
            db.SaveChanges();
            return Ok(x);
        }

        [HttpPost]
        [Route("api/admin/addService")]
        public IHttpActionResult addSerive()
        {
            var httpRequest = HttpContext.Current.Request;
            Service x = new Service()
            {
                Name = httpRequest.Form.Get("name"),
                Description = httpRequest.Form.Get("description"),
                TypeID = int.Parse(httpRequest.Form.Get("typeid"))
            };
            if (httpRequest.Files.Count > 0)
            {
                var file = httpRequest.Files[0];
                var mimeTime = file.ContentType.Split('/')[0];
                if (mimeTime != "image") x.ServicePicture = "service.jpg";
                else
                {
                    var fileName = "service-"+ Guid.NewGuid().ToString() + file.FileName;
                    var filepath = HttpContext.Current.Server.MapPath(@"~/Public/img/service/" + fileName);
                    file.SaveAs(filepath);
                    x.ServicePicture = fileName;
                }
            }
            db.Services.Add(x);
            db.SaveChanges();
            return Ok(x);
        }

        [HttpGet]
        [Route("api/admin/getTypes")]
        public IHttpActionResult getTypes()
        {
            var x = db.ServiceTypes.Select(ww => new { ww.ID, ww.Name, ww.ServicePicture }).ToList();
            
            return Ok(x);
        }

        [HttpGet]
        [Route("api/admin/getServices")]
        public IHttpActionResult getService()
        {
            var x = db.Services.Select(ww => new { ww.ID,ww.Name,ww.ServicePicture,ServiceType = ww.ServiceType.Name,ServiceTypeName = ww.ServiceType.Name}).ToList();

            return Ok(x);
        }

    }
}
