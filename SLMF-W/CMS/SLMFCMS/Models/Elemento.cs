using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Elemento")]
    public class Elemento
    {
        public Elemento()
        {
            this.EjerciciosConElElemento = new HashSet<Ejercicio>();
        }

        [Key]
        [Display(Name = "Elemento del Ejercicio")]
        public int ID { get; set; }

        [Display(Name = "Elemento")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Elemento del Ejercicio")]
        [StringLength(22, ErrorMessage = "El Nombre del Elemento del Ejercicio no debe exceder los 22 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Abreviatura")]
        [Required(ErrorMessage = "Debe ingresar la Abreviatura del Nombre del Elemento del Ejercicio")]
        [StringLength(5, ErrorMessage = "La Abreviatura del Nombre del Elemento del Ejercicio no debe exceder los 5 caracteres.")]
        public string Abreviacion { get; set; }

        [Display(Name = "Ejercicios con el Elemento")]
        public virtual ICollection<Ejercicio> EjerciciosConElElemento { get; set; }
    }
}