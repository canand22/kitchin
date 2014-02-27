using System.Web.Mvc;
using System.Reflection;

namespace KitchIn.Web.Controllers
{
    /// <summary>
    /// Describes Home Controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Displays Index view
        /// </summary>
        /// <returns>Index view</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Displays About view
        /// </summary>
        /// <returns>About view</returns>
        public ActionResult About()
        {
            return this.View();
        }
    }
}
