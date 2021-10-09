using UnityEditor;
using UnityEngine;

namespace Assets.CCDK.Editor.Tools
{
    public class Toolkit : ScriptableObject
    {
        private static bool TimedAutoSave = true;

        static Toolkit()
        {
            TimedAutoSave = EditorPrefs.GetBool("Tools/Timed Autosave", false);
            UnityEditor.Menu.SetChecked("Tools/Timed Autosave", TimedAutoSave);
        }

        [MenuItem("Tools/CCDK/Create Spawn")]
        static void DoIt()
        {
            GameObject spawnPoint = Instantiate(Resources.Load<GameObject>("CCDK/PrefabDefaults/Spawn Point"));
            spawnPoint.AddComponent<CCDKGame.SpawnPoint>();
        }

        [MenuItem("Tools/Timed Autosave")]
        private static void AutosaveEveryXSeconds()
        {
            TimedAutoSave = !TimedAutoSave;
            UnityEditor.Menu.SetChecked("Tools/Timed Autosave", TimedAutoSave);
        }
    }
}