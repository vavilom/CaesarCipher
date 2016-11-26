using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaesarCipher.Models;

namespace CaesarCipher.Controllers
{
    public class EncryptController : Controller
    {
        [HttpPost]
        public JsonResult GetEncryption(string userText, short rotate, bool encrypt)
        {
            var result = cipher.Encrypt(userText, rotate, encrypt);
            return Json(result);
        }
    }
}