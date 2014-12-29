using System;
using System.Globalization;
using System.Text;

// Installed from Nuget
using Newtonsoft.Json;

namespace parse_token
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
                return;

            string encodedToken = args[0];
            if (string.IsNullOrEmpty(encodedToken))
                return;

            try 
            {
                // JWT is made of three parts, separated by a '.'
                // First part is the header
                // Second part is the token
                // Third part is the signature
                string[] tokenParts = encodedToken.Split('.');
                if (tokenParts.Length < 3)
                {
                    Console.WriteLine("JWT must have three parts.");
                    return;
                }

                // Decode and pretty-print the header
                string header = PrettyPrintJson(Base64UrlEncoder.Decode(tokenParts[0]));

                // Decode and pretty-print the token
                string token = PrettyPrintJson(Base64UrlEncoder.Decode(tokenParts[1]));

                Console.WriteLine("Header: \n{0}\n\n", header);
                Console.WriteLine("Token: \n{0}", token);
            }
            catch(Exception ex)
            {
                Console.WriteLine("EXCEPTION: {0}", ex.Message);
            }
        }

        public static string PrettyPrintJson(string jsonString)
        {
            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(jsonString);
                return JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }

    static class Base64UrlEncoder
    {
        static char Base64PadCharacter = '=';
        static string DoubleBase64PadCharacter = String.Format(CultureInfo.InvariantCulture, "{0}{0}", Base64PadCharacter);
        static char Base64Character62 = '+';
        static char Base64Character63 = '/';
        static char Base64UrlCharacter62 = '-';
        static char Base64UrlCharacter63 = '_';

        public static byte[] DecodeBytes(string arg)
        {
            string s = arg;
            s = s.Replace(Base64UrlCharacter62, Base64Character62); // 62nd char of encoding
            s = s.Replace(Base64UrlCharacter63, Base64Character63); // 63rd char of encoding
            switch (s.Length % 4) // Pad 
            {
                case 0:
                    break; // No pad chars in this case
                case 2:
                    s += DoubleBase64PadCharacter; break; // Two pad chars
                case 3:
                    s += Base64PadCharacter; break; // One pad char
                default:
                    throw new ArgumentException("Illegal base64url string!", arg);
            }
            return Convert.FromBase64String(s); // Standard base64 decoder
        }

        public static string Decode(string arg)
        {
            return Encoding.UTF8.GetString(DecodeBytes(arg));
        }
    }
}
