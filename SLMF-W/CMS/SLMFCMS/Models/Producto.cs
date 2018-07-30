using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        [Display(Name = "Producto")]
        public int ID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Producto")]
        [StringLength(20, ErrorMessage = "El Nombre del Producto no debe exceder los 20 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Imagen")]
        [StringLength(250, ErrorMessage = "El ID de la Imagen solo puede medir 250 caracteres")]
        public string FileImage { get; set; }

        [Display(Name = "URL de la Tienda")]
        [StringLength(500, ErrorMessage = "La URL de la Tienda no debe exceder los 500 caracteres.")]
        public string URL { get; set; }

        [Display(Name = "Nutriente")]
        [Required(ErrorMessage = "Debe seleccionar el Nutriente principal que contiene el Producto")]
        public int NutrienteID { get; set; }

        [Display(Name = "Nutriente")]
        public virtual Nutriente Nutriente { get; set; }

        [Display(Name = "Visible")]
        public bool Visible
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