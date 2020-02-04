using Arch.Core;
using Arch.Core.Enums;
using Arch.Data.UnitofWork;
using Arch.Service.Interfaces;
using Arch.Utilities.Manager;
using Arch.Web.Framework.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Arch.Web.Controllers
{
    public class MediaController : AdminController
    {
        private readonly IMediaService _mediaService;
        private readonly IUtilityService _utilityService;
        public MediaController(
            IUnitofWork uow,
            ILogService logService,
            IUtilityService utilityService,
            IMediaService mediaService)
            : base(uow, logService)
        {
            _mediaService = mediaService;
            _utilityService = utilityService;
        }
        
        public FileContentResult Viewer(long id, int? w, int? h)
        {
            var doc = _mediaService.FindMedia(id);
            if (doc.ContentType.Contains("image"))
            {
                //System.Drawing.Image img = new System.Drawing.Bitmap(new MemoryStream(doc.Value));
                //var val = ImageManager.ScaleBySize(img, w.Value, h.Value, false);
                //return new FileContentResult(ImageManager.imageToByteArray(val, doc.ContentType), doc.ContentType);

                var val = ImageManager.ConvertToSize(doc.Value, w, h, doc.ContentType);
                return new FileContentResult(val, doc.ContentType);
            }
            return new FileContentResult(doc.Value, doc.ContentType);
        }

        public FileContentResult ViewerWithName(long id, string fileName)
        {
            var doc = _mediaService.FindMedia(id);
            return File(doc.Value, doc.ContentType, doc.FileName);
        }

        public FileContentResult Preview(int id, int? tempId)
        {
            var m = (Media)Session["Media" + id];
            return new FileContentResult(m.Value, m.ContentType);
        }

        public FileContentResult PreviewWithName(int id, int? tempId, string fileName)
        {
            var doc = (Media)Session["Media" + id];
            return File(doc.Value, doc.ContentType, doc.FileName);
        }

        [HttpPost]
        public ActionResult CreateTempMediaForTask(IEnumerable<HttpPostedFileBase> file)
        {
            var counter = 0;
            foreach (var item in file)
            {
                var byteValue = (new BinaryReader(item.InputStream)).ReadBytes(item.ContentLength);
                if (item.ContentType.Contains("image"))
                {
                    var img = ImageManager.byteArrayToImage(byteValue);
                    if (img.Width > 2000 || img.Height > 2000)
                        return AjaxMessage(MessageTitleTypes.Uyari, "Resim boyutları 2000 den fazla olmamalıdır.", MessageTypes.danger);
                }
                if (item.ContentLength > 3145728)
                    return AjaxMessage("Uyarı", "Dosya boyutu 3 Mb den fazla olmamalıdır.", MessageTypes.warning);
                counter++;
                if (!(item.ContentLength > 0))
                    return AjaxMessage(MessageTitleTypes.Uyari, "Dosya Boyutu 0 dan büyük olmaldır.", MessageTypes.danger);
                var entity = new Media
                {
                    Id = 0,
                    Value = byteValue,
                    FileName = item.FileName,
                    ContentLength = item.ContentLength,
                    ContentType = item.ContentType,
                    CreatedBy = Accesses.PersonId,
                };
                Session["Media" + counter] = entity;
            }
            Session["counter"] = counter;
            Session["Clear"] = null;
            var fileObject = file.Select(p => new { p.ContentType, p.FileName }).ToArray();
            return Json(fileObject, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClearTempMedia()
        {
            Session.Abandon();
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
            Session["Clear"] = "Clear";
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}