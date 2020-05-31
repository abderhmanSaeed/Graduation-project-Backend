namespace GraduationProjectCSITINET.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Service
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Service()
        {
            ServiceWorkers = new HashSet<ServiceWorker>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TypeID { get; set; }

        public string ServicePicture { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceWorker> ServiceWorkers { get; set; }

        public virtual ServiceType ServiceType { get; set; }
    }
}
