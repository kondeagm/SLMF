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
    public class TempoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: Tempo
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.SecuenciaSortParm = String.IsNullOrEmpty(sSortOrder) ? "secuencia_desc" : "";
            ViewBag.NameSortParm = sSortOrder == "nombre" ? "nombre_desc" : "nombre";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsTempo = from dbr in db.Tempo
                            select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsTempo = rowsTempo.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "secuencia_desc":
                    rowsTempo = rowsTempo.OrderByDescending(s => s.Secuencia).ThenBy(s => s.Nombre);
                    break;

                case "nombre_desc":
                    rowsTempo = rowsTempo.OrderByDescending(s => s.Nombre).ThenBy(s => s.Secuencia);
                    break;

                case "nombre":
                    rowsTempo = rowsTempo.OrderBy(s => s.Nombre).ThenBy(s => s.Secuencia);
                    break;

                default:
                    rowsTempo = rowsTempo.OrderBy(s => s.Secuencia).ThenBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsTempo.ToPagedList(pageNumber, pageSize));
        }

        // GET: Tempo/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tempo bdTempo = db.Tempo.Find(id);
            if (bdTempo == null)
            {
                return HttpNotFound();
            }
            return View(bdTempo);
        }

        // GET: Tempo/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            int iNextTiempo = 0;
            int iRegTiempo = db.Tempo.Count();
            if (iRegTiempo == 0)
            {
                iNextTiempo = 0;
            }
            else
            {
                iNextTiempo = db.Tempo.Select(y => y.Secuencia).Max();
            }
            Tempo bdTempo = new Tempo();
            bdTempo.Complementario = false;
            bdTempo.Secuencia = iNextTiempo + 1;
            return View(bdTempo);
        }

        // POST: Tempo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Secuencia,Prefijo,Nombre,Complementario")] Tempo tempo)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Tempo.Any(x => x.Nombre.Trim().ToUpper() == tempo.Nombre.Trim().ToUpper() && x.Prefijo.Trim().ToUpper() == tempo.Prefijo.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Tempo con ese Nombre y Prefijo");
                }
                else
                {
                    db.Tempo.Add(tempo);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Tempo", new { sSearchString = tempo.Nombre });
                }
            }
            return View(tempo);
        }

        // GET: Tempo/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tempo bdTempo = db.Tempo.Find(id);
            if (bdTempo == null)
            {
                return HttpNotFound();
            }
            return View(bdTempo);
        }

        // POST: Tempo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Secuencia,Prefijo,Nombre,Complementario")] Tempo tempo)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Tempo.Any(x => x.Nombre.Trim().ToUpper() == tempo.Nombre.Trim().ToUpper() && x.Prefijo.Trim().ToUpper() == tempo.Prefijo.Trim().ToUpper() && x.ID != tempo.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Tempo con ese Nombre y Prefijo");
                }
                else
                {
                    db.Entry(tempo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Tempo", new { sSearchString = tempo.Nombre });
                }
            }
            return View(tempo);
        }

        // GET: Tempo/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tempo bdTempo = db.Tempo.Find(id);
            if (bdTempo == null)
            {
                return HttpNotFound();
            }
            return View(bdTempo);
        }

        // POST: Tempo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Tempo bdTempo = db.Tempo.Find(id);
            if (bdTempo.DietasConElTempo.Count == 0)
            {
                db.Tempo.Remove(bdTempo);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Tempo", new { pagina = ViewBag.Pagina });
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