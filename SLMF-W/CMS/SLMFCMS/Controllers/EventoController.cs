using PagedList;
using SLMFCMS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin, EventAdmin")]
    public class EventoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: Evento
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            ViewBag.DisciplinaSortParm = sSortOrder == "disciplina" ? "disciplina_desc" : "disciplina";
            ViewBag.FechaSortParm = sSortOrder == "fecha" ? "fecha_desc" : "fecha";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsEvento = from dbr in db.Evento.Include(e => e.Disciplina)
                             select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsEvento = rowsEvento.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsEvento = rowsEvento.OrderByDescending(s => s.Nombre).ThenBy(s => s.Disciplina.Nombre).ThenBy(s => s.Fecha);
                    break;

                case "disciplina":
                    rowsEvento = rowsEvento.OrderBy(s => s.Disciplina.Nombre).ThenBy(s => s.Nombre).ThenBy(s => s.Fecha);
                    break;

                case "disciplina_desc":
                    rowsEvento = rowsEvento.OrderByDescending(s => s.Disciplina.Nombre).ThenBy(s => s.Nombre).ThenBy(s => s.Fecha);
                    break;

                case "fecha":
                    rowsEvento = rowsEvento.OrderBy(s => s.Fecha).ThenBy(s => s.Nombre);
                    break;

                case "fecha_desc":
                    rowsEvento = rowsEvento.OrderByDescending(s => s.Fecha).ThenBy(s => s.Nombre);
                    break;

                default:
                    rowsEvento = rowsEvento.OrderBy(s => s.Nombre).ThenBy(s => s.Disciplina.Nombre).ThenBy(s => s.Fecha);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsEvento.ToPagedList(pageNumber, pageSize));
        }

        // GET: Evento/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento bdEvento = db.Evento.Find(id);
            if (bdEvento == null)
            {
                return HttpNotFound();
            }
            return View(bdEvento);
        }

        // GET: Evento/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Evento bdEvento = new Evento();
            bdEvento.Fecha = System.DateTime.Today;
            bdEvento.Visible = false;
            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre");
            return View(bdEvento);
        }

        // POST: Evento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Titulo,Nombre,Fecha,Lugar,Direccion,DisciplinaID,URL,Visible")] Evento evento)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                db.Evento.Add(evento);
                db.SaveChanges();
                return RedirectToAction("Index", "Evento", new { sSearchString = evento.Nombre });
            }
            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre", evento.DisciplinaID);
            return View(evento);
        }

        // GET: Evento/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento bdEvento = db.Evento.Find(id);
            if (bdEvento == null)
            {
                return HttpNotFound();
            }
            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre", bdEvento.DisciplinaID);
            return View(bdEvento);
        }

        // POST: Evento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Titulo,Nombre,Fecha,Lugar,Direccion,DisciplinaID,URL,Visible")] Evento evento)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                db.Entry(evento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Evento", new { sSearchString = evento.Nombre });
            }
            ViewBag.DisciplinaID = new SelectList(db.Disciplina.OrderBy(s => s.Nombre), "ID", "Nombre", evento.DisciplinaID);
            return View(evento);
        }

        // GET: Evento/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento bdEvento = db.Evento.Find(id);
            if (bdEvento == null)
            {
                return HttpNotFound();
            }
            return View(bdEvento);
        }

        // POST: Evento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Evento bdEvento = db.Evento.Find(id);
            db.Evento.Remove(bdEvento);
            db.SaveChanges();
            return RedirectToAction("Index", "Evento", new { pagina = ViewBag.Pagina });
        }

        // GET: Evento/Show/5
        public ActionResult Show(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento bdEvento = db.Evento.Find(id);
            if (bdEvento == null)
            {
                return HttpNotFound();
            }
            bdEvento.Visible = true;
            db.Entry(bdEvento).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Evento", new { pagina = ViewBag.Pagina });
        }

        // GET: Evento/Hide/5
        public ActionResult Hide(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Evento bdEvento = db.Evento.Find(id);
            if (bdEvento == null)
            {
                return HttpNotFound();
            }
            bdEvento.Visible = false;
            db.Entry(bdEvento).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Evento", new { pagina = ViewBag.Pagina });
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