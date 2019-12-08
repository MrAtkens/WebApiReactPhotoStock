using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using WebApi.DataAcces;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*", SupportsCredentials = true)]
    public class PhotosController : ApiController
    {
        private PhotosContext db = new PhotosContext();


        [HttpGet]
        [ActionName("GetPhotos")]
        public async Task<IHttpActionResult> GetPhotos()
        {
            List<Photo> photos = await db.Photos.ToListAsync();
            return Ok(photos);
        }

        [HttpPost]
        [ActionName("UploadFile")]
        public async Task<IHttpActionResult> UploadFile()
        {
 
                var ctx = HttpContext.Current;
            string description = ctx.Request.Headers["Description"].ToString();
            string fileNameFromRequest = ctx.Request.Headers["Name"].ToString();
            string userName = ctx.Request.Headers["UserName"].ToString();

            var root = ctx.Server.MapPath("~/Images");
                var provider =
                    new MultipartFormDataStreamProvider(root);
            Photo photo = new Photo() { Path = "", Description = description, UserName = userName, FileName = fileNameFromRequest };

                try
                {
                    await Request.Content
                        .ReadAsMultipartAsync(provider);

                foreach (var file in provider.FileData)
                    {
                    if (file.Headers.ContentDisposition.FileName.Contains(".jpg"))
                    {
                        file.Headers.ContentDisposition.FileName = fileNameFromRequest + ".jpg";
                    }
                    else
                    {
                        file.Headers.ContentDisposition.FileName = fileNameFromRequest + ".png";
                    }
                    var name = file.Headers
                            .ContentDisposition
                            .FileName;

                        // remove double quotes from string.
                        name = name.Trim('"');

                        var localFileName = file.LocalFileName;
                        var filePath = Path.Combine(root, name);
                        photo.Path = "http://localhost:1801/Images/" + name;
                        File.Move(localFileName, filePath);
                    }
                }
                catch (Exception e)
                {
                    return Ok(new { status = false });
                }

            db.Photos.Add(photo);
            await db.SaveChangesAsync();

            return Ok(new { status = true, answerData = photo });

        }
    }
}