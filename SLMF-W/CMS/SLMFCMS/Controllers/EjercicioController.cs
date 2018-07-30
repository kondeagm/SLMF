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
    public class EjercicioController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string sFolderImagesRoutines = ConfigurationManager.AppSettings["App_FolderRoutinesImages"];
        private int pageSize = 8;

        // GET: Ejercicio
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            ViewBag.AccesorioSortParm = sSortOrder == "accesorio" ? "accesorio_desc" : "accesorio";
            ViewBag.ElementoSortParm = sSortOrder == "elemento" ? "elemento_desc" : "elemento";
            ViewBag.PosicionSortParm = sSortOrder == "posicion" ? "posicion_desc" : "posicion";
            ViewBag.MusculoSortParm = sSortOrder == "musculo" ? "musculo_desc" : "musculo";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsEjercicio = from dbr in db.Ejercicio.Include(e => e.AccesorioDelEjercicio).Include(e => e.ElementoDelEjercicio).Include(e => e.Musculo).Include(e => e.PosicionDelEjercicio)
                                select dbr;
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsEjercicio = rowsEjercicio.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsEjercicio = rowsEjercicio.OrderByDescending(s => s.Nombre).ThenBy(s => s.ElementoDelEjercicio.Nombre);
                    break;

                case "accesorio":
                    rowsEjercicio = rowsEjercicio.OrderBy(s => s.AccesorioDelEjercicio.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "accesorio_desc":
                    rowsEjercicio = rowsEjercicio.OrderByDescending(s => s.AccesorioDelEjercicio.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "elemento":
                    rowsEjercicio = rowsEjercicio.OrderBy(s => s.ElementoDelEjercicio.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "elemento_desc":
                    rowsEjercicio = rowsEjercicio.OrderByDescending(s => s.ElementoDelEjercicio.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "posicion":
                    rowsEjercicio = rowsEjercicio.OrderBy(s => s.PosicionDelEjercicio.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "posicion_desc":
                    rowsEjercicio = rowsEjercicio.OrderByDescending(s => s.PosicionDelEjercicio.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "musculo":
                    rowsEjercicio = rowsEjercicio.OrderBy(s => s.Musculo.Nombre).ThenBy(s => s.Nombre);
                    break;

                case "musculo_desc":
                    rowsEjercicio = rowsEjercicio.OrderByDescending(s => s.Musculo.Nombre).ThenBy(s => s.Nombre);
                    break;

                default:
                    rowsEjercicio = rowsEjercicio.OrderBy(s => s.Nombre).ThenBy(s => s.ElementoDelEjercicio.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsEjercicio.ToPagedList(pageNumber, pageSize));
        }

        // GET: Ejercicio/Details/5
        public ActionResult Details(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejercicio bdEjercicio = db.Ejercicio.Find(id);
            if (bdEjercicio == null)
            {
                return HttpNotFound();
            }
            return View(bdEjercicio);
        }

        // GET: Ejercicio/Create
        public ActionResult Create(int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Ejercicio bdEjercicio = new Ejercicio();
            ViewBag.AccesorioID = new SelectList(db.Accesorio.OrderBy(s => s.Nombre), "ID", "Nombre");
            ViewBag.ElementoID = new SelectList(db.Elemento.OrderBy(s => s.Nombre), "ID", "Nombre");
            ViewBag.MusculoID = new SelectList(db.Musculo.OrderBy(s => s.Nombre), "ID", "Nombre");
            ViewBag.PosicionID = new SelectList(db.Posicion.OrderBy(s => s.Nombre), "ID", "Nombre");
            return View(bdEjercicio);
        }

        // POST: Ejercicio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,Nombre,AccesorioID,ElementoID,PosicionID,MusculoID,VimeoID,FileImage")] Ejercicio ejercicio)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Ejercicio.Any(x => x.Nombre.Trim().ToUpper() == ejercicio.Nombre.Trim().ToUpper()
                    && x.AccesorioID == ejercicio.AccesorioID && x.ElementoID == ejercicio.ElementoID
                    && x.PosicionID == ejercicio.PosicionID && x.MusculoID == ejercicio.MusculoID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe un Ejercicio con ese Nombre");
                }
                else
                {
                    db.Ejercicio.Add(ejercicio);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Ejercicio", new { sSearchString = ejercicio.Nombre });
                }
            }
            ViewBag.AccesorioID = new SelectList(db.Accesorio.OrderBy(s => s.Nombre), "ID", "Nombre", ejercicio.AccesorioID);
            ViewBag.ElementoID = new SelectList(db.Elemento.OrderBy(s => s.Nombre), "ID", "Nombre", ejercicio.ElementoID);
            ViewBag.MusculoID = new SelectList(db.Musculo.OrderBy(s => s.Nombre), "ID", "Nombre", ejercicio.MusculoID);
            ViewBag.PosicionID = new SelectList(db.Posicion.OrderBy(s => s.Nombre), "ID", "Nombre", ejercicio.PosicionID);
            return View(ejercicio);
        }

        // GET: Ejercicio/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ejercicio bdEjercicio = db.Ejercicio.Find(id);
            if (bdEjercicio == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccesorioID = new SelectList(db.Accesorio.OrderBy(s => s.Nombre), "ID", "Nombre", bdEjercicio.AccesorioID);
            ViewBag.ElementoID = new SelectList(db.Elemento.OrderBy(s => s.Nombre), "ID", "Nombre", bdEjercicio.ElementoID);
            ViewBag.MusculoID = new SelectList(db.Musculo.OrderBy(s => s.Nombre), "ID", "Nombre", bdEjercicio.MusculoID);
            ViewBag.PosicionID = new SelectList(db.Posicion.OrderBy(s => s.Nombre), "ID", "Nombre", bdEjercicio.PosicionID);
            return View(bdEjercicio);
        }

        // POST: Ejercicio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,Nombre,AccesorioID,ElementoID,PosicionID,MusculoID,VimeoID,FileImage")] Ejercicio ejercicio)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.Ejercicio.Any(x => x.Nombre.Trim().ToUpper() == ejercicio.Nombre.Trim().ToUpper()
                    && x.AccesorioID == ejercicio.AccesorioID && x.ElementoID == ejercicio.ElementoID
                    && x.PosicionID == ejercicio.PosicionID && x.MusculoID == ejercicio.MusculoID && x.ID != ejercicio.ID))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una Ejercicio con ese Nombre");
                }
                else
                {
                    db.Entry(ejercicio).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Ejercicio", new { sSearchString = ejercicio.Nombre });
                }
            }
            ViewBag.AccesorioID = new SelectList(db.Accesorio.OrderBy(s => s.Nombre), "ID", "Nombre", ejercicio.AccesorioID);
            ViewBag.ElementoID = new SelectList(db.Elemento.OrderBy(s => s.Nombre), "ID", "Nombre", ejercicio.ElementoID);
            ViewBag.MusculoID = new SelectList(db.Musculo.OrderBy(s => s.Nombre), "ID", "Nombre", ejercicio.MusculoID);
            ViewBag.PosicionID = new SelectList(db.Posicion.OrderBy(s => s.Nombre), "ID", "Nombre", ejercicio.PosicionID);
            return View(ejercicio);
        }

        // GET: Ejercicio/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejercicio bdEjercicio = db.Ejercicio.Find(id);
            if (bdEjercicio == null)
            {
                return HttpNotFound();
            }
            return View(bdEjercicio);
        }

        // POST: Ejercicio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Ejercicio bdEjercicio = db.Ejercicio.Find(id);
            if (bdEjercicio.DiasConElEjercicio.Count == 0)
            {
                db.Ejercicio.Remove(bdEjercicio);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Ejercicio", new { pagina = ViewBag.Pagina });
        }

        // GET: Ejercicio/AddImage
        public ActionResult AddImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejercicio bdEjercicio = db.Ejercicio.Find(id);
            if (bdEjercicio == null)
            {
                return HttpNotFound();
            }
            return View(bdEjercicio);
        }

        // POST: Ejercicio/UploadImage/5
        [HttpPost]
        public JObject UploadImage(int? id, HttpPostedFileBase file)
        {
            JObject ArcJson = new JObject();
            int ejercicio = 0;
            try
            {
                if (id == null)
                {
                    ArcJson = Funcion.CreateJsonResponse(1, "El Ejercicio no Existe");
                }
                else
                {
                    ejercicio = (int)id;
                    if (file == null)
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "No hay Imagen Anexa");
                    }
                    else if (!Funcion.RoutineExist(ejercicio))
                    {
                        ArcJson = Funcion.CreateJsonResponse(1, "El Ejercicio no Existe");
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
                            ProcessFileImage(ejercicio, image, sExtension);
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

        // GET: Ejercicio/DeleteImage
        public ActionResult DeleteImage(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ejercicio bdEjercicio = db.Ejercicio.Find(id);
            if (bdEjercicio == null)
            {
                return HttpNotFound();
            }
            return View(bdEjercicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            Ejercicio bdEjercicio = db.Ejercicio.Find(id);
            if (bdEjercicio.FileImage != null)
            {
                string sArchivo = Server.MapPath(sFolderImagesRoutines + bdEjercicio.FileImage);
                bdEjercicio.FileImage = null;
                db.Entry(bdEjercicio).State = EntityState.Modified;
                db.SaveChanges();
                Funcion.EliminaArchivo(sArchivo);
            }
            return RedirectToAction("Details", "Ejercicio", new { id = bdEjercicio.ID, pagina = ViewBag.Pagina });
        }

        private void ProcessFileImage(int pEjercicioId, Image pImage, string pExtension)
        {
            Ejercicio bdEjercicio = db.Ejercicio.Find(pEjercicioId);
            string sToFile = "";
            string sNameFile = "slmf-ejercicio-" + Funcion.NameEncode(bdEjercicio.Nombre).Trim().ToLower();
            if (!String.IsNullOrEmpty(Convert.ToString(bdEjercicio.AccesorioID)))
            {
                sNameFile += "-" + Funcion.NameEncode(bdEjercicio.AccesorioDelEjercicio.Nombre).Trim().ToLower();
            }
            if (!String.IsNullOrEmpty(Convert.ToString(bdEjercicio.PosicionID)))
            {
                sNameFile += "-" + Funcion.NameEncode(bdEjercicio.PosicionDelEjercicio.Nombre).Trim().ToLower();
            }
            if (!String.IsNullOrEmpty(Convert.ToString(bdEjercicio.ElementoID)))
            {
                sNameFile += "-" + Funcion.NameEncode(bdEjercicio.ElementoDelEjercicio.Nombre).Trim().ToLower();
            }
            bdEjercicio.FileImage = sNameFile + "." + pExtension;
            db.Entry(bdEjercicio).State = EntityState.Modified;
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