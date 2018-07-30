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
    public class RedSocialController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 8;

        // GET: RedSocial
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
            var rowsRedSocial = from dbr in db.RedSocial
                                select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsRedSocial = rowsRedSocial.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsRedSocial = rowsRedSocial.OrderByDescending(s => s.Nombre);
                    break;

                default:
                    rowsRedSocial = rowsRedSocial.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsRedSocial.ToPagedList(pageNumber, pageSize));
        }

        // GET: RedSocial/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RedSocial bdRedSocial = db.RedSocial.Find(id);
            if (bdRedSocial == null)
            {
                return HttpNotFound();
            }
            return View(bdRedSocial);
        }

        // GET: RedSocial/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            RedSocial bdRedSocial = new RedSocial();
            bdRedSocial.NoPost = 1;
            return View(bdRedSocial);
        }

        // POST: RedSocial/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,Identificador,URL,APPId,APIKey,NoPost")] RedSocial redSocial)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.RedSocial.Any(x => x.Nombre.Trim().ToUpper() == redSocial.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Red Social con ese Nombre");
                }
                else if (db.RedSocial.Any(x => x.Identificador.Trim().ToUpper() == redSocial.Identificador.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Identificador", "Ya existe una Red Social con ese Identificador");
                }
                else
                {
                    db.RedSocial.Add(redSocial);
                    db.SaveChanges();
                    return RedirectToAction("Index", "RedSocial", new { sSearchString = redSocial.Nombre });
                }
            }
            return View(redSocial);
        }

        // GET: RedSocial/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RedSocial bdRedSocial = db.RedSocial.Find(id);
            if (bdRedSocial == null)
            {
                return HttpNotFound();
            }
            return View(bdRedSocial);
        }

        // POST: RedSocial/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,Identificador,URL,APPId,APIKey,NoPost")] RedSocial redSocial)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.RedSocial.Any(x => x.Nombre.Trim().ToUpper() == redSocial.Nombre.Trim().ToUpper() && x.ID != redSocial.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Red Social con ese Nombre");
                }
                else if (db.RedSocial.Any(x => x.Identificador.Trim().ToUpper() == redSocial.Identificador.Trim().ToUpper() && x.ID != redSocial.ID))
                {
                    ModelState.AddModelError("Identificador", "Ya existe una Red Social con ese Identificador");
                }
                else
                {
                    db.Entry(redSocial).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "RedSocial", new { sSearchString = redSocial.Nombre });
                }
            }
            return View(redSocial);
        }

        // GET: RedSocial/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RedSocial bdRedSocial = db.RedSocial.Find(id);
            if (bdRedSocial == null)
            {
                return HttpNotFound();
            }
            return View(bdRedSocial);
        }

        // POST: RedSocial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            RedSocial bdRedSocial = db.RedSocial.Find(id);
            if (bdRedSocial.PostEnLaRedSocial.Count == 0)
            {
                db.RedSocial.Remove(bdRedSocial);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "RedSocial", new { pagina = ViewBag.Pagina });
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