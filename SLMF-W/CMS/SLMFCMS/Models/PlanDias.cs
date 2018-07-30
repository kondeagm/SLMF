using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("PlanDias")]
    public class PlanDias
    {
        public PlanDias()
        {
            this.EjerciciosDelDia = new HashSet<PlanDiaEjercicios>();
        }

        [Key]
        [Display(Name = "Días del Plan")]
        public int ID { get; set; }

        [Display(Name = "Plan")]
        [Required(ErrorMessage = "Debe seleccionar el Plan al que pertenece el Detalle")]
        public int PlanID { get; set; }

        [Display(Name = "Plan")]
        public virtual Plan Plan { get; set; }

        [Display(Name = "Dia")]
        [Required(ErrorMessage = "Debe ingresar el Número del Día en el Plan")]
        [Range(1, 120, ErrorMessage = "El Número del Día no es Valido")]
        public int Dia { get; set; }

        [Display(Name = "Rutina")]
        [Required(ErrorMessage = "Debe seleccionar la Rutina a ejecutar este Día")]
        public int RutinaID { get; set; }

        [Display(Name = "Rutina")]
        public virtual Rutina Rutina { get; set; }

        [Display(Name = "Dieta")]
        [Required(ErrorMessage = "Debe seleccionar la Dieta a comer este Día")]
        public int DietaID { get; set; }

        [Display(Name = "Dieta")]
        public virtual Dieta Dieta { get; set; }

        [Display(Name = "Pro Tip")]
        [Required(ErrorMessage = "Debe seleccionar el ProTip del Día")]
        public int ProTipID { get; set; }

        [Display(Name = "Pro Tip")]
        public virtual ProTip ProTip { get; set; }

        [Display(Name = "Descanso")]
        [Required(ErrorMessage = "Debe indicar si es Día de Descanso")]
        public bool Descanso { get; set; }

        [Display(Name = "Ejercicios del Dia")]
        public virtual ICollection<PlanDiaEjercicios> EjerciciosDelDia { get; set; }

        [Display(Name = " No. Ejercicios")]
        public int NoEjercicios
        {
            get
            {
                int iNoEjercicios = 0;
                if (EjerciciosDelDia.Count > 0)
                {
                    iNoEjercicios = EjerciciosDelDia.Count;
                }
                return iNoEjercicios;
            }
        }

        public string CSSSinEjercicios
        {
            get
            {
                string sResponse = "";
                if (EjerciciosDelDia.Count == 0)
                {
                    sResponse = "btn-notag";
                }
                return sResponse;
            }
        }
    }
}