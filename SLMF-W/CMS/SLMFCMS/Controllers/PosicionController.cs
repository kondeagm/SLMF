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
    public class PosicionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: Posicion
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
            var rowsPosicion = from dbr in db.Posicion
                               select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsPosicion = rowsPosicion.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsPosicion = rowsPosicion.OrderByDescending(s => s.Nombre);
                    break;

                default:
                    rowsPosicion = rowsPosicion.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsPosicion.ToPagedList(pageNumber, pageSize));
        }

        // GET: Posicion/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posicion bdPosicion = db.Posicion.Find(id);
            if (bdPosicion == null)
            {
                return HttpNotFound();
            }
            return View(bdPosicion);
        }

        // GET: Posicion/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Posicion bdPosicion = new Posicion();
            return View(bdPosicion);
        }

        // POST: Posicion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,Abreviacion")] Posicion posicion)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Posicion.Any(x => x.Abreviacion.Trim().ToUpper() == posicion.Abreviacion.Trim().ToUpper() && !String.IsNullOrEmpty(posicion.Abreviacion)))
                {
                    ModelState.AddModelError("Abreviacion", "Ya existe una Posición con esa Abreviatura");
                }
                else if (db.Posicion.Any(x => x.Nombre.Trim().ToUpper() == posicion.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Posición con ese Nombre");
                }
                else
                {
                    db.Posicion.Add(posicion);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Posicion", new { sSearchString = posicion.Nombre });
                }
            }

            return View(posicion);
        }

        // GET: Posicion/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Posicion bdPosicion = db.Posicion.Find(id);
            if (bdPosicion == null)
            {
                return HttpNotFound();
            }
            return View(bdPosicion);
        }

        // POST: Posicion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,Abreviacion")] Posicion posicion)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Posicion.Any(x => x.Abreviacion.Trim().ToUpper() == posicion.Abreviacion.Trim().ToUpper() && !String.IsNullOrEmpty(posicion.Abreviacion) && x.ID != posicion.ID))
                {
                    ModelState.AddModelError("Abreviacion", "Ya existe una Posición con esa Abreviatura");
                }
                else if (db.Posicion.Any(x => x.Nombre.Trim().ToUpper() == posicion.Nombre.Trim().ToUpper() && x.ID != posicion.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Posición con ese Nombre");
                }
                else
                {
                    db.Entry(posicion).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Posicion", new { sSearchString = posicion.Nombre });
                }
            }
            return View(posicion);
        }

        // GET: Posicion/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posicion bdPosicion = db.Posicion.Find(id);
            if (bdPosicion == null)
            {
                return HttpNotFound();
            }
            return View(bdPosicion);
        }

        // POST: Posicion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Posicion bdPosicion = db.Posicion.Find(id);
            if (bdPosicion.EjerciciosEnLaPosicion.Count == 0)
            {
                db.Posicion.Remove(bdPosicion);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Posicion", new { pagina = ViewBag.Pagina });
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