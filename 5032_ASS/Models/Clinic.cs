namespace _5032_ASS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Clinic")]
    public partial class Clinic
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clinic()
        {
            Bookings = new HashSet<Booking>();
        }

        public int Id { get; set; }

        [Required]
        public string Clinic_Name { get; set; }

        [Required]
        public string Clinic_Phone { get; set; }

        [Required]
        public string Clinic_Lat { get; set; }

        [Required]
        public string Clinic_Lng { get; set; }

        public double? Clinic_Rating { get; set; }

        public string Clinic_Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
