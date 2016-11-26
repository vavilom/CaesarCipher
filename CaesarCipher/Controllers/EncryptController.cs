using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CaesarCipher.Controllers
{
    public class EncryptController : Controller
    {
        [HttpPost]
        public JsonResult GetEncryption(string userText, int rotate)
        {
            var result = userText.ToUpper() + " - " + rotate;
            return Json(result);
        }

    }
}