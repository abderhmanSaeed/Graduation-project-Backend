using GraduationProjectCSITINET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GraduationProjectCSITINET.Controllers
{
    public class AddressController : ApiController
    {
        private GP db = new GP();
        [HttpGet]
        [Route("api/address/getaddress")]
        public IHttpActionResult GetAddress()
        {
            var address = db.Adresses.Select(ww=>new { ww.ID,name = ww.Adress1}).ToList();
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }
        //get addresses which contain the string of specific address
        [HttpGet]
        [Route("api/address/searchAddress")]
        public IHttpActionResult searchAddress(string address)
        {
            var query = from add in db.Adresses
                        where add.Adress1.Contains(address)
                        select add;

            if (query == null)
            {
                return NotFound();
            }
            return Ok(query);
        }
        //return all workers with the the specific address
        [HttpGet]
        [Route("api/address/getWorkerswithAddress")]
        public IHttpActionResult getWorkerswithAddress(string id)
        {
            var query = from address in db.Adresses
                        join user in db.Users
on address.ID equals user.AddressID
                        join serviceworker in db.ServiceWorkers
                        on user.ID equals serviceworker.WorkerID
                        join service in db.Services
                        on serviceworker.ServiceID equals service.ID
                        where user.AddressID.ToString() == id
                        select new
                        {
                            service.ServiceType.Name,
                            serviceName = service.Name,
                            service.Description,
                            serviceworker.Rating,
                            serviceworker.ID,
                            serviceworker.User.Username,
                            serviceworker.User.ProfilePicture,
                            service.ServiceType.ServicePicture
                        };
            if (query == null)
            {
                return NotFound();
            }
            return Ok(query);

        }
    }
}
