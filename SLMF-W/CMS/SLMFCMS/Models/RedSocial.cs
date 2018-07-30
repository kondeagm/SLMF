using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("RedSocial")]
    public class RedSocial
    {
        public RedSocial()
        {
            this.PostEnLaRedSocial = new HashSet<ContenidoEstatico>();
        }

        [Key]
        [Display(Name = "Red Social")]
        public int ID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Debe ingresar el Nombre de la Red Social")]
        [StringLength(10, ErrorMessage = "El Nombre de la Red Social no debe exceder los 10 caracteres")]
        public string Nombre { get; set; }

        [Display(Name = "Identificador")]
        [Required(ErrorMessage = "Debe ingresar el Identificador de la Red Social")]
        [StringLength(25, ErrorMessage = "El Identificador de la Red Social no debe exceder los 25 caracteres")]
        public string Identificador { get; set; }

        [Display(Name = "URL de la Red Social de SLMF")]
        [StringLength(500, ErrorMessage = "La URL de la Red Social de SLMF no debe exceder los 500 caracteres.")]
        public string URL { get; set; }

        [Display(Name = "APP Id")]
        [StringLength(30, ErrorMessage = "El APP Id de la Red Social no debe exceder los 30 caracteres")]
        public string APPId { get; set; }

        [Display(Name = "API Key")]
        [StringLength(500, ErrorMessage = "El API Key de la Red Social no debe exceder los 500 caracteres")]
        public string APIKey { get; set; }

        [Display(Name = "No. de Publicaciones")]
        [Required(ErrorMessage = "Debe indicar el número de publicaciones de la Red Social a visualizar")]
        public int NoPost { get; set; }

        [Display(Name = "Post en la Red Social")]
        public virtual ICollection<ContenidoEstatico> PostEnLaRedSocial { get; set; }
    }
}