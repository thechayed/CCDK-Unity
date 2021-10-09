using UnityEditor;
using UnityEngine;

namespace CCDKEditor
{
    public class EditorTools : ScriptableObject
    {
        [MenuItem("Tools/Check For Field Duplicates")]
        static void DoIt()
        {
            EditorManager.FindFieldDuplicates();
        }
    }
}