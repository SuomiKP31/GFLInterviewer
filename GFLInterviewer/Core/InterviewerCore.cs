using System.Collections.Generic;
using System.IO;
using System.Text;
using GFLInterviewer.UI;
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
        #region Json

        

        
        static JObject configJson;

        public static string fontPath;

        public static string resourcePath;
        public static string avatarPath;
        public static string outputPath;
        public static string projectFilePath;

        public static void Init()
        {
            ReadConfig();
            
            HandleFont();
            SetPaths();
        }

        public static void ReadConfig()
        {
            StreamReader file = new StreamReader("C:/Users/fangz/RiderProjects/GFLInterviewer/GFLInterviewer/config.json", Encoding.Default);
            using JsonTextReader reader = new JsonTextReader(file);
            configJson = (JObject)JToken.ReadFrom(reader);
            
            file.Close();
        }

        public static void HandleFont()
        {
            fontPath = configJson.GetValue("fontPath").ToString();
            
        }

        /// <summary>
        /// Set:
        /// resourcePath: General image png resources location. incl. Frames and dialog bubbles
        /// avatarPath: .png avatars
        /// outputPath: output generated long image
        /// projectFilePath: Store project .json file
        /// </summary>
        static void SetPaths()
        {
            resourcePath = configJson.GetValue("resourcePath").ToString();
            avatarPath = configJson.GetValue("avatarPath").ToString();
            outputPath = configJson.GetValue("outputPath").ToString();
            projectFilePath = configJson.GetValue("projectPath").ToString();
        }
        
        #endregion

        #region UI

        // Windows
        public static Dictionary<string, GfluiWindow> WindowsToDraw = new Dictionary<string, GfluiWindow>(); // Singleton windows
        public static List<GfluiWindow> RepeatableWindows = new();

        public static void DrawAllWindow()
        {
            foreach (var windowKv in InterviewerCore.WindowsToDraw)
            {
                windowKv.Value.DrawUI();
            }

            foreach (var window in RepeatableWindows)
            {
                window.DrawUI();
            }
        }

        public static void AddRepeatableWindow(GfluiWindow wd)
        {
            RepeatableWindows.Add(wd);
        }

        public static void RemoveRepeatableWindow(GfluiWindow wd)
        {
            if (RepeatableWindows.Contains(wd))
            {
                RepeatableWindows.Remove(wd);
            }
        }

        #endregion
    }
}