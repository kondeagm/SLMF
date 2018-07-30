using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("PlanDiaEjercicios")]
    public class PlanDiaEjercicios
    {
        public PlanDiaEjercicios()
        {
            this.SeriesDelEjercicio = new HashSet<PlanDiaEjercicioSeries>();
        }

        [Key]
        [Display(Name = "Ejercicio del Día")]
        public int ID { get; set; }

        [Display(Name = "Dia")]
        [Required(ErrorMessage = "Debe seleccionar el Dia del Plan al que Pertenece el Ejercicio")]
        public int PlanDiasID { get; set; }

        [Display(Name = "Dia")]
        public virtual PlanDias PlanDias { get; set; }

        [Display(Name = "Secuencia")]
        [Required(ErrorMessage = "Debe indicar la secuencia del Ejercicio en este Día")]
        public int Secuencia { get; set; }

        [Display(Name = "Ejercicio")]
        [Required(ErrorMessage = "Debe seleccionar el Ejercicio a realizar en este Día")]
        public int EjercicioID { get; set; }

        [Display(Name = "Ejercicio")]
        public virtual Ejercicio Ejercicio { get; set; }

        [Display(Name = "Series")]
        [Required(ErrorMessage = "Debe ingresar el número de Series a realizar")]
        [Range(1, 10, ErrorMessage = "El número de Series es entre 1 y 10")]
        public int Series { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "Debe ingresar la Cantidad de repeticiones por Serie a realizar")]
        [StringLength(8, ErrorMessage = "El Texto de la Cantidad de repeticiones por Serie no debe exceder los 8 caracteres.")]
        public string Repeticiones { get; set; }

        [Display(Name = "Unidad")]
        [Required(ErrorMessage = "Debe seleccionar la Unidad en que se mide el Ejercicio")]
        public int UnidadEjercicioID { get; set; }

        [Display(Name = "Unidad")]
        public virtual UnidadEjercicio UnidadEjercicio { get; set; }

        [Display(Name = "Descanso")]
        [Required(ErrorMessage = "Debe ingresar los Minutos de descanso entre Series a realizar")]
        [Range(0, 5, ErrorMessage = "El número de Minutos de descanso entre Series a realizar no es Valido")]
        public int Descanso { get; set; }

        [Display(Name = "Peso")]
        [Required(ErrorMessage = "Debe ingresar el Nivel de Peso/Intensidad a utilizar en el Ejercicio")]
        [Range(0, 10, ErrorMessage = "El Nivel de Peso/Intensidad a utilizar en el Ejercicio no es Valido")]
        public int Nivel { get; set; }

        [Display(Name = "Nota")]
        [StringLength(30, ErrorMessage = "La Nota del Ejercicio no debe exceder los 30 caracteres")]
        public string Nota { get; set; }

        [Display(Name = "Series del Ejercicio")]
        public virtual ICollection<PlanDiaEjercicioSeries> SeriesDelEjercicio { get; set; }
    }
}