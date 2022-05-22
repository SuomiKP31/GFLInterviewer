using System;
using System.Drawing;
using System.Numerics;
using ImGuiNET;
using Newtonsoft.Json.Linq;

namespace GFLInterviewer.Core
{
    public class InterviewerBaseSpeakerNode : InterviewerBaseNode
    {
        string avatarName;

        public static InterviewerBaseSpeakerNode CreateInstance(InterviewerProjectFile owner, NodeConf conf)
        {
            var node =  new InterviewerBaseSpeakerNode();
            node.owner = owner;
            node.conf = conf;
            node.avatarName = "";
            node.confObject = InterviewerCore.GetConfigObject(conf);
            return node;
        }

        public static InterviewerBaseSpeakerNode CreateInstanceFromJObject(JObject nodeJson,
            InterviewerProjectFile owner)
        {
            var node =  new InterviewerBaseSpeakerNode();
            node.owner = owner;
            node.conf = nodeJson.GetValue("nodeConf").ToObject<NodeConf>();
            node.speakerName = nodeJson.GetValue("speaker").ToString();
            node.avatarName = nodeJson.GetValue("avatar").ToString();
            node.content = nodeJson.GetValue("content").ToString();
            node.fontSize = nodeJson.GetValue("fontSize").ToObject<float>();

            node.confObject = InterviewerCore.GetConfigObject(node.conf);
            return node;
        }


        public override void Render(Graphics g, Rectangle rect)
        {
            // 0. Frame
            Image frame = InterviewerCore.GetResourceImage(confObject.GetValue("frameFile").ToString());
            g.DrawImage(frame, rect, new Rectangle(0, 0, frame.Width, frame.Height), GraphicsUnit.Pixel);
            
            // 1. Draw Avatar

            Rectangle avatarRect = GetRectFromJObject((JObject)confObject.GetValue("avatarRect"));
            avatarRect.X += rect.X;
            avatarRect.Y += rect.Y;
            
            // Avatar Frame
            Image avatar;
            Pen avatarFramePen = new Pen(Color.Orange);
            avatarFramePen.Width = 5;
            
            if (InterviewerCore.HasAvatar(avatarName))
            {
                
                
                g.DrawRectangle(avatarFramePen, avatarRect);

                avatar = InterviewerCore.GetAvatarImage(avatarName);
                g.DrawImage(avatar, avatarRect, new Rectangle(0, 0, avatar.Width, avatar.Height), GraphicsUnit.Pixel);
            }
            
            
            // 2. Speaker Name
            Rectangle speakerRect = GetRectFromJObject((JObject)confObject.GetValue("speakerRect"));
            speakerRect.X += rect.X;
            speakerRect.Y += rect.Y;
            
           // g.DrawRectangle(avatarFramePen, speakerRect); // debug
           InterviewerCore.SwitchFontSizeAndStyle(18.0f, FontStyle.Bold);
           g.DrawString(speakerName, InterviewerCore.drawingFont, Brushes.Orange, speakerRect, GetStringFormatFromConfig(StrFormatType.Speaker));

            // 3. Content Text

            Rectangle contentRect = GetRectFromJObject((JObject) confObject.GetValue("contentRect"));
            contentRect.X += rect.X;
            contentRect.Y += rect.Y;
            
            InterviewerCore.SwitchFontSizeAndStyle(fontSize, FontStyle.Bold);
            g.DrawString(content, InterviewerCore.drawingFont, Brushes.White, contentRect, GetStringFormatFromConfig(StrFormatType.Content));
            
            // g.DrawRectangle(avatarFramePen, contentRect);

        }

        public override void DrawNode()
        {
            ImGui.Text(confObject.GetValue("displayName").ToString());
            // Chose speaker of this node. To add a speaker, use the editor menu
            if (ImGui.BeginCombo("说话人", speakerName))
            {
                foreach (var proj in owner.speakerNames)
                {
                    bool isSelected = speakerName == proj;
                    if (ImGui.Selectable(proj, isSelected))
                    {
                        speakerName = proj;
                    }

                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndCombo();
            }
            
            DrawAvatarList();
            // ImGui.SameLine();

            if (ImGui.InputFloat("字号", ref fontSize, 1.0f))
            {
                fontSize = Math.Clamp(fontSize, 5.0f, 35.0f);
            }
            
            ImGui.InputTextMultiline("内容", ref content, 256, new Vector2(550,125));
            
            if (ImGui.Button("切换方向"))
            {
                ChangeDirectionConf();
            }
        }

        public override JObject GenerateJObject()
        {
            JObject ret = new JObject();
            ret.Add("nodeConf", (int)conf);
            ret.Add("speaker", speakerName);
            ret.Add("avatar", avatarName);
            ret.Add("content", content);
            ret.Add("fontSize", fontSize);
            return ret;
        }

        public override void DrawAvatarList()
        {
            if (ImGui.BeginCombo("头像", avatarName))
            {
                foreach (var avatar in InterviewerCore.avatarNames)
                {
                    bool isSelected = avatarName == avatar;
                    if (ImGui.Selectable(avatar, isSelected))
                    {
                        avatarName = avatar;
                    }

                    if (isSelected)
                    {
                        ImGui.SetItemDefaultFocus();
                    }
                }
                ImGui.EndCombo();
            }
        }

        /// <summary>
        /// Generate StringFormat for text. Near/Far means align to the left/right.
        /// </summary>
        /// <param name="fmtType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected StringFormat GetStringFormatFromConfig(StrFormatType fmtType)
        {
            var fmt = new StringFormat();
            if (confObject.GetValue("speakerAlignmentRight").ToObject<bool>())
            {
                fmt.Alignment = StringAlignment.Far;
            }
            else
            {
                fmt.Alignment = StringAlignment.Near;
            }

            switch (fmtType)
            {
                case StrFormatType.Speaker:
                    fmt.LineAlignment = StringAlignment.Center;
                    break;
                case StrFormatType.Content:
                    // Do nothing
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fmtType), fmtType, null);
            }

            return fmt;
        }

        /// <summary>
        /// Change Direction
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected void ChangeDirectionConf()
        {
            switch (conf)
            {
                case NodeConf.DialogBubbleConfigL:
                    conf = NodeConf.DialogBubbleConfigR;
                    break;
                case NodeConf.DialogBubbleConfigR:
                    conf = NodeConf.DialogBubbleConfigL;
                    break;
                case NodeConf.NarratorConfig:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            confObject = InterviewerCore.GetConfigObject(conf);
        }

        public override int GetHeight()
        {
            return confObject.GetValue("height").ToObject<int>();
        }
    }
}