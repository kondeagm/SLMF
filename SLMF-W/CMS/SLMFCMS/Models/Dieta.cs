using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Dieta")]
    public class Dieta
    {
        public Dieta()
        {
            this.TemposDeLaDieta = new HashSet<DietaTempos>();
            this.DiasConLaDieta = new HashSet<PlanDias>();
        }

        [Key]
        [Display(Name = "Dieta")]
        public int ID { get; set; }

        [Display(Name = "Dieta")]
        [Required(ErrorMessage = "Debe ingresar el Nombre de la Dieta")]
        [StringLength(25, ErrorMessage = "El Nombre de la Dieta no debe exceder los 25 caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "Debe ingresar la Descripción de la Dieta")]
        [StringLength(500, ErrorMessage = "Solo tiene 500 caracteres, para redactar la Descripción de la Dieta")]
        public string Descripcion { get; set; }

        [Display(Name = "Imagen")]
        [StringLength(250, ErrorMessage = "El ID de la Imagen solo puede medir 250 caracteres")]
        public string FileImage { get; set; }

        [Display(Name = "Comidas de la Dieta")]
        public virtual ICollection<DietaTempos> TemposDeLaDieta { get; set; }

        [Display(Name = "Dias con la Dieta")]
        public virtual ICollection<PlanDias> DiasConLaDieta { get; set; }

        public int NoComidas
        {
            get
            {
                int iNoComidas = 0;
                iNoComidas = TemposDeLaDieta.Count;
                return iNoComidas;
            }
        }

        public bool Definido
        {
            get
            {
                bool bVisible = false;
                if (!String.IsNullOrEmpty(FileImage))
                {
                    if (TemposDeLaDieta.Count > 0)
                    {
                        bVisible = true;
                    }
                }
                return bVisible;
            }
        }

        public string CSSNoData
        {
            get
            {
                string sResponse = "";
                if (TemposDeLaDieta.Count == 0)
                {
                    sResponse = "btn-notag";
                }
                return sResponse;
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