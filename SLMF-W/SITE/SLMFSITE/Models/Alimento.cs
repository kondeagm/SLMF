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
    using System.Collections.Generic;
    
    public partial class Alimento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Alimento()
        {
            this.DietaAlimentacion = new HashSet<DietaAlimentacion>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int NutrienteID { get; set; }
        public bool Suplemento { get; set; }
        public bool Preparado { get; set; }
    
        public virtual Nutriente Nutriente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DietaAlimentacion> DietaAlimentacion { get; set; }
    }
}
