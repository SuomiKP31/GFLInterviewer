using System.Collections.Generic;
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

            _subWindow = EditorNodeListWindow.CreateInstance(this);
            _subWindow.SetActive(true);
            InterviewerCore.AddRepeatableWindow(_subWindow);
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
            
            _subWindow = EditorNodeListWindow.CreateInstance(this);
            _subWindow.SetActive(true);
            InterviewerCore.AddRepeatableWindow(_subWindow);
        }
        
        InterviewerProjectFile _project; // Core logic class

        #region UI Value Holders
        string _fileName;
        string _titleName;
        string _author;
        
        string _speakerName;
        bool _isAddingSpeaker;

        int _criticalOps = 0;

        InterviewerBaseNode _currentNode;
        EditorNodeListWindow _subWindow;
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

                    if (ImGui.MenuItem("添加旁白节点"))
                    {
                        AddNode(NodeConf.NarratorConfig);
                    }

                    if (ImGui.MenuItem("在当前节点后插入对话节点"))
                    {
                        InsertNodeAfterCur(NodeConf.DialogBubbleConfigL);
                    }
                    
                    if (ImGui.MenuItem("在当前节点后插入旁白节点"))
                    {
                        InsertNodeAfterCur(NodeConf.NarratorConfig);
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }
        }

        protected override void DrawBody()
        {
            ImGui.Text($"文件：{_fileName}");
            ImGui.SameLine();
            ImGui.Text($"节点数： {_project.GetNodeList().Count}");
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
                ImGui.Text($"Node No.{_curNodeIndex+1}");
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

                if (ImGui.Button("输出png"))
                {
                    OutputPngFile();
                }
            }

            
        }

        public override void OnClose()
        {
            base.OnClose();
            _subWindow.SetActive(false);
        }
        

        public void SelectNode(int index)
        {
            _curNodeIndex = index < 0 ? -1 : index;
            _currentNode = _project.GetNode(_curNodeIndex);
            CriticalOpCounter();
            if (_curNodeIndex == -1)
            {
                // For cycling back to the end of the list
                // GetNode will return the last node if parameter < 0
                _curNodeIndex = _project.GetNodeList().Count - 1;
            }
        }

        protected void AddNode(NodeConf conf)
        {
            int index = _project.CreateNode(conf);
            CriticalOpCounter(3);
            SelectNode(index);
        }

        protected void PrevNode()
        {
            SelectNode(_curNodeIndex - 1);
        }

        protected void NextNode()
        {
            SelectNode(_curNodeIndex + 1);
        }

        protected void RemoveCurNode()
        {
            _project.RemoveNode(_curNodeIndex);
            PrevNode();
            CriticalOpCounter(3);
        }

        public List<InterviewerBaseNode> GetNodeList()
        {
            return _project.GetNodeList();
        }

        protected void InsertNodeAfterCur(NodeConf conf)
        {
            int index = _project.CreateNode(conf);
            CriticalOpCounter(3);
            var node = _project.GetNode(index);
            _project.MoveNode(node, _curNodeIndex + 1);
        }

        /// <summary>
        /// Trigger auto save after a few critical ops
        /// Remove/Add counts more than one
        /// </summary>
        /// <param name="worth">How important you consider the operation to be, value > 10 triggers save immediately</param>
        public void CriticalOpCounter(int worth = 1)
        {
            _criticalOps += worth;
            if (_criticalOps > 10)
            {
                SaveFile();
                InterviewerCore.LogInfo("已自动保存");
                _criticalOps = 0;

            }
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
            InterviewerCore.LogInfo($"{_fileName}已保存。");
        }

        void OutputPngFile()
        {
            CriticalOpCounter(11);
            InterviewerPainter.RenderPngFile(_project);
        }
        #endregion
        
    }
}