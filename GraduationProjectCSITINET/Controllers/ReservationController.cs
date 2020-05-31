using GraduationProjectCSITINET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace GraduationProjectCSITINET.Controllers
{
    public class ReservationController : ApiController
    {
        private GP db = new GP();
        [HttpGet]
        [Authorize]
        [Route("api/reservation/getReservation")]
        public IHttpActionResult get()
        {
            var identity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            var id = Convert.ToInt32(claims.ToArray()[2].Value);
            //var user=db.Users.SingleOrDefault(ww=>ww.Email)
            var query = from user in db.Users
                        join reservation in db.Reservations
                        on user.ID equals reservation.UserID
                        join serviceworker in db.ServiceWorkers
                        on reservation.ServiceWorkerId equals serviceworker.ID
                        join service in db.Services on serviceworker.ServiceID equals service.ID
                        join serviceType in db.ServiceTypes on service.TypeID equals serviceType.ID
                        join comment in db.Comments on reservation.ID equals comment.ReservationID

                        where user.ID.ToString() == id.ToString()

                        select new
                        {
                            user.ID,
                            user.Username,
                            user.ProfilePicture,
                            user.RoleType.Rtype,
                            user.Adress.Adress1,
                            serviceworker.ServiceID,
                            service.Name,
                            serviceType = serviceType.Name,
                            reservation.Date,
                            serviceworker.WorkerID,
                            workername = serviceworker.User.Username,
                            comment.CommentBody,
                            comment.Rating
                        };
            return Ok(query);
        }
        //get reservation of specific worker and the status and the client who reserve the service and return also the service
        [HttpGet]
        [Authorize]
        [Route("api/reservation/getworkerReservation")]
        public IHttpActionResult getworkerReservation()
        {
            var identity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            var Workerid = Convert.ToInt32(claims.ToArray()[2].Value);
            var query = from serviceWorker in db.ServiceWorkers
                        join reservation in db.Reservations
                        on serviceWorker.ID equals reservation.ServiceWorkerId
                        join user in db.Users
                        on reservation.UserID equals user.ID
                        join service in db.Services
                        on serviceWorker.ServiceID  equals service.ID
                        where serviceWorker.WorkerID == Workerid && reservation.StatusID == 2
                        select new
                        {
                            reservation.ID,
                            reservation.ReservationStatu.Status,
                            user.Username,
                            service.Name,
                           servceID= service.ID,
                           serviceName= service.ServiceType.Name

                        };
            if (query == null)
            {
                return NotFound();
            }
            return Ok(query);


        }
        [HttpGet]
        [Route("api/reservation/getUser")]
        public IHttpActionResult getUserReservation()
        {
            var identity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            var id = Convert.ToInt32(claims.ToArray()[2].Value);
            var reservation = db.Reservations.Where(ww => ww.UserID == id).Select(ww => new { ww.ID, ww.ServiceWorkerId, ww.ReservationStatu.Status, ww.ServiceWorker.ServiceID }).ToList();
            return Ok(reservation);
        }



        [Route("api/reservation/addWokerReservation")]
        [HttpGet]
        [Authorize]
        public IHttpActionResult getUserReservation(int serviceid)
        {
            var identity = User.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            var id = Convert.ToInt32(claims.ToArray()[2].Value);

            var x = db.ServiceWorkers.Add(new ServiceWorker() { ServiceID = serviceid,WorkerID=id });
            db.SaveChanges();
            return Ok(x);


        }

    }
}

