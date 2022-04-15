using CleanBlog.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using Umbraco.Core.Services.Implement;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace CleanBlog.Core.Controllers
{
    /// <summary>
    /// Handle member registration
    /// </summary>
    public class RegisterController : SurfaceController
    {
        private const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/";

        public RegisterController()
        {
        }

        public ActionResult RenderRegister()
        {
            var vm = new RegisterViewModel();
            return PartialView(PARTIAL_VIEW_FOLDER + "Register.cshtml", vm);
        }

        

        
    }
}
