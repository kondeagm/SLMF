using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace SLMFCMS.Models
{
    [Table("Disciplina")]
    public class Disciplina
    {
        public Disciplina()
        {
            this.CuestionarioDeLaDisciplina = new HashSet<Cuestionario>();
            this.EventosDeLaDisciplina = new HashSet<Evento>();
            this.PlanesDeLaDisciplina = new HashSet<Plan>();
        }

        [Key]
        [Display(Name = "Disciplina")]
        public int ID { get; set; }

        [Display(Name = "Disciplina")]
        [RegularExpression(@"^([a-zA-Z0-9 &'-]+)$", ErrorMessage = "* El Nombre solo acepta números y letras")]
        [Required(ErrorMessage = "* Debe ingresar el Nombre de la Disciplina")]
        [StringLength(18, ErrorMessage = "* El Nombre de la Disciplina no debe exceder los 18 caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Siglas")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "* Las Siglas solo aceptan letras")]
        [Required(ErrorMessage = "* Debe ingresar las Siglas de la Disciplina")]
        [StringLength(3, ErrorMessage = "* Las Siglas de la Disciplina no debe exceder los 3 caracteres")]
        public string Siglas { get; set; }

        [Display(Name = "Slogan")]
        [Required(ErrorMessage = "* Debe ingresar el Slogan de la Disciplina")]
        [StringLength(45, ErrorMessage = "* El Slogan de la Disciplina no debe exceder los 45 caracteres")]
        public string Slogan { get; set; }

        [Display(Name = "Proposito")]
        [Required(ErrorMessage = "* Debe ingresar el Proposito de la Disciplina")]
        [StringLength(100, ErrorMessage = "* Solo tiene 100 caracteres, para describir el Proposito de la Disciplina.")]
        public string Proposito { get; set; }

        [Display(Name = "Imagen")]
        [StringLength(250, ErrorMessage = "* El ID de la Imagen solo puede medir 250 caracteres")]
        public string FileImage { get; set; }

        [Display(Name = "Icono")]
        [StringLength(250, ErrorMessage = "* El ID del Icono solo puede medir 250 caracteres")]
        public string IconImage { get; set; }

        [AllowHtml]
        [Display(Name = "Logotipo")]
        public string LogoSVG { get; set; }

        [Display(Name = "Letras")]
        [StringLength(250, ErrorMessage = "* El ID de la Imagen de las Siglas solo puede medir 250 caracteres")]
        public string SiglasImage { get; set; }

        [Display(Name = "Codigo de Color")]
        [Required(ErrorMessage = "* Debe ingresar el Codigo de Color para la Disciplina")]
        [StringLength(7, ErrorMessage = "* El Codigo de color debe ser en este formato #000000", MinimumLength = 7)]
        public string ColorCode { get; set; }

        [Display(Name = "Portada Entrenar")]
        [StringLength(250, ErrorMessage = "* El ID de la Imagen solo puede medir 250 caracteres")]
        public string ImageEntrenar { get; set; }

        [Display(Name = "Visible")]
        [Required(ErrorMessage = "* Debe indicar si la Disciplina es Visible")]
        public bool Visible { get; set; }

        [Display(Name = "Cuestionario de la Disciplina")]
        public virtual ICollection<Cuestionario> CuestionarioDeLaDisciplina { get; set; }

        [Display(Name = "Eventos de la Disciplina")]
        public virtual ICollection<Evento> EventosDeLaDisciplina { get; set; }

        [Display(Name = "Planes de la Disciplina")]
        public virtual ICollection<Plan> PlanesDeLaDisciplina { get; set; }

        public bool Definido
        {
            get
            {
                bool bVisible = false;
                if (!String.IsNullOrEmpty(FileImage) && !String.IsNullOrEmpty(IconImage) && !String.IsNullOrEmpty(LogoSVG) && !String.IsNullOrEmpty(SiglasImage) && !String.IsNullOrEmpty(ImageEntrenar))
                {
                    if (CuestionarioDeLaDisciplina.Count > 0)
                    {
                        if (PlanesDeLaDisciplina.Count > 0)
                        {
                            bVisible = true;
                        }
                    }
                }
                return bVisible;
            }
        }

        public string CSSNoImagen
        {
            get
            {
                string sResponse = "";
                if (String.IsNullOrEmpty(FileImage) || String.IsNullOrEmpty(IconImage) || String.IsNullOrEmpty(LogoSVG) || String.IsNullOrEmpty(SiglasImage) || String.IsNullOrEmpty(ImageEntrenar))
                {
                    sResponse = "btn-notag";
                }
                return sResponse;
            }
        }

        public string CSSNoData
        {
            get
            {
                string sResponse = "";
                if (CuestionarioDeLaDisciplina.Count == 0 || PlanesDeLaDisciplina.Count == 0)
                {
                    sResponse = "btn-notag";
                }
                return sResponse;
            }
        }
    }
}