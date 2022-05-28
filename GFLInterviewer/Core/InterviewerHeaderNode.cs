﻿using System.Drawing;
using Newtonsoft.Json.Linq;

namespace GFLInterviewer.Core
{
    public class InterviewerHeaderNode : InterviewerBaseNode
    {
        // A fixed node(1375x350). If selected, create a header that displays the author and title of the picture
        public InterviewerHeaderNode()
        {
            JObject tmpConfObj = new JObject();
            tmpConfObj.Add("spacingY", 25);
            tmpConfObj.Add("width", 1375);
            tmpConfObj.Add("height", 220);
            confObject = tmpConfObj;
        }
        public override void Render(Graphics g, Rectangle rect)
        {
            // 120 50 50
            // 0. Title = content
            Rectangle titleRect = rect;
            titleRect.Height = 120;
            var fmt = new StringFormat();
            InterviewerCore.SwitchFontSizeAndStyle(48, FontStyle.Bold);
            fmt.Alignment = StringAlignment.Center;
            fmt.LineAlignment = StringAlignment.Center;
            g.DrawString(content, InterviewerCore.drawingFont, Brushes.White, titleRect, fmt);
            
            // 1. Author = speaker

            fmt.Alignment = StringAlignment.Near;
            titleRect.Y += titleRect.Height;
            titleRect.Height = 50;
            InterviewerCore.SwitchFontSize(30);
            g.DrawString($"作者：{speakerName}", InterviewerCore.drawingFont, Brushes.Orange,  titleRect, fmt);
            
            // 2. other
            InterviewerCore.SwitchFontSizeAndStyle(32, FontStyle.Italic);
            titleRect.Y += titleRect.Height;
            g.DrawString("Generated By GFLInterviewer", InterviewerCore.drawingFont, Brushes.White,titleRect ,fmt);
        }

        public override void DrawNode()
        {
            // Don't need to draw it in the editor view
        }

        public override int GetHeight()
        {
            return 220;
        }
    }
}