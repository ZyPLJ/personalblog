using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;

namespace Personalblog.Controllers
{
    [Route("api/music")]
    public class MusicController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        public MusicController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        [HttpGet("{filename}")]
        public IActionResult Get(string filename)
        {
            var filePath = Path.Combine(_environment.WebRootPath, "mp3", filename);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var memoryStream = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memoryStream);
            }
            memoryStream.Position = 0;
            return new FileStreamResult(memoryStream, new MediaTypeHeaderValue("audio/mpeg"));
        }
        //public async Task<IActionResult> Get(string filename)
        //{
        //    var filePath = Path.Combine(_environment.WebRootPath, "mp3", filename);
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return NotFound();
        //    }
        //    var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //    Response.Headers.Add("Content-Disposition", $"attachment; filename={filename}");
        //    Response.Headers.Add("Accept-Ranges", "bytes");
        //    Response.Headers.Add("Content-Type", "audio/mpeg");
        //    Response.Headers.Add("Transfer-Encoding", "chunked");
        //    await stream.CopyToAsync(Response.Body);
        //    return new EmptyResult();
        //}
    }
}
