using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
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

        // Font holder
        public static string fontPath;
        public static Font drawingFont;
        static PrivateFontCollection pfc = new PrivateFontCollection();
        
        

        public static string resourcePath;
        public static string avatarPath;
        public static string outputPath;
        public static string projectFilePath;

        // Avatars stored in avatar path. They are used in preview and rendering
        public static List<string> avatarNames = new List<string>();
        static Dictionary<string, Image> avatarImages = new Dictionary<string, Image>();
        static Dictionary<string, Image> resourceImages = new Dictionary<string, Image>();
            
        // Logs
        public static Stack<string> logStacks = new Stack<string>();
        public static void Init()
        {
            ReadConfig();
            
            HandleFont();
            SetPaths();
            
            LoadCommonResources();
            RefreshAvatars();
        }
        
        public static void ReadConfig()
        {
            StreamReader file = new StreamReader("C:/Users/fangz/RiderProjects/GFLInterviewer/GFLInterviewer/config.json", Encoding.Default);
            using JsonTextReader reader = new JsonTextReader(file);
            configJson = (JObject)JToken.ReadFrom(reader);
            
            file.Close();
        }
        
        static void HandleFont()
        {
            fontPath = configJson.GetValue("fontPath").ToString();
            
            pfc.AddFontFile(fontPath);
            drawingFont = new Font(pfc.Families[0], 25.0f);

        }

        public static void SwitchFontSize(float size)
        {
            drawingFont = new Font(pfc.Families[0], size, drawingFont.Style);
        }

        public static void SwitchFontSizeAndStyle(float size, FontStyle style)
        {
            drawingFont = new Font(pfc.Families[0], size, style);
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

        public static void RemoveClosedWindow()
        {
            List<GfluiWindow> tmpList = new List<GfluiWindow>(RepeatableWindows);
            foreach (var window in tmpList)
            {
                if (!window.IsActive())
                {
                    RemoveRepeatableWindow(window);
                    window.OnClose();
                }
            }
        }

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

        public static void LogInfo(string log)
        {
            if (logStacks.Count > 10)
            {
                logStacks.Pop();
            }
            logStacks.Push(log);
        }
        #endregion

        #region Resources
        
        // Project Files
        public static List<string> FetchProjectNameList()
        {
            string[] files = Directory.GetFiles(projectFilePath, "*.json");


            return GFLIUtils.GetSelectionListFromFullPath(files);
        }

        // Common Image Resources
        public static void LoadCommonResources()
        {
            string[] files = Directory.GetFiles(resourcePath, "*.png");
            List<string> resourceNames = GFLIUtils.GetSelectionListFromFullPath(files);
            var newDict = new Dictionary<string, Image>();
            foreach (var res in resourceNames)
            {
                string path = $"{resourcePath}\\{res}";
                newDict.Add(res, Image.FromFile(path));
            }

            resourceImages = newDict;
        }
        
        // Avatars
        
        /// <summary>
        /// Load all avatars from avatar path
        /// </summary>
        public static void LoadAvatars()
        {
            var newDict = new Dictionary<string, Image>();
            foreach (var avatar in avatarNames)
            {
                string path = $"{avatarPath}\\{avatar}";
                newDict.Add(avatar, Image.FromFile(path));
            }

            avatarImages = newDict;
            
        }

        public static Image GetResourceImage(string name)
        {
            return resourceImages[name];
        }

        public static Image GetAvatarImage(string name)
        {
            return avatarImages[name];
        }

        public static bool HasAvatar(string name)
        {
            return avatarImages.ContainsKey(name);
        }
        
        public static List<string> GetAvatarNames()
        {
            string[] files = Directory.GetFiles(avatarPath, "*.png");
            List<string> avatarNames = new List<string>(GFLIUtils.GetSelectionListFromFullPath(files));

            return avatarNames;
        }

        public static void RefreshAvatars()
        {
            avatarNames = GetAvatarNames();
            LoadAvatars();
        }

        public static JObject GetConfigObject(NodeConf conf)
        {
            var nodeConfs = configJson.GetValue("nodeConfs") as JObject;
            
            return nodeConfs.GetValue(conf.ToString()) as JObject;
        }
        #endregion
    }
}