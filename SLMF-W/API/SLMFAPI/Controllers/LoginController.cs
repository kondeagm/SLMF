using Newtonsoft.Json.Linq;
using SLMFAPI.Functions;
using SLMFAPI.Models;
using SLMFAPI.Models.UserControl;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace SLMFAPI.Controllers
{
    public class LoginController : ApiController
    {
        private SlmfDBEntities db = new SlmfDBEntities();

        [Route("api/user/registro")]
        [HttpPost]
        public IHttpActionResult PostSLMFUser(JsonSLMFUser pJsonUser)
        {
            try
            {
                JObject joResponse = new JObject();
                if (pJsonUser == null)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (Funcion.UserExist(pJsonUser.facebookid) == false)
                {
                    return Unauthorized();
                }
                else
                {
                    UpdateSLMFUser(pJsonUser);
                    joResponse = new JObject(new JProperty("isRegistered", true));
                }
                return Ok(joResponse);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("api/user/delete")]
        [HttpPost]
        public IHttpActionResult PostDeleteSLMFUser(JsonFacebookUser pJsonUser)
        {
            try
            {
                JObject joResponse = new JObject();
                if (pJsonUser == null)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (Funcion.UserExist(pJsonUser.facebookid) == false)
                {
                    return Unauthorized();
                }
                else
                {
                    DeleteSLMFUser(pJsonUser);
                    joResponse = new JObject(new JProperty("isRegistered", false));
                }
                return Ok(joResponse);
            }
            catch
            {
                return InternalServerError();
            }
        }

        private void UpdateSLMFUser(JsonSLMFUser pJsonUser)
        {
            Usuario bdUsuario = db.Usuario.Where(x => x.FacebookID == pJsonUser.facebookid).First();
            bdUsuario.Nombre = pJsonUser.nombre;
            bdUsuario.Apellidos = pJsonUser.apellidos;
            bdUsuario.Correo = pJsonUser.correo;
            if (ModelState.IsValid)
            {
                db.Entry(bdUsuario).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private void DeleteSLMFUser(JsonFacebookUser pJsonUser)
        {
            Usuario bdUsuario = db.Usuario.Where(x => x.FacebookID == pJsonUser.facebookid).First();
            db.Usuario.Remove(bdUsuario);
            db.SaveChanges();
        }
    }
}