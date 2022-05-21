using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace GFLInterviewer.Core
{
    public static class InterviewerPainter
    {
        // Static API. Called from core. Render InterviewerNode to a png file, and output.

        public static void RenderPngFile(InterviewerProjectFile proj)
        {
            var nodes = proj.GetNodeList();
            var totalLength = CalcLength(nodes);
            var totalWidth = 1400;

            Bitmap baseImage = new Bitmap(totalWidth, totalLength, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(baseImage);
            
            g.FillRectangle(Brushes.Black, 0, 0, totalWidth, totalLength);

            string fName = $"{InterviewerCore.outputPath}\\{proj.projectName}.png";
            baseImage.Save(fName);
            InterviewerCore.LogInfo($"Saved PNG {proj.projectName}.png");
        }

        static int CalcLength(List<InterviewerBaseNode> nodes)
        {
            int y = 0;
            foreach (var node in nodes)
            {
                var nodeConf = node.confObject;
                y += nodeConf.GetValue("height").ToObject<int>();
                y += nodeConf.GetValue("spacingY").ToObject<int>();
            }

            return y;
        }
    }
}