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
    
    public partial class Musculo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Musculo()
        {
            this.Ejercicio = new HashSet<Ejercicio>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string NombreComun { get; set; }
        public string Descripcion { get; set; }
        public int GrupoMusculosID { get; set; }
        public string FileImage { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ejercicio> Ejercicio { get; set; }
        public virtual GrupoMusculos GrupoMusculos { get; set; }
    }
}
