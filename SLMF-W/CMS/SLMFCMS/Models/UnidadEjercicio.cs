using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("UnidadEjercicio")]
    public class UnidadEjercicio
    {
        public UnidadEjercicio()
        {
            this.DiasConLaUnidad = new HashSet<PlanDiaEjercicios>();
        }

        [Key]
        [Display(Name = "Medición del Ejercicio")]
        public int ID { get; set; }

        [Display(Name = "Medición del Ejercicio")]
        [Required(ErrorMessage = "Debe ingresar el Nombre de la Unidad de Medición del Ejercicio")]
        [StringLength(15, ErrorMessage = "El Nombre de la Unidad de Medición del Ejercicio no debe exceder los 15 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Abreviatura")]
        [Required(ErrorMessage = "Debe ingresar la Abreviatura de la Unidad de Medición del Ejercicio")]
        [StringLength(5, ErrorMessage = "La Abreviatura de la Unidad de Medición del Ejercicio no debe exceder los 5 caracteres.")]
        public string Abreviacion { get; set; }

        [Display(Name = "Dias con la Unidad")]
        public virtual ICollection<PlanDiaEjercicios> DiasConLaUnidad { get; set; }
    }
}