using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Guet.DataAccess;
using Guet.Entities.ApplicationOrganization;
using Guet.Entities.Attachments;
using Guet.ViewModels.Common;
using Guet.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Guet.Web.Controllers
{
    public class FileController : Controller
    {
        protected static string UploadImageBaseURL;
        protected IWebHostEnvironment _hostingEnv;
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IEntityRepository<BusinessImage> _businessImage;
        public FileController(
            IWebHostEnvironment hostingEnv,
            UserManager<ApplicationUser> userManager,
            IEntityRepository<BusinessImage> businessImage
            )
        {
            _hostingEnv = hostingEnv;
            _userManager = userManager;
            _businessImage = businessImage;
            UploadImageBaseURL = _hostingEnv.WebRootPath + $@"\images\UploadImages\";
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Upload(UploadInput model)
        {
            var res = HttpContext.Request;
            try
            {
                var image = Request.Form.Files.First();
                if (image == null)
                {
                    return Json(new UploadViewModel() { Code = 0, Status = false, Msg = "上传失败，未选择图片，请重试！" });
                }
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                var currImageName = image.FileName;
                var timeForFile = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-").Trim();
                string extensionName = currImageName.Substring(currImageName.LastIndexOf("."));
                var imageName = ContentDispositionHeaderValue
                                .Parse(image.ContentDisposition)
                                .FileName
                                .Trim('"')
                                .Substring(image.FileName.LastIndexOf("\\") + 1);
                var newImageName = timeForFile + model.ObjectId + extensionName;
                var boPath = "../../images/UploadImages/" + date + "/" + newImageName;
                var imagePath = $@"{UploadImageBaseURL}{date}";
                imageName = $@"{UploadImageBaseURL}{date}\{newImageName}";

                Directory.CreateDirectory(imagePath); //创建目录
                using (FileStream fs = System.IO.File.Create(imageName))
                {
                    image.CopyTo(fs);
                    fs.Flush();
                }
                var saveImage = _businessImage.GetAll().FirstOrDefault(a => a.RelevanceObjectId == model.ObjectId);
                if (saveImage != null)
                {
                    await _businessImage.DeleteAndSaveAsyn(saveImage.Id);
                }
                saveImage = new BusinessImage()
                {
                    DisplayName = currImageName,
                    RelevanceObjectId = model.ObjectId,
                    UploadPath = boPath,
                    FileSize = image.Length
                };
                bool r = await _businessImage.AddOrEditAndSaveAsyn(saveImage);
                if (r)
                {
                    var fullUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value + "/" + saveImage.UploadPath.Replace("../", "");
                    var images = new List<string>() { fullUrl };
                    if (model.IsRich)
                    {
                        return Json(new RichImageCallback()
                        {
                            Errno = 0,
                            Data = images
                        });
                    }
                    else
                    {
                        return Json(new UploadViewModel()
                        {
                            Code = 200,
                            Status = true,
                            Msg = "上传成功！",
                            Id = saveImage.Id,
                            ObjectId = model.ObjectId,
                            FullUrl = fullUrl,
                            Url = saveImage.UploadPath
                        });
                    }
                }
                else return Json(new UploadViewModel() { Code = 0, Status = false, Msg = "上传失败，请重试！" });
            }
            catch (Exception ex)
            {
                return Json(new UploadViewModel() { Code = 500, Status = false, Msg = ex.Message });
            }
        }
    }
}
