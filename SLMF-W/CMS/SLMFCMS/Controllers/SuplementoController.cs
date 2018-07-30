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
    public class SuplementoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: Suplemento
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            ViewBag.NutrienteSortParm = sSortOrder == "nutriente" ? "nutriente_desc" : "nutriente";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsAlimento = from s in db.Alimento.Where(p => p.Suplemento == true && p.Preparado == false).Include(p => p.Nutriente)
                               select s;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsAlimento = rowsAlimento.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsAlimento = rowsAlimento.OrderByDescending(s => s.Nombre).ThenBy(s => s.Nutriente.Nombre);
                    break;

                case "nutriente":
                    rowsAlimento = rowsAlimento.OrderBy(s => s.Nutriente.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "nutriente_desc":
                    rowsAlimento = rowsAlimento.OrderByDescending(s => s.Nutriente.Nombre).ThenBy(s => s.Nombre);
                    break;

                default:
                    rowsAlimento = rowsAlimento.OrderBy(s => s.Nombre).ThenBy(s => s.Nutriente.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsAlimento.ToPagedList(pageNumber, pageSize));
        }

        // GET: Suplemento/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alimento bdAlimento = db.Alimento.Where(p => p.ID == id && p.Suplemento == true && p.Preparado == false).First();
            if (bdAlimento == null)
            {
                return HttpNotFound();
            }
            return View(bdAlimento);
        }

        // GET: Suplemento/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Alimento bdAlimento = new Alimento();
            bdAlimento.Suplemento = true;
            bdAlimento.Preparado = false;
            ViewBag.NutrienteID = new SelectList(db.Nutriente.OrderBy(s => s.Nombre), "ID", "Nombre");
            return View(bdAlimento);
        }

        // POST: Suplemento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,NutrienteID,Suplemento,Preparado")] Alimento alimento)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Alimento.Any(x => x.Nombre.Trim().ToUpper() == alimento.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Suplemento con este Nombre");
                }
                else
                {
                    db.Alimento.Add(alimento);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Suplemento", new { sSearchString = alimento.Nombre });
                }
            }

            ViewBag.NutrienteID = new SelectList(db.Nutriente.OrderBy(s => s.Nombre), "ID", "Nombre", alimento.NutrienteID);
            return View(alimento);
        }

        // GET: Suplemento/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alimento bdAlimento = db.Alimento.Where(p => p.ID == id && p.Suplemento == true && p.Preparado == false).First();
            if (bdAlimento == null)
            {
                return HttpNotFound();
            }
            ViewBag.NutrienteID = new SelectList(db.Nutriente.OrderBy(s => s.Nombre), "ID", "Nombre", bdAlimento.NutrienteID);
            return View(bdAlimento);
        }

        // POST: Suplemento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,NutrienteID,Suplemento,Preparado")] Alimento alimento)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Alimento.Any(x => x.Nombre.Trim().ToUpper() == alimento.Nombre.Trim().ToUpper() && x.ID != alimento.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Suplemento con este Nombre");
                }
                else
                {
                    db.Entry(alimento).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Suplemento", new { sSearchString = alimento.Nombre });
                }
            }
            ViewBag.NutrienteID = new SelectList(db.Nutriente.OrderBy(s => s.Nombre), "ID", "Nombre", alimento.NutrienteID);
            return View(alimento);
        }

        // GET: Suplemento/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Alimento bdAlimento = db.Alimento.Where(p => p.ID == id && p.Suplemento == true && p.Preparado == false).First();
            if (bdAlimento == null)
            {
                return HttpNotFound();
            }
            return View(bdAlimento);
        }

        // POST: Suplemento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Alimento bdAlimento = db.Alimento.Where(p => p.ID == id && p.Suplemento == true && p.Preparado == false).First();
            if (bdAlimento.DietasConElAlimento.Count == 0)
            {
                db.Alimento.Remove(bdAlimento);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Suplemento", new { pagina = ViewBag.Pagina });
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