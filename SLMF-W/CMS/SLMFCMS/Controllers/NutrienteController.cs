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
    public class NutrienteController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: Nutriente
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsNutriente = from dbr in db.Nutriente
                                select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsNutriente = rowsNutriente.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsNutriente = rowsNutriente.OrderByDescending(s => s.Nombre);
                    break;

                default:
                    rowsNutriente = rowsNutriente.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsNutriente.ToPagedList(pageNumber, pageSize));
        }

        // GET: Nutriente/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nutriente bdNutriente = db.Nutriente.Find(id);
            if (bdNutriente == null)
            {
                return HttpNotFound();
            }
            return View(bdNutriente);
        }

        // GET: Nutriente/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Nutriente bdNutriente = new Nutriente();
            return View(bdNutriente);
        }

        // POST: Nutriente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,ColorCode")] Nutriente nutriente)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Nutriente.Any(x => x.Nombre.Trim().ToUpper() == nutriente.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Nutriente con ese Nombre");
                }
                else
                {
                    db.Nutriente.Add(nutriente);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Nutriente", new { sSearchString = nutriente.Nombre });
                }
            }
            return View(nutriente);
        }

        // GET: Nutriente/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nutriente bdNutriente = db.Nutriente.Find(id);
            if (bdNutriente == null)
            {
                return HttpNotFound();
            }
            return View(bdNutriente);
        }

        // POST: Nutriente/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,ColorCode")] Nutriente nutriente)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Nutriente.Any(x => x.Nombre.Trim().ToUpper() == nutriente.Nombre.Trim().ToUpper() && x.ID != nutriente.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Nutriente con ese Nombre");
                }
                else
                {
                    db.Entry(nutriente).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Nutriente", new { sSearchString = nutriente.Nombre });
                }
            }
            return View(nutriente);
        }

        // GET: Nutriente/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Nutriente bdNutriente = db.Nutriente.Find(id);
            if (bdNutriente == null)
            {
                return HttpNotFound();
            }
            return View(bdNutriente);
        }

        // POST: Nutriente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Nutriente bdNutriente = db.Nutriente.Find(id);
            if (bdNutriente.ProductosConElNutriente.Count == 0 && bdNutriente.AlimentosConElNutriente.Count == 0)
            {
                db.Nutriente.Remove(bdNutriente);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Nutriente", new { pagina = ViewBag.Pagina });
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