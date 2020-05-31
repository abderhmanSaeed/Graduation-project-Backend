namespace GraduationProjectCSITINET.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ServiceWorker")]
    public partial class ServiceWorker
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServiceWorker()
        {
            Comments = new HashSet<Comment>();
            Reservations = new HashSet<Reservation>();
        }

        public int ID { get; set; }

        public int WorkerID { get; set; }

        public int ServiceID { get; set; }

        public int? Rating { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }

        public virtual Service Service { get; set; }

        public virtual User User { get; set; }
    }
}
