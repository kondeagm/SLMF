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
    public class PorcionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: Porcion
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "descripcion_desc" : "";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsPorcion = from dbr in db.Porcion
                              select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsPorcion = rowsPorcion.Where(s => s.Descripcion.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "descripcion_desc":
                    rowsPorcion = rowsPorcion.OrderByDescending(s => s.Descripcion);
                    break;

                default:
                    rowsPorcion = rowsPorcion.OrderBy(s => s.Descripcion);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsPorcion.ToPagedList(pageNumber, pageSize));
        }

        // GET: Porcion/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Porcion bdPorcion = db.Porcion.Find(id);
            if (bdPorcion == null)
            {
                return HttpNotFound();
            }
            return View(bdPorcion);
        }

        // GET: Porcion/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Porcion bdPorcion = new Porcion();
            return View(bdPorcion);
        }

        // POST: Porcion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Descripcion,Abreviacion")] Porcion porcion)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Porcion.Any(x => x.Abreviacion.Trim().ToUpper() == porcion.Abreviacion.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Abreviacion", "Ya existe una Porción con esa Abreviatura");
                }
                else if (db.Porcion.Any(x => x.Descripcion.Trim().ToUpper() == porcion.Descripcion.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Descripcion", "Ya existe una Porción con esa Descripcion");
                }
                else
                {
                    db.Porcion.Add(porcion);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Porcion", new { sSearchString = porcion.Descripcion });
                }
            }
            return View(porcion);
        }

        // GET: Porcion/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Porcion bdPorcion = db.Porcion.Find(id);
            if (bdPorcion == null)
            {
                return HttpNotFound();
            }
            return View(bdPorcion);
        }

        // POST: Porcion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Descripcion,Abreviacion")] Porcion porcion)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Porcion.Any(x => x.Abreviacion.Trim().ToUpper() == porcion.Abreviacion.Trim().ToUpper() && x.ID != porcion.ID))
                {
                    ModelState.AddModelError("Abreviacion", "Ya existe una Porción con esa Abreviatura");
                }
                else if (db.Porcion.Any(x => x.Descripcion.Trim().ToUpper() == porcion.Descripcion.Trim().ToUpper() && x.ID != porcion.ID))
                {
                    ModelState.AddModelError("Descripcion", "Ya existe una Porción con esa Descripcion");
                }
                else
                {
                    db.Entry(porcion).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Porcion", new { sSearchString = porcion.Descripcion });
                }
            }
            return View(porcion);
        }

        // GET: Porcion/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Porcion bdPorcion = db.Porcion.Find(id);
            if (bdPorcion == null)
            {
                return HttpNotFound();
            }
            return View(bdPorcion);
        }

        // POST: Porcion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Porcion bdPorcion = db.Porcion.Find(id);
            if (bdPorcion.DietasConLaPorcion.Count == 0)
            {
                db.Porcion.Remove(bdPorcion);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Porcion", new { pagina = ViewBag.Pagina });
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