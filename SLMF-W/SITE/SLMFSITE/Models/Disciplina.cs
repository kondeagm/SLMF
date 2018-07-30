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
    
    public partial class Disciplina
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Disciplina()
        {
            this.Cuestionario = new HashSet<Cuestionario>();
            this.Evento = new HashSet<Evento>();
            this.Plan = new HashSet<Plan>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Siglas { get; set; }
        public string Slogan { get; set; }
        public string Proposito { get; set; }
        public string FileImage { get; set; }
        public string IconImage { get; set; }
        public string LogoSVG { get; set; }
        public string SiglasImage { get; set; }
        public string ColorCode { get; set; }
        public string ImageEntrenar { get; set; }
        public bool Visible { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cuestionario> Cuestionario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Evento> Evento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Plan> Plan { get; set; }
    }
}