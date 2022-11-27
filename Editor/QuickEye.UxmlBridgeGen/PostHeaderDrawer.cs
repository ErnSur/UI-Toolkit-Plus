namespace QuickEye.UxmlBridgeGen
{
    public abstract class PostHeaderDrawer
    {
        public UnityEditor.Editor Editor { get; private set; }

        public PostHeaderDrawer(UnityEditor.Editor editor)
        {
            Editor = editor;
        }
        
        public abstract void OnGUI();
    }
}