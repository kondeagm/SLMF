using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Pregunta")]
    public class Pregunta
    {
        public Pregunta()
        {
            this.RespuestasDeLaPregunta = new HashSet<Respuesta>();
        }

        [Key]
        [Display(Name = "Pregunta")]
        public int ID { get; set; }

        [Display(Name = "Cuestionario")]
        [Required(ErrorMessage = "Debe seleccionar el Cuestionario al que pertenece esta Pregunta")]
        public int CuestionarioID { get; set; }

        [Display(Name = "Cuestionario")]
        public virtual Cuestionario Cuestionario { get; set; }

        [Display(Name = "Pregunta")]
        [Required(ErrorMessage = "Debe ingresar el Texto de la Pregunta")]
        [StringLength(10, ErrorMessage = "El Texto de la Pregunta no debe exceder los 10 caracteres")]
        public string Texto { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Debe ingresar la Descripción de la Pregunta")]
        [StringLength(20, ErrorMessage = "La Descripción de la Pregunta no debe exceder los 20 caracteres")]
        public string Descripcion { get; set; }

        [Display(Name = "Clase CSS")]
        [Required(ErrorMessage = "Debe ingresar el Id de la Clase CSS que define a la Pregunta")]
        [StringLength(20, ErrorMessage = "El Id de la Clase CSS no debe exceder los 20 caracteres")]
        public string Clase { get; set; }

        [Display(Name = "Respuestas de la Pregunta")]
        public virtual ICollection<Respuesta> RespuestasDeLaPregunta { get; set; }

        public int NoRespuestas
        {
            get
            {
                int iNoRespuestas = 0;
                iNoRespuestas = RespuestasDeLaPregunta.Count;
                return iNoRespuestas;
            }
        }

        public bool Definido
        {
            get
            {
                bool bVisible = false;
                if (RespuestasDeLaPregunta.Count > 0)
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
                if (RespuestasDeLaPregunta.Count == 0)
                {
                    sResponse = "btn-notag";
                }
                return sResponse;
            }
        }
    }
}