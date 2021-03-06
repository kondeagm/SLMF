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
    
    public partial class PlanDiaEjercicios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PlanDiaEjercicios()
        {
            this.PlanDiaEjercicioSeries = new HashSet<PlanDiaEjercicioSeries>();
        }
    
        public int ID { get; set; }
        public int PlanDiasID { get; set; }
        public int Secuencia { get; set; }
        public int EjercicioID { get; set; }
        public int Series { get; set; }
        public string Repeticiones { get; set; }
        public int UnidadEjercicioID { get; set; }
        public int Descanso { get; set; }
        public int Nivel { get; set; }
        public string Nota { get; set; }
    
        public virtual Ejercicio Ejercicio { get; set; }
        public virtual PlanDias PlanDias { get; set; }
        public virtual UnidadEjercicio UnidadEjercicio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanDiaEjercicioSeries> PlanDiaEjercicioSeries { get; set; }
    }
}
