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
    public class ClienteController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 10;

        // GET: Cliente
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            ViewBag.ApellidoSortParm = sSortOrder == "apellidos" ? "apellidos_desc" : "apellidos";
            ViewBag.CorreoSortParm = sSortOrder == "correo" ? "correo_desc" : "correo";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsUsuario = from dbr in db.Usuario.Include(u => u.PlanDelUsuario)
                              select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsUsuario = rowsUsuario.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsUsuario = rowsUsuario.OrderByDescending(s => s.Nombre).ThenBy(s => s.Apellidos);
                    break;

                case "apellidos":
                    rowsUsuario = rowsUsuario.OrderBy(s => s.Apellidos).ThenBy(s => s.Nombre);
                    break;

                case "apellidos_desc":
                    rowsUsuario = rowsUsuario.OrderByDescending(s => s.Apellidos).ThenBy(s => s.Nombre);
                    break;

                case "correo":
                    rowsUsuario = rowsUsuario.OrderBy(s => s.Correo).ThenBy(s => s.Nombre);
                    break;

                case "correo_desc":
                    rowsUsuario = rowsUsuario.OrderByDescending(s => s.Correo).ThenBy(s => s.Nombre);
                    break;

                default:
                    rowsUsuario = rowsUsuario.OrderBy(s => s.Nombre).ThenBy(s => s.Apellidos);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsUsuario.ToPagedList(pageNumber, pageSize));
        }

        // GET: Cliente/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario bdUsuario = db.Usuario.Find(id);
            if (bdUsuario == null)
            {
                return HttpNotFound();
            }
            return View(bdUsuario);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario bdUsuario = db.Usuario.Find(id);
            if (bdUsuario == null)
            {
                return HttpNotFound();
            }
            return View(bdUsuario);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Usuario bdUsuario = db.Usuario.Find(id);
            db.Usuario.Remove(bdUsuario);
            db.SaveChanges();
            return RedirectToAction("Index", "Cliente", new { pagina = ViewBag.Pagina });
        }

        // GET: Cliente/ResetPlan/5
        public ActionResult ResetPlan(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario bdUsuario = db.Usuario.Find(id);
            if (bdUsuario == null)
            {
                return HttpNotFound();
            }
            bdUsuario.PlanID = null;
            bdUsuario.FechaInicioPlan = null;
            db.Entry(bdUsuario).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Cliente", new { pagina = ViewBag.Pagina });
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