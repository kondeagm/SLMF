using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("DietaTempos")]
    public class DietaTempos
    {
        public DietaTempos()
        {
            this.AlimentosDelTempo = new HashSet<DietaAlimentacion>();
        }

        [Key]
        [Display(Name = "Tempo de la Dieta")]
        public int ID { get; set; }

        [Display(Name = "Dieta")]
        [Required(ErrorMessage = "Debe seleccionar la Dieta a la que pertenece la Alimentación")]
        public int DietaID { get; set; }

        [Display(Name = "Dieta")]
        public virtual Dieta Dieta { get; set; }

        [Display(Name = "Tempo")]
        [Required(ErrorMessage = "Debe seleccionar el Tempo de Alimentación")]
        public int TempoID { get; set; }

        [Display(Name = "Tempo")]
        public virtual Tempo Tempo { get; set; }

        [Display(Name = "Hora")]
        [Range(1, 24, ErrorMessage = "El valor de la Hora seleccionada debe ser entre 1 y 24")]
        public int? Hora { get; set; }

        [Display(Name = "Alimentos de la Comida en la Dieta")]
        public virtual ICollection<DietaAlimentacion> AlimentosDelTempo { get; set; }

        public int NoAlimentos
        {
            get
            {
                int iNoAlimentos = 0;
                if (AlimentosDelTempo.Count > 0)
                {
                    iNoAlimentos = AlimentosDelTempo.Count;
                }
                return iNoAlimentos;
            }
        }

        public bool Definido
        {
            get
            {
                bool bVisible = false;
                if (AlimentosDelTempo.Count > 0)
                {
                    bVisible = true;
                }
                return bVisible;
            }
        }

        public string CSSNoData
        {
            get
            {
                string sResponse = "";
                if (AlimentosDelTempo.Count == 0)
                {
                    sResponse = "btn-notag";
                }
                return sResponse;
            }
        }
    }
}