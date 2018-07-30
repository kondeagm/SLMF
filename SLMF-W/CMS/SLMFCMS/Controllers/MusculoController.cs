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
    public class MusculoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string sFolderImagesAssets = ConfigurationManager.AppSettings["App_FolderAssetsImages"];
        private int pageSize = 8;

        // GET: Musculo
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            ViewBag.GrupoSortParm = sSortOrder == "anatomia" ? "anatomia_desc" : "anatomia";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsMusculo = from dbr in db.Musculo.Include(m => m.GrupoMusculos)
                              select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsMusculo = rowsMusculo.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsMusculo = rowsMusculo.OrderByDescending(s => s.Nombre).ThenBy(s => s.GrupoMusculos.Nombre);
                    break;

                case "anatomia":
                    rowsMusculo = rowsMusculo.OrderBy(s => s.GrupoMusculos.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "anatomia_desc":
                    rowsMusculo = rowsMusculo.OrderByDescending(s => s.GrupoMusculos.Nombre).ThenBy(s => s.Nombre);
                    break;

                default:
                    rowsMusculo = rowsMusculo.OrderBy(s => s.Nombre).ThenBy(s => s.GrupoMusculos.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsMusculo.ToPagedList(pageNumber, pageSize));
        }

        // GET: Musculo/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Musculo bdMusculo = db.Musculo.Find(id);
            if (bdMusculo == null)
            {
                return HttpNotFound();
            }
            return View(bdMusculo);
        }

        // GET: Musculo/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Musculo bdMusculo = new Musculo();
            ViewBag.GrupoMusculosID = new SelectList(Funcion.GetListaGrupoMusculos(), "Value", "Text");
            return View(bdMusculo);
        }

        // POST: Musculo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,NombreComun,Descripcion,GrupoMusculosID,FileImage")] Musculo musculo)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Musculo.Any(x => x.Nombre.Trim().ToUpper() == musculo.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Musculo con ese Nombre");
                }
                else if (db.Musculo.Any(x => x.NombreComun.Trim().ToUpper() == musculo.NombreComun.Trim().ToUpper()))
                {
                    ModelState.AddModelError("NombreComun", "Ya existe un Musculo con ese Nombre Comun");
                }
                else
                {
                    db.Musculo.Add(musculo);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Musculo", new { sSearchString = musculo.Nombre });
                }
            }

            ViewBag.GrupoMusculosID = new SelectList(Funcion.GetListaGrupoMusculos(), "Value", "Text", musculo.GrupoMusculosID);
            return View(musculo);
        }

        // GET: Musculo/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Musculo bdMusculo = db.Musculo.Find(id);
            if (bdMusculo == null)
            {
                return HttpNotFound();
            }
            ViewBag.GrupoMusculosID = new SelectList(Funcion.GetListaGrupoMusculos(), "Value", "Text", bdMusculo.GrupoMusculosID);
            return View(bdMusculo);
        }

        // POST: Musculo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,NombreComun,Descripcion,GrupoMusculosID,FileImage")] Musculo musculo)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Musculo.Any(x => x.Nombre.Trim().ToUpper() == musculo.Nombre.Trim().ToUpper() && x.ID != musculo.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Musculo con ese Nombre");
                }
                else if (db.Musculo.Any(x => x.NombreComun.Trim().ToUpper() == musculo.NombreComun.Trim().ToUpper() && x.ID != musculo.ID))
                {
                    ModelState.AddModelError("NombreComun", "Ya existe un Musculo con ese Nombre Comun");
                }
                else
                {
                    db.Entry(musculo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Musculo", new { sSearchString = musculo.Nombre });
                }
            }
            ViewBag.GrupoMusculosID = new SelectList(Funcion.GetListaGrupoMusculos(), "Value", "Text", musculo.GrupoMusculosID);
            return View(musculo);
        }

        // GET: Musculo/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Musculo bdMusculo = db.Musculo.Find(id);
            if (bdMusculo == null)
            {
                return HttpNotFound();
            }
            return View(bdMusculo);
        }

        // POST: Musculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Musculo bdMusculo = db.Musculo.Find(id);
            if (bdMusculo.EjerciciosConElMusculo.Count == 0)
            {
                db.Musculo.Remove(bdMusculo);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Musculo", new { pagina = ViewBag.Pagina });
        }

        // GET: Musculo/AddImage
        public ActionResult AddImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Musculo bdMusculo = db.Musculo.Find(id);
            if (bdMusculo == null)
            {
                return HttpNotFound();
            }
            return View(bdMusculo);
        }

        // POST: Musculo/UploadImage/5
        [HttpPost]
        public JObject UploadImage(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int musculo = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "El Musculo no Existe");
                }
                else
                {
                    musculo = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Imagen Anexa");
                    }
                    else if (!Funcion.MuscleExist(musculo))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "El Musculo no Existe");
                    }
                    else
                    {
                        string sExtension = System.IO.Path.GetExtension(file.FileName).Substring(1);
                        Stream stream = file.InputStream;
                        Image image = Image.FromStream(stream);
                        if (image.Height != 166 || image.Width != 166)
                        {
                            ArcJson = Funcion.CreateJsonResponse(1, "La Imagen no tiene las medidas Correctas");
                        }
                        else
                        {
                            ProcessFileImage(musculo, image, sExtension);
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

        // GET: Musculo/DeleteImage
        public ActionResult DeleteImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Musculo bdMusculo = db.Musculo.Find(id);
            if (bdMusculo == null)
            {
                return HttpNotFound();
            }
            return View(bdMusculo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Musculo bdMusculo = db.Musculo.Find(id);
            if (bdMusculo.FileImage != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesAssets + bdMusculo.FileImage);
                bdMusculo.FileImage = null;
                db.Entry(bdMusculo).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "Musculo", new { id = bdMusculo.ID, pagina = ViewBag.Pagina });
        }

        private void ProcessFileImage(int pMusculoId, Image pImage, string pExtension)
        {
            Musculo bdMusculo = db.Musculo.Find(pMusculoId);
            string sToFile = "";
            string sNameFile = "slmf-musculo-" + Funcion.NameEncode(bdMusculo.Nombre).Trim().ToLower();
            bdMusculo.FileImage = sNameFile + "." + pExtension;
            db.Entry(bdMusculo).State = EntityState.Modified;
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