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
            var salt = "SomeSalt";

            //Shopping cart parameter sample
            var parameters = new SortedDictionary<string, string>
            {
                { "CartItems", "[{\"ACCOUNTNUMBER\": \"12345\", \"AMOUNT\": \"134.50\",\"DUEDATE\":\"2023/06/01\"},{\"ACCOUNTNUMBER\": \"34567\", \"AMOUNT\": \"25.00\",\"DUEDATE\":\"2023/06/01\"}]" }
            };

            // standard parameters sample
            /*
            var parameters = new SortedDictionary<string, string>
            {
                {"Account","12345"},
                {"Zip","90210"},
                {"Amount", "134.50"},
                {"DueDate", "2023/06/01"}
            };
            */

            var parametersAsString = new StringBuilder();

            foreach(var p in parameters)
            {
                parametersAsString.AppendFormat("{0}{1}", p.Key, HttpUtility.UrlEncode(p.Value));
            }

            Console.WriteLine("***** string to hash *******");
            Console.WriteLine(parametersAsString);
            Console.WriteLine();

            Console.WriteLine("***** hash *******");
            var hash = HashString(parametersAsString.ToString(), salt);
            Console.WriteLine(hash);

            Console.WriteLine();
            Console.WriteLine("***** sample post request *******");
            // html form post
            var sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine(@"<body onload='document.forms[""form""].submit()'>");
            sb.AppendLine("<form name='form' action='[Replace with post url]' method='post'>");
            foreach (var p in parameters)
            {
                sb.AppendLine($" <input type='hidden' name='{p.Key}' value='{HttpUtility.HtmlEncode(p.Value)}'>");
            }
            sb.AppendLine($" <input type='hidden' name='ID' value='{hash}'>");
            sb.AppendLine("</form>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            Console.WriteLine(sb.ToString());
            Console.ReadLine();

            //shopping cart parameter hash: k5WxwK3C5b+thLlJY5YlzKI7+dWm4gySggsWzXhaYTE=

            //standard parameters hash: O0pwVrgrSAqNzkLTsAgohqQVeO/Rj5PsEzcBnZB0rgE=

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
