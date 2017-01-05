using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        //return regular expression for dinamic condition serch english words 
        public static Expression<Func<Word,bool>> getPredicate(string[] words) {
            var predicate = PredicateBuilder.False<Word>();

            foreach (string keyword in words)
            {
                predicate = predicate.Or(p => p.Value == keyword);
            }

            return predicate;
        }

        //iterate all rotation from 0 to 25, each variant checking in english words in database.
        //return variant who has maximum matches.
        public static int tryDecrypt(string ciphertext) {
            int resultRotation = -1;
            int maxMatches = 0;

            using (DatabaseWordsEntities db = new DatabaseWordsEntities())
            {
                for (short i = 0; i < 26; i++)
                {
                    string decryptStr = Encrypt(ciphertext, i, false);
                    string[] words = decryptStr.Split(new[] { ' ', ',', ':', '?', '!' }, StringSplitOptions.RemoveEmptyEntries);
                    int matches = db.Words.Where(getPredicate(words).Compile()).Count();
                    if (matches > maxMatches)
                    {
                        if (matches > 4) return i;//if found many matches not sense to find more
                        maxMatches = matches;
                        resultRotation = i;
                    }
                }
            }

            return resultRotation;
        }

        //iterate all rotation from 0 to 25, each variant checking in english words in NHunspell lib.
        //return variant who has maximum matches.
        public static int tryDecryptWithoutDatabase(string ciphertext)
        {
            int resultRotation = -1;
            int maxMatches = 0;

            using (DatabaseWordsEntities db = new DatabaseWordsEntities())
            {
                for (short i = 0; i < 26; i++)
                {
                    string decryptStr = Encrypt(ciphertext, i, false);
                    
                    int matches = Speller.Analyze(decryptStr);
                    if (matches > maxMatches && matches > 0)
                    {
                        maxMatches = matches;
                        resultRotation = i;
                    }
                }
            }

            return resultRotation;
        }
    }
}