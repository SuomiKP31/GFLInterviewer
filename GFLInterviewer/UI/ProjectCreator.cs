using System;
using GFLInterviewer.Core;

namespace GFLInterviewer.UI
{
    using ImGuiNET;
    public class ProjectCreator : GfluiWindow
    {
        public static ProjectCreator CreateInstance(bool activeOnStart, string name = "ProjectCreator")
        {
            var inst = new ProjectCreator();
            inst._isActive = activeOnStart;
            inst._name = name;
            return inst;
        }

        #region Attributes

        string m_projectName = String.Empty;

        #endregion
        protected override void DrawMenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Open..", "Ctrl+O"))
                    {
                        // Call core to open the project json file
                    }
                    if (ImGui.MenuItem("Save", "Ctrl+S"))   { /* Do stuff */ }
                    if (ImGui.MenuItem("Close", "Ctrl+W"))  { _isActive = false; }
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }
        }

        protected override void DrawBody()
        {
            ImGui.Text("FANART MAKER CREATED BY SUOMI");
            ImGui.Text("中文");
            ImGui.InputTextWithHint("项目名称", "填入想创建的文件名", ref m_projectName, 16);
        }
    }
}