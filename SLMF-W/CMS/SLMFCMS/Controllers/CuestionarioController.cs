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
    public class CuestionarioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: Cuestionario
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            ViewBag.DisciplinaSortParm = sSortOrder == "disciplina" ? "disciplina_desc" : "disciplina";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsCuestionario = from dbr in db.Cuestionario.Include(c => c.Disciplina)
                                   select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsCuestionario = rowsCuestionario.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsCuestionario = rowsCuestionario.OrderByDescending(s => s.Nombre).ThenBy(s => s.Disciplina.Nombre);
                    break;

                case "disciplina":
                    rowsCuestionario = rowsCuestionario.OrderBy(s => s.Disciplina.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "disciplina_desc":
                    rowsCuestionario = rowsCuestionario.OrderByDescending(s => s.Disciplina.Nombre).ThenBy(s => s.Nombre);
                    break;

                default:
                    rowsCuestionario = rowsCuestionario.OrderBy(s => s.Nombre).ThenBy(s => s.Disciplina.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsCuestionario.ToPagedList(pageNumber, pageSize));
        }

        // GET: Cuestionario/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuestionario bdCuestionario = db.Cuestionario.Find(id);
            if (bdCuestionario == null)
            {
                return HttpNotFound();
            }
            return View(bdCuestionario);
        }

        // GET: Cuestionario/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Cuestionario bdCuestionario = new Cuestionario();
            bdCuestionario.Visible = false;
            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre");
            return View(bdCuestionario);
        }

        // POST: Cuestionario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,DisciplinaID,Visible")] Cuestionario cuestionario)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Cuestionario.Any(x => x.Nombre.Trim().ToUpper() == cuestionario.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Cuestionario con ese Nombre");
                }
                else
                {
                    db.Cuestionario.Add(cuestionario);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Cuestionario", new { sSearchString = cuestionario.Nombre });
                }
            }

            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre", cuestionario.DisciplinaID);
            return View(cuestionario);
        }

        // GET: Cuestionario/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cuestionario bdCuestionario = db.Cuestionario.Find(id);
            if (bdCuestionario == null)
            {
                return HttpNotFound();
            }
            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre", bdCuestionario.DisciplinaID);
            return View(bdCuestionario);
        }

        // POST: Cuestionario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,DisciplinaID,Visible")] Cuestionario cuestionario)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Cuestionario.Any(x => x.Nombre.Trim().ToUpper() == cuestionario.Nombre.Trim().ToUpper() && x.ID != cuestionario.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Cuestionario con ese Nombre");
                }
                else
                {
                    db.Entry(cuestionario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Cuestionario", new { sSearchString = cuestionario.Nombre });
                }
            }
            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre", cuestionario.DisciplinaID);
            return View(cuestionario);
        }

        // GET: Cuestionario/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuestionario bdCuestionario = db.Cuestionario.Find(id);
            if (bdCuestionario == null)
            {
                return HttpNotFound();
            }
            return View(bdCuestionario);
        }

        // POST: Cuestionario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Cuestionario bdCuestionario = db.Cuestionario.Find(id);
            if (bdCuestionario.PreguntasDelCuestionario.Count == 0)
            {
                db.Cuestionario.Remove(bdCuestionario);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Cuestionario", new { pagina = ViewBag.Pagina });
        }

        // GET: Cuestionario/Show/5
        public ActionResult Show(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuestionario bdCuestionario = db.Cuestionario.Find(id);
            if (bdCuestionario == null)
            {
                return HttpNotFound();
            }
            if (bdCuestionario.Definido == true)
            {
                bdCuestionario.Visible = true;
                db.Entry(bdCuestionario).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Cuestionario", new { pagina = ViewBag.Pagina });
        }

        // GET: Cuestionario/Hide/5
        public ActionResult Hide(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuestionario bdCuestionario = db.Cuestionario.Find(id);
            if (bdCuestionario == null)
            {
                return HttpNotFound();
            }
            bdCuestionario.Visible = false;
            db.Entry(bdCuestionario).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Cuestionario", new { pagina = ViewBag.Pagina });
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