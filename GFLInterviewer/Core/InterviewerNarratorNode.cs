using System;
using System.Drawing;
using System.Numerics;
using ImGuiNET;
using Newtonsoft.Json.Linq;

namespace GFLInterviewer.Core
{
    public class InterviewerNarratorNode : InterviewerBaseNode
    {
        int _lineCount = 1;
        public static InterviewerNarratorNode CreateInstance(InterviewerProjectFile owner, NodeConf conf)
        {
            var node =  new InterviewerNarratorNode();
            node.owner = owner;
            node.conf = conf;
            node.confObject = InterviewerCore.GetConfigObject(conf);
            node._lineCount = 1;
            return node;
        }

        public static InterviewerNarratorNode CreateInstanceFromJObject(JObject nodeJson,
            InterviewerProjectFile owner)
        {
            var node =  new InterviewerNarratorNode();
            node.owner = owner;
            node.conf = nodeJson.GetValue("nodeConf").ToObject<NodeConf>();
            node.speakerName = nodeJson.GetValue("speaker").ToString();
            node.content = nodeJson.GetValue("content").ToString();
            node.fontSize = nodeJson.GetValue("fontSize").ToObject<float>();
            node._lineCount = nodeJson.GetValue("lineCount").ToObject<int>();

            node.confObject = InterviewerCore.GetConfigObject(node.conf);
            return node;
        }
        
        public override void Render(Graphics g, Rectangle rect)
        {
            
            
            InterviewerCore.SwitchFontSizeAndStyle(fontSize, FontStyle.Bold);
            
            StringFormat narratorTextFormat = new StringFormat();
            narratorTextFormat.Alignment = StringAlignment.Center;
            narratorTextFormat.LineAlignment = StringAlignment.Center;

            var contentRect = GetRectFromJObject((JObject)confObject.GetValue("contentRect"));
            
            contentRect.Height *= _lineCount;
            contentRect.X += rect.X;
            contentRect.Y += rect.Y;

            Brush narratorBgBrush = new SolidBrush(Color.FromArgb(31,31,31));
            Pen framePen = new Pen(Color.Orange);
            framePen.Width = 3;
            g.FillRectangle(narratorBgBrush, contentRect);
            g.DrawRectangle(framePen, contentRect);
            
            g.DrawString(content, InterviewerCore.drawingFont, Brushes.White, contentRect, narratorTextFormat);
        }

        public override void DrawNode()
        {
            ImGui.Text(confObject.GetValue("displayName").ToString());
            ImGui.DragInt("行数（拖动）", ref _lineCount, 0.2f, 1, 15);
            // I decided to fix the fontsize to 20 in narrator nodes.
            // Typically you don't need to change that.
            
            ImGui.InputTextMultiline("内容", ref content, 256, new Vector2(550,125));
        }
        
        public override JObject GenerateJObject()
        {
            JObject ret = new JObject();
            ret.Add("nodeConf", (int)conf);
            ret.Add("speaker", speakerName);   
            ret.Add("content", content);
            ret.Add("fontSize", fontSize);
            ret.Add("lineCount", _lineCount);
            return ret;
        }

        public override int GetHeight()
        {
            int h = 3 * _lineCount + confObject["contentRect"]["Height"].ToObject<int>() * _lineCount;
            return h;
        }

        public override string GetPreviewText()
        {
            bool tooLong = content.Length > 25;
            string contentString = content.Substring(0, tooLong ? 24 : content.Length)
                .Replace("\n", " ");
            if (tooLong)
            {
                contentString += "...";
            }
            return $"旁白: {contentString}";
        }
    }
}