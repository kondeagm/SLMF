using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("DietaAlimentacion")]
    public class DietaAlimentacion
    {
        [Key]
        [Display(Name = "Alimentos de la Dieta")]
        public int ID { get; set; }

        [Display(Name = "Tempo")]
        [Required(ErrorMessage = "Debe seleccionar el Tempo de la Dieta en el que se consume el Alimento")]
        public int DietaTemposID { get; set; }

        [Display(Name = "Tempo")]
        public virtual DietaTempos DietaTempos { get; set; }

        [Display(Name = "Cantidad")]
        [StringLength(5, ErrorMessage = "La Cantidad no puede exceder los 5 caracteres.")]
        public string Cantidad { get; set; }

        [Display(Name = "Porcion")]
        public int? PorcionID { get; set; }

        [Display(Name = "Porcion")]
        public virtual Porcion PorcionDelAlimento { get; set; }

        [Display(Name = "Alimento")]
        [Required(ErrorMessage = "Debe seleccionar el Alimento")]
        public int AlimentoID { get; set; }

        [Display(Name = "Alimento")]
        public virtual Alimento Alimento { get; set; }
    }
}