using System.Collections;
using UnityEngine;
using UnityEditor;
using CCDKEngine;
using CCDKGame;

[CustomEditor(typeof(Pawn))]
public class PawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Pawn pawn = (Pawn)target;

        if(GUILayout.Button("Construct Pawn"))
        {
            pawn.PawnConstructer();
        }
    }
}