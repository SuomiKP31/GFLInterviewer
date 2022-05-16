using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GFLInterviewer.Core
{
    public class InterviewerProjectFile
    {
        JObject InterviewerJsonObject;
        List<InterviewerBaseNode> nodeList;
        
        /// <summary>
        /// Create instance to edit with existing json file
        /// </summary>
        /// <param name="jsonName"></param>
        /// <returns></returns>
        public static InterviewerProjectFile CreateInstance(string jsonName)
        {
            string jsonPath = $"{InterviewerCore.projectFilePath}/{jsonName}.json";
            StreamReader file = new StreamReader(jsonPath, Encoding.Default);
            var f = new InterviewerProjectFile();
            using JsonTextReader reader = new JsonTextReader(file);
            f.SetJsonObject((JObject)JToken.ReadFrom(reader));
            
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

        public void SetJsonObject(JObject obj)
        {
            InterviewerJsonObject = obj;
        }
        public void SaveInstanceToFile(string filename)
        {
            
        }
        
        
    }
}