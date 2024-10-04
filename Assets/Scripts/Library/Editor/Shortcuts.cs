#if UNITY_EDITOR
using UnityEditor;

namespace Library.Editor
{
    public static class Shortcuts
    {
        // Add new personal shortcuts without overriding the defaults
        
        [MenuItem("Library/Shortcuts/Play")]
        private static void PlayGame()
        {
            EditorApplication.ExecuteMenuItem("Edit/Play");
        }
    }
}

#endif