using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace Restaurant.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files != null && Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/Menu/"), fileName);
                    file.SaveAs(path);
                    return Content("/Content/Images/Menu/" + fileName);
                }
            }
            return null;
        }
    }
}