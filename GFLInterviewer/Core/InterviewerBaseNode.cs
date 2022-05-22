using ImGuiNET;
using System;
using System.Drawing;
using System.Numerics;
using Newtonsoft.Json.Linq;

namespace GFLInterviewer.Core
{
    public enum NodeConf
    {
        DialogBubbleConfigL,
        DialogBubbleConfigR
    }

    public enum StrFormatType
    {
        Speaker,
        Content
    }
    public abstract class InterviewerBaseNode
    {

        public string speakerName = "";
        public string content = "";
        public float fontSize = 15.0f;
        protected InterviewerProjectFile owner;

        protected NodeConf conf; // Config name of the node

        public JObject confObject;
        

        /// <summary>
        /// Call Graphic api to draw on a bitmap png file
        /// TODO: parameters
        /// </summary>
        public abstract void Render(Graphics g, Rectangle rect);
        
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
        public virtual JObject GenerateJObject()
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

        protected Rectangle GetRectFromJObject(JObject rect)
        {
            int x = rect.GetValue("PosX").ToObject<int>();
            int y = rect.GetValue("PosY").ToObject<int>();
            int w = rect.GetValue("Width").ToObject<int>();
            int h = rect.GetValue("Height").ToObject<int>();

            return new Rectangle(x, y, w, h);

        }
    }
}