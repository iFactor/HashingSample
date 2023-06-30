using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HashingSample
{
    class HashingSample
    {
        static void Main()
        {
            var parameters = new SortedDictionary<string, string>
            {
                { "field1", "value 1" },
                { "field2", "value 2" },
                { "field3", "value 3" }
            };


            var parametersAsString = new StringBuilder();

            foreach(var p in parameters)
            {
                parametersAsString.AppendFormat("{0}{1}", p.Key, HttpUtility.UrlEncode(p.Value));
            }


            Console.WriteLine(HashString(parametersAsString.ToString(), "SomeSalt"));
            Console.ReadLine();

            //hash: roSjzyTSjTg7qpU/Ih1BwMsLxpuHz1mUIsPfZuk4NDA=

        }

        public static string HashString(string stringToHash, string salt)
        {
            var hashProvider = new SHA256Managed();

            stringToHash += salt;

            var inputbuff = Encoding.UTF8.GetBytes(stringToHash);
            var outputbuff = hashProvider.ComputeHash(inputbuff);
            string hashedString = Convert.ToBase64String(outputbuff);

            return hashedString;
        }

    }
}
