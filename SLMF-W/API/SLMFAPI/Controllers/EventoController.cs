using Newtonsoft.Json.Linq;
using SLMFAPI.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace SLMFAPI.Controllers
{
    public class EventoController : ApiController
    {
        private SlmfDBEntities db = new SlmfDBEntities();

        [Route("api/eventos")]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 120, ServerTimeSpan = 120)]
        public IHttpActionResult GetEventos()
        {
            try
            {
                JObject joEvents = CreateJsonEvents();
                return Ok(joEvents);
            }
            catch
            {
                return InternalServerError();
            }
        }

        private JObject CreateJsonEvents()
        {
            JObject joResponse = new JObject();
            var bdEventos = from data in db.Evento.Include(p => p.Disciplina)
                            where data.Visible == true
                            orderby data.Fecha ascending
                            select data;
            foreach (var bdEvento in bdEventos.OrderBy(s => s.Fecha))
            {
                JProperty jpEvent =
                    new JProperty(bdEvento.ID.ToString(),
                        new JObject(
                            new JProperty("discipline", bdEvento.Disciplina.Siglas.ToUpper()),
                            new JProperty("title", bdEvento.Titulo.Replace("\r\n", "<br />").ToUpper()),
                            new JProperty("subtitle", bdEvento.Nombre.ToUpper()),
                            new JProperty("location", bdEvento.Lugar + "<br />" + bdEvento.Direccion.Replace("\r\n", "<br />")),
                            new JProperty("date", bdEvento.Fecha.ToString("s")),
                            new JProperty("url", bdEvento.URL)
                        )
                    );
                joResponse.Add(jpEvent);
            }
            return joResponse;
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