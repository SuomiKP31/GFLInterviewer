using System;
using System.Collections.Generic;
using System.IO;
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
        #region Json and Nodes

        JObject interviewerJsonObject;
        List<InterviewerBaseNode> nodeList;
        public string fileName;
        
        public void SetJsonObject(JObject obj)
        {
            interviewerJsonObject = obj;
        }

        void InitFromJsonObject()
        {
            JObject? meta = interviewerJsonObject.GetValue("meta") as JObject;
            SetMetaData((string) meta.GetValue("author"), (string) meta.GetValue("projectName"));
        }
        
        public void SaveInstanceToFile()
        {
            string jsonPath = $"{InterviewerCore.projectFilePath}/{fileName}.json";
            WriteHeader();
            
            File.WriteAllText(jsonPath, interviewerJsonObject.ToString(), Encoding.UTF8);
        }

        public void SetMetaData(string _author, string _titleName)
        {
            author = _author;
            projectName = _titleName;
        }

        void WriteHeader()
        {
            JObject meta = new JObject();
            meta.Add("projectName", projectName);
            meta.Add("author", author);
            meta.Add("date", DateTime.Now);
            interviewerJsonObject.Add("meta", meta);
        }
        
        #endregion

        #region Project Specific Attrs
        // These will be write into a file header

        public string projectName;
        public string author;
        
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
            return f;
        }

        #region Resources List

        List<string> _avatarFileNames;
        List<string> _speakerNames;

        #endregion
    }
}