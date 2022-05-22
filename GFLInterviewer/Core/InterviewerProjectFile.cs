﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GFLInterviewer.Core
{
    /// <summary>
    /// Core logic class of InterviewerEditor. One editor instance have one project instance
    /// </summary>
    public class InterviewerProjectFile
    {
        #region Json


        
        public void SetJsonObject(JObject obj)
        {
            interviewerJsonObject = obj;
        }

        void InitFromJsonObject()
        {
            JObject? meta = interviewerJsonObject.GetValue("meta") as JObject;
            SetMetaData((string) meta.GetValue("author"), (string) meta.GetValue("projectName"));

            JArray speakerArray = interviewerJsonObject.GetValue("speakers") as JArray;
            SetSpeakerData(speakerArray.ToObject<List<string>>());
            
            
            nodeList = new List<InterviewerBaseNode>();
            JArray nodeJsonArray = interviewerJsonObject.GetValue("nodes") as JArray;
            SetNodesData(nodeJsonArray);
            
        }
        
        public void SaveInstanceToFile()
        {
            string jsonPath = $"{InterviewerCore.projectFilePath}/{fileName}";
            WriteHeader();
            WriteNodes();
            File.WriteAllText(jsonPath, interviewerJsonObject.ToString(), Encoding.UTF8);
        }

        public void SetMetaData(string _author, string _titleName)
        {
            author = _author;
            projectName = _titleName;
        }

        public void SetSpeakerData(List<string> speakers)
        {
            speakerNames = speakers;
        }

        public void SetNodesData(JArray jObjList)
        {
            foreach (var nodeJson in jObjList)
            {
                var obj = (JObject) nodeJson;
                NodeConf conf = obj.GetValue("nodeConf").ToObject<NodeConf>();
                InterviewerBaseNode jsonNode;
                if (conf != NodeConf.NarratorConfig)
                {
                    jsonNode = InterviewerBaseSpeakerNode.CreateInstanceFromJObject(obj, this);
                }
                else
                {
                    jsonNode = InterviewerNarratorNode.CreateInstanceFromJObject(obj, this);
                }
                
                nodeList.Add(jsonNode);
            }
            
        }

        public void AddSpeaker(string speaker)
        {
            if (speakerNames.Contains(speaker))
            {
                return;
            }
            speakerNames.Add(speaker);
        }

        void WriteHeader()
        {
            // Metadata
            JObject meta = new JObject();
            meta.Add("projectName", projectName);
            meta.Add("author", author);
            meta.Add("date", DateTime.Now);
            interviewerJsonObject["meta"] = meta;

            // Speakers
            JArray speakers = new JArray();
            foreach (var speaker in speakerNames)
            {
                speakers.Add(speaker);
            }

            interviewerJsonObject["speakers"] = speakers;
        }

        void WriteNodes()
        {
            JArray nodes = new JArray();
            foreach (var node in nodeList)
            {
                nodes.Add(node.GenerateJObject());
            }

            interviewerJsonObject["nodes"] = nodes;
        }
        
        #endregion

        #region Project Specific Attrs
        // These will be write into a file header

        public string projectName;
        public string author;

        public List<string> speakerNames;
        JObject interviewerJsonObject;
        List<InterviewerBaseNode> nodeList;
        public string fileName;
        
        #endregion

        #region Nodes

        /// <summary>
        /// Create a new node of specific configuration
        /// </summary>
        /// <param name="conf"></param>
        /// <returns>index</returns>
        public int CreateNode(NodeConf conf)
        {
            InterviewerBaseNode node;
            int index = 0;
            if (conf != NodeConf.NarratorConfig)
            {
                node = InterviewerBaseSpeakerNode.CreateInstance(this, conf);
                index = nodeList.Count;
            }
            else
            {
                node = InterviewerNarratorNode.CreateInstance(this, conf);
                index = nodeList.Count;
            }
            
            nodeList.Add(node);
            
            return index;
        }

        public InterviewerBaseNode GetNode(int index)
        {
            if (nodeList.Count == 0)
            {
                return null;
            }
            
            InterviewerBaseNode node;
            if (index < 0)
            {
                node = nodeList[^1];
            }
            else if (index > nodeList.Count - 1)
            {
                int i = CreateNode(NodeConf.DialogBubbleConfigL);
                node = nodeList[i];
            }
            else
            {
                node = nodeList[index];
            }

            return node;
        }

        public void RemoveNode(int index)
        {
            nodeList.Remove(nodeList[index]);
        }

        public List<InterviewerBaseNode> GetNodeList()
        {
            return nodeList;
        }
        #endregion
        
        /// <summary>
        /// Create instance to edit with existing json file
        /// </summary>
        /// <param name="jsonName"> Read json file from config.projectFilePath</param>
        /// <returns></returns>
        public static InterviewerProjectFile CreateInstance(string jsonName)
        {
            string jsonPath = $"{InterviewerCore.projectFilePath}/{jsonName}";
            StreamReader file = new StreamReader(jsonPath, Encoding.UTF8);
            var f = new InterviewerProjectFile();
            using JsonTextReader reader = new JsonTextReader(file);
            f.SetJsonObject((JObject)JToken.ReadFrom(reader));
            f.fileName = jsonName;
            f.InitFromJsonObject();
            
            file.Close();
            return f;
        }

        /// <summary>
        /// Create Empty InterviewerProject Instance
        /// </summary>
        /// <returns></returns>
        public static InterviewerProjectFile CreateInstance()
        {
            var f = new InterviewerProjectFile();
            f.SetJsonObject(new JObject());
            f.speakerNames = new List<string>();
            f.nodeList = new List<InterviewerBaseNode>();
            return f;
        }
        
    }
}