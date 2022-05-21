using System.Numerics;
using ImGuiNET;
using Newtonsoft.Json.Linq;

namespace GFLInterviewer.Core
{
    public class InterviewerBaseSpeakerNode : InterviewerBaseNode
    {

        public static InterviewerBaseSpeakerNode CreateInstance(InterviewerProjectFile owner, NodeConf conf)
        {
            var node =  new InterviewerBaseSpeakerNode();
            node.owner = owner;
            node.conf = conf;

            node.confObject = InterviewerCore.GetConfigObject(conf);
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

            ImGui.InputTextMultiline("内容", ref content, 256, new Vector2(325,125));
        }


    }
}