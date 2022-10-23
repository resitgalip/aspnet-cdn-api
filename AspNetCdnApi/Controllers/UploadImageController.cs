using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;


namespace AspNetCdnApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadImageController : Controller
    {
       

        [HttpPost]
        public IActionResult UploadImage()
        {
            try
            {
                if (Request.ContentLength == 0)
                    return NoContent();

                var file = Request.Form.Files[0];
                var site = Request.Form["site"].ToString();
                var user_id = Request.Form["user_id"].ToString();


                var folderName = Path.Combine("images", site);
                var pathWebSite = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var pathToSave = Path.Combine(pathWebSite, user_id);

                if (!Directory.Exists(pathWebSite))
                {
                    Directory.CreateDirectory(pathWebSite);
                }

                if (file.Length > 0 && !string.IsNullOrEmpty(site) && !string.IsNullOrEmpty(user_id))
                {

                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)?.FileName?.Trim('"');

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;

                    var fullPath = Path.Combine(pathToSave, uniqueFileName);

                    if (!Directory.Exists(pathToSave))
                        Directory.CreateDirectory(pathToSave);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    var siteUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SiteUrl").Value;
                    var imagePath = siteUrl + site + "/" + user_id + "/" + uniqueFileName;


                   
                    try
                    {
                        System.Drawing.Image imgInput = System.Drawing.Image.FromFile(fullPath);
                        System.Drawing.Graphics gInput = System.Drawing.Graphics.FromImage(imgInput);
                        System.Drawing.Imaging.ImageFormat thisFormat = imgInput.RawFormat;

                        return Ok(new { imagePath });

                    }
                    catch
                    {
                        return BadRequest();
                    }
                  
                }
                else
                {
                    return BadRequest();
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

    }
}
