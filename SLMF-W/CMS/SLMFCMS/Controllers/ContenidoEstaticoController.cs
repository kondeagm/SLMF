using SLMFCMS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin")]
    public class ContenidoEstaticoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContenidoEstatico/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContenidoEstatico bdContenidoEstatico = db.ContenidoEstatico.Find(id);
            if (bdContenidoEstatico == null)
            {
                return HttpNotFound();
            }
            return View(bdContenidoEstatico);
        }

        // GET: ContenidoEstatico/Create
        public ActionResult Create(int? redsocial, int? pagina)
        {
            if (redsocial == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RedSocial bdRedSocial = db.RedSocial.Find(redsocial);
            if (bdRedSocial == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pagina = (pagina ?? 1);
            ContenidoEstatico bdContenidoEstatico = new ContenidoEstatico();
            bdContenidoEstatico.RedSocialID = Convert.ToInt32(redsocial);
            return View(bdContenidoEstatico);
        }

        // POST: ContenidoEstatico/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,RedSocialID,Identificador")] ContenidoEstatico contenidoEstatico)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.ContenidoEstatico.Any(x => x.Identificador.Trim().ToUpper() == contenidoEstatico.Identificador.Trim().ToUpper() && x.RedSocialID == contenidoEstatico.RedSocialID))
                {
                    ModelState.AddModelError("Identificador", "Ya existe un Contenido Estatico con ese Identificador en esta Red Social");
                }
                else
                {
                    db.ContenidoEstatico.Add(contenidoEstatico);
                    db.SaveChanges();
                    return RedirectToAction("Details", "RedSocial", new { id = contenidoEstatico.RedSocialID, pagina = ViewBag.Pagina });
                }
            }
            return View(contenidoEstatico);
        }

        // GET: ContenidoEstatico/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ContenidoEstatico bdContenidoEstatico = db.ContenidoEstatico.Find(id);
            if (bdContenidoEstatico == null)
            {
                return HttpNotFound();
            }
            ViewBag.RedSocialID = new SelectList(db.RedSocial.OrderBy(s => s.Nombre), "ID", "Nombre", bdContenidoEstatico.RedSocialID);
            return View(bdContenidoEstatico);
        }

        // POST: ContenidoEstatico/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,RedSocialID,Identificador")] ContenidoEstatico contenidoEstatico)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.ContenidoEstatico.Any(x => x.Identificador.Trim().ToUpper() == contenidoEstatico.Identificador.Trim().ToUpper() && x.RedSocialID == contenidoEstatico.RedSocialID && x.ID != contenidoEstatico.ID))
                {
                    ModelState.AddModelError("Identificador", "Ya existe un Contenido Estatico con ese Identificador en esta Red Social");
                }
                else
                {
                    db.Entry(contenidoEstatico).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", "RedSocial", new { id = contenidoEstatico.RedSocialID, pagina = ViewBag.Pagina });
                }
            }
            return View(contenidoEstatico);
        }

        // GET: ContenidoEstatico/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContenidoEstatico bdContenidoEstatico = db.ContenidoEstatico.Find(id);
            if (bdContenidoEstatico == null)
            {
                return HttpNotFound();
            }
            return View(bdContenidoEstatico);
        }

        // POST: ContenidoEstatico/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            ContenidoEstatico bdContenidoEstatico = db.ContenidoEstatico.Find(id);
            int iRedSocialId = bdContenidoEstatico.RedSocialID;
            db.ContenidoEstatico.Remove(bdContenidoEstatico);
            db.SaveChanges();
            return RedirectToAction("Details", "RedSocial", new { id = iRedSocialId, pagina = ViewBag.Pagina });
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