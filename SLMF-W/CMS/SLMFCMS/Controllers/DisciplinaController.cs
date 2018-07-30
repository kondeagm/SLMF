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
    public class DisciplinaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string sFolderImagesAssets = ConfigurationManager.AppSettings["App_FolderAssetsImages"];

        private int pageSize = 4;

        // GET: Disciplina
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            ViewBag.SiglasSortParm = sSortOrder == "siglas" ? "siglas_desc" : "siglas";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsDisciplina = from dbr in db.Disciplina
                                 select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsDisciplina = rowsDisciplina.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsDisciplina = rowsDisciplina.OrderByDescending(s => s.Nombre);
                    break;

                case "siglas":
                    rowsDisciplina = rowsDisciplina.OrderBy(s => s.Siglas);
                    break;

                case "siglas_desc":
                    rowsDisciplina = rowsDisciplina.OrderByDescending(s => s.Siglas);
                    break;

                default:
                    rowsDisciplina = rowsDisciplina.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsDisciplina.ToPagedList(pageNumber, pageSize));
        }

        // GET: Disciplina/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            return View(bdDisciplina);
        }

        // GET: Disciplina/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Disciplina bdDisciplina = new Disciplina();
            bdDisciplina.Visible = false;
            return View(bdDisciplina);
        }

        // POST: Disciplina/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,Siglas,Slogan,Proposito,FileImage,IconImage,LogoSVG,SiglasImage,ColorCode,ImageEntrenar,Visible")] Disciplina disciplina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Disciplina.Any(x => x.Nombre.Trim().ToUpper() == disciplina.Nombre.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Disciplina con ese Nombre");
                }
                else if (db.Disciplina.Any(x => x.Siglas.Trim().ToUpper() == disciplina.Siglas.Trim().ToUpper()))
                {
                    ModelState.AddModelError("Siglas", "Ya existe una Disciplina con esas Siglas");
                }
                else
                {
                    db.Disciplina.Add(disciplina);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Disciplina", new { sSearchString = disciplina.Nombre });
                }
            }
            return View(disciplina);
        }

        // GET: Disciplina/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            return View(bdDisciplina);
        }

        // POST: Disciplina/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,Siglas,Slogan,Proposito,FileImage,IconImage,LogoSVG,SiglasImage,ColorCode,ImageEntrenar,Visible")] Disciplina disciplina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Disciplina.Any(x => x.Nombre.Trim().ToUpper() == disciplina.Nombre.Trim().ToUpper() && x.ID != disciplina.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Disciplina con ese Nombre");
                }
                else if (db.Disciplina.Any(x => x.Siglas.Trim().ToUpper() == disciplina.Siglas.Trim().ToUpper() && x.ID != disciplina.ID))
                {
                    ModelState.AddModelError("Siglas", "Ya existe una Disciplina con esas Siglas");
                }
                else
                {
                    db.Entry(disciplina).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Disciplina", new { sSearchString = disciplina.Nombre });
                }
            }
            return View(disciplina);
        }

        // GET: Disciplina/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            return View(bdDisciplina);
        }

        // POST: Disciplina/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina.CuestionarioDeLaDisciplina.Count == 0 && bdDisciplina.EventosDeLaDisciplina.Count == 0 && bdDisciplina.PlanesDeLaDisciplina.Count == 0)
            {
                ViewBag.Pagina = 1;
                db.Disciplina.Remove(bdDisciplina);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Disciplina", new { pagina = ViewBag.Pagina });
        }

        // FUNCIONALIDADES EXTRAS

        // GET: Disciplina/Show/5
        public ActionResult Show(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            if (bdDisciplina.Definido == true)
            {
                bdDisciplina.Visible = true;
                db.Entry(bdDisciplina).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Disciplina", new { pagina = ViewBag.Pagina });
        }

        // GET: Disciplina/Hide/5
        public ActionResult Hide(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            bdDisciplina.Visible = false;
            db.Entry(bdDisciplina).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Disciplina", new { pagina = ViewBag.Pagina });
        }

        // GET: /Disciplina/AddImage
        public ActionResult AddImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            return View(bdDisciplina);
        }

        // GET: /Disciplina/AddIcon
        public ActionResult AddIcon(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            return View(bdDisciplina);
        }

        // GET: /Disciplina/AddAcronym
        public ActionResult AddAcronym(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            return View(bdDisciplina);
        }

        // GET: /Disciplina/AddImageTraining
        public ActionResult AddImageTraining(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            return View(bdDisciplina);
        }

        // POST: /Disciplina/UploadImage/5
        [HttpPost]
        public JObject UploadImage(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int disciplina = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "La Disciplina no Existe");
                }
                else
                {
                    disciplina = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Imagen Anexa");
                    }
                    else if (!Funcion.DisciplineExist(disciplina))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "La Disciplina no Existe");
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
                            ProcessFileImage(disciplina, image, sExtension);
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

        // POST: /Disciplina/UploadIcon/5
        [HttpPost]
        public JObject UploadIcon(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int disciplina = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "La Disciplina no Existe");
                }
                else
                {
                    disciplina = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Icono Anexo");
                    }
                    else if (!Funcion.DisciplineExist(disciplina))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "La Disciplina no Existe");
                    }
                    else
                    {
                        string sExtension = System.IO.Path.GetExtension(file.FileName).Substring(1);
                        Stream stream = file.InputStream;
                        Image image = Image.FromStream(stream);
                        if (image.Height != 90 || image.Width != 45)
                        {
                            ArcJson = Funcion.CreateJsonResponse(1, "El Icono no tiene las medidas Correctas");
                        }
                        else
                        {
                            ProcessFileIcon(disciplina, image, sExtension);
                            ArcJson = Funcion.CreateJsonResponse(0, "El Icono se subio Correctamente");
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

        // POST: /Disciplina/UploadAcronym/5
        [HttpPost]
        public JObject UploadAcronym(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int disciplina = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "La Disciplina no Existe");
                }
                else
                {
                    disciplina = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Archivo Anexo");
                    }
                    else if (!Funcion.DisciplineExist(disciplina))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "La Disciplina no Existe");
                    }
                    else
                    {
                        string sExtension = System.IO.Path.GetExtension(file.FileName).Substring(1);
                        if (sExtension != "svg")
                        {
                            ArcJson = Funcion.CreateJsonResponse(1, "El Archivo no es un SVG");
                        }
                        else
                        {
                            ProcessFileAcronym(disciplina, file);
                            ArcJson = Funcion.CreateJsonResponse(0, "El Archivo se subio Correctamente");
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

        // POST: /Disciplina/UploadImageTraining/5
        [HttpPost]
        public JObject UploadImageTraining(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int disciplina = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "La Disciplina no Existe");
                }
                else
                {
                    disciplina = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Imagen Anexa");
                    }
                    else if (!Funcion.DisciplineExist(disciplina))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "La Disciplina no Existe");
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
                            ProcessFileTraining(disciplina, image, sExtension);
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

        // GET: /Disciplina/DeleteImage
        public ActionResult DeleteImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            return View(bdDisciplina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina.FileImage != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesAssets + bdDisciplina.FileImage);
                bdDisciplina.FileImage = null;
                db.Entry(bdDisciplina).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "Disciplina", new { id = bdDisciplina.ID, pagina = ViewBag.Pagina });
        }

        // GET: /Disciplina/DeleteIcon
        public ActionResult DeleteIcon(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            return View(bdDisciplina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteIcon(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina.IconImage != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesAssets + bdDisciplina.IconImage);
                bdDisciplina.IconImage = null;
                db.Entry(bdDisciplina).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "Disciplina", new { id = bdDisciplina.ID, pagina = ViewBag.Pagina });
        }

        // GET: /Disciplina/DeleteAcronym
        public ActionResult DeleteAcronym(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            return View(bdDisciplina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAcronym(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina.SiglasImage != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesAssets + bdDisciplina.SiglasImage);
                bdDisciplina.SiglasImage = null;
                db.Entry(bdDisciplina).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "Disciplina", new { id = bdDisciplina.ID, pagina = ViewBag.Pagina });
        }

        // GET: /Disciplina/DeleteImageTraining
        public ActionResult DeleteImageTraining(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina == null)
            {
                return HttpNotFound();
            }
            return View(bdDisciplina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImageTraining(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Disciplina bdDisciplina = db.Disciplina.Find(id);
            if (bdDisciplina.ImageEntrenar != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesAssets + bdDisciplina.ImageEntrenar);
                bdDisciplina.ImageEntrenar = null;
                db.Entry(bdDisciplina).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "Disciplina", new { id = bdDisciplina.ID, pagina = ViewBag.Pagina });
        }

        private void ProcessFileImage(int pDisciplinaId, Image pImage, string pExtension)
        {
            Disciplina bdDisciplina = db.Disciplina.Find(pDisciplinaId);
            string sToFile = "";
            string sNameFile = "slmf-portada-" + Funcion.NameEncode(bdDisciplina.Siglas).Trim().ToLower();
            bdDisciplina.FileImage = sNameFile + "." + pExtension;
            db.Entry(bdDisciplina).State = EntityState.Modified;
            db.SaveChanges();
            sToFile = Server.MapPath(sFolderImagesAssets + sNameFile + "." + pExtension);
            pImage.Save(sToFile);
        }

        private void ProcessFileIcon(int pDisciplinaId, Image pImage, string pExtension)
        {
            Disciplina bdDisciplina = db.Disciplina.Find(pDisciplinaId);
            string sToFile = "";
            string sNameFile = "slmf-icono-" + Funcion.NameEncode(bdDisciplina.Siglas).Trim().ToLower();
            bdDisciplina.IconImage = sNameFile + "." + pExtension;
            db.Entry(bdDisciplina).State = EntityState.Modified;
            db.SaveChanges();
            sToFile = Server.MapPath(sFolderImagesAssets + sNameFile + "." + pExtension);
            pImage.Save(sToFile);
        }

        private void ProcessFileAcronym(int pDisciplinaId, HttpPostedFileBase fileToUpload)
        {
            Disciplina bdDisciplina = db.Disciplina.Find(pDisciplinaId);
            string sToFile = "";
            string sTypeFile = System.IO.Path.GetExtension(fileToUpload.FileName).Substring(1);
            string sNameFile = "slmf-siglas-" + Funcion.NameEncode(bdDisciplina.Siglas).Trim().ToLower();
            bdDisciplina.SiglasImage = sNameFile + "." + sTypeFile;
            db.Entry(bdDisciplina).State = EntityState.Modified;
            db.SaveChanges();
            sToFile = Server.MapPath(sFolderImagesAssets + sNameFile + "." + sTypeFile);
            System.IO.File.WriteAllBytes(sToFile, ReadData(fileToUpload.InputStream));
        }

        private void ProcessFileTraining(int pDisciplinaId, Image pImage, string pExtension)
        {
            Disciplina bdDisciplina = db.Disciplina.Find(pDisciplinaId);
            string sToFile = "";
            string sNameFile = "slmf-entrenar-" + Funcion.NameEncode(bdDisciplina.Siglas).Trim().ToLower();
            bdDisciplina.ImageEntrenar = sNameFile + "." + pExtension;
            db.Entry(bdDisciplina).State = EntityState.Modified;
            db.SaveChanges();
            sToFile = Server.MapPath(sFolderImagesAssets + sNameFile + "." + pExtension);
            pImage.Save(sToFile);
        }

        private byte[] ReadData(Stream stream)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
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