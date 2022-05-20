using System;
using System.Collections.Generic;
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
            inst.RefreshProjectFiles();
            return inst;
        }

        #region Attributes

        string m_projectName = String.Empty;
        
        string m_chosenProjectName = String.Empty;
        
        List<string> m_projectNameList = new List<string>();

        #endregion
        protected override void DrawMenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("文件"))
                {
                    if (ImGui.MenuItem("刷新", "Ctrl+O"))
                    {
                        RefreshProjectFiles();
                    }
                    
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }
        }

        protected override void DrawBody()
        {
            ImGui.Text("FANART MAKER CREATED BY SUOMI");
            ImGui.InputTextWithHint("项目名称", "填入想创建的文件名", ref m_projectName, 16);
            if (ImGui.Button("创建项目"))
            {
                CreateProjectFile(m_projectName);
            }
            ImGui.Separator();
            if (ImGui.BeginCombo("选择现有项目打开",m_chosenProjectName))
            {
                foreach (var proj in m_projectNameList)
                {
                    bool isSelected = m_chosenProjectName == proj;
                    if (ImGui.Selectable(proj, isSelected))
                    {
                        m_chosenProjectName = proj;
                    }

                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndCombo();
            }

            if (ImGui.Button("打开已有项目"))
            {
                OpenProjectFile();
            }
        }

        void OpenProjectFile()
        {
            if (m_chosenProjectName == String.Empty)
            {
                return;
            }

            var projFile = InterviewerProjectFile.CreateInstance(m_chosenProjectName);
            var editor = InterviewerEditor.CreateInstance(projFile);
            InterviewerCore.AddRepeatableWindow(editor);
            editor.SetActive(true);
            
            RefreshProjectFiles();
        }

        void CreateProjectFile(string name)
        {
            var editor = InterviewerEditor.CreateInstance(name);
            InterviewerCore.AddRepeatableWindow(editor);
            editor.SetActive(true);
            
            RefreshProjectFiles();
        }

        void RefreshProjectFiles()
        {
            m_projectNameList = InterviewerCore.FetchProjectNameList();
        }
    }
}