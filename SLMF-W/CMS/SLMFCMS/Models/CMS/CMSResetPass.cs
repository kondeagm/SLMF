using System.ComponentModel.DataAnnotations;

namespace SLMFCMS.Models.CMS
{
    public class CMSResetPass
    {
        public string Id { get; set; }

        [Display(Name = "Nuevo Password")]
        [Required(ErrorMessage = "Debe ingresar el Nuevo Password del Usuario")]
        [StringLength(15, ErrorMessage = "El Nuevo Password debe medir entre 6 y 15 caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirme el Nuevo Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Los Passwords no Coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}