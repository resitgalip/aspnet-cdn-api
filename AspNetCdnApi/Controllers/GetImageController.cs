using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Text.RegularExpressions;

namespace AspNetCdnApi.Controllers
{
    public class GetImageController : Controller
    {
        [HttpGet("/{*path}"), DisableRequestSizeLimit]
        public IActionResult GetImage(string path)
        {
            try
            {

                var site_info = path.Split("/").Where(x => !string.IsNullOrEmpty(x)).ToArray(); //arrays..

                if (site_info.Length < 2)
                {
                    return NoContent();
                }

                var site_only_path_array = site_info.Skip(1).SkipLast(1).ToArray(); ///data/uploads/a/al/ali
                var site_url = site_info[0]; 
                var web_url = Path.Combine("https://", site_url.Replace("_", ".")); 
                var image = Path.GetFileName(path); //image.jpg
                var imageLocalPath = Path.Combine(Directory.GetCurrentDirectory(), "images/", path);
                var imageFolderLocalPath = Path.Combine(Directory.GetCurrentDirectory(), "images/", string.Join('/', site_info.SkipLast(1)));
                var imageCacheFolderLocalPath = Path.Combine(Directory.GetCurrentDirectory(), "images/", site_url, "cache/", string.Join('/', site_only_path_array));
                var originalUrl = web_url + "/" + String.Join('/', site_info.Skip(1));
                var originalUrlNoImage = web_url + "/" + String.Join('/', site_info.Skip(1).SkipLast(1));

                string pattern = @"-\d{1,9}x\d{1,9}\.";
                string size = "";

                if (Regex.IsMatch(image, pattern))
                {
                    var sub_cache_folder_path = Path.Combine("images", site_url, "cache/");

                    //creating cache folders...
                    if (!Directory.Exists(imageCacheFolderLocalPath))
                    {
                        foreach (var item in site_only_path_array)
                        {
                            if (!Directory.Exists(sub_cache_folder_path + item))
                                Directory.CreateDirectory(sub_cache_folder_path + item);

                            sub_cache_folder_path = sub_cache_folder_path + item + "/";
                        }
                    }

                    //getting sizes...
                    foreach (Match match in Regex.Matches(image, pattern))
                        size = match.ToString();
                    size = size.Replace("-", "").TrimEnd('.');
                    int width = int.Parse(size.Split("x")[0].ToString());
                    int height = int.Parse(size.Split("x")[1].ToString());


                    var pathToRead_cache = Path.Combine(imageCacheFolderLocalPath, image);
                    pathToRead_cache = Path.Combine(pathToRead_cache.Replace(image, ""), Path.GetFileNameWithoutExtension(image) + ".webp");

                    if (!System.IO.File.Exists(pathToRead_cache))
                    {
                        string imageName = Regex.Replace(image, pattern, ".");
                        var pathToReadNew = Path.Combine(imageCacheFolderLocalPath, imageName);
                        var finalPath = "";
                        if (!System.IO.File.Exists(pathToReadNew)) 
                        {
                            if (!System.IO.File.Exists(imageCacheFolderLocalPath + "/" + imageName))
                            {
                                var net = new System.Net.WebClient();
                                string noCacheOriginalUrl = Regex.Replace(originalUrl, pattern, ".").Replace("image/cache", "image");
                                net.DownloadFile(noCacheOriginalUrl, imageCacheFolderLocalPath + "/" + imageName);
                            }

                            finalPath = imageCacheFolderLocalPath + "/" + imageName;
                        }
                        else
                            finalPath = pathToReadNew; 

                        using (var image2 = new MagickImage(finalPath))
                        {
                            
                            image2.Resize(width, height);
                           

                            int width2 = image2.Width;
                            int height2 = image2.Height;

                            image2.Extent(width, height, Gravity.Center, MagickColors.White);

                            image2.Strip();
                     
                            image2.Quality = 90;
                            image2.SetCompression(CompressionMethod.LosslessJPEG);

                            image2.Write(pathToRead_cache);
                        }

                    }
                    var result_image = System.IO.File.OpenRead(pathToRead_cache);
                    return File(result_image, "image/jpeg");

                }
                else
                {

                    //Directory Creating...
                    if (!Directory.Exists(imageFolderLocalPath))
                    {
                        var sub_folder_path = "images/" + site_url + "/";
                        foreach (var item in site_only_path_array)
                        {
                            if (!Directory.Exists(sub_folder_path + item))
                                Directory.CreateDirectory(sub_folder_path + item);

                            sub_folder_path = sub_folder_path + item + "/";
                        }
                    }

                    if (!System.IO.File.Exists(imageLocalPath))
                    {
                        var net = new System.Net.WebClient();
                        net.DownloadFile(originalUrl, imageLocalPath);
                    }
                    var img = Image.FromFile(imageLocalPath);

                    var result_image = System.IO.File.OpenRead(imageLocalPath);
                    return File(result_image, "image/jpeg");

                }

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
