using ImGuiNET;
using System;
using Newtonsoft.Json.Linq;

namespace GFLInterviewer.Core
{
    public enum NodeConf
    {
        DialogBubbleConfigL,
        DialogBubbleConfigR
    }
    public abstract class InterviewerBaseNode
    {

        public string speakerName = "";
        public string content = "";
        public float fontSize = 15.0f;
        protected InterviewerProjectFile owner;

        public NodeConf conf; // Config name of the node

        protected JObject confObject;

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

        /// <summary>
        /// Generate a Json Object containing the node's content
        /// </summary>
        /// <returns></returns>
        public JObject GenerateJObject()
        {
            return null;
        }


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

        public virtual string GetPreviewText()
        {
            bool tooLong = content.Length > 11;
            return $"{speakerName}: {content.Substring(0, tooLong? 10 : content.Length)}...";
        }
    }
}