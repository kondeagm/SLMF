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
    public class PlanDiasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PlanDias/Exercices/5
        public ActionResult Exercices(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanDias bdPlanDias = db.PlanDias.Find(id);
            if (bdPlanDias == null)
            {
                return HttpNotFound();
            }
            return View(bdPlanDias);
        }

        // GET: PlanDias/Create
        public ActionResult Create(int? plan, int? pagina)
        {
            if (plan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Plan bdPlan = db.Plan.Find(plan);
            if (bdPlan == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pagina = (pagina ?? 1);
            int iNextDay = Funcion.GetLastPlanDay(plan) + 1;
            PlanDias bdPlanDias = new PlanDias();
            bdPlanDias.PlanID = Convert.ToInt32(plan);
            bdPlanDias.Dia = iNextDay;
            ViewBag.DietaID = new SelectList(Funcion.GetListaDietas(), "Value", "Text");
            ViewBag.ProTipID = new SelectList(Funcion.GetListaProTips(), "Value", "Text");
            ViewBag.RutinaID = new SelectList(db.Rutina.OrderBy(s => s.Nombre), "ID", "Nombre");
            return View(bdPlanDias);
        }

        // POST: PlanDias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int? pagina, [Bind(Include = "ID,PlanID,Dia,RutinaID,DietaID,ProTipID,Descanso")] PlanDias planDias)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.PlanDias.Any(x => x.PlanID == planDias.PlanID && x.Dia == planDias.Dia))
                {
                    ModelState.AddModelError("Dia", "Ya existe ese número de día en este Plan");
                }
                else
                {
                    db.PlanDias.Add(planDias);
                    db.SaveChanges();
                    return RedirectToAction("Days", "Plan", new { id = planDias.PlanID, pagina = ViewBag.Pagina });
                }
            }
            ViewBag.DietaID = new SelectList(Funcion.GetListaDietas(), "Value", "Text", planDias.DietaID);
            ViewBag.ProTipID = new SelectList(Funcion.GetListaProTips(), "Value", "Text", planDias.ProTipID);
            ViewBag.RutinaID = new SelectList(db.Rutina.OrderBy(s => s.Nombre), "ID", "Nombre", planDias.RutinaID);
            return View(planDias);
        }

        // GET: PlanDias/Edit/5
        public ActionResult Edit(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PlanDias bdPlanDias = db.PlanDias.Find(id);
            if (bdPlanDias == null)
            {
                return HttpNotFound();
            }
            ViewBag.DietaID = new SelectList(Funcion.GetListaDietas(), "Value", "Text", bdPlanDias.DietaID);
            ViewBag.ProTipID = new SelectList(Funcion.GetListaProTips(), "Value", "Text", bdPlanDias.ProTipID);
            ViewBag.RutinaID = new SelectList(db.Rutina.OrderBy(s => s.Nombre), "ID", "Nombre", bdPlanDias.RutinaID);
            return View(bdPlanDias);
        }

        // POST: PlanDias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? pagina, [Bind(Include = "ID,PlanID,Dia,RutinaID,DietaID,ProTipID,Descanso")] PlanDias planDias)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                if (db.PlanDias.Any(x => x.PlanID == planDias.PlanID && x.Dia == planDias.Dia && x.ID != planDias.ID))
                {
                    ModelState.AddModelError("Dia", "Ya existe ese número de día en este Plan");
                }
                else
                {
                    db.Entry(planDias).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Days", "Plan", new { id = planDias.PlanID, pagina = ViewBag.Pagina });
                }
            }
            ViewBag.DietaID = new SelectList(Funcion.GetListaDietas(), "Value", "Text", planDias.DietaID);
            ViewBag.ProTipID = new SelectList(Funcion.GetListaProTips(), "Value", "Text", planDias.ProTipID);
            ViewBag.RutinaID = new SelectList(db.Rutina.OrderBy(s => s.Nombre), "ID", "Nombre", planDias.RutinaID);
            return View(planDias);
        }

        // GET: PlanDias/Delete/5
        public ActionResult Delete(int? id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanDias bdPlanDias = db.PlanDias.Find(id);
            if (bdPlanDias == null)
            {
                return HttpNotFound();
            }
            return View(bdPlanDias);
        }

        // POST: PlanDias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            PlanDias bdPlanDias = db.PlanDias.Find(id);
            int iPlanId = bdPlanDias.PlanID;
            if (bdPlanDias.EjerciciosDelDia.Count == 0)
            {
                db.PlanDias.Remove(bdPlanDias);
                db.SaveChanges();
            }
            return RedirectToAction("Days", "Plan", new { id = iPlanId, pagina = ViewBag.Pagina });
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