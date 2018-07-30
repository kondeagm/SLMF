using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Cuestionario")]
    public class Cuestionario
    {
        public Cuestionario()
        {
            this.PreguntasDelCuestionario = new HashSet<Pregunta>();
        }

        [Key]
        [Display(Name = "Cuestionario")]
        public int ID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Cuestionario")]
        [StringLength(15, ErrorMessage = "El Nombre del Cuestionario no debe exceder los 15 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Disciplina")]
        [Required(ErrorMessage = "Debe seleccionar la Disciplina a la que pertenece este Cuestionario")]
        public int DisciplinaID { get; set; }

        [Display(Name = "Disciplina")]
        public virtual Disciplina Disciplina { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Debe indicar si el Cuestionario es Visible")]
        public bool Visible { get; set; }

        [Display(Name = "Preguntas del Cuestionario")]
        public virtual ICollection<Pregunta> PreguntasDelCuestionario { get; set; }

        public int NoPreguntas
        {
            get
            {
                int iNoPreguntas = 0;
                iNoPreguntas = PreguntasDelCuestionario.Count;
                return iNoPreguntas;
            }
        }

        public bool Definido
        {
            get
            {
                bool bVisible = false;
                if (PreguntasDelCuestionario.Count > 0)
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
                if (PreguntasDelCuestionario.Count == 0)
                {
                    sResponse = "btn-notag";
                }
                return sResponse;
            }
        }
    }
}