using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(RotationCopy))]
public class RotationCopyEditor : Editor
{

    RotationCopy script;
    void OnEnable()
    {
        script = (RotationCopy)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Fill Reference"))
        {
            script.FillReference_Editor();
        }
    }
    
}
