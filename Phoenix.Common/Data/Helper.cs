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
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
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
            switch (caste)
            {
                case 0:
                    return "(Vagabond)";
                case 1:
                    return "(Peasant)";
                case 2:
                    return "(Farmer)";
                case 3:
                    return "(Knight)";
                default:
                    return "(None)";
            }
        }

        public static string ReturnPhilosophyText(int philosophy)
        {
            switch (philosophy)
            {
                case 0:
                    return "(War)";
                case 1:
                    return "(Arcane)";
                case 2:
                    return "(Faith)";
                case 3:
                    return "(Subversion)";
                default:
                    return "(None)";
            }
        }

        public static string ReturnCharacterTypeText(int type)
        {
            switch (type)
            {
                case 0:
                    return "(Player)";
                case 1:
                    return "(Hero)";
                case 2:
                    return "(Immortal)";
                case 3:
                    return "(Demi-God)";
                case 4:
                    return "(God)";
                default:
                    return "(None)";
            }
        }

        public static string ReturnCharacterRankText(int rank)
        {
            switch (rank)
            {
                case 0:
                    return "(Initiate)";
                default:
                    return "(None)";
            }
        }

        public static string ReturnEntityTypeText(int type)
        {
            switch (type)
            {
                case 0:
                    return "(Friendly)";
                case 1:
                    return "(Spawned)";
                case 2:
                    return "(Neutral)";
                case 3:
                    return "(Hostile)";
                default:
                    return "(None)";
            }
        }

        public static string ReturnRarityText(int rarity)
        {
            switch (rarity)
            {
                case 0:
                    return "(Junk)";
                case 1:
                    return "(Common)";
                case 2:
                    return "(Uncommon)";
                case 3:
                    return "(Rare)";
                case 4:
                    return "(Epic)";
                case 5:
                    return "(Legendary)";
                case 6:
                    return "(Ancient)";
                case 7:
                    return "(Ethyreal)";
                case 8:
                    return "(Godly)";
                default:
                    return "(None)";
            }
        }

    }

}
