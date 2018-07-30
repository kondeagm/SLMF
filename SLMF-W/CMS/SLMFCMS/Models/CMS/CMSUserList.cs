using System.ComponentModel.DataAnnotations;

namespace SLMFCMS.Models.CMS
{
    public class CMSUserList
    {
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Correo")]
        public string Correo { get; set; }

        [Display(Name = "Rol")]
        public string Rol { get; set; }
    }
}