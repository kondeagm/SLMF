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
    [Authorize(Roles = "PageAdmin")]
    public class GrupoMusculosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string sFolderImagesAssets = ConfigurationManager.AppSettings["App_FolderAssetsImages"];
        private int pageSize = 8;

        // GET: GrupoMusculos
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
            var rowsGrupoMusculos = from dbr in db.GrupoMusculos
                                    select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsGrupoMusculos = rowsGrupoMusculos.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsGrupoMusculos = rowsGrupoMusculos.OrderByDescending(s => s.Nombre);
                    break;

                default:
                    rowsGrupoMusculos = rowsGrupoMusculos.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsGrupoMusculos.ToPagedList(pageNumber, pageSize));
        }

        // GET: GrupoMusculos/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrupoMusculos bdGrupoMusculos = db.GrupoMusculos.Find(id);
            if (bdGrupoMusculos == null)
            {
                return HttpNotFound();
            }
            return View(bdGrupoMusculos);
        }

        // GET: GrupoMusculos/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            GrupoMusculos bdGrupoMusculos = new GrupoMusculos();
            return View(bdGrupoMusculos);
        }

        // POST: GrupoMusculos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,Descripcion,FileImage")] GrupoMusculos grupoMusculos)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.GrupoMusculos.Any(x => x.Nombre.Trim().ToUpper() == grupoMusculos.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Grupo de Musculos con ese Nombre");
                }
                else
                {
                    db.GrupoMusculos.Add(grupoMusculos);
                    db.SaveChanges();
                    return RedirectToAction("Index", "GrupoMusculos", new { sSearchString = grupoMusculos.Nombre });
                }
            }
            return View(grupoMusculos);
        }

        // GET: GrupoMusculos/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            GrupoMusculos bdGrupoMusculos = db.GrupoMusculos.Find(id);
            if (bdGrupoMusculos == null)
            {
                return HttpNotFound();
            }
            return View(bdGrupoMusculos);
        }

        // POST: GrupoMusculos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,Descripcion,FileImage")] GrupoMusculos grupoMusculos)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.GrupoMusculos.Any(x => x.Nombre.Trim().ToUpper() == grupoMusculos.Nombre.Trim().ToUpper() && x.ID != grupoMusculos.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Grupo de Musculos con ese Nombre");
                }
                else
                {
                    db.Entry(grupoMusculos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "GrupoMusculos", new { sSearchString = grupoMusculos.Nombre });
                }
            }
            return View(grupoMusculos);
        }

        // GET: GrupoMusculos/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrupoMusculos bdGrupoMusculos = db.GrupoMusculos.Find(id);
            if (bdGrupoMusculos == null)
            {
                return HttpNotFound();
            }
            return View(bdGrupoMusculos);
        }

        // POST: GrupoMusculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            GrupoMusculos bdGrupoMusculos = db.GrupoMusculos.Find(id);
            if (bdGrupoMusculos.MusculosDelGrupo.Count == 0 && bdGrupoMusculos.RutinasDelGrupo.Count == 0)
            {
                db.GrupoMusculos.Remove(bdGrupoMusculos);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "GrupoMusculos", new { pagina = ViewBag.Pagina });
        }

        // GET: GrupoMusculos/AddImage
        public ActionResult AddImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrupoMusculos bdGrupoMusculos = db.GrupoMusculos.Find(id);
            if (bdGrupoMusculos == null)
            {
                return HttpNotFound();
            }
            return View(bdGrupoMusculos);
        }

        // POST: GrupoMusculos/UploadImage/5
        [HttpPost]
        public JObject UploadImage(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int musculos = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "El Grupo de Musculos no Existe");
                }
                else
                {
                    musculos = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Imagen Anexa");
                    }
                    else if (!Funcion.GroupMuscleExist(musculos))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "El Grupo de Musculos no Existe");
                    }
                    else
                    {
                        string sExtension = System.IO.Path.GetExtension(file.FileName).Substring(1);
                        Stream stream = file.InputStream;
                        Image image = Image.FromStream(stream);
                        if (image.Height != 1024 || image.Width != 1400)
                        {
                            ArcJson = Funcion.CreateJsonResponse(1, "La Imagen no tiene las medidas Correctas");
                        }
                        else
                        {
                            ProcessFileImage(musculos, image, sExtension);
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
            GrupoMusculos bdGrupoMusculos = db.GrupoMusculos.Find(id);
            if (bdGrupoMusculos == null)
            {
                return HttpNotFound();
            }
            return View(bdGrupoMusculos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            GrupoMusculos bdGrupoMusculos = db.GrupoMusculos.Find(id);
            if (bdGrupoMusculos.FileImage != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesAssets + bdGrupoMusculos.FileImage);
                bdGrupoMusculos.FileImage = null;
                db.Entry(bdGrupoMusculos).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "GrupoMusculos", new { id = bdGrupoMusculos.ID, pagina = ViewBag.Pagina });
        }

        private void ProcessFileImage(int pMusculosId, Image pImage, string pExtension)
        {
            GrupoMusculos bdGrupoMusculos = db.GrupoMusculos.Find(pMusculosId);
            string sToFile = "";
            string sNameFile = "slmf-anatomia-" + Funcion.NameEncode(bdGrupoMusculos.Nombre).Trim().ToLower();
            bdGrupoMusculos.FileImage = sNameFile + "." + pExtension;
            db.Entry(bdGrupoMusculos).State = EntityState.Modified;
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