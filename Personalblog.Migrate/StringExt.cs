using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Migrate
{
    public static class StringExt
    {
        public static string Limit(this string str, int length)
        {
            if (str.Length <= length)
            {
                return str;
            }

            return str[..length];
        }
    }
}
