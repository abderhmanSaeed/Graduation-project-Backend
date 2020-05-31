namespace GraduationProjectCSITINET.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Comments = new HashSet<Comment>();
            Reservations = new HashSet<Reservation>();
            ServiceWorkers = new HashSet<ServiceWorker>();
        }

        public int ID { get; set; }

        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        public string ProfilePicture { get; set; }

        [Required]
        public string Password { get; set; }

        public int RoleID { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        public int? AddressID { get; set; }

        public virtual Adress Adress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }

        public virtual RoleType RoleType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceWorker> ServiceWorkers { get; set; }
    }
}
