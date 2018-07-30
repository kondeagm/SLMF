using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Nutriente")]
    public class Nutriente
    {
        public Nutriente()
        {
            this.AlimentosConElNutriente = new HashSet<Alimento>();
            this.ProductosConElNutriente = new HashSet<Producto>();
        }

        [Key]
        [Display(Name = "Nutriente")]
        public int ID { get; set; }

        [Display(Name = "Nutriente")]
        [Required(ErrorMessage = "Debe ingresar la Clasificación del Nutriente")]
        [StringLength(15, ErrorMessage = "La Clasificación del Nutriente no debe exceder los 15 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Codigo de Color")]
        [Required(ErrorMessage = "Debe ingresar el Codigo de Color para el Nutriente")]
        [StringLength(7, ErrorMessage = "El Codigo de color debe ser en este formato #000000", MinimumLength = 7)]
        public string ColorCode { get; set; }

        [Display(Name = "Alimentos con este Nutriente")]
        public virtual ICollection<Alimento> AlimentosConElNutriente { get; set; }

        [Display(Name = "Productos con este Nutriente")]
        public virtual ICollection<Producto> ProductosConElNutriente { get; set; }
    }
}