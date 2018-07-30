using Newtonsoft.Json.Linq;
using SLMFAPI.Functions;
using SLMFAPI.Models;
using SLMFAPI.Models.Planificador;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace SLMFAPI.Controllers
{
    public class PlanController : ApiController
    {
        private SlmfDBEntities db = new SlmfDBEntities();
        private string sFolderImagesPlans = Convert.ToString(ConfigurationManager.AppSettings["App_FolderPlansImages"]).ToLower();
        private string sFolderImagenAssets = Convert.ToString(ConfigurationManager.AppSettings["App_FolderAssetsImages"]).ToLower();
        private string sFolderImagenRutinas = Convert.ToString(ConfigurationManager.AppSettings["App_FolderRoutinesImages"]).ToLower();
        private string sFolderImagenDietas = Convert.ToString(ConfigurationManager.AppSettings["App_FolderDietsImages"]).ToLower();
        private string sSiteURL = ConfigurationManager.AppSettings["App_SiteURL"];

        [Route("api/planes")]
        [HttpGet]
        public IHttpActionResult GetPlans()
        {
            try
            {
                JArray jaPlans = CreateJsonPlans();
                return Ok(jaPlans);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("api/plan")]
        [HttpPost]
        public IHttpActionResult GetPlan(JsonGetPlan pJsonPlan)
        {
            try
            {
                JObject joResponse = new JObject();
                if (pJsonPlan == null)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (Funcion.PlanExists(pJsonPlan.planid) == false)
                {
                    return NotFound();
                }
                else
                {
                    int iPlanID = Funcion.GetPlanId(pJsonPlan.planid);
                    joResponse = CreateJsonPlan(iPlanID);
                }
                return Ok(joResponse);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("api/plan/register")]
        [HttpPost]
        public IHttpActionResult PutRegisterUserInPlan(JsonRegistryPlan pJsonPlan)
        {
            try
            {
                JObject joResponse = new JObject();
                if (pJsonPlan == null)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (Funcion.SLMFUserExists(pJsonPlan.userId) == false)
                {
                    return NotFound();
                }
                if (Funcion.PlanExists(pJsonPlan.idPlan) == false)
                {
                    return NotFound();
                }
                else
                {
                    int iPlanID = Funcion.GetPlanId(pJsonPlan.idPlan);
                    int iUserID = Funcion.GetUserId(pJsonPlan.userId);
                    joResponse = UpdateUserPlan(iUserID, iPlanID, pJsonPlan.day);
                }
                return Ok(joResponse);
            }
            catch
            {
                return InternalServerError();
            }
        }

        [Route("api/plan/delete")]
        [HttpPost]
        public IHttpActionResult PutDeleteUserInPlan(JsonRegistryPlan pJsonPlan)
        {
            try
            {
                JObject joResponse = new JObject();
                if (pJsonPlan == null)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (Funcion.SLMFUserExists(pJsonPlan.userId) == false)
                {
                    return NotFound();
                }
                if (Funcion.PlanExists(pJsonPlan.idPlan) == false)
                {
                    return NotFound();
                }
                else
                {
                    int iPlanID = Funcion.GetPlanId(pJsonPlan.idPlan);
                    int iUserID = Funcion.GetUserId(pJsonPlan.userId);
                    joResponse = CancelUserPlan(iUserID);
                }
                return Ok(joResponse);
            }
            catch
            {
                return InternalServerError();
            }
        }

        private JArray CreateJsonPlans()
        {
            JArray jaResponse = new JArray();
            var bdPlanes = from data in db.Plan.Include(p => p.Disciplina).Include(p => p.PlanEtiquetas).Include(p => p.PlanDias)
                           where data.Visible == true
                           select data;
            foreach (var bdPlan in bdPlanes.OrderBy(s => s.Nombre))
            {
                JObject joPlan =
                    new JObject(
                        new JProperty("id", bdPlan.ID),
                        new JProperty("title", bdPlan.Nombre.ToUpper()),
                        new JProperty("subtitle", bdPlan.Leyenda.ToUpper()),
                        new JProperty("description", bdPlan.Descripcion),
                        new JProperty("iconType", GetIconTarget(bdPlan.PlanEtiquetas)),
                        new JProperty("url", sSiteURL + Funcion.NameEncode(bdPlan.Disciplina.Nombre).ToLower() + "/entrenar/" + Funcion.NameEncode(bdPlan.Nombre).ToLower()),
                        new JProperty("days", bdPlan.PlanDias.Count),
                        new JProperty("level", GetLevel(bdPlan.PlanEtiquetas)),
                        new JProperty("tags", GetJsonTagsPlan(bdPlan.PlanEtiquetas))
                    );
                jaResponse.Add(joPlan);
            }
            return jaResponse;
        }

        private JObject CreateJsonPlan(int pPlanId)
        {
            JObject joResponse = new JObject();
            var bdPlanes = from data in db.Plan.Where(s => s.ID == pPlanId).Include(p => p.PlanEtiquetas).Include(p => p.PlanDias).Include(p => p.Disciplina)
                           select data;
            var bdPlan = bdPlanes.Include(p => p.PlanEtiquetas).Include(p => p.PlanDias).Include(p => p.Disciplina).First();
            joResponse =
                new JObject(
                    new JProperty("home",
                        new JObject(
                            new JProperty("bgURL", sFolderImagesPlans.Replace("~/", sSiteURL) + bdPlan.FileImage),
                            new JProperty("title",
                                new JArray(
                                    new JValue(Funcion.DivideTitulo(bdPlan.Nombre.ToUpper(), 1)),
                                    new JValue(Funcion.DivideTitulo(bdPlan.Nombre.ToUpper(), 2))
                                )
                            ),
                            new JProperty("subtitle", bdPlan.Leyenda),
                            new JProperty("description", bdPlan.Descripcion),
                            new JProperty("level", GetLevel(bdPlan.PlanEtiquetas)),
                            new JProperty("levelPercent", GetLevelPercent(bdPlan.PlanEtiquetas)),
                            new JProperty("weeks", GetWeeksPlan(bdPlan.PlanDias)),
                            new JProperty("weeksPercent", GetWeeksPlanPercent(bdPlan.PlanDias)),
                            new JProperty("genderIcon", GetIconGender(bdPlan.PlanEtiquetas))
                    )
                ),
                new JProperty("muscleGroups", GetJsonMuscleGroups()),
                new JProperty("days", GetJsonDaysPlan(bdPlan.PlanDias)),
                new JProperty("banners", GetJsonBanners())
            );
            return joResponse;
        }

        private int GetIconTarget(ICollection<PlanEtiquetas> pEtiquetas)
        {
            int iResponse = 2;
            foreach (var bdIcon in pEtiquetas.Where(s => s.Respuesta.PreguntaID == 4))
            {
                if (bdIcon.RespuestaID == 9)
                {
                    iResponse = 1;
                }
            }
            return iResponse;
        }

        private string GetIconGender(ICollection<PlanEtiquetas> pEtiquetas)
        {
            string sResponse = "";
            foreach (var bdIcon in pEtiquetas.Where(s => s.Respuesta.PreguntaID == 1))
            {
                sResponse = bdIcon.Respuesta.LogoSVG.Replace("\"", "'");
            }
            return sResponse;
        }

        private int GetLevel(ICollection<PlanEtiquetas> pEtiquetas)
        {
            int iResponse = 2;
            foreach (var bdLevel in pEtiquetas.Where(s => s.Respuesta.PreguntaID == 3))
            {
                iResponse = bdLevel.RespuestaID - 5;
            }
            return iResponse;
        }

        private int GetLevelPercent(ICollection<PlanEtiquetas> pEtiquetas)
        {
            int iResponse = 0;
            foreach (var bdLevel in pEtiquetas.Where(s => s.Respuesta.PreguntaID == 3))
            {
                decimal iValor = 0;
                iValor = bdLevel.RespuestaID - 5;
                decimal iPorcentaje = ((iValor / 3) * 100);
                iResponse = (int)iPorcentaje;
            }
            return iResponse;
        }

        private int GetWeeksPlan(ICollection<PlanDias> pDias)
        {
            int iResponse = 0;
            int iDiasPlan = pDias.Count;
            if (iDiasPlan == 0)
            {
                iResponse = 0;
            }
            else if (iDiasPlan < 8)
            {
                iResponse = 1;
            }
            else if (iDiasPlan > 7)
            {
                iResponse = iDiasPlan / 7;
            }
            return iResponse;
        }

        private int GetWeeksPlanPercent(ICollection<PlanDias> pDias)
        {
            int iResponse = 0;
            decimal iDiasPlan = pDias.Count;
            decimal iSemanas = 0;
            decimal iPorcentaje = 0;
            if (iDiasPlan == 0)
            {
                iSemanas = 0;
            }
            else if (iDiasPlan < 8)
            {
                iSemanas = 1;
            }
            else if (iDiasPlan > 7)
            {
                iSemanas = iDiasPlan / 7;
            }
            if (iSemanas > 0)
            {
                iPorcentaje = ((iSemanas / 12) * 100);
            }
            iResponse = (int)iPorcentaje;
            return iResponse;
        }

        private JObject GetJsonTagsPlan(ICollection<PlanEtiquetas> pEtiquetas)
        {
            JObject joResponse = new JObject();
            foreach (var bdEtiqueta in pEtiquetas.OrderBy(s => s.RespuestaID))
            {
                JProperty jpEtiqueta = new JProperty(bdEtiqueta.Respuesta.Pregunta.Clase, bdEtiqueta.Respuesta.Clase);
                joResponse.Add(jpEtiqueta);
            }
            return joResponse;
        }

        private JArray GetJsonMuscleGroups()
        {
            JArray jaResponse = new JArray();
            var bdGrupoMusculos = from data in db.GrupoMusculos
                                  select data;
            foreach (var bdGrupo in bdGrupoMusculos.OrderBy(s => s.ID))
            {
                JObject joGrupo =
                    new JObject(
                        new JProperty("title", bdGrupo.Nombre.ToUpper()),
                        new JProperty("description", bdGrupo.Descripcion),
                        new JProperty("srcImg", sFolderImagenAssets.Replace("~/", sSiteURL) + bdGrupo.FileImage)
                    );
                jaResponse.Add(joGrupo);
            }
            return jaResponse;
        }

        private JArray GetJsonBanners()
        {
            JArray jaResponse = new JArray();
            var bdBanners = from data in db.Banner.Where(s => s.Visible == true)
                            select data;
            foreach (var bdBanner in bdBanners.OrderByDescending(s => s.Prioridad).ThenBy(s => s.ID))
            {
                JObject joGrupo =
                    new JObject(
                        new JProperty("identificador", bdBanner.Identificador),
                        new JProperty("link", bdBanner.LinkBanner),
                        new JProperty("srcImg", sFolderImagenAssets.Replace("~/", sSiteURL) + bdBanner.FileImage)
                    );
                jaResponse.Add(joGrupo);
            }
            return jaResponse;
        }

        private JArray GetJsonDaysPlan(ICollection<PlanDias> pDias)
        {
            JArray jaResponse = new JArray();
            foreach (var bdDia in pDias.OrderBy(s => s.Dia))
            {
                JObject joDia =
                    new JObject(
                        new JProperty("header",
                            new JObject(
                                new JProperty("indexMuscleGroups", bdDia.Rutina.GrupoMusculosID - 1),
                                new JProperty("nameDay", bdDia.Dia),
                                new JProperty("isRestDay", bdDia.Rutina.Inactividad)
                            )
                        ),
                        new JProperty("meals", GetJsonComidasPlan(bdDia.Dieta, bdDia.Rutina.Inactividad)
                        ),
                        new JProperty("training", GetJsonEntrenamientoPlan(bdDia, bdDia.Rutina.Inactividad)
                        )
                    );
                jaResponse.Add(joDia);
            }
            return jaResponse;
        }

        private JObject GetJsonComidasPlan(Dieta pDieta, bool pInactividad)
        {
            JObject joResponse = new JObject();
            if (pInactividad == false)
            {
                joResponse =
                    new JObject(
                        new JProperty("diet",
                            new JObject(
                                new JProperty("srcImg", sFolderImagenDietas.Replace("~/", sSiteURL) + pDieta.FileImage),
                                new JProperty("title", pDieta.Nombre.ToUpper()),
                                new JProperty("description", new JArray(new JValue(pDieta.Descripcion.Replace("\r\n", "<br />")))),
                                new JProperty("slider", new JArray(new JValue(""))),
                                new JProperty("listColors", GetJsonNutrientes())
                            )
                        ),
                        new JProperty("groups", GetJsonAlimentacionComplemento(pDieta.DietaTempos)),
                        new JProperty("hours", GetJsonAlimentacionComidas(pDieta.DietaTempos))
                    );
            }
            return joResponse;
        }

        private JObject GetJsonEntrenamientoPlan(PlanDias pbdDia, bool pInactividad)
        {
            JObject joResponse = new JObject();
            if (pInactividad == false)
            {
                joResponse =
                        new JObject(
                            new JProperty("protip",
                                new JObject(
                                    new JProperty("idVideo", String.IsNullOrEmpty(pbdDia.ProTip.VimeoID) ? "" : pbdDia.ProTip.VimeoID),
                                    new JProperty("srcImg", sFolderImagenRutinas.Replace("~/", sSiteURL) + pbdDia.ProTip.FileImage),
                                    new JProperty("title", pbdDia.ProTip.Nombre.ToUpper()),
                                    new JProperty("description", pbdDia.ProTip.Descripcion),
                                    new JProperty("author", pbdDia.ProTip.Autor)
                                )
                            ),
                            new JProperty("videos", GetJsonEjercicios(pbdDia.PlanDiaEjercicios))
                        );
            }
            return joResponse;
        }

        private JArray GetJsonNutrientes()
        {
            JArray jaResponse = new JArray();
            var bdNutrientes = from data in db.Nutriente
                               where data.ID < 4
                               select data;
            foreach (var bdNutrinete in bdNutrientes.OrderBy(s => s.ID))
            {
                JObject joNutrinete =
                    new JObject(
                        new JProperty("color", bdNutrinete.ColorCode),
                        new JProperty("title", bdNutrinete.Nombre)
                    );
                jaResponse.Add(joNutrinete);
            }
            return jaResponse;
        }

        private JArray GetJsonAlimentacionComplemento(ICollection<DietaTempos> pTiempos)
        {
            JArray jaResponse = new JArray();
            foreach (var bdTiempo in pTiempos.Where(s => s.Tempo.Complementario == true).OrderBy(s => s.Tempo.Secuencia))
            {
                JObject joTiempo =
                    new JObject(
                        new JProperty("title", bdTiempo.Tempo.Prefijo),
                        new JProperty("list", GetJsonAlimentos(bdTiempo.DietaAlimentacion))
                    );
                jaResponse.Add(joTiempo);
            }
            return jaResponse;
        }

        private JArray GetJsonAlimentacionComidas(ICollection<DietaTempos> pTiempos)
        {
            JArray jaResponse = new JArray();
            foreach (var bdTiempo in pTiempos.Where(s => s.Tempo.Complementario == false).OrderBy(s => s.Tempo.Secuencia))
            {
                JObject joTiempo =
                    new JObject(
                        new JProperty("cycle", Funcion.HoraTiempo(bdTiempo.Hora)),
                        new JProperty("hrs", Funcion.HoraStr(bdTiempo.Hora)),
                        new JProperty("list", GetJsonAlimentos(bdTiempo.DietaAlimentacion))
                    );
                jaResponse.Add(joTiempo);
            }
            return jaResponse;
        }

        private JArray GetJsonAlimentos(ICollection<DietaAlimentacion> pAlimentos)
        {
            JArray jaResponse = new JArray();
            foreach (var bdAlimento in pAlimentos.OrderBy(s => s.ID))
            {
                JObject joAlimento =
                    new JObject(
                        new JProperty("color", bdAlimento.Alimento.Nutriente.ColorCode),
                        new JProperty("description", bdAlimento.NombreAlimento),
                        new JProperty("letter", bdAlimento.Alimento.Nutriente.Nombre.Substring(0, 1).ToUpper())
                    );
                jaResponse.Add(joAlimento);
            }
            return jaResponse;
        }

        private JArray GetJsonEjercicios(ICollection<PlanDiaEjercicios> pEjercicios)
        {
            JArray jaResponse = new JArray();
            foreach (var bdEjercicio in pEjercicios.OrderByDescending(s => s.Secuencia))
            {
                JObject joEjercicio =
                    new JObject(
                        new JProperty("area", String.IsNullOrEmpty(bdEjercicio.Ejercicio.MusculoID.ToString()) ? "" : bdEjercicio.Ejercicio.Musculo.Nombre.ToUpper()),
                        new JProperty("idVideo", String.IsNullOrEmpty(bdEjercicio.Ejercicio.VimeoID) ? "" : bdEjercicio.Ejercicio.VimeoID),
                        new JProperty("level", Funcion.RangoNivel(bdEjercicio.Nivel)),
                        new JProperty("position", String.IsNullOrEmpty(bdEjercicio.Ejercicio.PosicionID.ToString()) ? "" : bdEjercicio.Ejercicio.Posicion.Nombre.ToUpper()),
                        new JProperty("units", bdEjercicio.Repeticiones),
                        new JProperty("textUnits", bdEjercicio.UnidadEjercicio.Abreviacion.ToUpper()),
                        new JProperty("series", bdEjercicio.Series),
                        new JProperty("srcImg", sFolderImagenRutinas.Replace("~/", sSiteURL) + bdEjercicio.Ejercicio.FileImage),
                        new JProperty("tipoSerie", String.IsNullOrEmpty(bdEjercicio.Nota) ? "" : bdEjercicio.Nota),
                        new JProperty("title", String.IsNullOrEmpty(bdEjercicio.Ejercicio.AccesorioID.ToString()) ? bdEjercicio.Ejercicio.Nombre.ToUpper() : bdEjercicio.Ejercicio.Nombre.ToUpper() + " C/" + bdEjercicio.Ejercicio.Accesorio.Nombre.ToUpper())
                    );
                jaResponse.Add(joEjercicio);
            }
            return jaResponse;
        }

        private JObject UpdateUserPlan(int pUserID, int pPlanID, int pDia)
        {
            JObject joResponse = new JObject();
            Usuario bdUsuario = db.Usuario.Find(pUserID);
            bdUsuario.PlanID = pPlanID;
            if (pDia == 1)
            {
                bdUsuario.FechaInicioPlan = System.DateTime.Today;
            }
            else if (pDia == 2)
            {
                bdUsuario.FechaInicioPlan = System.DateTime.Today.AddDays(1);
            }
            else
            {
                bdUsuario.FechaInicioPlan = System.DateTime.Today.AddDays(3);
            }
            db.Entry(bdUsuario).State = EntityState.Modified;
            db.SaveChanges();
            joResponse =
                new JObject(
                    new JProperty("status", "Success")
                );
            return joResponse;
        }

        private JObject CancelUserPlan(int pUserID)
        {
            JObject joResponse = new JObject();
            Usuario bdUsuario = db.Usuario.Find(pUserID);
            bdUsuario.PlanID = null;
            bdUsuario.FechaInicioPlan = null;
            db.Entry(bdUsuario).State = EntityState.Modified;
            db.SaveChanges();
            joResponse =
                new JObject(
                    new JProperty("status", "Success")
                );
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