using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Rutina")]
    public class Rutina
    {
        public Rutina()
        {
            this.DiasConLaRutina = new HashSet<PlanDias>();
        }

        [Key]
        [Display(Name = "Rutina")]
        public int ID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el Nombre de la Rutina")]
        [StringLength(30, ErrorMessage = "El Nombre de la Rutina no debe exceder los 30 caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar la Descripción de la Rutina")]
        [StringLength(190, ErrorMessage = "Solo tiene 190 caracteres, para escribir la Descripción de la Rutina")]
        public string Descripcion { get; set; }

        [Display(Name = "Grupo Muscular")]
        [Required(ErrorMessage = "Debe seleccionar el Grupo Muscular que trabaja la Rutina")]
        public int GrupoMusculosID { get; set; }

        [Display(Name = "Grupo Muscular")]
        public virtual GrupoMusculos GrupoMusculos { get; set; }

        [Display(Name = "Inactividad")]
        public bool Inactividad { get; set; }

        [Display(Name = "Dias con la Rutina")]
        public virtual ICollection<PlanDias> DiasConLaRutina { get; set; }
    }
}