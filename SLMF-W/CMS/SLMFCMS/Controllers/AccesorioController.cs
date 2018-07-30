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
    public class AccesorioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: Accesorio
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
            var rowsAccesorio = from dbr in db.Accesorio
                                select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsAccesorio = rowsAccesorio.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsAccesorio = rowsAccesorio.OrderByDescending(s => s.Nombre);
                    break;

                default:
                    rowsAccesorio = rowsAccesorio.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsAccesorio.ToPagedList(pageNumber, pageSize));
        }

        // GET: Accesorio/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accesorio bdAccesorio = db.Accesorio.Find(id);
            if (bdAccesorio == null)
            {
                return HttpNotFound();
            }
            return View(bdAccesorio);
        }

        // GET: Accesorio/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Accesorio bdAccesorio = new Accesorio();
            return View(bdAccesorio);
        }

        // POST: Accesorio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,Abreviacion")] Accesorio accesorio)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Accesorio.Any(x => x.Abreviacion.Trim().ToUpper() == accesorio.Abreviacion.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Abreviacion", "Ya existe un Accesorio con esa Abreviatura");
                }
                else if (db.Accesorio.Any(x => x.Nombre.Trim().ToUpper() == accesorio.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Accesorio con ese Nombre");
                }
                else
                {
                    db.Accesorio.Add(accesorio);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Accesorio", new { sSearchString = accesorio.Nombre });
                }
            }

            return View(accesorio);
        }

        // GET: Accesorio/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Accesorio bdAccesorio = db.Accesorio.Find(id);
            if (bdAccesorio == null)
            {
                return HttpNotFound();
            }
            return View(bdAccesorio);
        }

        // POST: Accesorio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,Abreviacion")] Accesorio accesorio)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Accesorio.Any(x => x.Abreviacion.Trim().ToUpper() == accesorio.Abreviacion.Trim().ToUpper() && x.ID != accesorio.ID))
                {
                    ModelState.AddModelError("Abreviacion", "Ya existe un Accesorio con esa Abreviatura");
                }
                else if (db.Accesorio.Any(x => x.Nombre.Trim().ToUpper() == accesorio.Nombre.Trim().ToUpper() && x.ID != accesorio.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Accesorio con ese Nombre");
                }
                else
                {
                    db.Entry(accesorio).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Accesorio", new { sSearchString = accesorio.Nombre });
                }
            }
            return View(accesorio);
        }

        // GET: Accesorio/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accesorio bdAccesorio = db.Accesorio.Find(id);
            if (bdAccesorio == null)
            {
                return HttpNotFound();
            }
            return View(bdAccesorio);
        }

        // POST: Accesorio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Accesorio bdAccesorio = db.Accesorio.Find(id);
            if (bdAccesorio.EjerciciosConElAccesorio.Count == 0)
            {
                db.Accesorio.Remove(bdAccesorio);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Accesorio", new { pagina = ViewBag.Pagina });
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