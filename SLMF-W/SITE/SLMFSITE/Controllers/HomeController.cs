using Newtonsoft.Json.Linq;
using SLMFSITE.Functions;
using SLMFSITE.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SLMFSITE.Controllers
{
    public class HomeController : Controller
    {
        public SlmfDBEntities db = new SlmfDBEntities();

        [Route("home/getSessionInfo")]
        [HttpGet]
        public JObject getSessionInfo()
        {
            JObject fileJson = new JObject();
            fileJson =
                new JObject(
                    new JProperty("dashboardCurrentPlan", Convert.ToString(Session["CurrentPlan"])),
                    new JProperty("dashboardCurrentDiscipline", Convert.ToString(Session["CurrentDiscipline"]))
                );
            return fileJson;
        }

        [Route("home/login/{facebookid}")]
        [HttpGet]
        public JObject getAppInfo(string facebookid)
        {
            bool blValidateFBid = facebookid.All(char.IsDigit);
            string slplanDiscipline = "";
            string slplanUser = "";
            int slplanDay = 0;
            string slplanUrl = "";
            string slplaStatus = "";
            string sluserName = "";
            string slisRegistered = "false";
            JObject fileJson = new JObject();
            if (string.IsNullOrEmpty(facebookid) || blValidateFBid == false)
            {
                fileJson =
                    new JObject(
                            new JProperty("isRegistered", Convert.ToBoolean(slisRegistered)),
                            new JProperty("isDashboard", Convert.ToBoolean(Session["ThisSecctionIsDashboard"])),
                            new JProperty("dashboardPlan", Convert.ToString(Session["CurrentPlan"])),
                            new JProperty("dashboardDiscipline", Convert.ToString(Session["CurrentDiscipline"])),
                            new JProperty("planDiscipline", slplanDiscipline),
                            new JProperty("planUser", slplanUser),
                            new JProperty("planDay", slplanDay),
                            new JProperty("planUrl", slplanUrl),
                            new JProperty("currentStatus", slplaStatus),
                            new JProperty("userName", sluserName)
                        );
            }
            else
            {
                if (ValidateUserExist(facebookid) == false)
                {
                    AddUser(facebookid);
                    fileJson =
                        new JObject(
                                new JProperty("isRegistered", Convert.ToBoolean(slisRegistered)),
                                new JProperty("isDashboard", Convert.ToBoolean(Session["ThisSecctionIsDashboard"])),
                                new JProperty("dashboardPlan", Convert.ToString(Session["CurrentPlan"])),
                                new JProperty("dashboardDiscipline", Convert.ToString(Session["CurrentDiscipline"])),
                                new JProperty("planDiscipline", slplanDiscipline),
                                new JProperty("planUser", slplanUser),
                                new JProperty("planDay", slplanDay),
                                new JProperty("planUrl", slplanUrl),
                                new JProperty("currentStatus", slplaStatus),
                                new JProperty("userName", sluserName)
                            );
                }
                else
                {
                    Usuario bdInfoUser = new Usuario();
                    bdInfoUser = GetUser(facebookid);
                    if (bdInfoUser.PlanID != null)
                    {
                        slplanDiscipline = bdInfoUser.Plan.Disciplina.Nombre.Replace(" ", "-").ToLower();
                        slplanUser = bdInfoUser.Plan.Nombre.Replace(" ", "-").Replace("&", "and").ToLower();
                        slplanUrl = Convert.ToString(Session["AppURL"]) + bdInfoUser.Plan.Disciplina.Nombre.Replace(" ", "-").ToLower() + "/entrenar/" + bdInfoUser.Plan.Nombre.Replace(" ", "-").Replace("&", "and").ToLower();
                        DateTime FechaInicio = Convert.ToDateTime(bdInfoUser.FechaInicioPlan);
                        DateTime FechaActual = System.DateTime.Today;
                        TimeSpan Diferencia = FechaActual.Subtract(FechaInicio);
                        int ilNoDias = bdInfoUser.Plan.PlanDias.Count();
                        slplanDay = Diferencia.Days;
                        if (slplanDay >= 0)
                        {
                            slplanDay++;
                            if (slplanDay > ilNoDias)
                            {
                                slplaStatus = "finished";
                                slplanDay = 1;
                            }
                            else
                            {
                                slplaStatus = "active";
                            }
                        }
                        else
                        {
                            slplaStatus = "inactive";
                        }
                    }
                    if (ValidateUserRegister(facebookid) == false)
                    {
                        slisRegistered = "false";
                        sluserName = "";
                    }
                    else
                    {
                        slisRegistered = "true";
                        sluserName = bdInfoUser.Nombre + " " + bdInfoUser.Apellidos;
                    }
                    fileJson =
                        new JObject(
                                new JProperty("isRegistered", Convert.ToBoolean(slisRegistered)),
                                new JProperty("isDashboard", Convert.ToBoolean(Session["ThisSecctionIsDashboard"])),
                                new JProperty("dashboardPlan", Convert.ToString(Session["CurrentPlan"])),
                                new JProperty("dashboardDiscipline", Convert.ToString(Session["CurrentDiscipline"])),
                                new JProperty("planDiscipline", slplanDiscipline),
                                new JProperty("planUser", slplanUser),
                                new JProperty("planDay", slplanDay),
                                new JProperty("planUrl", slplanUrl),
                                new JProperty("currentStatus", slplaStatus),
                                new JProperty("userName", sluserName)
                            );
                }
            }
            return fileJson;
        }

        [Route("home/logout")]
        [HttpGet]
        public JObject userLogout()
        {
            Session["UserLogged"] = "false";
            JObject fileJson =
                    new JObject(
                            new JProperty("isRegistered", false),
                            new JProperty("isDashboard", Convert.ToBoolean(Session["ThisSecctionIsDashboard"])),
                            new JProperty("dashboardPlan", Convert.ToString(Session["CurrentPlan"])),
                            new JProperty("dashboardDiscipline", Convert.ToString(Session["CurrentDiscipline"])),
                            new JProperty("planDiscipline", ""),
                            new JProperty("planUser", ""),
                            new JProperty("planDay", 0),
                            new JProperty("planUrl", ""),
                            new JProperty("userName", "")
                        );
            return fileJson;
        }

        [Route("home/setUserStatus/{status}")]
        [HttpPost]
        public bool setUserStatus(bool status)
        {
            Session["UserLogged"] = Convert.ToString(status);
            return status;
        }

        [Route("home/getUserStatus")]
        [HttpGet]
        public bool getUserStatus()
        {
            bool blEstatus = false;
            blEstatus = Convert.ToBoolean(Session["UserLogged"]);
            return blEstatus;
        }

        [Route]
        public ActionResult Index()
        {
            Session["Disciplina"] = "";
            Session["SeccionActiva"] = "";
            Session["ThisSecctionIsDashboard"] = "false";
            Session["CurrentPlan"] = "";
            Session["CurrentDiscipline"] = "";
            string sRoot = Convert.ToString(Session["AppURL"]);
            var bdDisciplinas = db.Disciplina.Where(s => s.ID > 0);
            string vLoaderImages = "<script>$(document).ready(function () { Home.init([" +
                                   "'" + sRoot + "Content/img/comunidad-slmf/bg.jpg" + "'" + "," +
                                   "'" + sRoot + "Content/img/home/bg-kai.jpg" + "'" + "," +
                                   "'" + sRoot + "Content/img/home/bg-labs.jpg" + "'" + "," +
                                   "'" + sRoot + "Content/img/home/bg-slmf.jpg" + "'" + "," +
                                   "'" + sRoot + "Content/img/home/bg-slmf-medium.jpg" + "'";

            foreach (var item in bdDisciplinas)
            {
                vLoaderImages += "," + "'" + Convert.ToString(Session["ImageAssetsURL"]) + item.FileImage + "'";
            }
            vLoaderImages += "]) });</script>";
            ViewBag.Loader = vLoaderImages;
            return View(bdDisciplinas.ToList());
        }

        [Route("comunidad")]
        public ActionResult Comunidad()
        {
            Session["Disciplina"] = "";
            Session["SeccionActiva"] = "Comunidad";
            Session["ThisSecctionIsDashboard"] = "false";
            Session["CurrentPlan"] = "";
            Session["CurrentDiscipline"] = "";
            string sRoot = Convert.ToString(Session["AppURL"]);
            return View();
        }

        [Route("about")]
        public ActionResult About()
        {
            Session["Disciplina"] = "";
            Session["SeccionActiva"] = "About";
            Session["ThisSecctionIsDashboard"] = "false";
            Session["CurrentPlan"] = "";
            Session["CurrentDiscipline"] = "";
            string sRoot = Convert.ToString(Session["AppURL"]);
            string vLoaderImages = "<script> $(document ).ready(function() {" +
                                    "About.init({ images:[ " +
                                    "'" + sRoot + "Content/img/comunidad-slmf/bg.jpg" + "'" + "," +
                                    "'" + sRoot + "Content/img/about/background-tile.jpg" + "'" + "," +
                                    "'" + sRoot + "Content/img/icon/arrows-updown.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/about/center_background.jpg" + "'" + "," +
                                    "'" + sRoot + "Content/img/about/left-background.jpg" + "'" + "," +
                                    "'" + sRoot + "Content/img/about/right-background.jpg" + "'" +
                                    "]";
            vLoaderImages += "}); }); </script>";
            ViewBag.Loader = vLoaderImages;
            return View();
        }

        [Route("slmf-team")]
        public ActionResult SLMF_Team()
        {
            Session["Disciplina"] = "";
            Session["SeccionActiva"] = "SLMF-Team";
            Session["ThisSecctionIsDashboard"] = "false";
            Session["CurrentPlan"] = "";
            Session["CurrentDiscipline"] = "";
            string sRoot = Convert.ToString(Session["AppURL"]);
            string vLoaderImages = "<script> $(document ).ready(function() {" +
                                    "SlmfTeam.init([ " +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_home_fondo.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_home_figures1.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_home_figures2.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_home_figures3.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_kai_fondo.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_kai_figure.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_kai_text.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/SLMF_team_title_kai.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/SLMF_team_title_greene.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_charro_diagonal.jpg" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_charro_figure.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_charro_text.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/ejercicio-fondo-diagonal.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_graham_figure.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_graham_text.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_tank_fondo.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_tank_figure.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/TEAM_tank_text.png" + "'" + "," +
                                    "'" + sRoot + "Content/img/slmf-team/bg.jpg" + "'" + ",";
            vLoaderImages += "] ); } ); </script>";
            ViewBag.Loader = vLoaderImages;
            return View();
        }

        [Route("slmf-labs")]
        public ActionResult SLMF_Labs()
        {
            Session["Disciplina"] = "";
            Session["SeccionActiva"] = "SLMF-Labs";
            Session["ThisSecctionIsDashboard"] = "false";
            Session["CurrentPlan"] = "";
            Session["CurrentDiscipline"] = "";
            string sRoot = Convert.ToString(Session["AppURL"]);
            string vLoaderImages = "<script> $(document ).ready(function() {" +
                                    "SlmfLabs.init([ " +
                                    "'" + sRoot + "Content/img/slmf-labs/bg.jpg" + "'" + ",";
            vLoaderImages += "] ); } ); </script>";
            ViewBag.Loader = vLoaderImages;
            return View();
        }

        [Route("{disciplinaTitle}")]
        public ActionResult Disciplina(string disciplinaTitle)
        {
            if (Funcion.DisciplinaExists(disciplinaTitle) == false)
            {
                return HttpNotFound();
            }
            else
            {
                Disciplina bdDisciplina = db.Disciplina.Where(s => s.Nombre.Replace(" ", "-").ToLower() == disciplinaTitle).First();
                Session["Disciplina"] = bdDisciplina.Nombre.Replace(" ", "-").ToLower();
                Session["SeccionActiva"] = "";
                Session["ThisSecctionIsDashboard"] = "false";
                Session["CurrentPlan"] = "";
                Session["CurrentDiscipline"] = bdDisciplina.Nombre.Replace(" ", "-").ToLower();
                string sRoot = Convert.ToString(Session["AppURL"]);
                string vLoaderImages = "<script>$(document).ready(function () { HomeDiscipline.init({ images:[" +
                                        "'" + sRoot + "Content/img/comunidad-slmf/bg.jpg" + "'" + ",";
                vLoaderImages += "'" + Convert.ToString(Session["ImageAssetsURL"]) + bdDisciplina.FileImage + "'";
                vLoaderImages += "], }); }); </script>";
                ViewBag.Loader = vLoaderImages;
                return View(bdDisciplina);
            }
        }

        [Route("{disciplinaTitle}/competir")]
        public ActionResult Competir(string disciplinaTitle)
        {
            if (Funcion.DisciplinaExists(disciplinaTitle) == false)
            {
                return HttpNotFound();
            }
            else
            {
                Disciplina bdDisciplina = db.Disciplina.Where(s => s.Nombre.Replace(" ", "-").ToLower() == disciplinaTitle).First();
                Session["Disciplina"] = bdDisciplina.Nombre.Replace(" ", "-").ToLower();
                Session["SeccionActiva"] = "Competir";
                Session["ThisSecctionIsDashboard"] = "false";
                Session["CurrentPlan"] = "";
                Session["CurrentDiscipline"] = bdDisciplina.Nombre.Replace(" ", "-").ToLower();
                string sRoot = Convert.ToString(Session["AppURL"]);
                string vLoaderImages = "<script>$(document).ready(function () { Competir.init({ images:[" +
                                        "'" + sRoot + "Content/img/comunidad-slmf/bg.jpg" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/mma-a-1.png" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/mma-b-1.png" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/mma-b-2.png" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/mma-c-1.png" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/mma-c-2.png" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/bbd-a-1.png" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/bbd-b-1.png" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/bbd-c-1.png" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/cft-a-1.png" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/cft-b-1.png" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/cft-c-1.png" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/background.jpg" + "'";
                vLoaderImages += "], }); }); </script>";
                ViewBag.Loader = vLoaderImages;
                return View();
            }
        }

        [Route("{disciplinaTitle}/potenciar")]
        public ActionResult Potenciar(string disciplinaTitle)
        {
            if (Funcion.DisciplinaExists(disciplinaTitle) == false)
            {
                return HttpNotFound();
            }
            else
            {
                Disciplina bdDisciplina = db.Disciplina.Where(s => s.Nombre.Replace(" ", "-").ToLower() == disciplinaTitle).First();
                Session["Disciplina"] = bdDisciplina.Nombre.Replace(" ", "-").ToLower();
                Session["SeccionActiva"] = "Potenciar";
                Session["ThisSecctionIsDashboard"] = "false";
                Session["CurrentPlan"] = "";
                Session["CurrentDiscipline"] = bdDisciplina.Nombre.Replace(" ", "-").ToLower();
                string sRoot = Convert.ToString(Session["AppURL"]);
                string vLoaderImages = "<script> $(document).ready(function(){" +
                                            "Potenciar.init({" +
                                                "images:[" +
                                                "'" + sRoot + "Content/img/potenciar/background.jpg" + "'";
                vLoaderImages += "], }); }); </script>";
                ViewBag.Loader = vLoaderImages;
                return View();
            }
        }

        [Route("{disciplinaTitle}/entrenar")]
        public ActionResult Entrenar(string disciplinaTitle)
        {
            if (Funcion.DisciplinaExists(disciplinaTitle) == false)
            {
                return HttpNotFound();
            }
            else
            {
                Disciplina bdDisciplina = db.Disciplina.Where(s => s.Nombre.Replace(" ", "-").ToLower() == disciplinaTitle).First();
                Session["Disciplina"] = bdDisciplina.Nombre.Replace(" ", "-").ToLower();
                Session["SeccionActiva"] = "Entrenar";
                Session["ThisSecctionIsDashboard"] = "false";
                Session["CurrentPlan"] = "";
                Session["CurrentDiscipline"] = bdDisciplina.Nombre.Replace(" ", "-").ToLower();
                string sRoot = Convert.ToString(Session["AppURL"]);
                string vLoaderImages = "<script>$(document).ready(function () { Entrenar.init({ images:[" +
                                        "'" + sRoot + "Content/img/comunidad-slmf/bg.jpg" + "'" + "," +
                                        "'" + sRoot + "Content/img/competir/background.jpg" + "'" + ",";
                vLoaderImages += "'" + Convert.ToString(Session["ImageAssetsURL"]) + bdDisciplina.ImageEntrenar + "'";
                vLoaderImages += "], }); }); </script>";
                ViewBag.Loader = vLoaderImages;
                return View(bdDisciplina);
            }
        }

        [Route("{disciplinaTitle}/entrenar/{planName}")]
        public ActionResult Plan(string disciplinaTitle, string planName)
        {
            if (Funcion.DisciplinaExists(disciplinaTitle) == false)
            {
                return HttpNotFound();
            }
            if (Funcion.PlanExists(planName) == false)
            {
                return HttpNotFound();
            }
            else
            {
                int iPlanID = Funcion.GetPlanId(planName);
                Plan bdPlan = db.Plan.Find(iPlanID);
                string sRoot = Convert.ToString(Session["AppURL"]);
                Session["Disciplina"] = bdPlan.Disciplina.Nombre.Replace(" ", "-").ToLower();
                Session["SeccionActiva"] = "";
                Session["ThisSecctionIsDashboard"] = "true";
                Session["CurrentPlan"] = bdPlan.Nombre.Replace(" ", "-").Replace("&", "and").ToLower();
                Session["CurrentDiscipline"] = bdPlan.Disciplina.Nombre.Replace(" ", "-").ToLower();
                string vLoaderImages = "";
                ViewBag.Loader = vLoaderImages;
                return View(bdPlan);
            }
        }

        public ActionResult FailError()
        {
            return View();
        }

        public ActionResult Error400()
        {
            return View();
        }

        public ActionResult Error401()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }

        private Usuario GetUser(string pUserFacebookId)
        {
            Usuario bdCurrentUser = db.Usuario.Where(x => x.FacebookID == pUserFacebookId).First();
            return bdCurrentUser;
        }

        private bool ValidateUserRegister(string pUserFacebookId)
        {
            return db.Usuario.Any(s => s.FacebookID == pUserFacebookId && (!string.IsNullOrEmpty(s.Nombre.Trim())));
        }

        private bool ValidateUserExist(string pUserFacebookId)
        {
            return db.Usuario.Any(s => s.FacebookID == pUserFacebookId);
        }

        private void AddUser(string pUserFacebookId)
        {
            Usuario bdUsuario = new Usuario();
            bdUsuario.FacebookID = pUserFacebookId;
            bdUsuario.Nombre = "";
            bdUsuario.Apellidos = "";
            bdUsuario.Correo = "";
            if (ModelState.IsValid)
            {
                db.Usuario.Add(bdUsuario);
                db.SaveChanges();
            }
        }
    }
}