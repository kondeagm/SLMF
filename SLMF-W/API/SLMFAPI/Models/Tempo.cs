//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SLMFAPI.Models
{
    using System.Collections.Generic;
    
    public partial class Tempo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tempo()
        {
            this.DietaTempos = new HashSet<DietaTempos>();
        }
    
        public int ID { get; set; }
        public int Secuencia { get; set; }
        public string Prefijo { get; set; }
        public string Nombre { get; set; }
        public bool Complementario { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DietaTempos> DietaTempos { get; set; }
    }
}
