using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Porcion")]
    public class Porcion
    {
        public Porcion()
        {
            this.DietasConLaPorcion = new HashSet<DietaAlimentacion>();
        }

        [Key]
        [Display(Name = "Porción")]
        public int ID { get; set; }

        [Display(Name = "Porción")]
        [Required(ErrorMessage = "Debe ingresar la Descripción de la Porción")]
        [StringLength(15, ErrorMessage = "La Descripción de la Porción no debe exceder los 15 caracteres.")]
        public string Descripcion { get; set; }

        [Display(Name = "Abreviatura")]
        [Required(ErrorMessage = "Debe ingresar la Abreviatura de la Porción")]
        [StringLength(5, ErrorMessage = "La Abreviatura de la Porción no debe exceder los 5 caracteres.")]
        public string Abreviacion { get; set; }

        [Display(Name = "Dietas con esta Porcion")]
        public virtual ICollection<DietaAlimentacion> DietasConLaPorcion { get; set; }
    }
}