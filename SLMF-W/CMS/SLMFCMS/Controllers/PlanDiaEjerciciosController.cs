using SLMFCMS.Functions;
using SLMFCMS.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin, ContentAdmin")]
    public class PlanDiaEjerciciosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PlanDiaEjercicios/Series/5
        public ActionResult Series(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanDiaEjercicios bdPlanDiaEjercicios = db.PlanDiaEjercicios.Find(id);
            if (bdPlanDiaEjercicios == null)
            {
                return HttpNotFound();
            }
            return View(bdPlanDiaEjercicios);
        }

        // GET: PlanDiaEjercicios/Create
        public ActionResult Create(int? dia, int? pagina)
        {
            if (dia == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanDias bdPlanDias = db.PlanDias.Find(dia);
            if (bdPlanDias == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pagina = (pagina ?? 1);
            int iNextExercise = Funcion.GetLastExcerciseDay(dia) + 1;
            PlanDiaEjercicios bdPlanDiaEjercicios = new PlanDiaEjercicios();
            bdPlanDiaEjercicios.PlanDiasID = Convert.ToInt32(dia);
            bdPlanDiaEjercicios.Secuencia = iNextExercise;
            ViewBag.EjercicioID = new SelectList(Funcion.GetListaEjercicios(), "Value", "Text");
            ViewBag.Series = new SelectList(Funcion.GetListaSeries(), "Value", "Text");
            ViewBag.UnidadEjercicioID = new SelectList(db.UnidadEjercicio.OrderBy(s => s.Nombre), "ID", "Nombre");
            ViewBag.Descanso = new SelectList(Funcion.GetListaMinutos(), "Value", "Text");
            ViewBag.Nivel = new SelectList(Funcion.GetListaNiveles(), "Value", "Text");
            return View(bdPlanDiaEjercicios);
        }

        // POST: PlanDiaEjercicios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,PlanDiasID,Secuencia,EjercicioID,Series,Repeticiones,UnidadEjercicioID,Descanso,Nivel,Nota")] PlanDiaEjercicios planDiaEjercicios)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                db.PlanDiaEjercicios.Add(planDiaEjercicios);
                db.SaveChanges();
                CreaSeriesEjercicio(planDiaEjercicios);
                return RedirectToAction("Exercices", "PlanDias", new { id = planDiaEjercicios.PlanDiasID, pagina = ViewBag.Pagina });
            }
            ViewBag.EjercicioID = new SelectList(Funcion.GetListaEjercicios(), "Value", "Text", planDiaEjercicios.EjercicioID);
            ViewBag.Series = new SelectList(Funcion.GetListaSeries(), "Value", "Text", planDiaEjercicios.Series);
            ViewBag.UnidadEjercicioID = new SelectList(db.UnidadEjercicio.OrderBy(s => s.Nombre), "ID", "Nombre", planDiaEjercicios.UnidadEjercicioID);
            ViewBag.Descanso = new SelectList(Funcion.GetListaMinutos(), "Value", "Text", planDiaEjercicios.Descanso);
            ViewBag.Nivel = new SelectList(Funcion.GetListaNiveles(), "Value", "Text", planDiaEjercicios.Nivel);
            return View(planDiaEjercicios);
        }

        // GET: PlanDiaEjercicios/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanDiaEjercicios bdPlanDiaEjercicios = db.PlanDiaEjercicios.Find(id);
            if (bdPlanDiaEjercicios == null)
            {
                return HttpNotFound();
            }
            ViewBag.EjercicioID = new SelectList(Funcion.GetListaEjercicios(), "Value", "Text", bdPlanDiaEjercicios.EjercicioID);
            ViewBag.Series = new SelectList(Funcion.GetListaSeries(), "Value", "Text", bdPlanDiaEjercicios.Series);
            ViewBag.UnidadEjercicioID = new SelectList(db.UnidadEjercicio.OrderBy(s => s.Nombre), "ID", "Nombre", bdPlanDiaEjercicios.UnidadEjercicioID);
            ViewBag.Descanso = new SelectList(Funcion.GetListaMinutos(), "Value", "Text", bdPlanDiaEjercicios.Descanso);
            ViewBag.Nivel = new SelectList(Funcion.GetListaNiveles(), "Value", "Text", bdPlanDiaEjercicios.Nivel);
            return View(bdPlanDiaEjercicios);
        }

        // POST: PlanDiaEjercicios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,PlanDiasID,Secuencia,EjercicioID,Series,Repeticiones,UnidadEjercicioID,Descanso,Nivel,Nota")] PlanDiaEjercicios planDiaEjercicios)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                int iNoSeriesRegistradas = db.PlanDiaEjercicioSeries.Where(x => x.PlanDiaEjerciciosID == planDiaEjercicios.ID).Count();
                int iNoSeriesAGenerar = planDiaEjercicios.Series;
                db.Entry(planDiaEjercicios).State = EntityState.Modified;
                db.SaveChanges();
                AjustaSeriesEjercicio(planDiaEjercicios, iNoSeriesRegistradas, iNoSeriesAGenerar);
                return RedirectToAction("Exercices", "PlanDias", new { id = planDiaEjercicios.PlanDiasID, pagina = ViewBag.Pagina });
            }
            ViewBag.EjercicioID = new SelectList(Funcion.GetListaEjercicios(), "Value", "Text", planDiaEjercicios.EjercicioID);
            ViewBag.Series = new SelectList(Funcion.GetListaSeries(), "Value", "Text", planDiaEjercicios.Series);
            ViewBag.UnidadEjercicioID = new SelectList(db.UnidadEjercicio.OrderBy(s => s.Nombre), "ID", "Nombre", planDiaEjercicios.UnidadEjercicioID);
            ViewBag.Descanso = new SelectList(Funcion.GetListaMinutos(), "Value", "Text", planDiaEjercicios.Descanso);
            ViewBag.Nivel = new SelectList(Funcion.GetListaNiveles(), "Value", "Text", planDiaEjercicios.Nivel);
            return View(planDiaEjercicios);
        }

        // GET: PlanDiaEjercicios/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanDiaEjercicios bdPlanDiaEjercicios = db.PlanDiaEjercicios.Find(id);
            if (bdPlanDiaEjercicios == null)
            {
                return HttpNotFound();
            }
            return View(bdPlanDiaEjercicios);
        }

        // POST: PlanDiaEjercicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            PlanDiaEjercicios bdPlanDiaEjercicios = db.PlanDiaEjercicios.Find(id);
            EliminaSeriesEjercicio(bdPlanDiaEjercicios);
            int iPlanDiasId = bdPlanDiaEjercicios.PlanDiasID;
            db.PlanDiaEjercicios.Remove(bdPlanDiaEjercicios);
            db.SaveChanges();
            return RedirectToAction("Exercices", "PlanDias", new { id = iPlanDiasId, pagina = ViewBag.Pagina });
        }

        private void EliminaSeriesEjercicio(PlanDiaEjercicios pPlanDiaEjercicios)
        {
            int iNoSeries = pPlanDiaEjercicios.Series;
            for (int i = 1; i < iNoSeries + 1; i++)
            {
                PlanDiaEjercicioSeries bdPlanDiaEjercicioSeries = db.PlanDiaEjercicioSeries.Where(x => x.PlanDiaEjerciciosID == pPlanDiaEjercicios.ID && x.Secuencia == i).First();
                db.PlanDiaEjercicioSeries.Remove(bdPlanDiaEjercicioSeries);
                db.SaveChanges();
            }
        }

        private void CreaSeriesEjercicio(PlanDiaEjercicios pPlanDiaEjercicios)
        {
            int iNoSeries = pPlanDiaEjercicios.Series;
            PlanDiaEjercicioSeries bdPlanDiaEjercicioSeries = new PlanDiaEjercicioSeries();
            bdPlanDiaEjercicioSeries.PlanDiaEjerciciosID = pPlanDiaEjercicios.ID;
            bdPlanDiaEjercicioSeries.Repeticiones = pPlanDiaEjercicios.Repeticiones;
            bdPlanDiaEjercicioSeries.Nivel = pPlanDiaEjercicios.Nivel;
            for (int i = 1; i < iNoSeries + 1; i++)
            {
                bdPlanDiaEjercicioSeries.Secuencia = i;
                db.PlanDiaEjercicioSeries.Add(bdPlanDiaEjercicioSeries);
                db.SaveChanges();
            }
        }

        private void AjustaSeriesEjercicio(PlanDiaEjercicios pPlanDiaEjercicios, int pNoSeriesRegistradas, int pNoSeriesAGenerar)
        {
            if (pNoSeriesRegistradas > pNoSeriesAGenerar)
            {
                EliminaSeriesExtraEjercicio(pPlanDiaEjercicios, pNoSeriesRegistradas, pNoSeriesAGenerar);
            }
            else if (pNoSeriesRegistradas < pNoSeriesAGenerar)
            {
                AgregaSeriesExtraEjercicio(pPlanDiaEjercicios, pNoSeriesRegistradas, pNoSeriesAGenerar);
            }
        }

        private void EliminaSeriesExtraEjercicio(PlanDiaEjercicios pPlanDiaEjercicios, int pNoSeriesRegistradas, int pNoSeriesAGenerar)
        {
            for (int i = pNoSeriesRegistradas; i > pNoSeriesAGenerar; i--)
            {
                PlanDiaEjercicioSeries bdPlanDiaEjercicioSeries = db.PlanDiaEjercicioSeries.Where(x => x.PlanDiaEjerciciosID == pPlanDiaEjercicios.ID && x.Secuencia == i).First();
                db.PlanDiaEjercicioSeries.Remove(bdPlanDiaEjercicioSeries);
                db.SaveChanges();
            }
        }

        private void AgregaSeriesExtraEjercicio(PlanDiaEjercicios pPlanDiaEjercicios, int pNoSeriesRegistradas, int pNoSeriesAGenerar)
        {
            PlanDiaEjercicioSeries bdPlanDiaEjercicioSeries = new PlanDiaEjercicioSeries();
            bdPlanDiaEjercicioSeries.PlanDiaEjerciciosID = pPlanDiaEjercicios.ID;
            bdPlanDiaEjercicioSeries.Repeticiones = pPlanDiaEjercicios.Repeticiones;
            bdPlanDiaEjercicioSeries.Nivel = pPlanDiaEjercicios.Nivel;
            for (int i = pNoSeriesRegistradas; i < pNoSeriesAGenerar; i++)
            {
                bdPlanDiaEjercicioSeries.Secuencia = i + 1;
                db.PlanDiaEjercicioSeries.Add(bdPlanDiaEjercicioSeries);
                db.SaveChanges();
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