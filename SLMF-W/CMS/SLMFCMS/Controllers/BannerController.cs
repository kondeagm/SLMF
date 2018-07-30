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
    public class BannerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string sFolderImagesAssets = ConfigurationManager.AppSettings["App_FolderAssetsImages"];
        private int pageSize = 8;

        // GET: Banner
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.IdSortParm = String.IsNullOrEmpty(sSortOrder) ? "identificador_desc" : "";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsBanner = from dbr in db.Banner
                             select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsBanner = rowsBanner.Where(s => s.Identificador.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "identificador_desc":
                    rowsBanner = rowsBanner.OrderByDescending(s => s.Identificador);
                    break;

                default:
                    rowsBanner = rowsBanner.OrderBy(s => s.Identificador);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsBanner.ToPagedList(pageNumber, pageSize));
        }

        // GET: Banner/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner bdBanner = db.Banner.Find(id);
            if (bdBanner == null)
            {
                return HttpNotFound();
            }
            return View(bdBanner);
        }

        // GET: Banner/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Banner bdBanner = new Banner();
            bdBanner.Prioridad = 0;
            bdBanner.Visible = false;
            return View(bdBanner);
        }

        // POST: Banner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Identificador,FileImage,LinkBanner,Prioridad,Visible")] Banner banner)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Banner.Any(x => x.Identificador.Trim().ToUpper() == banner.Identificador.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Identificador", "Ya existe un Banner con ese Identificador");
                }
                else
                {
                    db.Banner.Add(banner);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Banner", new { sSearchString = banner.Identificador });
                }
            }
            return View(banner);
        }

        // GET: Banner/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner bdBanner = db.Banner.Find(id);
            if (bdBanner == null)
            {
                return HttpNotFound();
            }
            return View(bdBanner);
        }

        // POST: Banner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Identificador,FileImage,LinkBanner,Prioridad,Visible")] Banner banner)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Banner.Any(x => x.Identificador.Trim().ToUpper() == banner.Identificador.Trim().ToUpper() && x.ID != banner.ID))
                {
                    ModelState.AddModelError("Identificador", "Ya existe un Banner con ese Identificador");
                }
                else
                {
                    db.Entry(banner).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Banner", new { sSearchString = banner.Identificador });
                }
            }
            return View(banner);
        }

        // GET: Banner/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner bdBanner = db.Banner.Find(id);
            if (bdBanner == null)
            {
                return HttpNotFound();
            }
            return View(bdBanner);
        }

        // POST: Banner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Banner bdBanner = db.Banner.Find(id);
            db.Banner.Remove(bdBanner);
            db.SaveChanges();
            return RedirectToAction("Index", "Banner", new { pagina = ViewBag.Pagina });
        }

        // GET: Banner/Show/5
        public ActionResult Show(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner bdBanner = db.Banner.Find(id);
            if (bdBanner == null)
            {
                return HttpNotFound();
            }
            if (bdBanner.Definido == true)
            {
                bdBanner.Visible = true;
                db.Entry(bdBanner).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Banner", new { pagina = ViewBag.Pagina });
        }

        // GET: Banner/Hide/5
        public ActionResult Hide(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner bdBanner = db.Banner.Find(id);
            if (bdBanner == null)
            {
                return HttpNotFound();
            }
            bdBanner.Visible = false;
            db.Entry(bdBanner).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Banner", new { pagina = ViewBag.Pagina });
        }

        // GET: Banner/AddImage
        public ActionResult AddImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner bdBanner = db.Banner.Find(id);
            if (bdBanner == null)
            {
                return HttpNotFound();
            }
            return View(bdBanner);
        }

        // POST: Banner/UploadImage/5
        [HttpPost]
        public JObject UploadImage(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int banner = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "El Banner no Existe");
                }
                else
                {
                    banner = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Imagen Anexa");
                    }
                    else if (!Funcion.ProductExist(banner))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "El Banner no Existe");
                    }
                    else
                    {
                        string sExtension = System.IO.Path.GetExtension(file.FileName).Substring(1);
                        Stream stream = file.InputStream;
                        Image image = Image.FromStream(stream);
                        if (image.Height != 125 || image.Width != 425)
                        {
                            ArcJson = Funcion.CreateJsonResponse(1, "La Imagen no tiene las medidas Correctas");
                        }
                        else
                        {
                            ProcessFileImage(banner, image, sExtension);
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

        // GET: Banner/DeleteImage
        public ActionResult DeleteImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner bdBanner = db.Banner.Find(id);
            if (bdBanner == null)
            {
                return HttpNotFound();
            }
            return View(bdBanner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Banner bdBanner = db.Banner.Find(id);
            if (bdBanner.FileImage != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesAssets + bdBanner.FileImage);
                bdBanner.FileImage = null;
                db.Entry(bdBanner).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "Banner", new { id = bdBanner.ID, pagina = ViewBag.Pagina });
        }

        private void ProcessFileImage(int pBannerId, Image pImage, string pExtension)
        {
            Banner bdBanner = db.Banner.Find(pBannerId);
            string sToFile = "";
            string sNameFile = "slmf-banner-" + Funcion.NameEncode(bdBanner.Identificador).Trim().ToLower();
            bdBanner.FileImage = sNameFile + "." + pExtension;
            db.Entry(bdBanner).State = EntityState.Modified;
            db.SaveChanges();
            sToFile = Server.MapPath(sFolderImagesAssets + sNameFile + "." + pExtension);
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