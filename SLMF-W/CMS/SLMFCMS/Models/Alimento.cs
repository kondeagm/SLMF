using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Alimento")]
    public class Alimento
    {
        public Alimento()
        {
            this.DietasConElAlimento = new HashSet<DietaAlimentacion>();
        }

        [Key]
        [Display(Name = "Alimento")]
        public int ID { get; set; }

        [Display(Name = "Alimento")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Alimento")]
        [StringLength(60, ErrorMessage = "El Nombre del Alimento no debe exceder los 60 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Nutriente")]
        [Required(ErrorMessage = "Debe seleccionar el Nutriente principal que contiene el Alimento")]
        public int NutrienteID { get; set; }

        [Display(Name = "Nutriente")]
        public virtual Nutriente Nutriente { get; set; }

        [Display(Name = "Suplemento")]
        [Required(ErrorMessage = "Debe indicar si el Alimento es un Sumplemento")]
        public bool Suplemento { get; set; }

        [Display(Name = "Preparado")]
        [Required(ErrorMessage = "Debe indicar si el Alimento es un Platillo")]
        public bool Preparado { get; set; }

        [Display(Name = "Dietas con este Alimento")]
        public virtual ICollection<DietaAlimentacion> DietasConElAlimento { get; set; }
    }
}