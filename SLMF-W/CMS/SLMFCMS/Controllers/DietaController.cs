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
    public class DietaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string sFolderImagesDiets = ConfigurationManager.AppSettings["App_FolderDietsImages"];
        private int pageSize = 6;

        // GET: Dieta
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
            var rowsDieta = from dbr in db.Dieta
                            select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsDieta = rowsDieta.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsDieta = rowsDieta.OrderByDescending(s => s.Nombre);
                    break;

                default:
                    rowsDieta = rowsDieta.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsDieta.ToPagedList(pageNumber, pageSize));
        }

        // GET: Dieta/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dieta bdDieta = db.Dieta.Find(id);
            if (bdDieta == null)
            {
                return HttpNotFound();
            }
            return View(bdDieta);
        }

        // GET: Dieta/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Dieta bdDieta = new Dieta();
            return View(bdDieta);
        }

        // POST: Dieta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,Descripcion,FileImage")] Dieta dieta)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Dieta.Any(x => x.Nombre.Trim().ToUpper() == dieta.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Dieta con ese Nombre");
                }
                else
                {
                    db.Dieta.Add(dieta);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Dieta", new { sSearchString = dieta.Nombre });
                }
            }
            return View(dieta);
        }

        // GET: Dieta/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Dieta bdDieta = db.Dieta.Find(id);
            if (bdDieta == null)
            {
                return HttpNotFound();
            }
            return View(bdDieta);
        }

        // POST: Dieta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,Descripcion,FileImage")] Dieta dieta)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Dieta.Any(x => x.Nombre.Trim().ToUpper() == dieta.Nombre.Trim().ToUpper() && x.ID != dieta.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Dieta con ese Nombre");
                }
                else
                {
                    db.Entry(dieta).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Dieta", new { sSearchString = dieta.Nombre });
                }
            }
            return View(dieta);
        }

        // GET: Dieta/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dieta bdDieta = db.Dieta.Find(id);
            if (bdDieta == null)
            {
                return HttpNotFound();
            }
            return View(bdDieta);
        }

        // POST: Dieta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Dieta bdDieta = db.Dieta.Find(id);
            if (bdDieta.DiasConLaDieta.Count == 0 && bdDieta.TemposDeLaDieta.Count == 0)
            {
                db.Dieta.Remove(bdDieta);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Dieta", new { pagina = ViewBag.Pagina });
        }

        // GET: Dieta/Tempos/5
        public ActionResult Tempos(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dieta bdDieta = db.Dieta.Find(id);
            if (bdDieta == null)
            {
                return HttpNotFound();
            }
            return View(bdDieta);
        }

        // GET: Dieta/AddImage
        public ActionResult AddImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dieta bdDieta = db.Dieta.Find(id);
            if (bdDieta == null)
            {
                return HttpNotFound();
            }
            return View(bdDieta);
        }

        // POST: Dieta/UploadImage/5
        [HttpPost]
        public JObject UploadImage(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int dieta = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "La Dieta no Existe");
                }
                else
                {
                    dieta = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Imagen Anexa");
                    }
                    else if (!Funcion.DietExist(dieta))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "La Dieta no Existe");
                    }
                    else
                    {
                        string sExtension = System.IO.Path.GetExtension(file.FileName).Substring(1);
                        Stream stream = file.InputStream;
                        Image image = Image.FromStream(stream);
                        if (image.Height != 1024 || image.Width != 1420)
                        {
                            ArcJson = Funcion.CreateJsonResponse(1, "La Imagen no tiene las medidas Correctas");
                        }
                        else
                        {
                            ProcessFileImage(dieta, image, sExtension);
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

        // GET: Dieta/DeleteImage
        public ActionResult DeleteImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dieta bdDieta = db.Dieta.Find(id);
            if (bdDieta == null)
            {
                return HttpNotFound();
            }
            return View(bdDieta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Dieta bdDieta = db.Dieta.Find(id);
            if (bdDieta.FileImage != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesDiets + bdDieta.FileImage);
                bdDieta.FileImage = null;
                db.Entry(bdDieta).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "Dieta", new { id = bdDieta.ID, pagina = ViewBag.Pagina });
        }

        private void ProcessFileImage(int pDietaId, Image pImage, string pExtension)
        {
            Dieta bdDieta = db.Dieta.Find(pDietaId);
            string sToFile = "";
            string sNameFile = "slmf-dieta-" + Funcion.NameEncode(bdDieta.Nombre).Trim().ToLower();
            bdDieta.FileImage = sNameFile + "." + pExtension;
            db.Entry(bdDieta).State = EntityState.Modified;
            db.SaveChanges();
            sToFile = Server.MapPath(sFolderImagesDiets + sNameFile + "." + pExtension);
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