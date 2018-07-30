using System.ComponentModel.DataAnnotations;

namespace SLMFAPI.Models.Planificador
{
    public class JsonRegistryPlan
    {
        public int day { get; set; }

        [Required()]
        public string userId { get; set; }

        [Required()]
        public string idPlan { get; set; }
    }
}