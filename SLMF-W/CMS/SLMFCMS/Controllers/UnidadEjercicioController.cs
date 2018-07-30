using PagedList;
using SLMFCMS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin")]
    public class UnidadEjercicioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: UnidadEjercicio
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
            var rowsUnidadEjercicio = from dbr in db.UnidadEjercicio
                                      select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsUnidadEjercicio = rowsUnidadEjercicio.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsUnidadEjercicio = rowsUnidadEjercicio.OrderByDescending(s => s.Nombre);
                    break;

                default:
                    rowsUnidadEjercicio = rowsUnidadEjercicio.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsUnidadEjercicio.ToPagedList(pageNumber, pageSize));
        }

        // GET: UnidadEjercicio/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadEjercicio bdUnidadEjercicio = db.UnidadEjercicio.Find(id);
            if (bdUnidadEjercicio == null)
            {
                return HttpNotFound();
            }
            return View(bdUnidadEjercicio);
        }

        // GET: UnidadEjercicio/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            UnidadEjercicio bdUnidadEjercicio = new UnidadEjercicio();
            return View(bdUnidadEjercicio);
        }

        // POST: UnidadEjercicio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,Abreviacion")] UnidadEjercicio unidadEjercicio)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.UnidadEjercicio.Any(x => x.Abreviacion.Trim().ToUpper() == unidadEjercicio.Abreviacion.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Abreviacion", "Ya existe una Unidad con esa Abreviatura");
                }
                else if (db.UnidadEjercicio.Any(x => x.Nombre.Trim().ToUpper() == unidadEjercicio.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Unidad con ese Nombre");
                }
                else
                {
                    db.UnidadEjercicio.Add(unidadEjercicio);
                    db.SaveChanges();
                    return RedirectToAction("Index", "UnidadEjercicio", new { sSearchString = unidadEjercicio.Nombre });
                }
            }

            return View(unidadEjercicio);
        }

        // GET: UnidadEjercicio/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadEjercicio bdUnidadEjercicio = db.UnidadEjercicio.Find(id);
            if (bdUnidadEjercicio == null)
            {
                return HttpNotFound();
            }
            return View(bdUnidadEjercicio);
        }

        // POST: UnidadEjercicio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,Abreviacion")] UnidadEjercicio unidadEjercicio)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.UnidadEjercicio.Any(x => x.Abreviacion.Trim().ToUpper() == unidadEjercicio.Abreviacion.Trim().ToUpper() && x.ID != unidadEjercicio.ID))
                {
                    ModelState.AddModelError("Abreviacion", "Ya existe una Unidad con esa Abreviatura");
                }
                else if (db.UnidadEjercicio.Any(x => x.Nombre.Trim().ToUpper() == unidadEjercicio.Nombre.Trim().ToUpper() && x.ID != unidadEjercicio.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Unidad con ese Nombre");
                }
                else
                {
                    db.Entry(unidadEjercicio).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "UnidadEjercicio", new { sSearchString = unidadEjercicio.Nombre });
                }
            }
            return View(unidadEjercicio);
        }

        // GET: UnidadEjercicio/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadEjercicio bdUnidadEjercicio = db.UnidadEjercicio.Find(id);
            if (bdUnidadEjercicio == null)
            {
                return HttpNotFound();
            }
            return View(bdUnidadEjercicio);
        }

        // POST: UnidadEjercicio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            UnidadEjercicio bdUnidadEjercicio = db.UnidadEjercicio.Find(id);
            if (bdUnidadEjercicio.DiasConLaUnidad.Count == 0)
            {
                db.UnidadEjercicio.Remove(bdUnidadEjercicio);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "UnidadEjercicio", new { pagina = ViewBag.Pagina });
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