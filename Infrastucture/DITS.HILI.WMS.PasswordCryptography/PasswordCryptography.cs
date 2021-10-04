using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DITS.HILI.WMS.PasswordCrypt
{

    public class PasswordCryptography
    {

        #region "Definition"
        private static readonly int DEFAULT_MIN_PASSWORD_LENGTH = 8;
        private static readonly int DEFAULT_MAX_PASSWORD_LENGTH = 10;
        private static readonly string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstuwxyz";
        private static readonly string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTVWXYZ";
        private static readonly string PASSWORD_CHARS_NUMERIC = "23456789";
        #endregion
        private static readonly string PASSWORD_CHARS_SPECIAL = "*$-+?&=!%/";

        #region "Compare"
        internal static bool IsValidCurrentPassword(string _CurrentPassword, string _PasswordSalt, string _PasswordHash)
        {
            string currentHash = GenerateHashPassword(_CurrentPassword, _PasswordSalt);
            if ((currentHash != _PasswordHash))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region "Generate Random"
        public static string GenRandomPwd()
        {
            return GenRandomPwd(DEFAULT_MIN_PASSWORD_LENGTH, DEFAULT_MAX_PASSWORD_LENGTH);
        }

        public static string GenRandomPwd(int length)
        {
            return GenRandomPwd(length, length);
        }

        public static string GenRandomPwd(int minLength, int maxLength)
        {
            string functionReturnValue = null;
            // Make sure that input parameters are valid.
            if ((minLength <= 0 | maxLength <= 0 | minLength > maxLength))
            {
                functionReturnValue = null;
            }

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][] {
            PASSWORD_CHARS_LCASE.ToCharArray(),
            PASSWORD_CHARS_UCASE.ToCharArray(),
            PASSWORD_CHARS_NUMERIC.ToCharArray(),
            PASSWORD_CHARS_SPECIAL.ToCharArray()
        };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            int I = 0;
            for (I = 0; I <= charsLeftInGroup.Length - 1; I++)
            {
                charsLeftInGroup[I] = charGroups[I].Length;
            }

            // Use this array to track (iterate through) unused character groups.
            int[] leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (I = 0; I <= leftGroupsOrder.Length - 1; I++)
            {
                leftGroupsOrder[I] = I;
            }

            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = ((randomBytes[0] & 0x7f) << 24 | randomBytes[1] << 16 | randomBytes[2] << 8 | randomBytes[3]);

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;
            // Allocate appropriate memory for the password.
            if ((minLength < maxLength))
            {
                password = new char[random.Next(minLength - 1, maxLength) + 1];
            }
            else
            {
                password = new char[minLength];
            }

            // Index of the next character to be added to password.
            int nextCharIdx = 0;

            // Index of the next character group to be processed.
            int nextGroupIdx = 0;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx = 0;

            // Index of the last non-processed character in a group.
            int lastCharIdx = 0;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate password characters one at a time.

            for (I = 0; I <= password.Length - 1; I++)
            {
                // If only one character group remained unprocessed, process it;
                // otherwise, pick a random character group from the unprocessed
                // group list. To allow a special character to appear in the
                // first position, increment the second parameter of the Next
                // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                if ((lastLeftGroupsOrderIdx == 0))
                {
                    nextLeftGroupsOrderIdx = 0;
                }
                else
                {
                    nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx);
                }

                // Get the actual index of the character group, from which we will
                // pick the next character.
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                // Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                // If only one unprocessed character is left, pick it; otherwise,
                // get a random character from the unused character list.
                if ((lastCharIdx == 0))
                {
                    nextCharIdx = 0;
                }
                else
                {
                    nextCharIdx = random.Next(0, lastCharIdx + 1);
                }

                // Add this character to the password.
                password[I] = charGroups[nextGroupIdx][nextCharIdx];

                // If we processed the last character in this group, start over.
                if ((lastCharIdx == 0))
                {
                    charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
                    // There are more unprocessed characters left.
                }
                else
                {
                    // Swap processed character with the last unprocessed character
                    // so that we don't pick it until we process all characters in
                    // this group.
                    if ((lastCharIdx != nextCharIdx))
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] = charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }

                    // Decrement the number of unprocessed characters in this group.
                    charsLeftInGroup[nextGroupIdx] = charsLeftInGroup[nextGroupIdx] - 1;
                }

                // If we processed the last group, start all over.
                if ((lastLeftGroupsOrderIdx == 0))
                {
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                }
                else
                {
                    if ((lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx))
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx = lastLeftGroupsOrderIdx - 1;
                }
            }

            // Convert password characters into a string and return the result.
            functionReturnValue = new string(password);
            return functionReturnValue;
        }
        #endregion

        #region "Crypto"
        public static string Encrypt(string clearText, string sharedSecret)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(sharedSecret, new byte[] {
                0x49,
                0x76,
                0x61,
                0x6e,
                0x20,
                0x4d,
                0x65,
                0x64,
                0x76,
                0x65,
                0x64,
                0x65,
                0x76
            });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText, string sharedSecret)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(sharedSecret, new byte[] {
                0x49,
                0x76,
                0x61,
                0x6e,
                0x20,
                0x4d,
                0x65,
                0x64,
                0x76,
                0x65,
                0x64,
                0x65,
                0x76
            });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion

        #region "Generate Password"
        public static string GenerateHashPassword(string p_strPwd, string p_salt)
        {
            // SHA 256 is not supported on Windows XP (Vista ok)
            HashAlgorithm hashAlgo = new SHA1CryptoServiceProvider();

            if (p_salt.Length <= 5)
            {
                p_salt = "%8SbCI!lO9&";
            }

            if (p_strPwd == string.Empty)
            {
                p_strPwd = "HelloHowAreYou00";
            }

            string strConcat = p_salt.Substring(0, 3) + p_strPwd + "*" + p_salt.Substring(4);
            byte[] pwdBytes = System.Text.Encoding.Default.GetBytes(strConcat);
            byte[] resultByte = hashAlgo.ComputeHash(pwdBytes);

            return System.Convert.ToBase64String(resultByte);
        }

        public static string GenerateCharPassword(int length, int numberOfNonAlphanumericCharacters)
        {
            //Make sure length and numberOfNonAlphanumericCharacters are valid....
            if (((length < 1) || (length > 128)))
            {
                throw new ArgumentException("Password_Incorrect_Length");
            }

            if (((numberOfNonAlphanumericCharacters > length) || (numberOfNonAlphanumericCharacters < 0)))
            {
                throw new ArgumentException("Password_Incorrect_Character");
            }

            while (true)
            {
                int i = 0;
                int nonANcount = 0;
                byte[] buffer1 = new byte[length];

                //chPassword contains the password's characters as it's built up
                char[] chPassword = new char[length];
                //chPunctionations contains the list of legal non-alphanumeric characters
                char[] chPunctuations = "!@@$%^^*()_-+=[{]};:>|./?".ToCharArray();

                //'Get a cryptographically strong series of bytes
                System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
                rng.GetBytes(buffer1);

                for (i = 0; i <= length - 1; i++)
                {
                    //Convert each byte into its representative character
                    int rndChr = (buffer1[i] % 87);
                    if ((rndChr < 10))
                    {
                        chPassword[i] = Convert.ToChar(Convert.ToUInt16(48 + rndChr));
                    }
                    else
                    {
                        if ((rndChr < 36))
                        {
                            chPassword[i] = Convert.ToChar(Convert.ToUInt16((65 + rndChr) - 10));
                        }
                        else
                        {
                            if ((rndChr < 62))
                            {
                                chPassword[i] = Convert.ToChar(Convert.ToUInt16((97 + rndChr) - 36));
                            }
                            else
                            {
                                chPassword[i] = chPunctuations[rndChr - 62];
                                nonANcount += 1;
                            }
                        }
                    }
                }

                if (nonANcount < numberOfNonAlphanumericCharacters)
                {
                    Random rndNumber = new Random();
                    for (i = 0; i <= (numberOfNonAlphanumericCharacters - nonANcount) - 1; i++)
                    {
                        int passwordPos = 0;
                        do
                        {
                            passwordPos = rndNumber.Next(0, length);
                        } while (!char.IsLetterOrDigit(chPassword[passwordPos]));
                        chPassword[passwordPos] = chPunctuations[rndNumber.Next(0, chPunctuations.Length)];
                    }
                }
                return new string(chPassword);
            }
            return null;
        }

        #endregion

    }
}
