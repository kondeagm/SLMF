using SLMFSITE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SLMFSITE.Functions
{
    public static class Funcion
    {
        private static SlmfDBEntities db = new SlmfDBEntities();

        internal static bool PlanExists(string pPlanId)
        {
            bool bResponse = false;
            string sPlanName = pPlanId.Replace("-and-", "-&-").Replace("-", " ");
            bResponse = db.Plan.Any(s => s.Nombre == sPlanName);
            if (bResponse == false)
            {
                sPlanName = pPlanId.Replace("-", " ");
                bResponse = db.Plan.Any(s => s.Nombre == sPlanName);
            }
            return bResponse;
        }

        internal static int GetPlanId(string pPlanId)
        {
            int iResponse = 0;
            bool bExiste = false;
            string sPlanName = pPlanId.Replace("-and-", "-&-").Replace("-", " ");
            bExiste = db.Plan.Any(s => s.Nombre == sPlanName);
            if (bExiste == true)
            {
                iResponse = db.Plan.Where(s => s.Nombre == sPlanName).First().ID;
            }
            else
            {
                sPlanName = pPlanId.Replace("-", " ");
                bExiste = db.Plan.Any(s => s.Nombre == sPlanName);
                if (bExiste == true)
                {
                    iResponse = db.Plan.Where(s => s.Nombre == sPlanName).First().ID;
                }
            }
            return iResponse;
        }

        internal static bool DisciplinaExists(string pDisciplinaTitle)
        {
            string sDisciplinaName = pDisciplinaTitle.Replace("-and-", "-&-").Replace("-", " ");
            return db.Disciplina.Any(s => s.Nombre.ToLower() == sDisciplinaName.ToLower());
        }
    }
}