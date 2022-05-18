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

        }

        protected override void DrawBody()
        {
            ImGui.Text("FANART MAKER CREATED BY SUOMI");
            ImGui.InputTextWithHint("项目名称", "填入想创建的文件名", ref m_projectName, 16);
            if (ImGui.Button("创建项目"))
            {
                CreateProjectFile(m_projectName);
            }
        }

        void OpenProjectFile()
        {
            
        }

        void CreateProjectFile(string name)
        {
            var editor = InterviewerEditor.CreateInstance(name);
            InterviewerCore.AddRepeatableWindow(editor);
            editor.SetActive(true);
        }
        
    }
}