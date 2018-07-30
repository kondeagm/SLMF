using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Posicion")]
    public class Posicion
    {
        public Posicion()
        {
            this.EjerciciosEnLaPosicion = new HashSet<Ejercicio>();
        }

        [Key]
        [Display(Name = "Posición del Ejercicio")]
        public int ID { get; set; }

        [Display(Name = "Posición")]
        [Required(ErrorMessage = "Debe ingresar el Nombre de la Posición del Ejercicio")]
        [StringLength(20, ErrorMessage = "El Nombre de la Posición del Ejercicio no debe exceder los 20 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Abreviatura")]
        [StringLength(5, ErrorMessage = "La Abreviatura de la Posición del Ejercicio no debe exceder los 5 caracteres.")]
        public string Abreviacion { get; set; }

        [Display(Name = "Ejercicios en la Posición")]
        public virtual ICollection<Ejercicio> EjerciciosEnLaPosicion { get; set; }
    }
}