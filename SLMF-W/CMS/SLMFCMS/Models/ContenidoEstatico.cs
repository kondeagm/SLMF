using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SLMFCMS.Models
{
    [Table("ContenidoEstatico")]
    public class ContenidoEstatico
    {
        [Key]
        [Display(Name = "Contenido Estatico")]
        public int ID { get; set; }

        [Display(Name = "Red Social")]
        [Required(ErrorMessage = "Debe seleccionar la Red Social a la que pertenece el Contenido Estatico")]
        public int RedSocialID { get; set; }

        [Display(Name = "Red Social")]
        public virtual RedSocial RedSocial { get; set; }

        [Display(Name = "Post Id")]
        [Required(ErrorMessage = "Debe indicar el Identificador del Post en la Red Social")]
        [StringLength(300, ErrorMessage = "El Identificador del Post en la Red Social no debe exceder los 300 caracteres")]
        public string Identificador { get; set; }
    }
}