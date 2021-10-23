namespace _5032_ASS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Booking")]
    public partial class Booking
    {
        public int Id { get; set; }

        [Required]
        public string Booking_Name { get; set; }

        public string Booking_Email { get; set; }

        [Required]
        public string Booking_Phone { get; set; }

        [Column(TypeName ="date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime Booking_Date { get; set; }

        public string Booking_Description { get; set; }

        public double? Booking_Rating { get; set; }

        [Required]
        public string User_Id { get; set; }

        public int Clinic_Id { get; set; }

        public virtual Clinic Clinic { get; set; }
    }
}
