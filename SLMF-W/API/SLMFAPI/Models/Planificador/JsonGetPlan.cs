using System.ComponentModel.DataAnnotations;

namespace SLMFAPI.Models.Planificador
{
    public class JsonGetPlan
    {
        [Required()]
        public string planid { get; set; }
    }
}