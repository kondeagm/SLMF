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

    public partial class Usuario
    {
        public int ID { get; set; }
        public string FacebookID { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public Nullable<int> PlanID { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public Nullable<System.DateTime> FechaInicioPlan { get; set; }
    
        public virtual Plan Plan { get; set; }
    }
}
