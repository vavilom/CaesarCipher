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
            string result = string.Empty;
            if (userText.Length > 0)
            {
                result = cipher.Encrypt(userText, rotate, encrypt);
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult tryDecryp(string userEncryptText)
        {
            int result = -1;

            if (userEncryptText.Length > 0)
            {
                //result = cipher.tryDecrypt(userEncryptText);
                result = cipher.tryDecryptWithoutDatabase(userEncryptText);
            }   

            return Json(result);
        }
    }
}