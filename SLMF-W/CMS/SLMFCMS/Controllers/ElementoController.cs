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
    public class ElementoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: Elemento
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
            var rowsElemento = from dbr in db.Elemento
                               select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsElemento = rowsElemento.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsElemento = rowsElemento.OrderByDescending(s => s.Nombre);
                    break;

                default:
                    rowsElemento = rowsElemento.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsElemento.ToPagedList(pageNumber, pageSize));
        }

        // GET: Elemento/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elemento bdElemento = db.Elemento.Find(id);
            if (bdElemento == null)
            {
                return HttpNotFound();
            }
            return View(bdElemento);
        }

        // GET: Elemento/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Elemento bdElemento = new Elemento();
            return View(bdElemento);
        }

        // POST: Elemento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,Abreviacion")] Elemento elemento)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Elemento.Any(x => x.Abreviacion.Trim().ToUpper() == elemento.Abreviacion.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Abreviacion", "Ya existe un Elemento con esa Abreviatura");
                }
                else if (db.Elemento.Any(x => x.Nombre.Trim().ToUpper() == elemento.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Elemento con ese Nombre");
                }
                else
                {
                    db.Elemento.Add(elemento);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Elemento", new { sSearchString = elemento.Nombre });
                }
            }

            return View(elemento);
        }

        // GET: Elemento/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Elemento bdElemento = db.Elemento.Find(id);
            if (bdElemento == null)
            {
                return HttpNotFound();
            }
            return View(bdElemento);
        }

        // POST: Elemento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,Abreviacion")] Elemento elemento)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Elemento.Any(x => x.Abreviacion.Trim().ToUpper() == elemento.Abreviacion.Trim().ToUpper() && x.ID != elemento.ID))
                {
                    ModelState.AddModelError("Abreviacion", "Ya existe un Elemento con esa Abreviatura");
                }
                else if (db.Elemento.Any(x => x.Nombre.Trim().ToUpper() == elemento.Nombre.Trim().ToUpper() && x.ID != elemento.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Elemento con ese Nombre");
                }
                else
                {
                    db.Entry(elemento).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Elemento", new { sSearchString = elemento.Nombre });
                }
            }
            return View(elemento);
        }

        // GET: Elemento/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elemento bdElemento = db.Elemento.Find(id);
            if (bdElemento == null)
            {
                return HttpNotFound();
            }
            return View(bdElemento);
        }

        // POST: Elemento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Elemento bdElemento = db.Elemento.Find(id);
            if (bdElemento.EjerciciosConElElemento.Count == 0)
            {
                db.Elemento.Remove(bdElemento);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Elemento", new { pagina = ViewBag.Pagina });
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