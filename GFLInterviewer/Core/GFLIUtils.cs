using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
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

        private static int MapColorFloatToInt(float v)
        {
            float mapped = (v / 1) * 255;
            return Convert.ToInt32(mapped);
        }

        public static Color MapColorVector(Vector3 color)
        {
            return Color.FromArgb(MapColorFloatToInt(color.X), MapColorFloatToInt(color.Y), MapColorFloatToInt(color.Z));
        }
    }
}