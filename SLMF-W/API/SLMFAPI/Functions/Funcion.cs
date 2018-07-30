using SLMFAPI.Models;
using System;
using System.Linq;

namespace SLMFAPI.Functions
{
    public static class Funcion
    {
        private static SlmfDBEntities db = new SlmfDBEntities();

        internal static bool UserExist(string pFacebookId)
        {
            return db.Usuario.Any(s => s.FacebookID == pFacebookId);
        }

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

        internal static string DivideTitulo(string pCadena, int pParte)
        {
            string sResponse = "";
            string sNuevaCadena = pCadena;
            int iIteracciones = sNuevaCadena.Length;
            int iNoCaracteres = 0;
            int iPosicion = 0;
            int iTamanoCadena = sNuevaCadena.Length;
            while (iIteracciones > 0)
            {
                iPosicion = sNuevaCadena.IndexOf(" ");
                if (iPosicion == -1)
                {
                    iIteracciones = 0;
                }
                else
                {
                    if ((1 + iPosicion + iNoCaracteres) > 10)
                    {
                        iIteracciones = 0;
                    }
                    else
                    {
                        iNoCaracteres = iNoCaracteres + iPosicion + 1;
                        sNuevaCadena = pCadena.Substring((iNoCaracteres), (@iTamanoCadena - iNoCaracteres));
                    }
                }
                iIteracciones--;
            }
            if (pParte == 1)
            {
                sResponse = pCadena.Substring(0, iNoCaracteres);
            }
            else
            {
                sResponse = pCadena.Substring((iNoCaracteres), (@iTamanoCadena - iNoCaracteres));
            }
            return sResponse;
        }

        internal static string HoraTiempo(int? pHora)
        {
            string sResponse = "";
            if (pHora != null)
            {
                if (pHora < 13)
                {
                    sResponse = "AM";
                }
                else
                {
                    sResponse = "PM";
                }
            }
            return sResponse;
        }

        internal static string HoraStr(int? pHora)
        {
            string sResponse = "";
            if (pHora != null)
            {
                if (pHora < 10)
                {
                    sResponse = "0" + Convert.ToString(pHora) + ":00";
                }
                else
                {
                    if (pHora > 12)
                    {
                        sResponse = "0" + Convert.ToString(pHora - 12) + ":00";
                    }
                    else
                    {
                        sResponse = Convert.ToString(pHora) + ":00";
                    }
                }
            }
            return sResponse;
        }

        internal static int RangoNivel(int pNivel)
        {
            int iResponse = 1;
            if (pNivel < 4)
            {
                iResponse = 1;
            }
            else if (pNivel < 8)
            {
                iResponse = 2;
            }
            else
            {
                iResponse = 3;
            }
            return iResponse;
        }

        internal static bool SLMFUserExists(string pUserId)
        {
            return db.Usuario.Any(s => s.FacebookID == pUserId);
        }

        internal static int GetUserId(string pUserId)
        {
            return db.Usuario.Where(x => x.FacebookID == pUserId).First().ID;
        }
    }
}