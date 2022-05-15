using System.IO;
using System.Text;
using ImGuiNET;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GFLInterviewer.Core
{
    /// <summary>
    /// Global Manager. Entry of file create/input
    /// Also stores global variable like paths
    /// </summary>
    public static class InterviewerCore
    {
        static JObject configJson;

        public static string fontPath;
        
        public static void Init()
        {
            ReadConfig();
            
            HandleFont();
        }

        public static void ReadConfig()
        {
            StreamReader file = new StreamReader("C:/Users/fangz/RiderProjects/GFLInterviewer/GFLInterviewer/config.json", Encoding.Default);
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                configJson = (JObject)JToken.ReadFrom(reader);
            }
        }

        public static void HandleFont()
        {
            fontPath = configJson.GetValue("fontPath").ToString();
            
        }
    }
}