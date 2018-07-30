using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("PlanDiaEjercicioSeries")]
    public class PlanDiaEjercicioSeries
    {
        [Key]
        [Display(Name = "Series del Ejercicio")]
        public int ID { get; set; }

        [Display(Name = "Ejercicio del Día")]
        [Required(ErrorMessage = "Debe seleccionar el Ejercicio del Dia al que pertenecen las Series")]
        public int PlanDiaEjerciciosID { get; set; }

        [Display(Name = "Ejercicio del Día")]
        public virtual PlanDiaEjercicios PlanDiaEjercicios { get; set; }

        [Display(Name = "Serie")]
        [Required(ErrorMessage = "Debe indicar la secuencia de la Serie del Ejercicio en este Día")]
        public int Secuencia { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Debe ingresar la Cantidad de repeticiones por Serie a realizar")]
        [StringLength(8, ErrorMessage = "El Texto de la Cantidad de repeticiones por Serie no debe exceder los 8 caracteres.")]
        public string Repeticiones { get; set; }

        [Display(Name = "Peso")]
        [Required(ErrorMessage = "Debe ingresar el Nivel de Peso/Intensidad a utilizar en el Ejercicio")]
        [Range(0, 10, ErrorMessage = "El Nivel de Peso/Intensidad a utilizar en el Ejercicio no es Valido")]
        public int Nivel { get; set; }
    }
}