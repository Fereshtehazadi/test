using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashMaker
{
    // class HashGenerator räknar ut hashar för ett lösenord. 
    // Skapar hashar av typen sha256,sha384,sha512
    class HashGenerator
    {       
        SHA256Managed sha256 = null;
        SHA384Managed sha384 = null;
        SHA512Managed sha512 = null;

        // Konstruktor initierar de tre field (fält) i klassen
        public HashGenerator() 
        {
            sha256 = new SHA256Managed();
            sha384 = new SHA384Managed();           
            sha512 = new SHA512Managed();
        }

        // calculateHash() beräkna hars för det lösenordet som finns i 
        // (passwordHashData.Password)
        public void calculateHash(PasswordHashData passwordHashData)
        {
            // Gör om password till en byte[] med hjälp av Encoding klassen
            byte[] bytes = Encoding.UTF8.GetBytes(passwordHashData.Password);

            // Det skapas tre olika funktionen i klassen och här anropas dessa och skickas "bytes"
            // till dessa fuktioner som inparametern och får tillbaka motsvarande "hash"
            passwordHashData.HashSha384 = getHashSha384(bytes);
            passwordHashData.HashSha256 = getHashSha256(bytes);
            passwordHashData.HashSha512 = getHashSha512(bytes);
        }

        // nedanstående tre funktioner om vandla en byte[] till hash
        public string getHashSha384(byte[] input)
        {
            byte[] hash = sha384.ComputeHash(input);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }

        public string getHashSha256(byte[] input)
        {
            byte[] hash = sha256.ComputeHash(input);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }

        public string getHashSha512(byte[] input)
        {
            byte[] hash = sha512.ComputeHash(input);
            return BitConverter.ToString(hash).Replace("-", String.Empty);
        }
    }
}
