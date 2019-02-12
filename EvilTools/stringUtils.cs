using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvilTools {
    
    public class StringUtils {
        /// <summary>
        /// Capitalizes the first letter in a string
        /// </summary>
        /// <param name="strToCapitalize">String to capitalize</param>
        /// <returns>The capitalized string or String.empty if 
        /// string was null or only whitespace</returns>
        public static string CapitalizeString(string strToCapitalize) {
            if (String.IsNullOrWhiteSpace(strToCapitalize)) {
                return String.Empty;
            }
            strToCapitalize = strToCapitalize.Trim();
            if (strToCapitalize.Length == 1) {
                return char.ToUpper(strToCapitalize[0]).ToString();
            }
            return char.ToUpper(strToCapitalize[0]) + 
                strToCapitalize.Substring(1);
        }

        /// <summary>
        /// Gets the percentege, as a string, of the number represents over basis:
        /// (Number/Basis) * 100
        /// </summary>
        /// <param name="number">Number that represnts the percentage over basis</param>
        /// <param name="basis">The basis of the number</param>
        /// <returns>A string in the format x.xx%</returns>
        public static string GeneratePercentageAsStr(int number, int basis) {
            return (((float)number / (float)basis) * (float)100).ToString("0.0");
        }

        /// <summary>
        /// Retruns a string of whitespace with a number of spaces equal to spaces
        /// </summary>
        /// <param name="spaces"></param>
        /// <returns></returns>
        public static string GenerateSpaces(int spaces) {
            string str = "";
            for (int i = 0; i < spaces; i++) {
                str += " ";
            }
            return str;
        }
    }
}
