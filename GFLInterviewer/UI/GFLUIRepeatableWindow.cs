using GFLInterviewer.Core;

namespace GFLInterviewer.UI
{
    public abstract class GFLUIRepeatableWindow : GfluiWindow
    {
        public override void OnClose()
        {
            base.OnClose();
            InterviewerCore.RemoveRepeatableWindow(this);
        }
    }
}