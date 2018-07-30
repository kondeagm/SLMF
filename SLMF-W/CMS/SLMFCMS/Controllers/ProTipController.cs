using Newtonsoft.Json.Linq;
using PagedList;
using SLMFCMS.Functions;
using SLMFCMS.Models;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin, ContentAdmin")]
    public class ProTipController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string sFolderImagesRoutines = ConfigurationManager.AppSettings["App_FolderRoutinesImages"];
        private int pageSize = 6;

        // GET: ProTip
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
            var rowsProTip = from dbr in db.ProTip
                             select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsProTip = rowsProTip.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsProTip = rowsProTip.OrderByDescending(s => s.Nombre);
                    break;

                default:
                    rowsProTip = rowsProTip.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsProTip.ToPagedList(pageNumber, pageSize));
        }

        // GET: ProTip/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProTip bdProTip = db.ProTip.Find(id);
            if (bdProTip == null)
            {
                return HttpNotFound();
            }
            return View(bdProTip);
        }

        // GET: ProTip/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            ProTip bdProTip = new ProTip();
            return View(bdProTip);
        }

        // POST: ProTip/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,Descripcion,Autor,VimeoID,FileImage")] ProTip proTip)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.ProTip.Any(x => x.Nombre.Trim().ToUpper() == proTip.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Protip con ese Nombre");
                }
                else
                {
                    db.ProTip.Add(proTip);
                    db.SaveChanges();
                    return RedirectToAction("Index", "ProTip", new { sSearchString = proTip.Nombre });
                }
            }

            return View(proTip);
        }

        // GET: ProTip/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProTip bdProTip = db.ProTip.Find(id);
            if (bdProTip == null)
            {
                return HttpNotFound();
            }
            return View(bdProTip);
        }

        // POST: ProTip/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,Descripcion,Autor,VimeoID,FileImage")] ProTip proTip)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.ProTip.Any(x => x.Nombre.Trim().ToUpper() == proTip.Nombre.Trim().ToUpper() && x.ID != proTip.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Protip con ese Nombre");
                }
                else
                {
                    db.Entry(proTip).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "ProTip", new { sSearchString = proTip.Nombre });
                }
            }
            return View(proTip);
        }

        // GET: ProTip/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProTip bdProTip = db.ProTip.Find(id);
            if (bdProTip == null)
            {
                return HttpNotFound();
            }
            return View(bdProTip);
        }

        // POST: ProTip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            ProTip bdProTip = db.ProTip.Find(id);
            if (bdProTip.DiasConElProTip.Count == 0)
            {
                db.ProTip.Remove(bdProTip);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "ProTip", new { pagina = ViewBag.Pagina });
        }

        // GET: ProTip/AddImage
        public ActionResult AddImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProTip bdProTip = db.ProTip.Find(id);
            if (bdProTip == null)
            {
                return HttpNotFound();
            }
            return View(bdProTip);
        }

        // POST: ProTip/UploadImage/5
        [HttpPost]
        public JObject UploadImage(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int protip = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "El Pro-Tip no Existe");
                }
                else
                {
                    protip = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Imagen Anexa");
                    }
                    else if (!Funcion.ProTipExist(protip))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "El Pro-Tip no Existe");
                    }
                    else
                    {
                        string sExtension = System.IO.Path.GetExtension(file.FileName).Substring(1);
                        Stream stream = file.InputStream;
                        Image image = Image.FromStream(stream);
                        if (image.Height != 720 || image.Width != 1280)
                        {
                            ArcJson = Funcion.CreateJsonResponse(1, "La Imagen no tiene las medidas Correctas");
                        }
                        else
                        {
                            ProcessFileImage(protip, image, sExtension);
                            ArcJson = Funcion.CreateJsonResponse(0, "La Imagen se subio Correctamente");
                        }
                    }
                }
                return ArcJson;
            }
            catch (Exception)
            {
                ArcJson = Funcion.CreateJsonResponse(1, "Ocurrio un Error grave en el Server de Imagenes");
                return ArcJson;
            }
        }

        // GET: Producto/DeleteImage
        public ActionResult DeleteImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProTip bdProTip = db.ProTip.Find(id);
            if (bdProTip == null)
            {
                return HttpNotFound();
            }
            return View(bdProTip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            ProTip bdProTip = db.ProTip.Find(id);
            if (bdProTip.FileImage != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesRoutines + bdProTip.FileImage);
                bdProTip.FileImage = null;
                db.Entry(bdProTip).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "ProTip", new { id = bdProTip.ID, pagina = ViewBag.Pagina });
        }

        private void ProcessFileImage(int pProTipId, Image pImage, string pExtension)
        {
            ProTip bdProTip = db.ProTip.Find(pProTipId);
            string sToFile = "";
            string sNameFile = "slmf-protip-" + Funcion.NameEncode(bdProTip.Nombre).Trim().ToLower();
            bdProTip.FileImage = sNameFile + "." + pExtension;
            db.Entry(bdProTip).State = EntityState.Modified;
            db.SaveChanges();
            sToFile = Server.MapPath(sFolderImagesRoutines + sNameFile + "." + pExtension);
            pImage.Save(sToFile);
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