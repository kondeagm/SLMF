using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Banner")]
    public class Banner
    {
        [Key]
        [Display(Name = "Banner")]
        public int ID { get; set; }

        [Display(Name = "Identificador")]
        [Required(ErrorMessage = "Debe ingresar el Identificador del Banner")]
        [StringLength(60, ErrorMessage = "El Identificador del Banner no debe exceder los 60 caracteres.")]
        public string Identificador { get; set; }

        [Display(Name = "Imagen")]
        [StringLength(250, ErrorMessage = "El Nombre del Archivo de la Imagen solo puede medir 250 caracteres")]
        public string FileImage { get; set; }

        [Display(Name = "Link del Banner")]
        [Required(ErrorMessage = "Debe ingresar el Link del Banner")]
        [StringLength(500, ErrorMessage = "El Link del Banner no debe exceder los 500 caracteres.")]
        public string LinkBanner { get; set; }

        [Display(Name = "Prioridad")]
        [Required(ErrorMessage = "Debe indicar la Prioridad del Banner")]
        public int Prioridad { get; set; }

        [Display(Name = "Activo")]
        [Required(ErrorMessage = "Debe indicar si el Banner esta Activo")]
        public bool Visible { get; set; }

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

        public string CSSSinImage
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