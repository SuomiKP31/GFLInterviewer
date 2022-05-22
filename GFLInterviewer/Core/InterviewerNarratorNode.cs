using System.Drawing;
using System.Numerics;
using ImGuiNET;
using Newtonsoft.Json.Linq;

namespace GFLInterviewer.Core
{
    public class InterviewerNarratorNode : InterviewerBaseNode
    {
        int _lineCount = 0;
        public static InterviewerNarratorNode CreateInstance(InterviewerProjectFile owner, NodeConf conf)
        {
            var node =  new InterviewerNarratorNode();
            node.owner = owner;
            node.conf = conf;
            node.confObject = InterviewerCore.GetConfigObject(conf);
            node._lineCount = 0;
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
            // TODO
        }

        public override void DrawNode()
        {
            ImGui.Text(confObject.GetValue("displayName").ToString());
            ImGui.DragInt("行数", ref _lineCount, 0.2f, 1, 5);
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
    }
}