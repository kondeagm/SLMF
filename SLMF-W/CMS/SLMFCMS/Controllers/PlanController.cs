using Newtonsoft.Json.Linq;
using PagedList;
using SLMFCMS.Functions;
using SLMFCMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin, ContentAdmin")]
    public class PlanController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string sFolderImagesPlans = ConfigurationManager.AppSettings["App_FolderPlansImages"];
        private int pageSize = 8;

        // GET: Plan
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            ViewBag.DisciplinaSortParm = sSortOrder == "disciplina" ? "disciplina_desc" : "disciplina";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsPlan = from dbr in db.Plan.Include(p => p.Disciplina)
                           select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsPlan = rowsPlan.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsPlan = rowsPlan.OrderByDescending(s => s.Nombre);
                    break;

                case "disciplina":
                    rowsPlan = rowsPlan.OrderBy(s => s.Disciplina.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "disciplina_desc":
                    rowsPlan = rowsPlan.OrderByDescending(s => s.Disciplina.Nombre).ThenBy(s => s.Nombre);
                    break;

                default:
                    rowsPlan = rowsPlan.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsPlan.ToPagedList(pageNumber, pageSize));
        }

        // GET: Plan/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan bdPlan = db.Plan.Find(id);
            if (bdPlan == null)
            {
                return HttpNotFound();
            }
            return View(bdPlan);
        }

        // GET: Plan/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Plan bdPlan = new Plan();
            bdPlan.Visible = false;
            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre");
            return View(bdPlan);
        }

        // POST: Plan/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,DisciplinaID,Nombre,Leyenda,Descripcion,FileImage,Visible")] Plan plan)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Plan.Any(x => x.Nombre.Trim().ToUpper() == plan.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Plan con ese Nombre");
                }
                else
                {
                    db.Plan.Add(plan);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Plan", new { sSearchString = plan.Nombre });
                }
            }
            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre", plan.DisciplinaID);
            return View(plan);
        }

        // GET: Plan/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Plan bdPlan = db.Plan.Find(id);
            if (bdPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre", bdPlan.DisciplinaID);
            return View(bdPlan);
        }

        // POST: Plan/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,DisciplinaID,Nombre,Leyenda,Descripcion,FileImage,Visible")] Plan plan)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Plan.Any(x => x.Nombre.Trim().ToUpper() == plan.Nombre.Trim().ToUpper() && x.ID != plan.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Plan con ese Nombre");
                }
                else
                {
                    db.Entry(plan).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Plan", new { sSearchString = plan.Nombre });
                }
            }
            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre", plan.DisciplinaID);
            return View(plan);
        }

        // GET: Plan/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan bdPlan = db.Plan.Find(id);
            if (bdPlan == null)
            {
                return HttpNotFound();
            }
            return View(bdPlan);
        }

        // POST: Plan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Plan bdPlan = db.Plan.Find(id);
            if (bdPlan.DiasDelPlan.Count == 0 && bdPlan.EtiquetasDelPlan.Count == 0 && bdPlan.UsuariosDelPlan.Count == 0)
            {
                db.Plan.Remove(bdPlan);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Plan", new { pagina = ViewBag.Pagina });
        }

        // GET: Plan/Show/5
        public ActionResult Show(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan bdPlan = db.Plan.Find(id);
            if (bdPlan == null)
            {
                return HttpNotFound();
            }
            if (bdPlan.Definido == true)
            {
                bdPlan.Visible = true;
                db.Entry(bdPlan).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Plan", new { pagina = ViewBag.Pagina });
        }

        // GET: Plan/Hide/5
        public ActionResult Hide(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan bdPlan = db.Plan.Find(id);
            if (bdPlan == null)
            {
                return HttpNotFound();
            }
            bdPlan.Visible = false;
            db.Entry(bdPlan).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Plan", new { pagina = ViewBag.Pagina });
        }

        // GET: Plan/Days/5
        public ActionResult Days(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan bdPlan = db.Plan.Find(id);
            if (bdPlan == null)
            {
                return HttpNotFound();
            }
            return View(bdPlan);
        }

        // GET: Plan/Tags/5
        public ActionResult Tags(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan bdPlan = db.Plan.Find(id);
            if (bdPlan == null)
            {
                return HttpNotFound();
            }
            var recRespuestas = from s in db.Respuesta
                                select s;
            ViewBag.Respuestas = recRespuestas;
            return View(bdPlan);
        }

        // GET: Plan/AddImage
        public ActionResult AddImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan bdPlan = db.Plan.Find(id);
            if (bdPlan == null)
            {
                return HttpNotFound();
            }
            return View(bdPlan);
        }

        // POST: Plan/UploadImage/5
        [HttpPost]
        public JObject UploadImage(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int plan = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "El Plan no Existe");
                }
                else
                {
                    plan = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Imagen Anexa");
                    }
                    else if (!Funcion.PlanExist(plan))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "El Plan no Existe");
                    }
                    else
                    {
                        string sExtension = System.IO.Path.GetExtension(file.FileName).Substring(1);
                        Stream stream = file.InputStream;
                        Image image = Image.FromStream(stream);
                        if (image.Height != 1024 || image.Width != 1420)
                        {
                            ArcJson = Funcion.CreateJsonResponse(1, "La Imagen no tiene las medidas Correctas");
                        }
                        else
                        {
                            ProcessFileImage(plan, image, sExtension);
                            ArcJson = Funcion.CreateJsonResponse(0, "La Imagen se subio Correctamente");
                        }
                    }
                }
                return ArcJson;
            }
            catch (Exception)
            {
                ArcJson = Funcion.CreateJsonResponse(1, "Ocurrio un Error grave en el Server de Imagenes");
                return ArcJson;
            }
        }

        // GET: Plan/DeleteImage
        public ActionResult DeleteImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan bdPlan = db.Plan.Find(id);
            if (bdPlan == null)
            {
                return HttpNotFound();
            }
            return View(bdPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Plan bdPlan = db.Plan.Find(id);
            if (bdPlan.FileImage != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesPlans + bdPlan.FileImage);
                bdPlan.FileImage = null;
                db.Entry(bdPlan).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "Plan", new { id = bdPlan.ID, pagina = ViewBag.Pagina });
        }

        private void ProcessFileImage(int pPlanId, Image pImage, string pExtension)
        {
            Plan bdPlan = db.Plan.Find(pPlanId);
            string sToFile = "";
            string sNameFile = "slmf-planes-" + Funcion.NameEncode(bdPlan.Nombre).Trim().ToLower();
            bdPlan.FileImage = sNameFile + "." + pExtension;
            db.Entry(bdPlan).State = EntityState.Modified;
            db.SaveChanges();
            sToFile = Server.MapPath(sFolderImagesPlans + sNameFile + "." + pExtension);
            pImage.Save(sToFile);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult SaveList(int id, IList<string> center)
        {
            int iIdPlan = id;
            int iIdDisciplina = db.Plan.Where(x => x.ID == iIdPlan).First().DisciplinaID;
            int iIdCuetionario = db.Cuestionario.Where(s => s.DisciplinaID == iIdDisciplina && s.Visible == true).First().ID;
            int iNoEtiquetas = db.PlanEtiquetas.Where(x => x.PlanID == iIdPlan).Count();
            for (int i = 1; i <= iNoEtiquetas; i++)
            {
                PlanEtiquetas bdValoracion = db.PlanEtiquetas.Where(x => x.PlanID == iIdPlan).First();
                db.PlanEtiquetas.Remove(bdValoracion);
                db.SaveChanges();
            }
            if (center != null)
            {
                int iTotalEtiquetas = center.Count();
                if (iTotalEtiquetas > 0)
                {
                    foreach (var item in center)
                    {
                        int iPosicion = item.IndexOf(":");
                        string sPregunta = item.Substring(0, iPosicion);
                        string sRespuesta = item.Substring(iPosicion + 2, (item.Length - (iPosicion + 2)));
                        int iIdPregunta = db.Pregunta.Where(x => x.CuestionarioID == iIdCuetionario && x.Texto == sPregunta).First().ID;
                        int iIdRespuesta = db.Respuesta.Where(x => x.PreguntaID == iIdPregunta && x.Texto == sRespuesta).First().ID;
                        PlanEtiquetas bdValoracion = new PlanEtiquetas();
                        bdValoracion.PlanID = iIdPlan;
                        bdValoracion.RespuestaID = iIdRespuesta;
                        db.PlanEtiquetas.Add(bdValoracion);
                        db.SaveChanges();
                    }
                }
                return Json(new { Center = from i in center select i.ToLower() });
            }
            else
            {
                return Json(new { Center = "Vacio" });
            }
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