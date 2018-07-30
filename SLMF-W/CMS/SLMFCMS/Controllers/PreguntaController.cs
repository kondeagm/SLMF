using SLMFCMS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin")]
    public class PreguntaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pregunta/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta bdPregunta = db.Pregunta.Find(id);
            if (bdPregunta == null)
            {
                return HttpNotFound();
            }
            return View(bdPregunta);
        }

        // GET: Pregunta/Create
        public ActionResult Create(int? cuestionario, int? pagina)
        {
            if (cuestionario == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuestionario bdCuestionario = db.Cuestionario.Find(cuestionario);
            if (bdCuestionario == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pagina = (pagina ?? 1);
            Pregunta bdPregunta = new Pregunta();
            bdPregunta.CuestionarioID = Convert.ToInt32(cuestionario);
            return View(bdPregunta);
        }

        // POST: Pregunta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,CuestionarioID,Texto,Descripcion,Clase")] Pregunta pregunta)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Pregunta.Any(x => x.Texto.Trim().ToUpper() == pregunta.Texto.Trim().ToUpper() && x.CuestionarioID == pregunta.CuestionarioID))
                {
                    ModelState.AddModelError("Texto", "Ya existe una Pregunta con ese Texto en este Cuestionario ");
                }
                else if (db.Pregunta.Any(x => x.Clase.Trim().ToUpper() == pregunta.Clase.Trim().ToUpper() && x.CuestionarioID == pregunta.CuestionarioID))
                {
                    ModelState.AddModelError("Clase", "Ya existe una Pregunta con esa Clase en este Cuestionario");
                }
                else
                {
                    db.Pregunta.Add(pregunta);
                    db.SaveChanges();
                    return RedirectToAction("Details", "Cuestionario", new { id = pregunta.CuestionarioID, pagina = ViewBag.Pagina });
                }
            }
            return View(pregunta);
        }

        // GET: Pregunta/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pregunta bdPregunta = db.Pregunta.Find(id);
            if (bdPregunta == null)
            {
                return HttpNotFound();
            }
            return View(bdPregunta);
        }

        // POST: Pregunta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,CuestionarioID,Texto,Descripcion,Clase")] Pregunta pregunta)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Pregunta.Any(x => x.Texto.Trim().ToUpper() == pregunta.Texto.Trim().ToUpper() && x.CuestionarioID == pregunta.CuestionarioID && x.ID != pregunta.ID))
                {
                    ModelState.AddModelError("Texto", "Ya existe una Pregunta con ese Texto en este Cuestionario ");
                }
                else if (db.Pregunta.Any(x => x.Clase.Trim().ToUpper() == pregunta.Clase.Trim().ToUpper() && x.CuestionarioID == pregunta.CuestionarioID && x.ID != pregunta.ID))
                {
                    ModelState.AddModelError("Clase", "Ya existe una Pregunta con esa Clase en este Cuestionario");
                }
                else
                {
                    db.Entry(pregunta).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "Cuestionario", new { id = pregunta.CuestionarioID, pagina = ViewBag.Pagina });
                }
            }
            return View(pregunta);
        }

        // GET: Pregunta/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pregunta bdPregunta = db.Pregunta.Find(id);
            if (bdPregunta == null)
            {
                return HttpNotFound();
            }
            return View(bdPregunta);
        }

        // POST: Pregunta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Pregunta bdPregunta = db.Pregunta.Find(id);
            int iCuestionarioId = bdPregunta.CuestionarioID;
            if (bdPregunta.RespuestasDeLaPregunta.Count == 0)
            {
                db.Pregunta.Remove(bdPregunta);
                db.SaveChanges();
            }
            return RedirectToAction("Details", "Cuestionario", new { id = iCuestionarioId, pagina = ViewBag.Pagina });
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