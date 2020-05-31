namespace GraduationProjectCSITINET.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int ServiceWorkerId { get; set; }

        public int ReservationID { get; set; }

        public string CommentBody { get; set; }

        public int? Rating { get; set; }

        public DateTime? Date { get; set; }

        public virtual Reservation Reservation { get; set; }

        public virtual ServiceWorker ServiceWorker { get; set; }

        public virtual User User { get; set; }
    }
}
