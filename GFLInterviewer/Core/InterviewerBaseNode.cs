namespace GFLInterviewer.Core
{
    public abstract class InterviewerBaseNode
    {
        public string speakerName = "";
        public string content = "";
        public float fontSize = 15.0f;
        protected InterviewerProjectFile owner;

        InterviewerBaseNode(InterviewerProjectFile owner)
        {
            this.owner = owner;
        }


        /// <summary>
        /// Call Graphic api to draw on a bitmap png file
        /// TODO: parameters
        /// </summary>
        public abstract void Render();
        
        /// <summary>
        /// Call ImGui methods to draw the UI
        /// The UI takes input, and call owner methods if necessary (e.g.: prev node, next node)
        /// </summary>
        public abstract void DrawNode();


        public void SetAllNodeAttr(string _content, float _fontsize, string _speaker = "")
        {
            speakerName = _speaker;
            content = _content;
            fontSize = _fontsize;
        }
    }
}