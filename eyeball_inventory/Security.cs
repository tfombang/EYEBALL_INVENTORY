using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace EYEBALL_INVENTORY
{
    public class Security
    {
        public string ConvertToMD5(string inputPassword)
        {
            //Create an instance of the MD5 class
            MD5 md5 = MD5.Create();

            //Hash the Input Password and store in an array
            byte[] hashPassword = md5.ComputeHash(Encoding.Default.GetBytes(inputPassword));
            
            //Instantiate the stringBuilder class for an object
            StringBuilder returnValue = new StringBuilder();
            //Store the hash value into the stringBuilder object
            for (int i = 0; i < hashPassword.Length; i++)
            {
                returnValue.Append(hashPassword[i].ToString());
            }

            return returnValue.ToString();
        }

        public string ConvertToSHA1(string inputUsername)
        {
            //Create SHA1 instance
            SHA1 sha1 = SHA1.Create();

            //Hash the input username and store in the array
            byte[] hashUsername = sha1.ComputeHash(Encoding.Default.GetBytes(inputUsername));

            //Instantiate an object from the StringBuilder Claass
            StringBuilder hashedUsername = new StringBuilder();

            //Store the hashed value into the StringBuilder object
            for (int j = 0; j < hashedUsername.Length; j++)
            {
                hashedUsername.Append(hashUsername[j].ToString());
            }

            return hashedUsername.ToString();
        }
    }
}
