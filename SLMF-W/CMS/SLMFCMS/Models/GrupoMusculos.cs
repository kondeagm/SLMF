using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("GrupoMusculos")]
    public class GrupoMusculos
    {
        public GrupoMusculos()
        {
            this.MusculosDelGrupo = new HashSet<Musculo>();
            this.RutinasDelGrupo = new HashSet<Rutina>();
        }

        [Key]
        [Display(Name = "Grupo de Musculos")]
        public int ID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Grupo")]
        [StringLength(25, ErrorMessage = "El Nombre del Grupo no debe exceder los 25 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Debe ingresar la Descripción del Grupo de Musculos")]
        [StringLength(190, ErrorMessage = "Solo tiene 190 caracteres, para escribir la Descripción del Grupo de Musculos")]
        public string Descripcion { get; set; }

        [Display(Name = "Imagen")]
        [StringLength(250, ErrorMessage = "El ID de la Imagen solo puede medir 250 caracteres")]
        public string FileImage { get; set; }

        [Display(Name = "Musculos del Grupo Muscular")]
        public virtual ICollection<Musculo> MusculosDelGrupo { get; set; }

        [Display(Name = "Rutinas del Grupo Muscular")]
        public virtual ICollection<Rutina> RutinasDelGrupo { get; set; }

        public int NoMusculos
        {
            get
            {
                int iNoMusculos = 0;
                iNoMusculos = RutinasDelGrupo.Count;
                return iNoMusculos;
            }
        }

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

        public string CSSNoData
        {
            get
            {
                string sResponse = "";
                if (MusculosDelGrupo.Count == 0)
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