using PagedList;
using SLMFCMS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin, ContentAdmin")]
    public class RutinaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: Rutina
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsRutina = from dbr in db.Rutina.Include(r => r.GrupoMusculos)
                             select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsRutina = rowsRutina.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsRutina = rowsRutina.OrderByDescending(s => s.Nombre);
                    break;

                default:
                    rowsRutina = rowsRutina.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsRutina.ToPagedList(pageNumber, pageSize));
        }

        // GET: Rutina/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rutina bdRutina = db.Rutina.Find(id);
            if (bdRutina == null)
            {
                return HttpNotFound();
            }
            return View(bdRutina);
        }

        // GET: Rutina/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Rutina bdRutina = new Rutina();
            bdRutina.Inactividad = false;
            ViewBag.GrupoMusculosID = new SelectList(db.GrupoMusculos.OrderBy(s => s.Nombre), "ID", "Nombre");
            return View(bdRutina);
        }

        // POST: Rutina/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,Descripcion,GrupoMusculosID,Inactividad")] Rutina rutina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Rutina.Any(x => x.Nombre.Trim().ToUpper() == rutina.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Rutina con ese Nombre");
                }
                else
                {
                    db.Rutina.Add(rutina);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Rutina", new { sSearchString = rutina.Nombre });
                }
            }
            ViewBag.GrupoMusculosID = new SelectList(db.GrupoMusculos.OrderBy(s => s.Nombre), "ID", "Nombre", rutina.GrupoMusculosID);
            return View(rutina);
        }

        // GET: Rutina/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Rutina bdRutina = db.Rutina.Find(id);
            if (bdRutina == null)
            {
                return HttpNotFound();
            }
            ViewBag.GrupoMusculosID = new SelectList(db.GrupoMusculos.OrderBy(s => s.Nombre), "ID", "Nombre", bdRutina.GrupoMusculosID);
            return View(bdRutina);
        }

        // POST: Rutina/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,Descripcion,GrupoMusculosID,Inactividad")] Rutina rutina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Rutina.Any(x => x.Nombre.Trim().ToUpper() == rutina.Nombre.Trim().ToUpper() && x.ID != rutina.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Rutina con ese Nombre");
                }
                else
                {
                    db.Entry(rutina).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Rutina", new { sSearchString = rutina.Nombre });
                }
            }
            ViewBag.GrupoMusculosID = new SelectList(db.GrupoMusculos.OrderBy(s => s.Nombre), "ID", "Nombre", rutina.GrupoMusculosID);
            return View(rutina);
        }

        // GET: Rutina/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rutina bdRutina = db.Rutina.Find(id);
            if (bdRutina == null)
            {
                return HttpNotFound();
            }
            return View(bdRutina);
        }

        // POST: Rutina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Rutina bdRutina = db.Rutina.Find(id);
            if (bdRutina.DiasConLaRutina.Count == 0)
            {
                db.Rutina.Remove(bdRutina);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Rutina", new { pagina = ViewBag.Pagina });
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