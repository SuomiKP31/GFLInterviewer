﻿using System.Collections.Generic;
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
            var totalWidth = 1405;

            // Base Black Image
            Bitmap baseImage = new Bitmap(totalWidth, totalLength, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(baseImage);
            g.FillRectangle(Brushes.Black, 0, 0, totalWidth, totalLength);

            // Initial Offsets
            int nodeX = 15;
            int nodeY = 25;
            // Get a Rect and render the node on it
            foreach (var node in nodes)
            {
                int width = node.confObject.GetValue("width").ToObject<int>();
                int height = node.confObject.GetValue("height").ToObject<int>();
                int spacingY = node.confObject.GetValue("spacingY").ToObject<int>();
                
                
                Rectangle rect = new Rectangle(nodeX, nodeY, width, height);
                node.Render(g, rect);
                
                nodeY += height + spacingY;
            }

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

            return y + 25;
        }
    }
}