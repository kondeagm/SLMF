using System.ComponentModel.DataAnnotations;

namespace SLMFAPI.Models.UserControl
{
    public class JsonFacebookUser
    {
        [Required()]
        public string facebookid { get; set; }
    }
}