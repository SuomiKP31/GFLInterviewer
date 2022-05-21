using System.Numerics;
using ImGuiNET;
using Newtonsoft.Json.Linq;

namespace GFLInterviewer.Core
{
    public class InterviewerBaseSpeakerNode : InterviewerBaseNode
    {
        string avatarName;

        public static InterviewerBaseSpeakerNode CreateInstance(InterviewerProjectFile owner, NodeConf conf)
        {
            var node =  new InterviewerBaseSpeakerNode();
            node.owner = owner;
            node.conf = conf;
            node.avatarName = "";
            node.confObject = InterviewerCore.GetConfigObject(conf);
            return node;
        }

        public static InterviewerBaseSpeakerNode CreateInstanceFromJObject(JObject nodeJson,
            InterviewerProjectFile owner)
        {
            var node =  new InterviewerBaseSpeakerNode();
            node.owner = owner;
            node.conf = nodeJson.GetValue("nodeConf").ToObject<NodeConf>();
            node.speakerName = nodeJson.GetValue("speaker").ToString();
            node.avatarName = nodeJson.GetValue("avatar").ToString();
            node.content = nodeJson.GetValue("content").ToString();
            node.fontSize = nodeJson.GetValue("fontSize").ToObject<float>();

            node.confObject = InterviewerCore.GetConfigObject(node.conf);
            return node;
        }


        public override void Render()
        {
            throw new System.NotImplementedException();
        }

        public override void DrawNode()
        {
            ImGui.Text(confObject.GetValue("displayName").ToString());
            // Chose speaker of this node. To add a speaker, use the editor menu
            if (ImGui.BeginCombo("说话人", speakerName))
            {
                foreach (var proj in owner.speakerNames)
                {
                    bool isSelected = speakerName == proj;
                    if (ImGui.Selectable(proj, isSelected))
                    {
                        speakerName = proj;
                    }

                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndCombo();
            }
            
            DrawAvatarList();
            // ImGui.SameLine();

            ImGui.InputFloat("字号", ref fontSize);
            ImGui.InputTextMultiline("内容", ref content, 256, new Vector2(325,125));
        }

        public override JObject GenerateJObject()
        {
            JObject ret = new JObject();
            ret.Add("nodeConf", (int)conf);
            ret.Add("speaker", speakerName);
            ret.Add("avatar", avatarName);
            ret.Add("content", content);
            ret.Add("fontSize", fontSize);
            return ret;
        }

        public override void DrawAvatarList()
        {
            if (ImGui.BeginCombo("头像", avatarName))
            {
                foreach (var avatar in InterviewerCore.avatarNames)
                {
                    bool isSelected = avatarName == avatar;
                    if (ImGui.Selectable(avatar, isSelected))
                    {
                        avatarName = avatar;
                    }

                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndCombo();
            }
        }
    }
}