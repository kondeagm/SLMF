using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Evento")]
    public class Evento
    {
        [Key]
        [Display(Name = "Evento")]
        public int ID { get; set; }

        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "Debe ingresar el Titulo del Evento")]
        [StringLength(25, ErrorMessage = "El Titulo del Evento no debe exceder los 25 caracteres")]
        public string Titulo { get; set; }

        [Display(Name = "Evento")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Evento")]
        [StringLength(60, ErrorMessage = "El Nombre del Evento no debe exceder los 60 caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "Debe ingresar la Fecha del Evento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Debe ingresar el Lugar del Evento")]
        [StringLength(50, ErrorMessage = "El Lugar del Evento no debe exceder los 50 caracteres")]
        public string Lugar { get; set; }

        [Required(ErrorMessage = "Debe ingresar la Dirección del Evento")]
        [StringLength(150, ErrorMessage = "Solo tiene 150 caracteres, para escribir la Dirección del Evento")]
        public string Direccion { get; set; }

        [Display(Name = "Disciplina")]
        [Required(ErrorMessage = "Debe seleccionar la Disciplina a la que pertenece el Evento")]
        public int DisciplinaID { get; set; }

        [Display(Name = "Disciplina")]
        public virtual Disciplina Disciplina { get; set; }

        [Display(Name = "URL del Evento")]
        [StringLength(500, ErrorMessage = "La URL de información del Evento no debe exceder los 500 caracteres.")]
        public string URL { get; set; }

        [Display(Name = "Activo")]
        public bool Visible { get; set; }
    }
}