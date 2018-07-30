using Newtonsoft.Json.Linq;
using SLMFAPI.Models;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace SLMFAPI.Controllers
{
    public class ProductoController : ApiController
    {
        private SlmfDBEntities db = new SlmfDBEntities();
        private string sFolderImagesProducts = ConfigurationManager.AppSettings["App_FolderProductsImages"];
        private string sSiteURL = ConfigurationManager.AppSettings["App_SiteURL"];

        [Route("api/productos")]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 120, ServerTimeSpan = 120)]
        public IHttpActionResult GetProductos()
        {
            try
            {
                JArray jaProducts = CreateJsonProducts();
                return Ok(jaProducts);
            }
            catch
            {
                return InternalServerError();
            }
        }

        private JArray CreateJsonProducts()
        {
            JArray jaResponse = new JArray();
            var bdProductos = from data in db.Producto.Include(p => p.Nutriente)
                              select data;
            foreach (var bdProducto in bdProductos.OrderBy(s => s.NutrienteID).ThenBy(s => s.Nombre))
            {
                JObject joProducto =
                    new JObject(
                        new JProperty("group", bdProducto.Nutriente.Nombre.ToLower()),
                        new JProperty("image_url", sFolderImagesProducts.Replace("~/", sSiteURL) + bdProducto.FileImage),
                        new JProperty("name", bdProducto.Nombre),
                        new JProperty("store_url", bdProducto.URL)
                    );
                jaResponse.Add(joProducto);
            }
            return jaResponse;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}