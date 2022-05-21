using GFLInterviewer.Core;
using ImGuiNET;

namespace GFLInterviewer.UI
{
    public class InterviewerEditor : GFLUIRepeatableWindow
    {
        public static InterviewerEditor CreateInstance(string fileName)
        {
            return new InterviewerEditor($"{fileName}.json");
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
            _speakerName = "";
            _name = "Editor";
            SelectNode(0);
        }

        InterviewerEditor(string fileName)
        {
            _project = InterviewerProjectFile.CreateInstance();

            _project.fileName = fileName;
            _fileName = fileName;
            _titleName = "标题";
            _author = "";
            _speakerName = "";
            _name = "Editor";
        }
        
        InterviewerProjectFile _project; // Core logic class

        #region UI Value Holders
        string _fileName;
        string _titleName;
        string _author;
        
        string _speakerName;
        bool _isAddingSpeaker;

        InterviewerBaseNode _currentNode;
        int _curNodeIndex = 0;
        #endregion
        
        protected override void DrawMenuBar()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("Project"))
                {
                    if (ImGui.MenuItem("保存", "Ctrl+O"))
                    {
                        // Call core to save the project json file
                        SaveFile();
                    }

                    if (ImGui.MenuItem("关闭", "Ctrl+W"))
                    {
                        // Get a menu to ask: Save or Not
                        SaveFile();
                        Toggle();
                    }

                    if (ImGui.MenuItem("增加说话者"))
                    {
                        _isAddingSpeaker = true;
                    }
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("对话节点"))
                {
                    if (ImGui.MenuItem("添加对话节点-左"))
                    {
                        AddNode(NodeConf.DialogBubbleConfigL);
                    }

                    if (ImGui.MenuItem("添加对话节点-右"))
                    {
                        AddNode(NodeConf.DialogBubbleConfigR);
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }
        }

        protected override void DrawBody()
        {
            ImGui.Text($"文件：{_fileName}");
            ImGui.InputTextWithHint("题目", "项目题头", ref _titleName, 36);
            ImGui.InputTextWithHint("作者", "作者", ref _author, 36);

            if (_isAddingSpeaker)
            {
                ImGui.Separator();
                ImGui.InputTextWithHint("说话者", "新说话者名", ref _speakerName, 16);
                ImGui.SameLine();
                if (ImGui.Button("添加"))
                {
                    _project.AddSpeaker(_speakerName);
                    _isAddingSpeaker = false;
                }
            }

            if (_currentNode != null)
            {
                ImGui.Separator();
                ImGui.Text($"Node No.{_curNodeIndex}");
                _currentNode.DrawNode();
                if (ImGui.Button("上一个节点"))
                {
                    PrevNode();
                }
                ImGui.SameLine();
                if (ImGui.Button("下一个节点"))
                {
                    NextNode();
                }
                ImGui.SameLine();
                if (ImGui.Button("删除此节点"))
                {
                    RemoveCurNode();
                }
            }

            
        }

        protected void UpdateNodeListWindow()
        {
            // TODO: A scroll view window that holds a brief view of all nodes
        }

        public void SelectNode(int index)
        {
            _curNodeIndex = index < 0 ? 0 : index;
            _currentNode = _project.GetNode(_curNodeIndex);
            UpdateNodeListWindow();
        }

        public void AddNode(NodeConf conf)
        {
            int index = _project.CreateNode(conf);
            SelectNode(index);
        }

        public void PrevNode()
        {
            SelectNode(_curNodeIndex - 1);
        }

        public void NextNode()
        {
            SelectNode(_curNodeIndex + 1);
        }

        public void RemoveCurNode()
        {
            _project.RemoveNode(_curNodeIndex);
            PrevNode();
        }

        #region Project File Operation

        void WriteMetaData()
        {
            _project.SetMetaData(_author, _titleName);
        }

        void SaveFile()
        {
            WriteMetaData();
            _project.SaveInstanceToFile();
        }

        #endregion
        
    }
}