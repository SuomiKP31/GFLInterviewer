using ImGuiNET;

namespace GFLInterviewer.Core
{
    public abstract class InterviewerBaseNode
    {

        public string speakerName = "";
        public string content = "";
        public float fontSize = 15.0f;
        protected InterviewerProjectFile owner;
        


        /// <summary>
        /// Call Graphic api to draw on a bitmap png file
        /// TODO: parameters
        /// </summary>
        public abstract void Render();
        
        /// <summary>
        /// Call ImGui methods to draw the UI
        /// The UI takes input, and call owner methods if necessary (e.g.: prev node, next node)
        /// These calls will be inserted to the Editor view
        /// </summary>
        public abstract void DrawNode();


        public virtual void SetAllNodeAttr(string _content, float _fontsize, string _speaker = "")
        {
            speakerName = _speaker;
            content = _content;
            fontSize = _fontsize;
        }

        /// <summary>
        /// Draw a dropdown list containing avatars got from avatar path
        /// </summary>
        public virtual void DrawAvatarList()
        {
            
        }
    }
}