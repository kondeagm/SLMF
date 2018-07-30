using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using SLMFCMS.Functions;
using SLMFCMS.Models;
using SLMFCMS.Models.CMS;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SLMFCMS.Controllers
{
    [Authorize(Roles = "PageAdmin")]
    public class CMSUsuarioController : Controller
    {
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();
        private int pageSize = 10;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: CMSUsuario
        public ActionResult Index(string sSortOrder, string sCurrentFilter, string sSearchString, int? pagina)
        {
            ViewBag.CurrentSort = sSortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sSortOrder) ? "nombre_desc" : "";
            ViewBag.CorreoSortParm = sSortOrder == "correo" ? "correo_desc" : "correo";
            if (sSearchString != null)
            {
                pagina = 1;
            }
            else
            {
                sSearchString = sCurrentFilter;
            }
            ViewBag.CurrentFilter = sSearchString;
            var rowsUsuario = Funcion.GetAllUsers();
            if (!String.IsNullOrEmpty(sSearchString))
            {
                rowsUsuario = rowsUsuario.Where(s => s.Nombre.ToUpper().Contains(sSearchString.ToUpper()));
            }
            switch (sSortOrder)
            {
                case "nombre_desc":
                    rowsUsuario = rowsUsuario.OrderByDescending(s => s.Nombre);
                    break;

                case "correo":
                    rowsUsuario = rowsUsuario.OrderBy(s => s.Correo).ThenBy(s => s.Nombre);
                    break;

                case "correo_desc":
                    rowsUsuario = rowsUsuario.OrderByDescending(s => s.Correo).ThenBy(s => s.Nombre);
                    break;

                default:
                    rowsUsuario = rowsUsuario.OrderBy(s => s.Nombre);
                    break;
            }
            int pageNumber = (pagina ?? 1);
            ViewBag.Pagina = pageNumber;
            return View(rowsUsuario.ToPagedList(pageNumber, pageSize));
        }

        // GET: CMSUsuario/ResetPassword/usernameid
        public async Task<ActionResult> ResetPassword(string username, int? pagina)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByNameAsync(username);
            if (user == null)
            {
                return RedirectToAction("Index", "CMSUsuario", new { pagina = ViewBag.Pagina });
            }
            CMSResetPass bdResetPass = new CMSResetPass();
            bdResetPass.Id = user.Id;
            return View(bdResetPass);
        }

        // POST: CMSUsuario/ResetPassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(int? pagina, [Bind(Include = "Id,Password,ConfirmPassword")] CMSResetPass cmsresetpass)
        {
            ViewBag.Pagina = (pagina ?? 1);
            if (ModelState.IsValid)
            {
                UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                userManager.RemovePassword(cmsresetpass.Id);
                userManager.AddPassword(cmsresetpass.Id, cmsresetpass.Password);
                return RedirectToAction("Index", "CMSUsuario", new { pagina = ViewBag.Pagina });
            }
            return View(cmsresetpass);
        }
    }
}