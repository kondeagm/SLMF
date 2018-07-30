using SLMFCMS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin, ContentAdmin")]
    public class DietaAlimentacionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DietaAlimentacion/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DietaAlimentacion bdDietaAlimentacion = db.DietaAlimentacion.Find(id);
            if (bdDietaAlimentacion == null)
            {
                return HttpNotFound();
            }
            return View(bdDietaAlimentacion);
        }

        // GET: DietaAlimentacion/Create
        public ActionResult Create(int? comida, int? pagina)
        {
            if (comida == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DietaTempos bdComida = db.DietaTempos.Find(comida);
            if (bdComida == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pagina = (pagina ?? 1);
            DietaAlimentacion bdDietaAlimentacion = new DietaAlimentacion();
            bdDietaAlimentacion.DietaTemposID = Convert.ToInt32(comida);
            ViewBag.AlimentoID = new SelectList(db.Alimento.OrderBy(s => s.Nombre), "ID", "Nombre");
            ViewBag.PorcionID = new SelectList(db.Porcion.OrderBy(s => s.Descripcion), "ID", "Descripcion");
            return View(bdDietaAlimentacion);
        }

        // POST: DietaAlimentacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,DietaTemposID,Cantidad,PorcionID,AlimentoID")] DietaAlimentacion dietaAlimentacion)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                db.DietaAlimentacion.Add(dietaAlimentacion);
                db.SaveChanges();
                return RedirectToAction("Details", "DietaTempos", new { id = dietaAlimentacion.DietaTemposID, pagina = ViewBag.Pagina });
            }
            ViewBag.AlimentoID = new SelectList(db.Alimento.OrderBy(s => s.Nombre), "ID", "Nombre", dietaAlimentacion.AlimentoID);
            ViewBag.PorcionID = new SelectList(db.Porcion.OrderBy(s => s.Descripcion), "ID", "Descripcion", dietaAlimentacion.PorcionID);
            return View(dietaAlimentacion);
        }

        // GET: DietaAlimentacion/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DietaAlimentacion bdDietaAlimentacion = db.DietaAlimentacion.Find(id);
            if (bdDietaAlimentacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlimentoID = new SelectList(db.Alimento.OrderBy(s => s.Nombre), "ID", "Nombre", bdDietaAlimentacion.AlimentoID);
            ViewBag.PorcionID = new SelectList(db.Porcion.OrderBy(s => s.Descripcion), "ID", "Descripcion", bdDietaAlimentacion.PorcionID);
            return View(bdDietaAlimentacion);
        }

        // POST: DietaAlimentacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,DietaTemposID,Cantidad,PorcionID,AlimentoID")] DietaAlimentacion dietaAlimentacion)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                db.Entry(dietaAlimentacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "DietaTempos", new { id = dietaAlimentacion.DietaTemposID, pagina = ViewBag.Pagina });
            }
            ViewBag.AlimentoID = new SelectList(db.Alimento.OrderBy(s => s.Nombre), "ID", "Nombre", dietaAlimentacion.AlimentoID);
            ViewBag.PorcionID = new SelectList(db.Porcion.OrderBy(s => s.Descripcion), "ID", "Descripcion", dietaAlimentacion.PorcionID);
            return View(dietaAlimentacion);
        }

        // GET: DietaAlimentacion/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DietaAlimentacion bdDietaAlimentacion = db.DietaAlimentacion.Find(id);
            if (bdDietaAlimentacion == null)
            {
                return HttpNotFound();
            }
            return View(bdDietaAlimentacion);
        }

        // POST: DietaAlimentacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            DietaAlimentacion bdDietaAlimentacion = db.DietaAlimentacion.Find(id);
            int iComidaId = bdDietaAlimentacion.DietaTemposID;
            db.DietaAlimentacion.Remove(bdDietaAlimentacion);
            db.SaveChanges();
            return RedirectToAction("Details", "DietaTempos", new { id = iComidaId, pagina = ViewBag.Pagina });
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