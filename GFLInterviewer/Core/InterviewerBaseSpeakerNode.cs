namespace GFLInterviewer.Core
{
    public class InterviewerLeftSpeakerNode : InterviewerBaseNode
    {
        public static InterviewerLeftSpeakerNode CreateInstance(InterviewerProjectFile owner)
        {
            var node =  new InterviewerLeftSpeakerNode();
            node.owner = owner;
            return node;
        }


        public override void Render()
        {
            // TODO
        }

        public override void DrawNode()
        {
            
        }
    }
}