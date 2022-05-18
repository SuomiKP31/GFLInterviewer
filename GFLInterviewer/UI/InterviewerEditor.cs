using GFLInterviewer.Core;
using ImGuiNET;

namespace GFLInterviewer.UI
{
    public class InterviewerEditor : GFLUIRepeatableWindow
    {
        public static InterviewerEditor CreateInstance(string fileName)
        {
            return new InterviewerEditor(fileName);
        }
        
        public static InterviewerEditor CreateInstance(InterviewerProjectFile file)
        {
            return new InterviewerEditor(file);
        }

        InterviewerEditor(InterviewerProjectFile file)
        {
            _project = file;
            _fileName = file.fileName;
            _titleName = file.projectName;
            _author = file.author;
            _name = "Editor";
        }

        InterviewerEditor(string fileName)
        {
            _project = InterviewerProjectFile.CreateInstance();

            _project.fileName = fileName;
            _fileName = fileName;
            _titleName = "标题";
            _author = "";
            _name = "Editor";
        }
        
        InterviewerProjectFile _project; // Core logic class

        #region UI Value Holders
        string _fileName;
        string _titleName;
        string _author;
        
        #endregion
        
        protected override void DrawMenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("Project"))
                {
                    if (ImGui.MenuItem("Save", "Ctrl+O"))
                    {
                        // Call core to save the project json file
                        SaveFile();
                    }

                    if (ImGui.MenuItem("Close", "Ctrl+W"))
                    {
                        // Get a menu to ask: Save or Not
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }
        }

        protected override void DrawBody()
        {
            ImGui.Text($"文件：{_fileName}.json");
            ImGui.InputTextWithHint("题目", "项目题头", ref _titleName, 36);
            ImGui.InputTextWithHint("作者", "作者", ref _author, 36);
        }

        void WriteMetaData()
        {
            _project.author = _author;
            _project.projectName = _titleName;
        }

        void SaveFile()
        {
            WriteMetaData();
            _project.SaveInstanceToFile();
        }
    }
}