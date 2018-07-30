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
    using System;
    using System.Collections.Generic;
    
    public partial class Ejercicio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ejercicio()
        {
            this.PlanDiaEjercicios = new HashSet<PlanDiaEjercicios>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> AccesorioID { get; set; }
        public Nullable<int> ElementoID { get; set; }
        public Nullable<int> PosicionID { get; set; }
        public int MusculoID { get; set; }
        public string VimeoID { get; set; }
        public string FileImage { get; set; }
    
        public virtual Accesorio Accesorio { get; set; }
        public virtual Elemento Elemento { get; set; }
        public virtual Musculo Musculo { get; set; }
        public virtual Posicion Posicion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanDiaEjercicios> PlanDiaEjercicios { get; set; }
    }
}
