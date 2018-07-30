using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Accesorio")]
    public class Accesorio
    {
        public Accesorio()
        {
            this.EjerciciosConElAccesorio = new HashSet<Ejercicio>();
        }

        [Key]
        [Display(Name = "Accesorio del Ejercicio")]
        public int ID { get; set; }

        [Display(Name = "Accesorio")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Accesorio del Ejercicio")]
        [StringLength(20, ErrorMessage = "El Nombre del Accesorio del Ejercicio no debe exceder los 20 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Abreviatura")]
        [Required(ErrorMessage = "Debe ingresar la Abreviatura del Nombre del Accesorio del Ejercicio")]
        [StringLength(5, ErrorMessage = "La Abreviatura del Nombre del Accesorio del Ejercicio no debe exceder los 5 caracteres.")]
        public string Abreviacion { get; set; }

        [Display(Name = "Ejercicios con el Accesorio")]
        public virtual ICollection<Ejercicio> EjerciciosConElAccesorio { get; set; }
    }
}