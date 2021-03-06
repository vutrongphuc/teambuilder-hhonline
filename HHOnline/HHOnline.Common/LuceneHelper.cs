﻿using System;
using System.Globalization;

namespace HHOnline.Common
{
    public class LuceneHelper
    {
        private static readonly string[] LuceneKeywords = new string[] { @"\", "(", ")", ":", "^", "[", "]", "{", "}", "~", "*", "?", "!", "\"", "'" };
        private static readonly string[] LuceneOperator = new string[] { "AND", "OR", "NOT" };

        public static string LuceneKeywordsScrubber(string str)
        {
            str = Remove(str, LuceneKeywords);
            foreach (string op in LuceneOperator)
            {
                str = str.Replace(op, op.ToLower());
            }
            return str.Trim();
        }

        private static string Remove(string str, string[] removeStrings)
        {
            if (string.IsNullOrEmpty(str) || (removeStrings == null))
            {
                return str;
            }
            string[] strArray = str.Split(removeStrings, StringSplitOptions.RemoveEmptyEntries);
            return string.Join("", strArray);
        }

        public static bool StartsWithLetterOrDigit(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            char c = str[0];
            switch (char.GetUnicodeCategory(c))
            {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.DecimalDigitNumber:
                    return true;
            }
            return false;
        }

    }
}
