using SLMFCMS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin")]
    public class RespuestaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Respuesta/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta bdRespuesta = db.Respuesta.Find(id);
            if (bdRespuesta == null)
            {
                return HttpNotFound();
            }
            return View(bdRespuesta);
        }

        // GET: Respuesta/Create
        public ActionResult Create(int? pregunta, int? pagina)
        {
            if (pregunta == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta bdPregunta = db.Pregunta.Find(pregunta);
            if (bdPregunta == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pagina = (pagina ?? 1);
            Respuesta bdRespuesta = new Respuesta();
            bdRespuesta.PreguntaID = Convert.ToInt32(pregunta);
            bdRespuesta.Filtro = false;
            return View(bdRespuesta);
        }

        // POST: Respuesta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,PreguntaID,Texto,Clase,Filtro,LogoSVG")] Respuesta respuesta)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Respuesta.Any(x => x.Texto.Trim().ToUpper() == respuesta.Texto.Trim().ToUpper() && x.PreguntaID == respuesta.PreguntaID))
                {
                    ModelState.AddModelError("Texto", "Ya existe una Respuesta con ese Texto en esta Pregunta ");
                }
                else if (db.Respuesta.Any(x => x.Clase.Trim().ToUpper() == respuesta.Clase.Trim().ToUpper() && x.PreguntaID == respuesta.PreguntaID))
                {
                    ModelState.AddModelError("Clase", "Ya existe una Respuesta con esa Clase en esta Pregunta");
                }
                else
                {
                    db.Respuesta.Add(respuesta);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Pregunta", new { id = respuesta.PreguntaID, pagina = ViewBag.Pagina });
                }
            }
            return View(respuesta);
        }

        // GET: Respuesta/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Respuesta bdRespuesta = db.Respuesta.Find(id);
            if (bdRespuesta == null)
            {
                return HttpNotFound();
            }
            return View(bdRespuesta);
        }

        // POST: Respuesta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,PreguntaID,Texto,Clase,Filtro,LogoSVG")] Respuesta respuesta)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Respuesta.Any(x => x.Texto.Trim().ToUpper() == respuesta.Texto.Trim().ToUpper() && x.PreguntaID == respuesta.PreguntaID && x.ID != respuesta.ID))
                {
                    ModelState.AddModelError("Texto", "Ya existe una Respuesta con ese Texto en esta Pregunta ");
                }
                else if (db.Respuesta.Any(x => x.Clase.Trim().ToUpper() == respuesta.Clase.Trim().ToUpper() && x.PreguntaID == respuesta.PreguntaID && x.ID != respuesta.ID))
                {
                    ModelState.AddModelError("Clase", "Ya existe una Respuesta con esa Clase en esta Pregunta");
                }
                else
                {
                    db.Entry(respuesta).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "Pregunta", new { id = respuesta.PreguntaID, pagina = ViewBag.Pagina });
                }
            }
            return View(respuesta);
        }

        // GET: Respuesta/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta bdRespuesta = db.Respuesta.Find(id);
            if (bdRespuesta == null)
            {
                return HttpNotFound();
            }
            return View(bdRespuesta);
        }

        // POST: Respuesta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Respuesta bdRespuesta = db.Respuesta.Find(id);
            int iPreguntaId = bdRespuesta.PreguntaID;
            if (bdRespuesta.PlanesDeLaEtiqueta.Count == 0)
            {
                db.Respuesta.Remove(bdRespuesta);
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Pregunta", new { id = iPreguntaId, pagina = ViewBag.Pagina });
        }

        // FUNCIONALIDADES EXTRAS

        // GET: Respuesta/Filter/5
        public ActionResult Filter(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta bdRespuesta = db.Respuesta.Find(id);
            if (bdRespuesta == null)
            {
                return HttpNotFound();
            }
            bdRespuesta.Filtro = true;
            db.Entry(bdRespuesta).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", "Pregunta", new { id = bdRespuesta.PreguntaID, pagina = ViewBag.Pagina });
        }

        // GET: Respuesta/NoFilter/5
        public ActionResult NoFilter(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuesta bdRespuesta = db.Respuesta.Find(id);
            if (bdRespuesta == null)
            {
                return HttpNotFound();
            }
            bdRespuesta.Filtro = false;
            db.Entry(bdRespuesta).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", "Pregunta", new { id = bdRespuesta.PreguntaID, pagina = ViewBag.Pagina });
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