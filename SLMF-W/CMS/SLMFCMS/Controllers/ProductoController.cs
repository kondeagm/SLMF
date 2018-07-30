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
    public class ProductoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string sFolderImagesProducts = ConfigurationManager.AppSettings["App_FolderProductsImages"];
        private int pageSize = 8;

        // GET: Producto
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            ViewBag.NutrienteSortParm = sSortOrder == "categoria" ? "categoria_desc" : "categoria";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsProducto = from dbr in db.Producto.Include(p => p.Nutriente)
                               select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsProducto = rowsProducto.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsProducto = rowsProducto.OrderByDescending(s => s.Nombre).ThenBy(s => s.Nutriente.Nombre);
                    break;

                case "categoria":
                    rowsProducto = rowsProducto.OrderBy(s => s.Nutriente.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "categoria_desc":
                    rowsProducto = rowsProducto.OrderByDescending(s => s.Nutriente.Nombre).ThenBy(s => s.Nombre);
                    break;

                default:
                    rowsProducto = rowsProducto.OrderBy(s => s.Nombre).ThenBy(s => s.Nutriente.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsProducto.ToPagedList(pageNumber, pageSize));
        }

        // GET: Producto/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto bdProducto = db.Producto.Find(id);
            if (bdProducto == null)
            {
                return HttpNotFound();
            }
            return View(bdProducto);
        }

        // GET: Producto/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Producto bdProducto = new Producto();
            ViewBag.NutrienteID = new SelectList(db.Nutriente.OrderBy(p => p.Nombre).Where(p => p.ID < 4), "ID", "Nombre");
            return View(bdProducto);
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,FileImage,URL,NutrienteID")] Producto producto)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Producto.Any(x => x.Nombre.Trim().ToUpper() == producto.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Producto con ese Nombre");
                }
                else
                {
                    db.Producto.Add(producto);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Producto", new { sSearchString = producto.Nombre });
                }
            }
            ViewBag.NutrienteID = new SelectList(db.Nutriente.OrderBy(s => s.Nombre).Where(p => p.ID < 4), "ID", "Nombre", producto.NutrienteID);
            return View(producto);
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Producto bdProducto = db.Producto.Find(id);
            if (bdProducto == null)
            {
                return HttpNotFound();
            }
            ViewBag.NutrienteID = new SelectList(db.Nutriente.OrderBy(s => s.Nombre).Where(p => p.ID < 4), "ID", "Nombre", bdProducto.NutrienteID);
            return View(bdProducto);
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,FileImage,URL,NutrienteID")] Producto producto)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Producto.Any(x => x.Nombre.Trim().ToUpper() == producto.Nombre.Trim().ToUpper() && x.ID != producto.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Producto con ese Nombre");
                }
                else
                {
                    db.Entry(producto).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Producto", new { sSearchString = producto.Nombre });
                }
            }
            ViewBag.NutrienteID = new SelectList(db.Nutriente.OrderBy(s => s.Nombre).Where(p => p.ID < 4), "ID", "Nombre", producto.NutrienteID);
            return View(producto);
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto bdProducto = db.Producto.Find(id);
            if (bdProducto == null)
            {
                return HttpNotFound();
            }
            return View(bdProducto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Producto bdProducto = db.Producto.Find(id);
            db.Producto.Remove(bdProducto);
            db.SaveChanges();
            return RedirectToAction("Index", "Producto", new { pagina = ViewBag.Pagina });
        }

        // GET: Producto/AddImage
        public ActionResult AddImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto bdProducto = db.Producto.Find(id);
            if (bdProducto == null)
            {
                return HttpNotFound();
            }
            return View(bdProducto);
        }

        // POST: Producto/UploadImage/5
        [HttpPost]
        public JObject UploadImage(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int producto = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "El Producto no Existe");
                }
                else
                {
                    producto = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Imagen Anexa");
                    }
                    else if (!Funcion.ProductExist(producto))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "El Producto no Existe");
                    }
                    else
                    {
                        string sExtension = System.IO.Path.GetExtension(file.FileName).Substring(1);
                        Stream stream = file.InputStream;
                        Image image = Image.FromStream(stream);
                        if (image.Height != 555 || image.Width != 360)
                        {
                            ArcJson = Funcion.CreateJsonResponse(1, "La Imagen no tiene las medidas Correctas");
                        }
                        else
                        {
                            ProcessFileImage(producto, image, sExtension);
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
            Producto bdProducto = db.Producto.Find(id);
            if (bdProducto == null)
            {
                return HttpNotFound();
            }
            return View(bdProducto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Producto bdProducto = db.Producto.Find(id);
            if (bdProducto.FileImage != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesProducts + bdProducto.FileImage);
                bdProducto.FileImage = null;
                db.Entry(bdProducto).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "Producto", new { id = bdProducto.ID, pagina = ViewBag.Pagina });
        }

        private void ProcessFileImage(int pProductoId, Image pImage, string pExtension)
        {
            Producto bdProducto = db.Producto.Find(pProductoId);
            string sToFile = "";
            string sNameFile = "slmf-suplementos-" + Funcion.NameEncode(bdProducto.Nombre).Trim().ToLower();
            bdProducto.FileImage = sNameFile + "." + pExtension;
            db.Entry(bdProducto).State = EntityState.Modified;
            db.SaveChanges();
            sToFile = Server.MapPath(sFolderImagesProducts + sNameFile + "." + pExtension);
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