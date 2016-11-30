using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaesarCipher.Models;
using System.Threading.Tasks;

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

        [HttpPost]
        public JsonResult tryDecryp(string userEncryptText)
        {
            int result = cipher.tryAsyncDecrypt(userEncryptText);
            return Json(result);
        }
    }
}