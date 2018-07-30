using SLMFCMS.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace SLMFCMS.Models
{
    [Table("Plan")]
    public class Plan
    {
        public Plan()
        {
            this.DiasDelPlan = new HashSet<PlanDias>();
            this.EtiquetasDelPlan = new HashSet<PlanEtiquetas>();
            this.UsuariosDelPlan = new HashSet<Usuario>();
        }

        [Key]
        [Display(Name = "Plan")]
        public int ID { get; set; }

        [Display(Name = "Disciplina")]
        [Required(ErrorMessage = "Debe seleccionar la Disciplina a la que pertenece el Plan")]
        public int DisciplinaID { get; set; }

        [Display(Name = "Disciplina")]
        public virtual Disciplina Disciplina { get; set; }

        [Display(Name = "Nombre")]
        [RegularExpression(@"^[a-zA-Z0-9 _&]*$", ErrorMessage = "El Nombre solo acepta números y letras")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Plan")]
        [StringLength(15, ErrorMessage = "El Nombre del Plan no debe exceder los 15 caracteres.")]
        public string Nombre { get; set; }

        [Display(Name = "Leyenda")]
        [Required(ErrorMessage = "Debe ingresar la Leyenda del Plan")]
        [StringLength(35, ErrorMessage = "La Leyenda del Plan no debe exceder los 35 caracteres.")]
        public string Leyenda { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "Debe ingresar la Descripción del Plan")]
        [StringLength(150, ErrorMessage = "Solo tiene 150 caracteres, para escribir la Descripción del Plan.")]
        public string Descripcion { get; set; }

        [Display(Name = "Imagen")]
        [StringLength(250, ErrorMessage = "El ID de la Imagen solo puede medir 250 caracteres")]
        public string FileImage { get; set; }

        [Display(Name = "Activo")]
        public bool Visible { get; set; }

        [Display(Name = "Dias del Plan")]
        public virtual ICollection<PlanDias> DiasDelPlan { get; set; }

        [Display(Name = "Etiquetas del Plan")]
        public virtual ICollection<PlanEtiquetas> EtiquetasDelPlan { get; set; }

        [Display(Name = "Usuarios con el Plan")]
        public virtual ICollection<Usuario> UsuariosDelPlan { get; set; }

        public string URLPlan
        {
            get
            {
                string sResponse = "";
                string sSiteDomain = ConfigurationManager.AppSettings["App_SiteURL"];
                sResponse = sSiteDomain + Funcion.NameEncode(Disciplina.Nombre).ToLower() + "/entrenar/";
                if (!String.IsNullOrEmpty(FileImage))
                {
                    if (EtiquetasDelPlan.Count > 0)
                    {
                        if (DiasDelPlan.Count > 0)
                        {
                            sResponse += Funcion.NameEncode(Nombre).ToLower();
                        }
                    }
                }
                return sResponse;
            }
        }

        public bool Definido
        {
            get
            {
                bool bVisible = false;
                if (!String.IsNullOrEmpty(FileImage))
                {
                    if (EtiquetasDelPlan.Count > 0)
                    {
                        if (DiasDelPlan.Count > 0)
                        {
                            bVisible = true;
                        }
                    }
                }
                return bVisible;
            }
        }

        public string CSSNoTagueado
        {
            get
            {
                string sResponse = "";
                if (EtiquetasDelPlan.Count == 0)
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

        public string CSSSinDias
        {
            get
            {
                string sResponse = "";
                if (DiasDelPlan.Count == 0)
                {
                    sResponse = "btn-notag";
                }
                return sResponse;
            }
        }

        public string CSSNoActivo
        {
            get
            {
                string sResponse = "";
                if (!Visible)
                {
                    sResponse = "btn-notag";
                }
                return sResponse;
            }
        }
    }
}