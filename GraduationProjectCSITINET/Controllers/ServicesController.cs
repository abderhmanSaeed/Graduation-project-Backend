using GraduationProjectCSITINET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GraduationProjectCSITINET.Controllers
{
    public class ServicesController : ApiController
    {
        private GP db = new GP();
        [HttpGet]
        [Authorize]
        ////get reservation data for the user includes 
        ///sevice name from service table
        ///comments 
        ///{
        //    "ID": 6,
        //    "Username": "WnofDDVNNBhqlz",
        //    "ProfilePicture": "profile.png",
        //    "Rtype": "client",
        //    "Address": "ismailia",
        //    "ServiceID": 2,
        //    "Name": "home electrician",
        //    "serviceType": "Electrician",
        //    "Date": "2006-08-08T00:00:00",
        //    "WorkerID": 10,
        //    "workername": "WbnlHUWZAJixzd",
        //    "CommentBody": "nhuiokmn",
        //    "Rating": 3
        //},


        // get all services with type by id of the specific service

        [HttpPost]
        [Authorize]
        [Route("api/user/getservicesWithType")]
        public IHttpActionResult getservices(string id)
        {
            var query = from service in db.Services
                        join type in db.ServiceTypes
                       on service.TypeID equals type.ID
                        where service.TypeID.ToString() == id
                        select new
                        {
                            type.Name,
                            type.ServicePicture,
                            service.Description,
                            ServiceType = service.Name
                        };
            query = query.Take(7);

            if (query == null)
            {
                return NotFound();
            }
            return Ok(query);

        }
        [HttpGet]
        [Authorize]
        //User:Worker 
        //service 
        //get all services for the worker and types and name and rating
        [Route("api/user/getUserServices")]
        public IHttpActionResult getUserServices(string id)
        {
            var query = from user in db.Users
                        join workerservices in db.ServiceWorkers
                        on user.ID equals workerservices.WorkerID
                        join service in db.Services
                        on workerservices.ServiceID equals service.ID
                        join type in db.ServiceTypes
                        on service.TypeID equals type.ID
                        where user.ID.ToString() == id
                        select new
                        {
                            user.Username,
                            type.ServicePicture,
                            user.Adress.Adress1,
                            service.Name,
                            service.Description,
                            workerservices.Rating,
                            ServiceTyp = type.Name



                        };
            if (query == null)
            {
                return NotFound();
            }
            return Ok(query);
        }
        //get services types only  and the picture of the service
        [HttpGet]
        [Authorize]
        [Route("api/user/getservices")]
        public IHttpActionResult getService()

        {
            // var query = db.ServiceTypes.Select(ww => ww.Name&&);
            var query = from type in db.ServiceTypes


                        select new
                        {
                            type.Name,
                            type.ID,
                            type.ServicePicture
                        };


            if (query == null)
            {
                return NotFound();

            }
            return Ok(query);
        }
        [HttpGet]

        [Route("api/services/getservicesWorker")]
        public IHttpActionResult getservicesWorker(string id)
        {
            var query = from service in db.Services
                        join serviceWorker in db.ServiceWorkers
                        on service.ID equals serviceWorker.ServiceID

                        where service.ID.ToString() == id
                        select new
                        {
                            service.ServiceType.Name,
                            service.Description,
                            serviceWorker.Rating,
                            serviceWorker.ID,
                            serviceWorker.User.Username,
                            serviceWorker.User.ProfilePicture,
                            service.ServiceType.ServicePicture


                        };
            if (query == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(query);
            }

        }
        [HttpPost]
        [Authorize]
        [Route("api/services/addReservation")]
        public IHttpActionResult AddReservation(string sid, string wid, String uId, [FromBody]Reservation res)
        {
            var identity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            var id = Convert.ToInt32(claims.ToArray()[2].Value);
            var serviceWorkerID = db.ServiceWorkers.SingleOrDefault(ww => ww.ServiceID.ToString() == sid && ww.WorkerID.ToString() == wid).ID;
            var UserID = db.Users.SingleOrDefault(ww => ww.ID.ToString() == id).ID;
            res.ServiceWorkerId = serviceWorkerID;
            res.UserID = int.Parse(id);
            res.StatusID = 2;
            db.Reservations.Add(res);
            db.SaveChanges();
            if (serviceWorkerID == null || uId == null)
            {
                return BadRequest();
            }
            else
            {
                return Ok(res);
            }
        }
        //get services which contain the string of specific service
        [HttpGet]
        [Route("api/services/searchService")]
        public IHttpActionResult searchService(string serv)
        {
            var service = db.Services.ToList().Select(ww => ww.Name).Contains(serv);
            if (service == null)
            {
                return NotFound();
            }
            return Ok(service);
        }
    }
}
