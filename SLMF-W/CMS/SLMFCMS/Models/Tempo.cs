using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Tempo")]
    public class Tempo
    {
        public Tempo()
        {
            this.DietasConElTempo = new HashSet<DietaTempos>();
        }

        [Key]
        [Display(Name = "Tempo")]
        public int ID { get; set; }

        [Display(Name = "Secuencia")]
        [Required(ErrorMessage = "Debe indicar la secuencia del Tempo durante el Día")]
        public int Secuencia { get; set; }

        [Display(Name = "Prefijo")]
        [StringLength(6, ErrorMessage = "El Prefijo del Tempo no debe exceder los 6 caracteres.")]
        public string Prefijo { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Tempo")]
        [StringLength(20, ErrorMessage = "El Nombre del Tempo no debe exceder los 20 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Complementario")]
        [Required(ErrorMessage = "Debe indicar si el Tempo es de una alimentación Complementaria")]
        public bool Complementario { get; set; }

        [Display(Name = "Tempos de la Dieta")]
        public virtual ICollection<DietaTempos> DietasConElTempo { get; set; }

        [Display(Name = "Nombre Completo")]
        public string NombreCompleto
        {
            get
            {
                string sNombre = "";
                if (String.IsNullOrEmpty(Prefijo))
                {
                    sNombre = Nombre;
                }
                else
                {
                    sNombre = Prefijo + " " + Nombre;
                }
                return sNombre;
            }
        }
    }
}