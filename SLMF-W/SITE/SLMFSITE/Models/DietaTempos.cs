//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SLMFSITE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DietaTempos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DietaTempos()
        {
            this.DietaAlimentacion = new HashSet<DietaAlimentacion>();
        }
    
        public int ID { get; set; }
        public int DietaID { get; set; }
        public int TempoID { get; set; }
        public Nullable<int> Hora { get; set; }
    
        public virtual Dieta Dieta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DietaAlimentacion> DietaAlimentacion { get; set; }
        public virtual Tempo Tempo { get; set; }
    }
}
