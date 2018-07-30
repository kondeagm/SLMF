using System.Web.Optimization;

namespace SLMFSITE
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/script-files/global").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/script-files/global-footer").Include(
                        "~/Scripts/vendor/ScrollToPlugin_1.7.3.min.js",
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/vendor/jquery.html5-placeholder-shim.js",
                        "~/Scripts/main.js",
                        "~/Scripts/plugins.js",
                        "~/Scripts/header.js",
                        "~/Scripts/login/main.js",
                        "~/Scripts/comunidad-slmf/main.min.js"));

            bundles.Add(new ScriptBundle("~/home/scripts").Include(
                        "~/Scripts/home/main.min.js"));

            bundles.Add(new ScriptBundle("~/home-about/scripts").Include(
                        "~/Scripts/about/main.min.js"));

            bundles.Add(new ScriptBundle("~/home-team/scripts").Include(
                        "~/Scripts/slmf-team/main.min.js"));

            bundles.Add(new ScriptBundle("~/home-labs/scripts").Include(
                        "~/Scripts/slmf-labs/main.min.js"));

            bundles.Add(new ScriptBundle("~/member-team/scripts").Include(
                        "~/Scripts/slmf-team-file/main.min.js"));

            bundles.Add(new ScriptBundle("~/home-discipline/scripts").Include(
                        "~/Scripts/home-discipline/main.min.js"));

            bundles.Add(new ScriptBundle("~/training-discipline/scripts").Include(
                        "~/Scripts/entrenar/main.min.js"));

            bundles.Add(new ScriptBundle("~/competition-discipline/scripts").Include(
                        "~/Scripts/competir/main.min.js"));

            bundles.Add(new ScriptBundle("~/power-discipline/scripts").Include(
                        "~/Scripts/potenciar/main.min.js"));

            bundles.Add(new ScriptBundle("~/home-plan/scripts").Include(
                        "~/Scripts/dashboard/main.min.js"));

            bundles.Add(new ScriptBundle("~/comunidad/scripts").Include(
                        "~/Scripts/comunidad-slmf/main.min.js"));

            bundles.Add(new ScriptBundle("~/home/css").Include(
                        "~/Content/css/home/main.css"));

            bundles.Add(new ScriptBundle("~/about/css").Include(
                        "~/Content/css/about/main.css"));

            bundles.Add(new ScriptBundle("~/comunidad/css").Include(
                        "~/Content/css/comunidad-slmf/main.css"));

            bundles.Add(new ScriptBundle("~/home-team/css").Include(
                        "~/Content/css/slmf-team/main.css"));

            bundles.Add(new ScriptBundle("~/home-labs/css").Include(
                        "~/Content/css/slmf-labs/main.css"));

            bundles.Add(new ScriptBundle("~/member-team/css").Include(
                        "~/Content/css/slmf-team-file/main.css"));

            bundles.Add(new ScriptBundle("~/training-discipline/css").Include(
                        "~/Content/css/entrenar/main.css"));

            bundles.Add(new ScriptBundle("~/competition-discipline/css").Include(
                        "~/Content/css/competir/main.css"));

            bundles.Add(new ScriptBundle("~/power-discipline/css").Include(
                        "~/Content/css/potenciar/main.css"));

            bundles.Add(new ScriptBundle("~/plan/css").Include(
                        "~/Content/css/dashboard/main.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}