using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("Ejercicio")]
    public class Ejercicio
    {
        public Ejercicio()
        {
            this.DiasConElEjercicio = new HashSet<PlanDiaEjercicios>();
        }

        [Key]
        [Display(Name = "Ejercicio")]
        public int ID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el Nombre del Ejercicio")]
        [StringLength(25, ErrorMessage = "El Nombre del Ejercicio no debe exceder los 25 caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Accesorio")]
        public int? AccesorioID { get; set; }

        [Display(Name = "Accesorio")]
        public virtual Accesorio AccesorioDelEjercicio { get; set; }

        [Display(Name = "Elemento")]
        public int? ElementoID { get; set; }

        [Display(Name = "Elemento")]
        public virtual Elemento ElementoDelEjercicio { get; set; }

        [Display(Name = "Posicion")]
        public int? PosicionID { get; set; }

        [Display(Name = "Posicion")]
        public virtual Posicion PosicionDelEjercicio { get; set; }

        [Display(Name = "Musculo")]
        [Required(ErrorMessage = "Debe seleccionar el Musculo principal que trabaja el Ejercicio")]
        public int MusculoID { get; set; }

        [Display(Name = "Musculo")]
        public virtual Musculo Musculo { get; set; }

        [Display(Name = "ID de Vimeo")]
        [Required(ErrorMessage = "Debe ingresar el ID del video en Vimeo del Ejercicio")]
        [StringLength(50, ErrorMessage = "El ID del video en Vimeo del Ejercicio no debe exceder los 50 caracteres")]
        public string VimeoID { get; set; }

        [Display(Name = "Imagen")]
        [StringLength(250, ErrorMessage = "El Nombre de la Imagen de Proteccion del Video solo puede medir 250 caracteres")]
        public string FileImage { get; set; }

        [Display(Name = "Dias con el Ejercicio")]
        public virtual ICollection<PlanDiaEjercicios> DiasConElEjercicio { get; set; }

        [Display(Name = "Ejercicio")]
        public String NombreCompleto
        {
            get
            {
                string sNombre = "";
                sNombre = Nombre;
                if (AccesorioDelEjercicio != null)
                {
                    sNombre += " c/" + AccesorioDelEjercicio.Nombre;
                }
                if (PosicionDelEjercicio != null)
                {
                    sNombre += " " + PosicionDelEjercicio.Nombre;
                }
                if (ElementoDelEjercicio != null)
                {
                    sNombre += " en " + ElementoDelEjercicio.Nombre;
                }
                return sNombre;
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