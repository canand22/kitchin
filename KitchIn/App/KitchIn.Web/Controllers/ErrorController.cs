using System.Web.Mvc;

using KitchIn.Core;

namespace KitchIn.Web.Controllers
{
    public class ErrorController : HttpErrorHandler.ErrorController
    {
        /// <summary>
        /// Action that corresponds to http status code 404
        /// </summary>
        /// <returns>'NotFound' view, which has to be in your /Views/Error/ folder.</returns>
        public override ActionResult NotFound()
        {
            LogWriter.WriteError("Error 404: Not Found at " + this.Request.Url.Query);

            return base.NotFound();
        }

        /// <summary>
        /// Action that corresponds to http status code 500
        /// </summary>
        /// <returns>'InternalServerError' view, which has to be in your /Views/Error/ folder.</returns>
        public override ActionResult InternalServerError()
        {
            LogWriter.WriteError("Error 500: Internal Server Error at " + this.Request.Url.Query);

            return base.InternalServerError();
        }
    }
}
