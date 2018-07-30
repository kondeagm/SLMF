using System.Web.Optimization;

namespace SLMFCMS
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/jscolor/jscolor.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/wyswyg").Include(
                        "~/Scripts/tinymce/tinymce.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/lightbox").Include(
                      "~/Scripts/lightbox-2.6.js"));

            bundles.Add(new ScriptBundle("~/bundles/dragscript").Include(
                        "~/Scripts/jquery-ui-1.11.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-image-discipline").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/discipline-upload-image.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-icon-discipline").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/discipline-upload-icon.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-acronym-discipline").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/discipline-upload-acronym.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-image-training").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/discipline-upload-training.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-image-product").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/product-upload-image.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-image-protip").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/protip-upload-image.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-image-diet").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/diet-upload-image.js"));

            bundles.Add(new ScriptBundle("~/bundles/schedulepickerday").Include(
                      "~/Scripts/jquery-2.2.3.js",
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/calendar-day-picker.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-image-groupmuscle").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/groupmuscle-upload-image.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-image-muscle").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/muscle-upload-image.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-image-exercice").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/exercice-upload-image.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-image-plan").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/plan-upload-image.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload-image-banner").Include(
                      "~/Scripts/jquery-ui-1.11.4.min.js",
                      "~/Scripts/jquery.filedrop.min.js",
                      "~/Scripts/banner-upload-image.js"));

            bundles.Add(new StyleBundle("~/Content/picker").Include(
                      "~/Content/themes/base/all.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-slate.css",
                      "~/Content/lightbox.css",
                      "~/Content/font-awesome.css",
                      "~/Content/tools.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/multiselect").Include(
                      "~/Content/multi-select.css"));

            bundles.Add(new StyleBundle("~/Content/dragcss").Include(
                      "~/Content/themes/base/all.css"));
        }
    }
}