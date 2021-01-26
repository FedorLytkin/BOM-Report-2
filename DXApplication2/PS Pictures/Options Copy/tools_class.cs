using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveDXF.OptionsFold
{
    public class tools_class
    {
        public string FixInvalidChars(string text, string replaceTo)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (text.Length <= 0)
                return text.ToString();

            foreach (string badChar in new[] { "\\", "/", ":", "*", Convert.ToString('"'), ">", "<", "|", "+", "?" })
                text = text.Replace(badChar, replaceTo);
            return text.ToString();
        }
        public static string FixInvalidChars_St(string text, string replaceTo)
        {
            if (text == null)
                return text;
                //throw new ArgumentNullException("text");
            if (text.Length <= 0)
                return text.ToString();

            foreach (string badChar in new[] { "\\", "/", ":", "*", Convert.ToString('"'), ">", "<", "|", "+", "?", "\u0001" })
                text = text.Replace(badChar, replaceTo);
            return text.ToString();
        }
        public static string SplitString(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            if (string.IsNullOrWhiteSpace(text)) return text;
            string[] splitter = new string[] { "@"};
            string[] arr = text.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            return arr[0];
        }
    }
}
