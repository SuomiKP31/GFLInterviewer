using System;
using System.Text;

namespace GFLInterviewer.Core
{
    public static class GFLIUtils
    {
        public static string GetUTFString(string uniString)
        {
            byte[] bytes = Encoding.Default.GetBytes(uniString);
            uniString = Encoding.UTF8.GetString(bytes);

            return uniString;
        }
    }
}