using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SLMFCMS.Models
{
    [Table("Respuesta")]
    public class Respuesta
    {
        public Respuesta()
        {
            this.PlanesDeLaEtiqueta = new HashSet<PlanEtiquetas>();
        }

        [Key]
        [Display(Name = "Respuesta")]
        public int ID { get; set; }

        [Display(Name = "Pregunta")]
        [Required(ErrorMessage = "Debe seleccionar la Pregunta a la que pertenece esta Respuesta")]
        public int PreguntaID { get; set; }

        [Display(Name = "Pregunta")]
        public virtual Pregunta Pregunta { get; set; }

        [Display(Name = "Respuesta")]
        [Required(ErrorMessage = "Debe ingresar el Texto de la Respuesta")]
        [StringLength(15, ErrorMessage = "El Texto de la Respuesta no debe exceder los 15 caracteres")]
        public string Texto { get; set; }

        [Display(Name = "Clase CSS")]
        [StringLength(20, ErrorMessage = "El Texto de la Clase no debe exceder los 20 caracteres")]
        public string Clase { get; set; }

        [Display(Name = "Filtrable")]
        [Required(ErrorMessage = "Debe indicar si esta respuesta crea un Filtro en los Planes")]
        public bool Filtro { get; set; }

        [AllowHtml]
        [Display(Name = "Logotipo")]
        public string LogoSVG { get; set; }

        [Display(Name = "Planes con esta Etiqueta")]
        public virtual ICollection<PlanEtiquetas> PlanesDeLaEtiqueta { get; set; }
    }
}