using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class DebugFunctionEditor : EditorWindow
{
    Vector2 globalScrollPosition;
    [MenuItem("Debug/功能调试 ")]
    static void Init()
    {
        DebugFunctionEditor window = (DebugFunctionEditor)EditorWindow.GetWindow(typeof(DebugFunctionEditor));
        window.Show();

    }
    void OnGUI()
    {
        globalScrollPosition = GUILayout.BeginScrollView(globalScrollPosition, new GUILayoutOption[0]);
        EditorGUILayout.HelpBox("村长:Application must in play mode", MessageType.Info);

        if (GUILayout.Button("Print"))
        {
            Debug.Log(GlobalStructure.Vector3ToString(Vector3.right));
            Debug.Log(GlobalStructure.StringToVector3("50.3 23.5 24"));
        }
        GUILayout.EndScrollView();
    }
}
