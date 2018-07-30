using System.ComponentModel.DataAnnotations;

namespace SLMFAPI.Models.UserControl
{
    public class JsonSLMFUser
    {
        [Required()]
        public string facebookid { get; set; }

        [Required()]
        public string nombre { get; set; }

        [Required()]
        public string apellidos { get; set; }

        [Required()]
        public string correo { get; set; }
    }
}