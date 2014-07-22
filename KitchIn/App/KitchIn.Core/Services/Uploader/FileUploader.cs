using System.IO;
using System.Web.Mvc;

namespace KitchIn.Core.Services.Uploader
{
    [ModelBinder(typeof(ModelBinder))]
    public class FileUploader
    {
        public string Filename { get; set; }

        public Stream InputStream { get; set; }

        public class ModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var request = controllerContext.RequestContext.HttpContext.Request;
                var formUpload = request.Files.Count > 0;

                // find filename
                var xFileName = request.Headers["X-File-Name"];
                var qqFile = request["qqfile"];
                var formFilename = formUpload ? request.Files[0].FileName : null;

                var upload = new FileUploader
                {
                    Filename = xFileName ?? qqFile ?? formFilename,
                    InputStream = formUpload ? request.Files[0].InputStream : request.InputStream
                };

                return upload;
            }
        }
    }
}
