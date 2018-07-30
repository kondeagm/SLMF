using Newtonsoft.Json.Linq;
using SLMFCMS.Models;
using SLMFCMS.Models.CMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SLMFCMS.Functions
{
    public static class Funcion
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        internal static string NameEncode(string pTitulo)
        {
            string sNameEncode = "";
            string sCadena = pTitulo.Trim().Replace(" ", "-").Replace(".", "").Replace(",", "").Replace(":", "").Replace(";", "").Replace("+", "").Replace("%", "").Replace("#", "").Replace("&", "and").Replace("®", "").Replace("!", "").Replace("'", "");
            string sLetrasConSignos = "áéíóúñÁÉÍÓÚÑÜü";
            string sLetrasSinSignos = "aeiounAEIOUNUu";
            for (int v = 0; v < sLetrasSinSignos.Length; v++)
            {
                string i = sLetrasConSignos.Substring(v, 1);
                string j = sLetrasSinSignos.Substring(v, 1);
                sCadena = sCadena.Replace(i, j);
            }
            sNameEncode = sCadena;
            return sNameEncode;
        }

        internal static string GetGUID()
        {
            string sResponse = "";
            sResponse = Guid.NewGuid().ToString();
            return sResponse;
        }

        internal static bool DisciplineExist(int pDisciplinaId)
        {
            return db.Disciplina.Any(s => s.ID == pDisciplinaId);
        }

        internal static bool DietExist(int pDietaId)
        {
            return db.Dieta.Any(s => s.ID == pDietaId);
        }

        internal static bool ProductExist(int pProductoId)
        {
            return db.Producto.Any(s => s.ID == pProductoId);
        }

        internal static bool ProTipExist(int pProTipId)
        {
            return db.ProTip.Any(s => s.ID == pProTipId);
        }

        internal static JObject CreateJsonResponse(int pStatusValue, string pMessage)
        {
            JObject joResponse = new JObject();
            joResponse =
                new JObject(
                    new JProperty("estado", pStatusValue),
                    new JProperty("mensaje", pMessage)
                );
            return joResponse;
        }

        internal static void EliminaArchivo(string sArchivo)
        {
            if (System.IO.File.Exists(sArchivo))
            {
                System.IO.File.Delete(sArchivo);
            }
        }

        internal static List<SelectListItem> GetListaHoras()
        {
            List<SelectListItem> lsResponse = new List<SelectListItem>();
            for (int i = 1; i < 25; i++)
            {
                string sHora = "";
                if (i < 10)
                {
                    sHora = "0" + i.ToString() + ":00";
                }
                else
                {
                    sHora = i.ToString() + ":00";
                }
                SelectListItem selListItem = new SelectListItem() { Value = i.ToString(), Text = sHora };
                lsResponse.Add(selListItem);
            }
            return lsResponse;
        }

        internal static bool GroupMuscleExist(int pMusculos)
        {
            return db.GrupoMusculos.Any(s => s.ID == pMusculos);
        }

        internal static bool MuscleExist(int pMusculo)
        {
            return db.Musculo.Any(s => s.ID == pMusculo);
        }

        internal static bool RoutineExist(int pEjercicio)
        {
            return db.Ejercicio.Any(s => s.ID == pEjercicio);
        }

        internal static bool PlanExist(int pPlan)
        {
            return db.Plan.Any(s => s.ID == pPlan);
        }

        internal static int GetLastPlanDay(int? pPlan)
        {
            int iResponse = 0;
            int iDiasRegistrados = db.PlanDias.Where(x => x.PlanID == pPlan).Count();
            if (iDiasRegistrados > 0)
            {
                iResponse = db.PlanDias.Where(x => x.PlanID == pPlan).Select(y => y.Dia).Max();
            }
            return iResponse;
        }

        internal static int GetLastExcerciseDay(int? pDia)
        {
            int iResponse = 0;
            int iEjerciciosRegistrados = db.PlanDiaEjercicios.Where(x => x.PlanDiasID == pDia).Count();
            if (iEjerciciosRegistrados > 0)
            {
                iResponse = db.PlanDiaEjercicios.Where(x => x.PlanDiasID == pDia).Select(y => y.Secuencia).Max();
            }
            return iResponse;
        }

        internal static List<SelectListItem> GetListaEjercicios()
        {
            List<SelectListItem> lsResponse = new List<SelectListItem>();
            var bdEjercicios = (from Eje in db.Ejercicio
                                join Acc in db.Accesorio on Eje.AccesorioID equals Acc.ID into tEjerAcc
                                from Acc in tEjerAcc.DefaultIfEmpty()
                                join Ele in db.Elemento on Eje.ElementoID equals Ele.ID into tEjerEle
                                from Ele in tEjerEle.DefaultIfEmpty()
                                join Pos in db.Posicion on Eje.PosicionID equals Pos.ID into tEjerPos
                                from Pos in tEjerPos.DefaultIfEmpty()
                                orderby Eje.Nombre, Acc.Nombre, Pos.Nombre, Ele.Nombre
                                select new
                                {
                                    tmpID = Eje.ID,
                                    tmpNombre = Eje.Nombre,
                                    tmpAccesorio = Acc != null ? " c/" + Acc.Nombre : "",
                                    tmpPosicion = Pos != null ? " " + Pos.Nombre : "",
                                    tmpElemento = Ele != null ? " en " + Ele.Nombre : ""
                                }).ToList();
            foreach (var vEjercicio in bdEjercicios)
            {
                SelectListItem selListItem = new SelectListItem() { Value = vEjercicio.tmpID.ToString(), Text = vEjercicio.tmpNombre + vEjercicio.tmpAccesorio + vEjercicio.tmpPosicion + vEjercicio.tmpElemento };
                lsResponse.Add(selListItem);
            }
            return lsResponse;
        }

        internal static List<SelectListItem> GetListaSeries()
        {
            List<SelectListItem> lsResponse = new List<SelectListItem>();
            for (int i = 1; i < 11; i++)
            {
                SelectListItem selListItem = new SelectListItem() { Value = i.ToString(), Text = i.ToString() };
                lsResponse.Add(selListItem);
            }
            return lsResponse;
        }

        internal static List<SelectListItem> GetListaMinutos()
        {
            List<SelectListItem> lsResponse = new List<SelectListItem>();
            for (int i = 0; i < 6; i++)
            {
                SelectListItem selListItem = new SelectListItem() { Value = i.ToString(), Text = i.ToString() };
                lsResponse.Add(selListItem);
            }
            return lsResponse;
        }

        internal static List<SelectListItem> GetListaNiveles()
        {
            List<SelectListItem> lsResponse = new List<SelectListItem>();
            for (int i = 1; i < 11; i++)
            {
                SelectListItem selListItem = new SelectListItem() { Value = i.ToString(), Text = i.ToString() };
                lsResponse.Add(selListItem);
            }
            return lsResponse;
        }

        internal static List<SelectListItem> GetListaDietas()
        {
            List<SelectListItem> lsResponse = new List<SelectListItem>();
            var bdDietas = (from data in db.Dieta
                            select data).ToList();
            foreach (var vDieta in bdDietas.Where(s => s.Definido == true).OrderBy(s => s.Nombre))
            {
                SelectListItem selListItem = new SelectListItem() { Value = vDieta.ID.ToString(), Text = vDieta.Nombre };
                lsResponse.Add(selListItem);
            }
            return lsResponse;
        }

        internal static List<SelectListItem> GetListaGrupoMusculos()
        {
            List<SelectListItem> lsResponse = new List<SelectListItem>();
            var bdGruposMusculos = (from data in db.GrupoMusculos
                                    select data).ToList();
            foreach (var vGrupo in bdGruposMusculos.Where(s => s.Definido == true).OrderBy(s => s.Nombre))
            {
                SelectListItem selListItem = new SelectListItem() { Value = vGrupo.ID.ToString(), Text = vGrupo.Nombre };
                lsResponse.Add(selListItem);
            }
            return lsResponse;
        }

        internal static IEnumerable<CMSUserList> GetAllUsers()
        {
            List<CMSUserList> luResponse = new List<CMSUserList>();
            var context = new ApplicationDbContext();
            var allUsers = context.Users.ToList();
            foreach (var item in allUsers)
            {
                string sRolId = item.Roles.First().RoleId;
                string sRolName = context.Roles.Where(x => x.Id == sRolId).First().Name;
                CMSUserList bdCMSUser = new CMSUserList();
                bdCMSUser.Nombre = item.UserName;
                bdCMSUser.Correo = item.Email;
                bdCMSUser.Rol = sRolName;
                luResponse.Add(bdCMSUser);
            }
            return luResponse;
        }

        internal static List<SelectListItem> GetListaProTips()
        {
            List<SelectListItem> lsResponse = new List<SelectListItem>();
            var bdProTips = (from data in db.ProTip
                             select data).ToList();
            foreach (var vProTip in bdProTips.Where(s => s.Definido == true).OrderBy(s => s.Nombre))
            {
                SelectListItem selListItem = new SelectListItem() { Value = vProTip.ID.ToString(), Text = vProTip.Nombre };
                lsResponse.Add(selListItem);
            }
            return lsResponse;
        }
    }
}