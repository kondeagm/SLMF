using SLMFCMS.Functions;
using SLMFCMS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin, ContentAdmin")]
    public class DietaTemposController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DietaTempos/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DietaTempos bdDietaTempos = db.DietaTempos.Find(id);
            if (bdDietaTempos == null)
            {
                return HttpNotFound();
            }
            return View(bdDietaTempos);
        }

        // GET: DietaTempos/Create
        public ActionResult Create(int? dieta, int? pagina)
        {
            if (dieta == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dieta bdDieta = db.Dieta.Find(dieta);
            if (bdDieta == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pagina = (pagina ?? 1);
            DietaTempos bdDietaTempos = new DietaTempos();
            bdDietaTempos.DietaID = Convert.ToInt32(dieta);
            ViewBag.TempoID = new SelectList(db.Tempo.OrderBy(s => s.Secuencia), "ID", "NombreCompleto");
            ViewBag.Hora = new SelectList(Funcion.GetListaHoras(), "Value", "Text");
            return View(bdDietaTempos);
        }

        // POST: DietaTempos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,DietaID,TempoID,Hora")] DietaTempos dietaTempos)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.DietaTempos.Any(x => x.ID == dietaTempos.ID && x.TempoID == dietaTempos.TempoID && x.Hora == dietaTempos.Hora))
                {
                    ModelState.AddModelError("Hora", "Ya existe una Comida a esa Hora en la Dieta");
                }
                else
                {
                    db.DietaTempos.Add(dietaTempos);
                    db.SaveChanges();
                    return RedirectToAction("Tempos", "Dieta", new { id = dietaTempos.DietaID, pagina = ViewBag.Pagina });
                }
            }
            ViewBag.TempoID = new SelectList(db.Tempo.OrderBy(s => s.Secuencia), "ID", "NombreCompleto", dietaTempos.TempoID);
            ViewBag.Hora = new SelectList(Funcion.GetListaHoras(), "Value", "Text", dietaTempos.Hora);
            return View(dietaTempos);
        }

        // GET: DietaTempos/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DietaTempos bdDietaTempos = db.DietaTempos.Find(id);
            if (bdDietaTempos == null)
            {
                return HttpNotFound();
            }
            ViewBag.TempoID = new SelectList(db.Tempo.OrderBy(s => s.Secuencia), "ID", "NombreCompleto", bdDietaTempos.TempoID);
            ViewBag.Hora = new SelectList(Funcion.GetListaHoras(), "Value", "Text", bdDietaTempos.Hora);
            return View(bdDietaTempos);
        }

        // POST: DietaTempos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,DietaID,TempoID,Hora")] DietaTempos dietaTempos)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.DietaTempos.Any(x => x.ID == dietaTempos.ID && x.TempoID == dietaTempos.TempoID && x.Hora == dietaTempos.Hora && x.ID != dietaTempos.ID))
                {
                    ModelState.AddModelError("Hora", "Ya existe una Comida a esa Hora en la Dieta");
                }
                else
                {
                    db.Entry(dietaTempos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Tempos", "Dieta", new { id = dietaTempos.DietaID, pagina = ViewBag.Pagina });
                }
            }
            ViewBag.TempoID = new SelectList(db.Tempo.OrderBy(s => s.Secuencia), "ID", "NombreCompleto", dietaTempos.TempoID);
            ViewBag.Hora = new SelectList(Funcion.GetListaHoras(), "Value", "Text", dietaTempos.Hora);
            return View(dietaTempos);
        }

        // GET: DietaTempos/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DietaTempos bdDietaTempos = db.DietaTempos.Find(id);
            if (bdDietaTempos == null)
            {
                return HttpNotFound();
            }
            return View(bdDietaTempos);
        }

        // POST: DietaTempos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            DietaTempos bdDietaTempos = db.DietaTempos.Find(id);
            int iDietaId = bdDietaTempos.DietaID;
            if (bdDietaTempos.AlimentosDelTempo.Count == 0)
            {
                db.DietaTempos.Remove(bdDietaTempos);
                db.SaveChanges();
            }
            return RedirectToAction("Tempos", "Dieta", new { id = iDietaId, pagina = ViewBag.Pagina });
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