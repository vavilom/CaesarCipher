using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaesarCipher.Models
{
    public static class cipher
    {
        private static string alfpabet = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Encrypt or decript text
        /// </summary>
        /// <param name="initialText">Text from user</param>
        /// <param name="rotate">Rotate</param>
        /// <param name="encrypt">Select encrypt or decrypt text  (true - encrypt / false - decrypt)</param>
        /// <returns></returns>
        public static string Encrypt(string initialText, short rotate, bool encrypt)
        {
            if (rotate <= 0 || rotate >= alfpabet.Length) return initialText;

            StringBuilder resultText = new StringBuilder();

            for (int i = 0; i < initialText.Length; i++)
            {
                //check availability symbol in english alfpabet 
                bool findLetter = false;

                for (short z = 0; z < alfpabet.Length; z++)
                {
                    char checkLetter = initialText[i];
                    bool upLetter = false; //check UpperCase letter

                    if (Char.IsUpper(checkLetter))
                    {
                        checkLetter = Char.ToLower(checkLetter);
                        upLetter = true;
                    }

                    //if this letter available in english alfpabet
                    if (checkLetter == alfpabet[z])
                    {
                        findLetter = true;
                        int shiftIndex = ShiftEncrypt(z, rotate, encrypt);

                        if (upLetter)
                        {
                            resultText.Append(Char.ToUpper(alfpabet[shiftIndex]));
                        }
                        else
                        {
                            resultText.Append(alfpabet[shiftIndex]);
                        }

                        break;
                    }
                }
                //if this symbol did not find, add symbol without change
                if (!findLetter) resultText.Append(initialText[i]);
            }
            return resultText.ToString();
        }

        /// <summary>
        /// calculation index of the encoded letter
        /// </summary>
        /// <param name="index">The index letter of the original text</param>
        /// <param name="rotate">Rotate</param>
        /// <param name="encrypt">Select encrypt or decrypt text  (true - encrypt / false - decrypt)</param>
        /// <returns></returns>
        private static int ShiftEncrypt(short index, short rotate, bool encrypt)
        {
            int result;

            if (encrypt)
            {
                result = index + rotate;
                if (result >= alfpabet.Length) result -= alfpabet.Length;
            }
            else
            {
                result = index - rotate;
                if (result < 0) result += alfpabet.Length;
            }

            return result;
        }

        //return the number of matches in real english words 
        public static int countMatches(string text) {
            string[] words = text.Split(new[] { ' ', ',', ':', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);
            int counter = -1;

            foreach (var item in words)
            {
                using (DatabaseWordsEntities db = new DatabaseWordsEntities())
                {
                    //serch word in database
                    var serchWord = db.Words.Where(w => w.Value.Contains(item)).FirstOrDefault();
                    if (serchWord != null) counter++;
                }
            }

            return counter;
        }

        //iterate all rotation from 0 to 25, each variant checking in english words in database.
        //return variant who has maximum matches.
        public static int tryDecrypt(string ciphertext) {
            int resultRotation = -1;
            int maxMatches = -1;

            for (short i = 0; i < 26; i++)
            {
                string decryptStr = Encrypt(ciphertext, i, false);
                int matches = countMatches(decryptStr);
                if(matches > maxMatches)
                {
                    maxMatches = matches;
                    resultRotation = i;
                }
            }

            return resultRotation;
        }

        public static int tryAsyncDecrypt(string ciphertext)
        {
            Dictionary<int, int> all = new Dictionary<int, int>();

            for (short i = 0; i < 26; i++)
            {
                string decryptStr = Encrypt(ciphertext, i, false);
                var matches = SomeActionAsync(decryptStr);
                all[i] = matches.Result;
                matches.Wait();
            }
            
            return all.FirstOrDefault(x => x.Value == all.Values.Max()).Key;
        }

        public static Task<int> SomeActionAsync(string text)
        {
            var tcs = new TaskCompletionSource<int>();

            Task.Run(() => {
                try
                {
                    var result = countMatches(text);
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            });

            return tcs.Task;
        }
    }
}