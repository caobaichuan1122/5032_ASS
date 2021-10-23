using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace _5032_ASS.Models
{
    public partial class BookModel : DbContext
    {
        public BookModel()
            : base("name=BookModel")
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Clinic> Clinics { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clinic>()
                .HasMany(e => e.Bookings)
                .WithRequired(e => e.Clinic)
                .HasForeignKey(e => e.Clinic_Id)
                .WillCascadeOnDelete(false);
        }
    }
}
