using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GFLInterviewer.Core;
using GFLInterviewer.UI;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

using static ImGuiNET.ImGuiNative;

namespace ImGuiNET
{
    class Program
    {
        private static Sdl2Window _window;
        private static GraphicsDevice _gd;
        private static CommandList _cl;
        private static ImGuiController _controller;
        // private static MemoryEditor _memoryEditor;

        // UI state
        private static float _f = 0.0f;
        private static int _dragInt = 0;
        private static Vector3 _clearColor = new Vector3(0.45f, 0.55f, 0.6f);
        private static uint s_tab_bar_flags = (uint)ImGuiTabBarFlags.Reorderable;
        
        


        static void SetThing(out float i, float val) { i = val; }

        static void Main(string[] args)
        {
            // Create window, GraphicsDevice, and all resources necessary for the demo.
            VeldridStartup.CreateWindowAndGraphicsDevice(
                new WindowCreateInfo(50, 50, 1920, 1080, WindowState.Normal, "GFL Interviewer"),
                new GraphicsDeviceOptions(true, null, true, ResourceBindingModel.Improved, true, true),
                out _window,
                out _gd);
            _window.Resized += () =>
            {
                _gd.MainSwapchain.Resize((uint)_window.Width, (uint)_window.Height);
                _controller.WindowResized(_window.Width, _window.Height);
            };
            
            _cl = _gd.ResourceFactory.CreateCommandList();


            
            //Init stuff
            InitCore();
            InitWindows();
            
            // Construct renderer
            _controller = new ImGuiController(_gd, _gd.MainSwapchain.Framebuffer.OutputDescription, _window.Width, _window.Height);
            
            // IME HANDLER
            var mvp = ImGui.GetMainViewport();
            mvp.PlatformHandleRaw = _window.Handle;


            // Main application loop
            while (_window.Exists)
            {
                InputSnapshot snapshot = _window.PumpEvents();
                if (!_window.Exists) { break; }
                _controller.Update(1f / 60f, snapshot); // Feed the input events to our ImGui controller, which passes them through to ImGui.

                SubmitUI();

                _cl.Begin();
                _cl.SetFramebuffer(_gd.MainSwapchain.Framebuffer);
                _cl.ClearColorTarget(0, new RgbaFloat(_clearColor.X, _clearColor.Y, _clearColor.Z, 1f));
                _controller.Render(_gd, _cl);
                _cl.End();
                _gd.SubmitCommands(_cl);
                _gd.SwapBuffers(_gd.MainSwapchain);
            }

            // Clean up Veldrid resources
            _gd.WaitForIdle();
            _controller.Dispose();
            _cl.Dispose();
            _gd.Dispose();
        }

        #region Inits
        /// <summary>
        /// Ready the UI window classes
        /// </summary>
        static void InitWindows()
        {
            ProjectCreator projectCreator = ProjectCreator.CreateInstance(true);
            InterviewerCore.WindowsToDraw.Add(projectCreator.GetName(), projectCreator);
        }

        static void InitCore()
        {
            InterviewerCore.Init();
        }
        #endregion
        

        private static void SubmitUI()
        {
            // Demo code adapted from the official Dear ImGui demo program:
            // https://github.com/ocornut/imgui/blob/master/examples/example_win32_directx11/main.cpp#L172

            // 1. Show a simple window.
            // Tip: if we don't call ImGui.BeginWindow()/ImGui.EndWindow() the widgets automatically appears in a window called "Debug".
            {
                float framerate = ImGui.GetIO().Framerate;
                ImGui.Text($"Application average {1000.0f / framerate:0.##} ms/frame ({framerate:0.#} FPS)");

                // Singleton Window Toggles
                foreach (var windowKv in InterviewerCore.WindowsToDraw)
                {
                    GfluiWindow w;
                    if (ImGui.Button(windowKv.Key))
                    {
                        if (InterviewerCore.WindowsToDraw.TryGetValue(windowKv.Key, out w))
                        {
                            w.Toggle();
                        }
                    }
                }
                
                // Debug Message Stack
                ImGui.Separator();
                ImGui.Text("Debug Info");
                ImGui.Separator();
                foreach (var log in InterviewerCore.logStacks)
                {
                    ImGui.Text(log);
                }
            }
            InterviewerCore.RemoveClosedWindow();
            InterviewerCore.DrawAllWindow();

            // 2. Show another simple window. In most cases you will use an explicit Begin/End pair to name your windows.
            /*if (true)
            {
                ImGui.Begin("Another Window", ref _showAnotherWindow);
                ImGui.Text("Hello from another window!");
                if (ImGui.Button("Close Me"))
                    _showAnotherWindow = false;
                ImGui.End();
            }*/
            
            
            

            ImGuiIOPtr io = ImGui.GetIO();
            
            SetThing(out io.DeltaTime, 2f);


        }
    }
}
