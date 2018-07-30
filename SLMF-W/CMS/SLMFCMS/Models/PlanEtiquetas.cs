using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("PlanEtiquetas")]
    public class PlanEtiquetas
    {
        [Key]
        [Display(Name = "Etiqueta del Plan")]
        public int ID { get; set; }

        [Display(Name = "Plan")]
        [Required(ErrorMessage = "Debe seleccionar el Plan al que pertenece esta Etiqueta")]
        public int PlanID { get; set; }

        [Display(Name = "Plan")]
        public virtual Plan Plan { get; set; }

        [Display(Name = "Etiqueta")]
        [Required(ErrorMessage = "Debe seleccionar la Etiqueta para el Plan")]
        public int RespuestaID { get; set; }

        [Display(Name = "Etiqueta")]
        public virtual Respuesta Respuesta { get; set; }
    }
}