using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Phoenix.Common.Data
{
    public class Helper
    {
        public static bool hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static string RemovePipe(string input)
        {
            return input.Replace("|", "&pipe&");
        }

        public static string ReturnPipe(string input)
        {
            return input.Replace("&pipe&", "|");
        }

        public static string RemoveTilda(string input)
        {
            return input.Replace("~", "&tilda&");
        }

        public static string ReturnTilda(string input)
        {
            return input.Replace("&tilda&", "~");
        }

        public static string RemoveCaret(string input)
        {
            return input.Replace("^", "&caret&");
        }

        public static string ReturnCaret(string input)
        {
            return input.Replace("&caret&", "^");
        }

        public static bool HasUpperLowerDigit(string text)
        {
            bool hasUpper = false; bool hasLower = false; bool hasDigit = false;
            for (int i = 0; i < text.Length && !(hasUpper && hasLower && hasDigit); i++)
            {
                char c = text[i];
                if (!hasUpper) hasUpper = char.IsUpper(c);
                if (!hasLower) hasLower = char.IsLower(c);
                if (!hasDigit) hasDigit = char.IsDigit(c);
            }
            return hasUpper && hasLower && hasDigit;
        }

        public static string ReturnCasteText(int caste)
        {
            string casteString = "";

            /*Vagabond
Peasant
Farmer/Merchant/Craftsmen
Knight/Vassal Initiate, 
Baron/Baroness
Viscount/Viscountess
Earl/Countess
Duke/Duchess
Prince/Princess
King/Queen*/

            switch (caste)
            {
                case 0:

                    return casteString = "Vagabond";
                case 1:

                    return casteString = "Peasant";
                case 2:

                    return casteString = "Farmer";
                case 3:

                    return casteString = "Knight";
                default:
                    return casteString;
            }
        }

        public static string ReturnPhilosophyText(int philosophy)
        {
            string philosophyString = "";

            switch (philosophy)
            {
                case 0:

                    return philosophyString = "War";
                case 1:

                    return philosophyString = "Faith";
                case 2:

                    return philosophyString = "Chaos";
                case 3:

                    return philosophyString = "Subversion";
                default:
                    return philosophyString;
            }
        }

    }

}
