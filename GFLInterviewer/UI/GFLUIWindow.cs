using ImGuiNET;

namespace GFLInterviewer.UI
{
    public abstract class GfluiWindow
    {
        protected bool _isActive;
        protected string _name;

        /// <summary>
        /// Main Entry of UI drawing
        /// </summary>
        public virtual void DrawUI()
        {
            if (_isActive)
            {
                ImGui.Begin(_name, ref _isActive, ImGuiWindowFlags.MenuBar);
                DrawMenuBar();
                DrawBody();
                ImGui.End();
            }
        }

        protected abstract void DrawMenuBar();
        protected abstract void DrawBody();

        public void SetActive(bool active)
        {
            _isActive = active;
        }

        public string GetName()
        {
            return _name;
        }

        public virtual void Toggle()
        {
            if (_isActive)
            {
                OnClose();
            }
            
            SetActive(!_isActive);
        }

        /// <summary>
        /// Callback when the window is closed
        /// </summary>
        public virtual void OnClose()
        {
            
        }
    }
}