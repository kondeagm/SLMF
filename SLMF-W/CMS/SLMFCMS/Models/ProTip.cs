using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("ProTip")]
    public class ProTip
    {
        public ProTip()
        {
            this.DiasConElProTip = new HashSet<PlanDias>();
        }

        [Key]
        [Display(Name = "ProTip")]
        public int ID { get; set; }

        [Display(Name = "ProTip")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del ProTip")]
        [StringLength(20, ErrorMessage = "El Nombre del ProTip no debe exceder los 20 caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar la Descripción del ProTip")]
        [StringLength(250, ErrorMessage = "Solo tiene 250 caracteres, para describir el ProTip")]
        public string Descripcion { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "Debe ingresar el Autor del ProTip")]
        [StringLength(40, ErrorMessage = "El Nombre del Autor del ProTip no debe exceder los 40 caracteres")]
        public string Autor { get; set; }

        [Display(Name = "ID de Vimeo")]
        [Required(ErrorMessage = "Debe ingresar el ID del video en Vimeo del ProTip")]
        [StringLength(50, ErrorMessage = "El ID del video en Vimeo del ProTip no debe exceder los 50 caracteres")]
        public string VimeoID { get; set; }

        [Display(Name = "Imagen")]
        [StringLength(250, ErrorMessage = "El Nombre de la Imagen de Proteccion del Video solo puede medir 250 caracteres")]
        public string FileImage { get; set; }

        [Display(Name = "Dias con el ProTip")]
        public virtual ICollection<PlanDias> DiasConElProTip { get; set; }

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