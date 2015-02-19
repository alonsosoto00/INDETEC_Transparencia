using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

public class UploadController : ApiController
{
    public async Task<HttpResponseMessage> PostFormData()
    {
        // Check if the request contains multipart/form-data.
        if (!Request.Content.IsMimeMultipartContent())
        {
            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        }

        string root = HttpContext.Current.Server.MapPath("~/App_Data");
        var provider = new MultipartFormDataStreamProvider(root);

        try
        {
            // Read the form data.
            await Request.Content.ReadAsMultipartAsync(provider);

            // This illustrates how to get the file names.
            foreach (MultipartFileData file in provider.FileData)
            {
                Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                Trace.WriteLine("Server file path: " + file.LocalFileName);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        catch (System.Exception e)
        {
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        }
    }

    public async Task<HttpResponseMessage> PostStuff()
    {
        //if (!Request.Content.IsMimeMultipartContent())
        //{
        //    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //}

        //var root = HttpContext.Current.Server.MapPath("~/App_Data/");
        //Directory.CreateDirectory(root);
        //var provider = new MultipartFormDataStreamProvider(root);
        //var result = await Request.Content.ReadAsMultipartAsync(provider);
        //if (result.FormData["model"] == null)
        //{
        //    throw new HttpResponseException(HttpStatusCode.BadRequest);
        //}

        //var model = result.FormData["model"];
        ////TODO: Do something with the json model which is currently a string



        ////get the files
        //foreach (var file in result.FileData)
        //{
        //    //TODO: Do something with each uploaded file
        //}

        return Request.CreateResponse(HttpStatusCode.OK, "success!");
    }
}