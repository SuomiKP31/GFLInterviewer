using System.Numerics;
using GFLInterviewer.Core;
using ImGuiNET;

namespace GFLInterviewer.UI
{
    public class EditorNodeListWindow : GFLUIRepeatableWindow
    {
        InterviewerEditor owner;
        InterviewerBaseNode _curListSelectedNode;
        public static EditorNodeListWindow CreateInstance(InterviewerEditor owner)
        {
            var wd = new EditorNodeListWindow();
            wd.owner = owner;
            wd._name = "Node List";
            return wd;
        }

        protected override void DrawMenuBar()
        {
            
        }

        protected override void DrawBody()
        {
            var nodes = owner.GetNodeList();
            if (nodes.Count == 0)
            {
                return;
            }
            
            if (_curListSelectedNode == null)
            {
                _curListSelectedNode = nodes[0];
            }
            
            if (ImGui.BeginListBox("条目", new Vector2(550, 480)))
            {
                foreach (var bnode in nodes)
                {
                    bool isSelected = _curListSelectedNode == bnode;
                    if (ImGui.Selectable(bnode.GetPreviewText(), isSelected))
                    {
                        _curListSelectedNode = bnode;
                        // var dbg = bnode.GetPreviewText();

                        int idx = nodes.IndexOf(bnode);
                        owner.SelectNode(idx);
                    }

                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndListBox();
            }
        }
    }
}