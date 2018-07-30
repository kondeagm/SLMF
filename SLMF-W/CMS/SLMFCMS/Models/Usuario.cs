using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Display(Name = "Usuario")]
        public int ID { get; set; }

        [Display(Name = "FacebookID")]
        [Required(ErrorMessage = "Debe ingresar el ID de Facebook del Usuario")]
        [StringLength(50, ErrorMessage = "El Nombre del Usuario no debe exceder los 50 caracteres.")]
        public string FacebookID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el Nombre de Facebook del Usuario")]
        [StringLength(100, ErrorMessage = "El Nombre de Facebook del Usuario no debe exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Apellidos")]
        [StringLength(80, ErrorMessage = "Los Apellidos del Usuario no debe exceder los 80 caracteres.")]
        public string Apellidos { get; set; }

        [Display(Name = "Correo")]
        [StringLength(500, ErrorMessage = "El Correo del Usuario no debe exceder los 500 caracteres.")]
        public string Correo { get; set; }

        [Display(Name = "Plan")]
        public int? PlanID { get; set; }

        [Display(Name = "Plan")]
        public virtual Plan PlanDelUsuario { get; set; }

        [Display(Name = "Registro")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime FechaRegistro { get; set; }

        [Display(Name = "Inicio del Plan")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> FechaInicioPlan { get; set; }
    }
}