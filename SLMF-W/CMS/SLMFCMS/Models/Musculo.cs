using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Musculo")]
    public class Musculo
    {
        public Musculo()
        {
            this.EjerciciosConElMusculo = new HashSet<Ejercicio>();
        }

        [Key]
        [Display(Name = "Musculo")]
        public int ID { get; set; }

        [Display(Name = "Musculo")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Musculo")]
        [StringLength(20, ErrorMessage = "El Nombre del Musculo no debe exceder los 20 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Nombre Comun")]
        [Required(ErrorMessage = "Debe ingresar el Nombre Comun del Musculo")]
        [StringLength(15, ErrorMessage = "El Nombre Comun del Musculo no debe exceder los 15 caracteres.")]
        public string NombreComun { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Debe ingresar la Descripción del Musculo")]
        [StringLength(190, ErrorMessage = "Solo tiene 190 caracteres, para escribir la Descripción del Musculo")]
        public string Descripcion { get; set; }

        [Display(Name = "Grupo de Musculos")]
        [Required(ErrorMessage = "Debe seleccionar el Grupo Muscular al que pertenece el Musculo")]
        public int GrupoMusculosID { get; set; }

        [Display(Name = "Grupo de Musculos")]
        public virtual GrupoMusculos GrupoMusculos { get; set; }

        [Display(Name = "Imagen")]
        [StringLength(250, ErrorMessage = "El ID de la Imagen solo puede medir 250 caracteres")]
        public string FileImage { get; set; }

        [Display(Name = "Ejercicios con el Musculo")]
        public virtual ICollection<Ejercicio> EjerciciosConElMusculo { get; set; }

        public bool Definido
        {
            get
            {
                bool bVisible = false;
                if (!String.IsNullOrEmpty(FileImage))
                {
                    bVisible = true;
                }
                return bVisible;
            }
        }

        public string CSSNoImagen
        {
            get
            {
                string sResponse = "";
                if (String.IsNullOrEmpty(FileImage))
                {
                    sResponse = "btn-notag";
                }
                return sResponse;
            }
        }
    }
}