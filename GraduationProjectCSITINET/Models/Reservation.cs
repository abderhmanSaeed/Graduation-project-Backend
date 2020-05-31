namespace GraduationProjectCSITINET.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Reservation")]
    public partial class Reservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reservation()
        {
            Comments = new HashSet<Comment>();
        }

        public int ID { get; set; }

        public int UserID { get; set; }

        public int ServiceWorkerId { get; set; }

        public DateTime Date { get; set; }

        public int StatusID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ReservationStatu ReservationStatu { get; set; }

        public virtual ServiceWorker ServiceWorker { get; set; }

        public virtual User User { get; set; }
    }
}
