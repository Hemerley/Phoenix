using System;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Phoenix.Common.Data
{
    public class Helper
    {

        #region -- Validation --

        public static bool HasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,^";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }

        public static bool EHasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«£§€{};'<>,";
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
                static string DomainMapper(Match match)
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

        #endregion

        #region -- Mark-Up --

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

        public static string RemovePercent(string input)
        {
            return input.Replace("%", "&percent&");
        }

        public static string ReturnPercent(string input)
        {
            return input.Replace("&percent&", "%");
        }

        #endregion

        #region -- Returns --

        public static string ReturnCasteText(int caste)
        {
            return caste switch
            {
                0 => "(Vagabond)",
                1 => "(Peasant)",
                2 => "(Farmer)",
                3 => "(Knight)",
                _ => "(None)",
            };
        }

        public static string ReturnPhilosophyText(int philosophy)
        {
            return philosophy switch
            {
                0 => "(War)",
                1 => "(Arcane)",
                2 => "(Faith)",
                3 => "(Subversion)",
                _ => "(None)",
            };
        }

        public static string ReturnCharacterTypeText(int type)
        {
            return type switch
            {
                0 => "(Player)",
                1 => "(Hero)",
                2 => "(Immortal)",
                3 => "(Demi-God)",
                4 => "(God)",
                _ => "(None)",
            };
        }

        public static string ReturnCharacterRankText(int rank)
        {
            return rank switch
            {
                0 => "(Initiate)",
                _ => "(None)",
            };
        }

        public static string ReturnNPCTypeText(int type)
        {
            return type switch
            {
                0 => "(Friendly)",
                1 => "(Spawned)",
                2 => "(Neutral)",
                3 => "(Hostile)",
                _ => "(None)",
            };
        }

        public static string ReturnRarityText(int rarity)
        {
            return rarity switch
            {
                0 => "(Junk)",
                1 => "(Common)",
                2 => "(Uncommon)",
                3 => "(Rare)",
                4 => "(Epic)",
                5 => "(Legendary)",
                6 => "(Ancient)",
                7 => "(Ethyreal)",
                8 => "(Godly)",
                _ => "(None)",
            };
        }

        #endregion

        #region -- Color --
        public static Color ReturnColor(char code)
        {
            return code switch
            {
                'b' => Color.Blue,
                'w' => Color.White,
                'g' => Color.LawnGreen,
                'r' => Color.Red,
                'o' => Color.Orange,
                'y' => Color.Yellow,
                'q' => Color.BlueViolet,
                'v' => Color.MediumOrchid,
                'm' => Color.Magenta,
                'p' => Color.Pink,
                'l' => Color.DarkGray,
                'c' => Color.Cyan,
                _ => Color.White,
            };
        }
        #endregion
    }

}
