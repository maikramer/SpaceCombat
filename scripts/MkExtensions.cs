using Godot;

namespace MkGames {
    public static class Extensions {
        public static T FindClass<T> (this Node node) where T : Node {

            var children = node.GetChildren ();
            foreach (Node t in children) {
                if (t is T child)
                    return child;
                if (t.GetChildCount () > 0) {
                    var ret = t.FindClass<T> ();
                    if (ret != null)
                        return ret;
                }
            }

            return null;
        }
    }
}