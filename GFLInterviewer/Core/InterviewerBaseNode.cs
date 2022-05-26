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
        DialogBubbleConfigR,
        NarratorConfig
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
        public float fontSize = 20.0f;
        public Vector3 colorVector = new Vector3(1,1,1);
        protected InterviewerProjectFile owner;

        public NodeConf conf; // Config name of the node

        public JObject confObject;
        

        /// <summary>
        /// Call Graphic api to draw on a bitmap png file
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
            bool tooLong = content.Length > 25;
            string contentString = content.Substring(0, tooLong ? 24 : content.Length);
                
            contentString = contentString.Replace("\n", " "); // Get rid of new lines
            if (tooLong)
            {
                contentString += "...";
            }
            return $"{GetAcronymByConf()}|{speakerName}: {contentString}";
        }

        protected string GetAcronymByConf()
        {
            string ret = "";
            switch (conf)
            {
                case NodeConf.DialogBubbleConfigL:
                    ret = "左";
                    break;
                case NodeConf.DialogBubbleConfigR:
                    ret = "右";
                    break;
                case NodeConf.NarratorConfig:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ret;
        }

        protected Rectangle GetRectFromJObject(JObject rect)
        {
            int x = rect.GetValue("PosX").ToObject<int>();
            int y = rect.GetValue("PosY").ToObject<int>();
            int w = rect.GetValue("Width").ToObject<int>();
            int h = rect.GetValue("Height").ToObject<int>();

            return new Rectangle(x, y, w, h);

        }

        public abstract int GetHeight();
    }
}