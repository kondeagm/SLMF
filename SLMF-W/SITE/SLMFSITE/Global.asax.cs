using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SLMFSITE
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["AppURL"] = Convert.ToString(ConfigurationManager.AppSettings["App_URL"]);
            Session["ImageAssetsURL"] = Convert.ToString(Session["AppURL"]) + "Files/Images/Assets/";
            Session["ImageDietaURL"] = Convert.ToString(Session["AppURL"]) + "Files/Images/Dieta/";
            Session["ImageTeamURL"] = Convert.ToString(Session["AppURL"]) + "Files/Images/EliteTeam/";
            Session["ImagePlanURL"] = Convert.ToString(Session["AppURL"]) + "Files/Images/Planes/";
            Session["ImageProductoURL"] = Convert.ToString(Session["AppURL"]) + "Files/Images/Productos/";
            Session["ImageRutinaURL"] = Convert.ToString(Session["AppURL"]) + "Files/Images/Rutinas/";
            Session["AppAssetsURL"] = Convert.ToString(Session["AppURL"]) + "Content/img/";
            Session["Disciplina"] = "";
            Session["SeccionActiva"] = "";
            Session["UserLogged"] = "false";
            Session["ThisSecctionIsDashboard"] = "false";
            Session["CurrentPlan"] = "";
            Session["CurrentDiscipline"] = "";
        }
    }
}