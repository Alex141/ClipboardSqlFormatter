using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClipboardSqlFormatter
{
    static class StringExtensions
    {
        public static string Truncate(this string str, int maxLength)
        {
            return str.Length <= maxLength ? str : str.Substring(0, maxLength) + "...";
        }
    }
}
