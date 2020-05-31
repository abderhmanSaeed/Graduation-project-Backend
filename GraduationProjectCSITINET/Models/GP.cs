namespace GraduationProjectCSITINET.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GP : DbContext
    {
        public GP()
            : base("name=GP")
        {
        }

        public virtual DbSet<Adress> Adresses { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationStatu> ReservationStatus { get; set; }
        public virtual DbSet<RoleType> RoleTypes { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<ServiceWorker> ServiceWorkers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adress>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Adress)
                .HasForeignKey(e => e.AddressID);

            modelBuilder.Entity<Reservation>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Reservation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ReservationStatu>()
                .HasMany(e => e.Reservations)
                .WithRequired(e => e.ReservationStatu)
                .HasForeignKey(e => e.StatusID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoleType>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.RoleType)
                .HasForeignKey(e => e.RoleID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Service>()
                .HasMany(e => e.ServiceWorkers)
                .WithRequired(e => e.Service)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceType>()
                .HasMany(e => e.Services)
                .WithRequired(e => e.ServiceType)
                .HasForeignKey(e => e.TypeID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceWorker>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.ServiceWorker)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ServiceWorker>()
                .HasMany(e => e.Reservations)
                .WithRequired(e => e.ServiceWorker)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Reservations)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ServiceWorkers)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.WorkerID)
                .WillCascadeOnDelete(false);
        }
    }
}
