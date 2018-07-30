using SLMFCMS.Functions;
using SLMFCMS.Models;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin, ContentAdmin")]
    public class PlanDiaEjercicioSeriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PlanDiaEjercicioSeries/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanDiaEjercicioSeries bdPlanDiaEjercicioSeries = db.PlanDiaEjercicioSeries.Find(id);
            if (bdPlanDiaEjercicioSeries == null)
            {
                return HttpNotFound();
            }
            ViewBag.Nivel = new SelectList(Funcion.GetListaNiveles(), "Value", "Text", bdPlanDiaEjercicioSeries.Nivel);
            return View(bdPlanDiaEjercicioSeries);
        }

        // POST: PlanDiaEjercicioSeries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,PlanDiaEjerciciosID,Secuencia,Repeticiones,Nivel")] PlanDiaEjercicioSeries planDiaEjercicioSeries)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                db.Entry(planDiaEjercicioSeries).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Series", "PlanDiaEjercicios", new { id = planDiaEjercicioSeries.PlanDiaEjerciciosID, pagina = ViewBag.Pagina });
            }
            ViewBag.Nivel = new SelectList(Funcion.GetListaNiveles(), "Value", "Text", planDiaEjercicioSeries.Nivel);
            return View(planDiaEjercicioSeries);
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