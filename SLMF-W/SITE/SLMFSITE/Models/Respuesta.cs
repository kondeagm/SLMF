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
    
    public partial class Respuesta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Respuesta()
        {
            this.PlanEtiquetas = new HashSet<PlanEtiquetas>();
        }
    
        public int ID { get; set; }
        public int PreguntaID { get; set; }
        public string Texto { get; set; }
        public string Clase { get; set; }
        public bool Filtro { get; set; }
        public string LogoSVG { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanEtiquetas> PlanEtiquetas { get; set; }
        public virtual Pregunta Pregunta { get; set; }
    }
}
