using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using RootMotion.FinalIK;

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

        if (GUILayout.Button("Print VecToStr"))
        {
            Debug.Log(GlobalStructure.Vector3ToString(Vector3.right));
            Debug.Log(GlobalStructure.StringToVector3("50.3 23.5 24"));
        }
        if (GUILayout.Button("print LeftHand"))
        {
            FullBodyBipedIK ik = GameObject.Find("Robot Kyle").GetComponent<FullBodyBipedIK>();

            Debug.Log("" + ik.solver.GetEffector(FullBodyBipedEffector.LeftHand).bone.position);
            
        }
        if (GUILayout.Button("move LeftHand"))
        {
            FullBodyBipedIK ik = GameObject.Find("Robot Kyle").GetComponent<FullBodyBipedIK>();
            ik.solver.GetEffector(FullBodyBipedEffector.LeftHand).position = new Vector3(-0.9f, 1.2f, 0);
        }
        if (GUILayout.Button("set LeftHand weight"))
        {
            FullBodyBipedIK ik = GameObject.Find("Robot Kyle").GetComponent<FullBodyBipedIK>();
            ik.solver.GetEffector(FullBodyBipedEffector.LeftHand).positionWeight = 0;
        }

        GUILayout.EndScrollView();
    }
}
