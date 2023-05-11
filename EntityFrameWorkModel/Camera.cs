//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityFrameWorkModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Camera
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Camera()
        {
            this.Sightings = new HashSet<Sighting>();
        }
    
        public int CameraId { get; set; }
        public string RoadName { get; set; }
        public string RoadNumber { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public Nullable<int> AddressId { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual SpeedCamera SpeedCamera { get; set; }
        public virtual TrafficLightCamera TrafficLightCamera { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sighting> Sightings { get; set; }
    }
}
