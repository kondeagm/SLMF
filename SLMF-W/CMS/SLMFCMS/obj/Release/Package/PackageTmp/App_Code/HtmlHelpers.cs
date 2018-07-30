using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace SLMFCMS.App_Code
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString Image(this HtmlHelper helper, string url, string name)
        {
            return Image(helper, url, name, null);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string url, string name, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("img");
            tagBuilder.Attributes["src"] = new UrlHelper(helper.ViewContext.RequestContext).Content(url);
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString ActionImage(this HtmlHelper helper, string controller, string action, object parameters, string url, string name, object htmlAttributes)
        {
            var tagBuilder = new TagBuilder("img");
            tagBuilder.Attributes["src"] = new UrlHelper(helper.ViewContext.RequestContext).Content(url);
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            string image = tagBuilder.ToString(TagRenderMode.SelfClosing);
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            string urlAction = urlHelper.Action(action, controller, parameters);
            StringBuilder htmlAncla = new StringBuilder();
            htmlAncla.Append("<a href=\"");
            htmlAncla.Append(urlAction);
            htmlAncla.Append("\">");
            htmlAncla.Append(image);
            htmlAncla.Append("</a>");
            return MvcHtmlString.Create(htmlAncla.ToString());
        }

        public static MvcHtmlString MenuActionLink(this HtmlHelper helper, string texto, string controller, string action, string iconClass)
        {
            TagBuilder tagI = new TagBuilder("i");
            tagI.AddCssClass(iconClass);
            tagI.SetInnerText(" ");
            string sTagI = tagI.ToString();
            UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            string urlAction = urlHelper.Action(action, controller);
            StringBuilder htmlAncla = new StringBuilder();
            htmlAncla.Append("<a href=\"");
            htmlAncla.Append(urlAction);
            htmlAncla.Append("\">");
            htmlAncla.Append(sTagI);
            htmlAncla.Append(texto);
            htmlAncla.Append("</a>");
            return MvcHtmlString.Create(htmlAncla.ToString());
        }
    }
}
