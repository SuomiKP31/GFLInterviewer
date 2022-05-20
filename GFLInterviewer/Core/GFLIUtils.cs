using System;
using System.Collections.Generic;
using System.Text;

namespace GFLInterviewer.Core
{
    public static class GFLIUtils
    {
        public static string GetFileNameFromFullPath(string fullPath)
        {
            var fileName = fullPath.Split("\\")[1];
            return fileName;
        }

        public static List<string> GetSelectionListFromFullPath(string[] fullPathArray)
        {
            List<string> newList = new List<string>();
            foreach (var path in fullPathArray)
            {
                newList.Add(GetFileNameFromFullPath(path));
            }

            return newList;
        }
    }
}