using System.Collections;
using UnityEngine;
using UnityEditor;
using CCDKEngine;
using CCDKGame;

[CustomEditor(typeof(Controller))]
public class ControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Controller PC = (Controller)target;

        if(GUILayout.Button("Construct Player Controller"))
        {
            PC.PCConstructor();
        }
    }
}